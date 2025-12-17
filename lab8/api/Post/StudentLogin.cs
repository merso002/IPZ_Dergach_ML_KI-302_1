using System.Text.Json;
using System.Windows;

namespace lab2_8.api.Post
{
    public class StudentLogin
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        // Метод для симуляції логіну користувача
        public static async Task<bool> LoginAsync(string email, string password)
        {
            await Task.Delay(300); // Імітація мережевої затримки

            // Імітація запиту до сервера
            var registerModel = new
            {
                command = "login_user",
                email = email,
                password = password
            };

            var request = JsonSerializer.Serialize(registerModel);
            Console.WriteLine("Simulated request to server:");
            Console.WriteLine(request);

            // Імітація відповіді сервера
            var jsonResponse = new Dictionary<string, string>
            {
                { "success", "True" },
                { "message", $"Користувач {email} успішно увійшов у систему." }
            };

            string response = JsonSerializer.Serialize(jsonResponse);
            Console.WriteLine("Simulated server response:");
            Console.WriteLine(response);

            try
            {
                var parsedResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(response);
                // Перевірка успішності
                if (parsedResponse?["success"] == true.ToString())

                {
                    return true;
                }

                MessageBox.Show(parsedResponse?["message"]);
                return false;
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing response: {ex.Message}");
                return false;
            }
        }
    }
}