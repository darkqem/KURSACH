using System.Windows;
using System.Windows.Controls;


namespace KURSACH
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            // Создаем экземпляр второго окна
            var quest = new Quest();

            // Отображаем второе окно
            quest.Show();

            // Закрываем текущее окно после открытия второго
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Закрытие всего приложения
        }
    }
}