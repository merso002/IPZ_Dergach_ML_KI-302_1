using System.Windows;
using lab2_8.api.Post;
using lab8.Entity;

namespace lab8;
 
public partial class Login : Window
{
    private LoginValues _person;

    public Login()
    {
        InitializeComponent();

        _person = new LoginValues();
        DataContext = _person;
    }

    private async void ButtonClick(object sender, RoutedEventArgs e)
    {
        try
        {
            _person.Password = PasswordBox.Password;

            // Перевірка, чи поля порожні
            if (string.IsNullOrWhiteSpace(_person.Email) || string.IsNullOrWhiteSpace(_person.Password))
            {
                MessageBox.Show("Будь ласка, введіть і email, і пароль.", "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            
            // Використання імітації серверного запиту для логіну
            bool loginSuccessful = await StudentLogin.LoginAsync(_person.Email, _person.Password);
            
            if (loginSuccessful)
            {
                var mainPage = new MainWindow();
                mainPage.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Логін не вдався. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RegisterClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var register = new Register();
            register.Show();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}