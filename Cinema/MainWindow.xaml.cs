using System.Windows;

namespace Cinema
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(new Cinema.Pages.HomePage());
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Cinema.Pages.HomePage());
        }
    }
}
