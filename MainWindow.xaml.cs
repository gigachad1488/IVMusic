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
            cursound = new Sound();
            standartcolor = stopbutton.Background;
        }

        private void ButtonColorChangeEnter(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            b.Foreground = Brushes.Aquamarine;
        }

        private void ButtonColorChangeLeave(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            string name = b.Name;
            
            string hex = "#FF14252B";
            Color color = (Color)ColorConverter.ConvertFromString(hex);
            b.Foreground = new SolidColorBrush(color);
            
        }

        private void stopbutton_Click(object sender, RoutedEventArgs e)
        {
            stopbutton.Content = (string)stopbutton.Content == "▶" ? "🛑" : "▶";
        }

        private void browsebutton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void volumeslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            cursound.ChangeVolume((int)volumeslider.Value);
        }
    }
}
