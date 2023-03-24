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
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(500);
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
            string text = stopbutton.Content.ToString();
            stopbutton.Content = text == "▶" ? "🛑" : "▶";
            if (cursound.IsInit && cursound != null)
            {
                if (text == "▶")
                    cursound.Stop();
                else
                    cursound.Play();
            }
        }

        private void browsebutton_Click(object sender, RoutedEventArgs e)
        {
            PlayList playlist = new PlayList();
            List<Sound> l = new List<Sound>();
            List<string> filepaths = new List<string>();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                playlist.Title = new DirectoryInfo(dialog.SelectedPath).Name;
                filepaths = Directory.GetFiles(dialog.SelectedPath, "*.mp3").ToList();
                System.Windows.Controls.Image im = new System.Windows.Controls.Image();
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                string[] impath = Directory.GetFiles(dialog.SelectedPath, "*.png");               
                image.UriSource = new Uri(impath[0], UriKind.Absolute);
                image.EndInit();
                playlist.image = image;
                foreach (var i in filepaths)
                { 
                    playlist.AddSound(new Sound(i));
                }
            }
            else
            {
                return;
            }
            AllPlayLists.AddPlayList(playlist);
            ResetPlayLists();
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
            for (int j = 0; j < curplaylist.Count; j++)
            {
                Sound sound = curplaylist.GetSound(j);
                listview.Items.Add(new MyItem {dur = sound.Duration, name = sound.Name});
            }
            
            
        }
        private void playlistslistbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UploadPlayList(playlistslistbox.SelectedIndex);
        }

        private void h_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("робит");
        }

        private void listview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
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
            durlabel.Content = cursound.Duration.TotalSeconds.ToString();
            stopbutton.Content = "▶";
            soundnametextbox.Text = cursound.Name;
            cursound.Play();
            timer.Start();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            timeslider.Value = cursound.Time.Seconds;
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
            int i = listview.SelectedIndex;
            i++;
            if (i >= listview.Items.Count)
            {
                i = 0;
            }
            listview.SelectedIndex = i;
            UploadSound(listview.SelectedIndex);
        }

        private void prevbutton_Click(object sender, RoutedEventArgs e)
        {
            int i = listview.SelectedIndex;
            i--;
            if (i <= 0)
            {
                i = listview.Items.Count;
            }
            listview.SelectedIndex = i;
            UploadSound(listview.SelectedIndex);
        }
        private void timeslider_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void timeslider_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void timeslider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void timeslider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void timeslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void timeslider_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
        }

        private void timeslider_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
 
        }

        private void timeslider_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                cursound.Time = TimeSpan.FromSeconds(timeslider.Value);
                timer.Start();
                
            }
            else if (e.LeftButton == MouseButtonState.Released)
            {
                timer.Stop();
            }
            
        }
    }
}

public class MyItem
{   
    public string name { get; set; }

    public TimeSpan dur { get; set; }
}
