using System.Text.Json;
using System.Windows;
using lab8.Entity;

namespace lab8.api.Post
{
    public class PaySalaryRequest
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        // Метод для симуляції виплати зарплати
        public static async Task<bool> PaySalaryAsync(Worker worker, int amount)
        {
            await Task.Delay(300); // Імітація мережевої затримки

            // Імітація запиту
            var model = new
            {
                command = "add_salary_or_bonus",
                passportNumber = worker.PassportId.ToString(),
                amount = amount.ToString(),
                type = "Salary"
            };

            var request = JsonSerializer.Serialize(model);
            Console.WriteLine("Simulated request to server:");
            Console.WriteLine(request);

            // Імітація відповіді сервера
            var jsonResponse = new Dictionary<string, string>
            {
                { "success", "True" },
                { "message", $"Зарплату у розмірі {amount} грн успішно виплачено працівнику '{worker.Name}'." }
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