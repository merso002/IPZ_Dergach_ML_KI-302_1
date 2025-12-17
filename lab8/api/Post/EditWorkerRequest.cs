using System.Text.Json;
using System.Windows;
using lab8.Entity;

namespace lab8.api.Post
{
    public class EditWorkerRequest
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        // Метод для симуляції редагування працівника
        public static async Task<bool> EditWorkerRequestAsync(Worker worker)
        {
            await Task.Delay(300); // Імітація мережевої затримки

            // Імітований запит
            var model = new
            {
                command = "update_employee",
                username = worker.Name,
                passportNumber = worker.PassportId.ToString(),
                hours = worker.Hours.ToString(),
                pricePerHours = worker.PricePerHour.ToString()
            };

            var request = JsonSerializer.Serialize(model);
            Console.WriteLine("Simulated request to server:");
            Console.WriteLine(request);

            // Імітація відповіді сервера
            var jsonResponse = new Dictionary<string, string>
            {
                { "success", "True" },
                { "message", $"Дані працівника '{worker.Name}' успішно оновлено." }
            };

            string response = JsonSerializer.Serialize(jsonResponse);
            Console.WriteLine("Simulated server response:");
            Console.WriteLine(response);

            try
            {
                var parsedResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(response);
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