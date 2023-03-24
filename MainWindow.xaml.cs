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

namespace IVMusic
{
    
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Sound cursound = new Sound();
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

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
                Image im = new Image();
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                string[] impath = Directory.GetFiles(dialog.SelectedPath, "*.png");
                System.Windows.MessageBox.Show(impath[0]);
                image.UriSource = new Uri(impath[0], UriKind.Absolute);
                image.EndInit();
                playlist.image = im;
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
            cursound = playlist.GetSound(0);
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
            PlayList list = AllPlayLists.GetPlayList(i);
            playlistnametextbox.Text = list.Title;
            picturepanel.Source = list.image;
            for (int j = 0; j < list.Count; j++)
            {
                Sound sound = list.GetSound(j);
                listview.Items.Add(new MyItem {dur = 50, name = sound.Name});
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
    }
}

public class MyItem
{   
    public string name { get; set; }

    public int dur { get; set; }
}
