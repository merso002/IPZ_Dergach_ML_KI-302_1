using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using WorkerService1.service;

public class TCPServer
{
    private readonly UserService _userService;
    private readonly EmployeeService _employeeService;
    private readonly SalaryService _salaryService;
    private readonly ILogger _logger;

    public TCPServer(
        UserService userService,
        EmployeeService employeeService,
        SalaryService salaryService,
        ILogger<TCPServer> logger)
    {
        _userService = userService;
        _employeeService = employeeService;
        _salaryService = salaryService;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        _logger.LogInformation("TCP Server started on port 5000.");

        while (!cancellationToken.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        _logger.LogInformation("Client connected.");
        using (var stream = client.GetStream())
        {
            var buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(request);
            var command = data?["command"];
            
            Console.WriteLine(request);
            
            try
            {
                if (command == "create_user")
                {
                    var username = data?["username"];
                    var email = data?["email"];
                    var password = data?["password"];

                    var (success, message) = await _userService.RegisterUser(username, email, password);

                    var response = new
                    {
                        success = success.ToString(),
                        message = message
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "login_user")
                {
                    var email = data?["email"];
                    var password = data?["password"];

                    var (success, message) = await _userService.LoginUser(email, password);
                    var response = new
                    {
                        success = success.ToString(),
                        message = message
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "create_employee")
                {
                    var username = data?["username"];
                    var passportNumber = long.Parse(data?["passportNumber"]);
                    var hours = Convert.ToDouble(data?["hours"]);
                    var pricePerHours = Convert.ToDouble(data?["pricePerHours"]);

                    var (success, message) =
                        await _employeeService.CreateEmployee(username, passportNumber, hours, pricePerHours);
                    var response = new
                    {
                        success = success.ToString(),
                        message = message
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "get_employees")
                {
                    var (success, message, employees) = await _employeeService.GetAllEmployees();

                    var response = new
                    {
                        success = success,
                        message = employees
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    Console.WriteLine(jsonResponse);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "update_employee")
                {
                    var username = data?["username"];
                    var passportNumber = long.Parse(data["passportNumber"]);
                    var hours = Convert.ToDouble(data?["hours"]);
                    var pricePerHours = Convert.ToDouble(data?["pricePerHours"]);

                    var (success, message) =
                        await _employeeService.UpdateEmployee(username, passportNumber, hours, pricePerHours);
                    var response = new
                    {
                        success = success.ToString(),
                        message = message
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "add_salary_or_bonus")
                {
                    var passportNumber = long.Parse(data?["passportNumber"]);
                    var amount = Convert.ToDouble(data?["amount"]);
                    var type = data?["type"];

                    var (success, message) = await _salaryService.AddSalaryOrBonus(passportNumber, amount, type);
                    var response = new
                    {
                        success = success.ToString(),
                        message = message
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else
                {
                    var response = new
                    {
                        success = false.ToString(),
                        message = "Invalid command."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    success = false.ToString(),
                    message = $"Error: {ex.Message}"
                };
                var jsonResponse = JsonSerializer.Serialize(response);
                await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
            }
        }

        client.Close();
    }
}