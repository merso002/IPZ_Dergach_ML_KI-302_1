using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using lab8.Entity;

namespace lab8.api.Post;

public class PayBonusRequest
{
    private const string ServerAddress = "localhost"; // Адреса сервера
    private const int ServerPort = 5000; // Порт сервера

    public static async Task<bool> PayBonusAsync(Worker worker, int amount)
    {
        var model = new
        {
            command = "add_salary_or_bonus",
            passportNumber = worker.PassportId.ToString(),
            amount = amount.ToString(),
            type = "Bonus"
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

                var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(response);
                if (jsonResponse["success"] == true.ToString())
                {
                    return true;
                }
                else
                {
                    MessageBox.Show(jsonResponse["message"]);
                    return false;
                }
            }
        }
    }
}