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
        

        Brush standartcolor;
        Sound cursound;
        public MainWindow()
        {
            InitializeComponent();
            standartcolor = stopbutton.Background;
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
            if (text == "▶")
                cursound.Stop();
            else
                cursound.Play();
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
                foreach (var i in filepaths)
                { 
                    System.Windows.MessageBox.Show(i);
                    playlist.AddSound(new Sound(i));
                }
            }
            AllPlayLists.AddPlayList(playlist);
            cursound = playlist.sounds[0];
            cursound.Play();
            volumeslider.Value = cursound.Volume;
            stopbutton.Content = "▶";
        }

        private void volumeslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            cursound.Volume = (float)volumeslider.Value;
        }
    }
}
