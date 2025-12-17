using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;
using lab8.api.Get;
using lab8.Entity;
using lab8.Pages;

namespace lab8;

public partial class MainWindow : Window
{
    public ObservableCollection<Worker> Workers { get; set; }
    private Button? currentSelectedButton = null;
    private Worker? selectedWorker = null;

    public MainWindow()
    {
        InitializeComponent();
        Workers = new ObservableCollection<Worker>();
        GetWorkersFromServer();
        DataContext = this;
    }


    private async void GetWorkersFromServer()
    {
        try
        {
            var workers = await GetWorkers.GetWorkersAsync();
            Workers.Clear();
            foreach (var el in workers)
            {
                Workers.Add(el);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error fetching sessions from server: {ex.Message}, {ex.StackTrace}");
            Workers = new ObservableCollection<Worker>();
        }
    }



    private void SelectWorker(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Worker worker)
        {
            if (currentSelectedButton != null)
            {
                currentSelectedButton.Background = Brushes.Gray;
            }

            if (button != currentSelectedButton)
            {
                button.Background = Brushes.LightGreen;
                currentSelectedButton = button;
                selectedWorker = worker;
            }
            else
            {
                currentSelectedButton = null;
                selectedWorker = null;
            }
        }
    }

    private void DetailedInfoClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (selectedWorker != null)
            {
                var worker = new WorkerManage(selectedWorker);
                worker.Show();
                Close();
            }
            else
            {
                throw new InvalidOperationException("Працівник не вибраний");
            }
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show("Будь ласка, виберіть працівника спершу.",
                "Попередження",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася непередбачена помилка: {ex.Message}",
                "Помилка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void AddWorkerClick(object sender, RoutedEventArgs e)
    {
        var addWorkerWindow = new AddWorker();
        addWorkerWindow.Show();
        Close();
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        Close();
        var Login = new Login();
        Login.Show();
    }
}