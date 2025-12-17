using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using lab8.Entity;

namespace lab8.api.Post;

public class EditWorkerRequest
{
    private const string ServerAddress = "localhost"; // Адреса сервера
    private const int ServerPort = 5000; // Порт сервера

    // Метод для виконання запиту на реєстрацію
    public static async Task<bool> EditWorkerRequestAsync(Worker worker)
    {
        var model = new
        {
            command = "update_employee",
            username = worker.Name,
            passportNumber = worker.PassportId.ToString(),
            hours = worker.Hours.ToString(),
            pricePerHours = worker.PricePerHour.ToString()
        };

        var request = JsonSerializer.Serialize(model);

        var bytesToSend = Encoding.UTF8.GetBytes(request);

        using (var client = new TcpClient(ServerAddress, ServerPort))
        {
            using (var stream = client.GetStream())
            {
                await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);

                var buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine(response);

                var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(response);
                if (jsonResponse["success"] == true.ToString())
                {
                    return true;
                }

                MessageBox.Show(jsonResponse["message"]);
                return false;
            }
        }
    }
}