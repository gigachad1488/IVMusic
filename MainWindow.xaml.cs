using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Animation;

namespace IVMusic
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PlayList curplaylist = new PlayList();
        Sound cursound = new Sound();
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        bool IsLooping;
        bool IsMixing;
        public MainWindow()
        {
            InitializeComponent();
            Storyboard board = FindResource("GradientAnimation") as Storyboard;
            board.Begin();
            IsLooping = true;
            cyclebutton.Background.Opacity = 60;
            IsMixing = false;
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += timer_Tick;
            volumeslider.Value = 1;

        }

        private void ButtonColorChangeEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Controls.Button b = sender as System.Windows.Controls.Button;
            b.Foreground = Brushes.Aquamarine;
        }

        private void ButtonColorChangeLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Controls.Button b = sender as System.Windows.Controls.Button;
            string name = b.Name;
            string hex = "#FF14252B";
            Color color = (Color)ColorConverter.ConvertFromString(hex);
            b.Foreground = new SolidColorBrush(color);

        }

        private void stopbutton_Click(object sender, RoutedEventArgs e)
        {
            StopSound();
        }

        public void StopSound()
        {
            string text = stopbutton.Content.ToString();
            stopbutton.Content = text == "▶" ? "🛑" : "▶";
            if (cursound.IsInit && cursound != null)
            {
                if (text == "▶")
                    cursound.Stop();
                else
                {
                    if (!IsLooping && cursound.Time == cursound.Duration)
                        PlaySoundFromStart();
                    else
                        cursound.Play();
                }
            }
        }

        private void browsebutton_Click(object sender, RoutedEventArgs e)
        {
            AddBrowsePlayList();
        }

        public async void AddBrowsePlayList()
        {
            PlayList playlist = new PlayList();
            List<Sound> l = new List<Sound>();
            List<string> filepaths = new List<string>();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                playlist.Title = new DirectoryInfo(dialog.SelectedPath).Name;
                if (AllPlayLists.IsUnique(playlist))
                {
                    await Task.Run(() =>
                    {
                        filepaths = Directory.GetFiles(dialog.SelectedPath, "*.mp3").ToList();
                        if (filepaths.Count > 0)
                        {
                            foreach (var i in filepaths)
                            {
                                playlist.AddSound(new Sound(i));
                            }
                        }
                    });
                    if (filepaths.Count <= 0)
                    {
                        System.Windows.MessageBox.Show("в папке отсутствуют звуковые файлы формата mp3");
                        return;
                    }
                    string[] impath = Directory.GetFiles(dialog.SelectedPath, "*.png");
                    if (impath.Length > 0)
                    {
                        System.Windows.Controls.Image im = new System.Windows.Controls.Image();
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(impath[0], UriKind.Absolute);
                        image.EndInit();
                        playlist.image = image;
                    }
                    AllPlayLists.AddPlayList(playlist);
                    ResetPlayLists();
                }
                else
                {
                    System.Windows.MessageBox.Show("плейлист с таким названием уже добавлен");
                }
            }
            else
            {
                return;
            }

        }

        public void ResetPlayLists()
        {
            playlistslistbox.Items.Clear();
            for (int i = 0; i < AllPlayLists.Count; i++)
            {
                PlayList list = AllPlayLists.GetPlayList(i);
                playlistslistbox.Items.Add(list.Title);
            }
        }

        private void volumeslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (cursound.IsInit && cursound != null)
                cursound.Volume = (float)volumeslider.Value;

        }

        private void UploadPlayList(int i)
        {
            listview.Items.Clear();
            curplaylist = AllPlayLists.GetPlayList(i);
            playlistnametextbox.Text = curplaylist.Title;
            picturepanel.Source = curplaylist.image;
            countlabel.Content = $"треков: {curplaylist.Count}";
            for (int j = 0; j < curplaylist.Count; j++)
            {
                Sound sound = curplaylist.GetSound(j);
                listview.Items.Add(new MyItem { dur = sound.Duration, name = sound.Name });
            }
        }
        private void playlistslistbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (playlistslistbox.SelectedIndex >= 0)
                UploadPlayList(playlistslistbox.SelectedIndex);
        }

        private void h_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("робит");
        }

        private void listview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listview.SelectedIndex >= 0)
                UploadSound(listview.SelectedIndex);
        }
        bool first = true;
        public void UploadSound(int i)
        {
            if (!first)
            {
                cursound.Reset();
            }
            else
            {
                first = false;
            }
            cursound = curplaylist.GetSound(listview.SelectedIndex);
            timeslider.Value = 0;
            timeslider.Minimum = 0;
            timeslider.Maximum = cursound.Duration.TotalSeconds;
            durlabel.Content = cursound.Duration.ToString().Substring(3, 5);
            stopbutton.Content = "▶";
            soundnametextbox.Text = cursound.Name;
            PlaySoundFromStart();
            cursound.Volume = (float)volumeslider.Value;
        }

        public void PlaySoundFromStart()
        {
            cursound.Time = new TimeSpan(0);
            cursound.Play();
            timer.Start();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            timeslider.Value = cursound.Time.TotalSeconds;
            timelabel.Content = cursound.Time.ToString().Substring(3, 5);
            if (cursound.Time == cursound.Duration)
            {
                if (IsLooping)
                {
                    cursound.Time = new TimeSpan(0);
                }
                else
                {
                    NextSound();
                }
            }
        }

        private void browsebutton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            browsebutton.Background = Brushes.GreenYellow;
        }

        private void browsebutton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            browsebutton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString($"#FFD0C3C3"));
        }

        private void nextbutton_Click(object sender, RoutedEventArgs e)
        {
            NextSound();
        }

        public void NextSound()
        {
            if (cursound.IsInit)
            {
                if (IsMixing)
                {
                    Random rand = new Random();
                    listview.SelectedIndex = rand.Next(0, listview.Items.Count - 1);
                }
                else
                {
                    int i = listview.SelectedIndex;
                    i++;
                    if (i >= listview.Items.Count)
                    {
                        i = 0;
                    }
                    listview.SelectedIndex = i;
                }
                UploadSound(listview.SelectedIndex);
            }
        }

        private void prevbutton_Click(object sender, RoutedEventArgs e)
        {
            if (cursound.IsInit)
            {
                if (IsMixing)
                {
                    Random rand = new Random();
                    listview.SelectedIndex = rand.Next(0, listview.Items.Count - 1);
                }
                else
                {
                    int i = listview.SelectedIndex;
                    if (i <= 0)
                    {
                        i = listview.Items.Count;
                    }
                    listview.SelectedIndex = i - 1;
                }
                UploadSound(listview.SelectedIndex);
            }
        }

        private void timeslider_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            if (cursound.IsInit)
                cursound.Time = TimeSpan.FromSeconds(timeslider.Value);
        }

        private void soundnametextbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void menubutton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cyclebutton_Click(object sender, RoutedEventArgs e)
        {
            if (IsLooping)
            {
                IsLooping = false;
                cyclebutton.Background.Opacity = 0;
            }
            else
            {
                IsLooping = true;
                cyclebutton.Background.Opacity = 60;
            }
        }

        private void mixbutton_Click(object sender, RoutedEventArgs e)
        {
            if (IsMixing)
            {
                IsMixing = false;
                mixbutton.Background.Opacity = 0;
            }
            else
            {
                IsMixing = true;
                mixbutton.Background.Opacity = 60;
            }
        }

        private void menucombobox_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Controls.ComboBox b = sender as System.Windows.Controls.ComboBox;
            b.Foreground = Brushes.Aquamarine;
        }

        private void menucombobox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Controls.ComboBox b = sender as System.Windows.Controls.ComboBox;
            string hex = "#FF14252B";
            Color color = (Color)ColorConverter.ConvertFromString(hex);
            b.Foreground = new SolidColorBrush(color);
        }

        private void pathbutton_Click(object sender, RoutedEventArgs e)
        {
            if (cursound.IsInit)
            {
                System.Windows.MessageBox.Show(cursound.Path);
            }
        }

        private void menucombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            menucombobox.SelectedIndex = -1;
        }
    }
}

public class MyItem
{
    public string name { get; set; }

    public TimeSpan dur { get; set; }
}
