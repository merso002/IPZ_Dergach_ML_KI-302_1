using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace lab2_8.api.Post;

public class StudentLogin
{
    private const string ServerAddress = "localhost"; // Адреса сервера
    private const int ServerPort = 5000; // Порт сервера

    // Метод для виконання запиту на логін
    public static async Task<bool> LoginAsync(string email, string password)
    {
        var registerModel = new
        {
            command = "login_user",
            email = email,
            password = password,
        };

        var request = JsonSerializer.Serialize(registerModel);
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

                MessageBox.Show(jsonResponse["message"]);
                return false;
            }
        }
    }
}