using System;
using System.Windows;
using lab8.api.Post;
using lab8.Entity;

namespace lab8.Pages
{
    public partial class AddWorker : Window
    {
        Worker NewWorker;

        public AddWorker()
        {
            InitializeComponent();
            NewWorker = new Worker(0, 0, string.Empty, 0, 0); // Ініціалізація
            DataContext = NewWorker;
        }

        private async void SaveWorkerClick(object sender, RoutedEventArgs e)
        {
            if (!AreFieldsValid())
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.", "Помилка вводу", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var worker = await CreateNewWorker.CreateNewWorkerAsync(NewWorker);
                if (worker)
                {
                    var window = new MainWindow();
                    window.Show();
                    Close();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Будь ласка, введіть коректні числові значення.", "Помилка вводу", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private bool AreFieldsValid()
        {
            // Перевірка, чи всі поля заповнені
            return !string.IsNullOrWhiteSpace(NewWorker.Name) && 
                   NewWorker.PassportId > 0 && 
                   NewWorker.Hours > 0 && 
                   NewWorker.PricePerHour > 0;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }
    }
}