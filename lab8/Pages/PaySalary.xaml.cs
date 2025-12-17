using System.Windows;
using lab8.api.Post;
using lab8.Entity;

namespace lab8.Pages;

public partial class PaySalary : Window
{
    public Salary Salary { get; set; }
    public Worker Worker { get; set; }

    public PaySalary(Worker worker)
    {
        InitializeComponent();
        Worker = worker;
        Salary = new Salary(Worker.PricePerHour * Worker.Hours);
        DataContext = Salary;
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        var window = new MainWindow();
        window.Show();
        Close();
    }

    private async void PayClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var req = await PaySalaryRequest.PaySalaryAsync(Worker, Salary.Amount);
            if (req)
            {
                MessageBox.Show("Pay salary successful");
            }
            else
            {
                MessageBox.Show("Pay salary failed");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка при виплаті зарплати: {ex.Message}");
        }
        finally
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }
    }
}