using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using lab8.Entity;

namespace lab8.api.Get
{
    public class GetWorkers
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        // Метод для виконання запиту на отримання працівників
        public static async Task<ObservableCollection<Worker>> GetWorkersAsync()
        {
            var model = new
            {
                command = "get_employees"
            };

            var request = JsonSerializer.Serialize(model);
            var bytesToSend = Encoding.UTF8.GetBytes(request);

            using (var client = new TcpClient(ServerAddress, ServerPort))
            {
                using (var stream = client.GetStream())
                {
                    await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);

                    var buffer = new byte[4096];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    Console.WriteLine(response);

                    try
                    {
                        var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                        if (responseObject?.message != null)
                        {
                            return new ObservableCollection<Worker>(responseObject.message);
                        }
                        else
                        {
                            return new ObservableCollection<Worker>();
                        }
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show($"Error parsing response: {ex.Message}");
                        return new ObservableCollection<Worker>();
                    }
                }
            }
        }

        private class ResponseWrapper
        {
            public List<Worker> message { get; set; }
        }
    }
}
