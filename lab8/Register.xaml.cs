using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using lab2_8.api.Post;
using lab8.Pages;
using lab8.Entity;

namespace lab8;

public partial class Register : Window
{
    private RegisterValues _person;

    public Register()
    {
        InitializeComponent();

        _person = new RegisterValues();
        DataContext = _person;
    }

    private async void RegisterClick(object sender, RoutedEventArgs e)
    {
        _person.Password = PasswordBox.Password;

        // Перевірка, чи всі поля заповнені
        if (string.IsNullOrWhiteSpace(_person.Name) || string.IsNullOrWhiteSpace(_person.Email) ||
            string.IsNullOrWhiteSpace(_person.Password))
        {
            MessageBox.Show("Будь ласка, заповніть усі поля.", "Помилка", MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }
        
        bool registrationSuccessful = await StudentRegister.RegisterAsync(_person.Name, _person.Password, _person.Email);

        if (registrationSuccessful)
        {
            MessageBox.Show($"Успішна реєстрація:\n\nІм'я: {_person.Name}\nEmail: {_person.Email}", "Успіх");

            // Перехід на головну сторінку
            var mainPage = new MainWindow();
            mainPage.Show();
            Close();
        }
        else
        {
            MessageBox.Show("Реєстрація не вдалася. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void GoBackToLogin(object sender, RoutedEventArgs e)
    {
        var loginPage = new Login();
        loginPage.Show();
        Close();
    }
}