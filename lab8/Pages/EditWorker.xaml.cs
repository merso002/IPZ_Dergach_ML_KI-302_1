using System.Windows;
using lab8.api.Post;
using lab8.Entity;

namespace lab8.Pages;

public partial class EditWorker : Window
{
    Worker EditedWorker;

    public EditWorker(Worker worker)
    {
        InitializeComponent();
        EditedWorker = worker;
        DataContext = EditedWorker;
    }

    private async void SaveWorkerClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var worker = await EditWorkerRequest.EditWorkerRequestAsync(EditedWorker);
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

    private void Close(object sender, RoutedEventArgs e)
    {
        var window = new MainWindow();
        window.Show();
        Close();
    }
}