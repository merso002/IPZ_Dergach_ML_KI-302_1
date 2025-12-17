using WorkerService1;
using WorkerService1.service;

namespace lab8_server;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();
            var employeeService = scope.ServiceProvider.GetRequiredService<EmployeeService>();
            var salaryService = scope.ServiceProvider.GetRequiredService<SalaryService>();
            var tcpServer = scope.ServiceProvider.GetRequiredService<TCPServer>();
            await tcpServer.StartAsync(stoppingToken);
        }
    }

}