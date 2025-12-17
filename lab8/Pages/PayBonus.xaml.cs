using System.Windows;
using lab8.api.Post;
using lab8.Entity;

namespace lab8.Pages;

public partial class PayBonus : Window
{
    public Bonus Bonus { get; set; }
    public Worker Worker { get; set; }
    
    public PayBonus(Worker worker)
    {
        InitializeComponent();
        Worker = worker;
        Bonus = new Bonus(10 * Worker.PricePerHour);
        DataContext = Bonus;
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
            var req = await PayBonusRequest.PayBonusAsync(Worker, Bonus.Amount);
            if (req)
            {
                MessageBox.Show("Pay bonus successful");
            }
            else
            {
                MessageBox.Show("Pay bonus failed");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка: {ex.Message}");
        }
        finally
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }
    }
}