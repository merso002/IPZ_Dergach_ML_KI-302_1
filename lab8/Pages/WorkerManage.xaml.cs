using System.Windows;
using lab8.Entity;

namespace lab8.Pages
{
    public partial class WorkerManage : Window
    {
        public Worker Worker { get; set; }

        public WorkerManage(Worker worker)
        {
            InitializeComponent();
            Worker = worker;
            DataContext = Worker;
        }

        private void EditWorkerClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var editWindow = new EditWorker(Worker);
                editWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка при редагуванні працівника: {ex.Message}");
            }
        }

        private void PaySalaryClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var salary = new PaySalary(Worker);
                salary.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка при виплаті зарплати: {ex.Message}");
            }
        }
        
        private void PayBonusClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var bonus = new PayBonus(Worker);
                bonus.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка при виплаті бонусу: {ex.Message}");
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }
    }
}