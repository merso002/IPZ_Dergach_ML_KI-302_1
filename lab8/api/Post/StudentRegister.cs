using System.Text.Json;
using System.Windows;

namespace lab2_8.api.Post
{
    public class StudentRegister
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        // Метод для симуляції реєстрації користувача
        public static async Task<bool> RegisterAsync(string username, string password, string email)
        {
            await Task.Delay(300); // Імітація затримки "запиту до сервера"

            // Імітація запиту
            var registerModel = new
            {
                command = "create_user",
                username = username,
                password = password,
                email = email
            };

            var request = JsonSerializer.Serialize(registerModel);
            Console.WriteLine("Simulated request to server:");
            Console.WriteLine(request);

            // Імітована успішна відповідь сервера
            var jsonResponse = new Dictionary<string, string>
            {
                { "success", "True" },
                { "message", $"Користувача '{username}' успішно зареєстровано з email: {email}." }
            };

            string response = JsonSerializer.Serialize(jsonResponse);
            Console.WriteLine("Simulated server response:");
            Console.WriteLine(response);

            try
            {
                var parsedResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(response);
                if (parsedResponse?["success"] == "True")
                {
                    return true;
                }
                else
                {
                    MessageBox.Show(parsedResponse?["message"]);
                    return false;
                }
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing response: {ex.Message}");
                return false;
            }
        }
    }
}