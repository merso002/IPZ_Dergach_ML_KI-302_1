using Microsoft.EntityFrameworkCore;
using lab8_server.api;
using WorkerService1.model;

namespace WorkerService1.service;

public class EmployeeService
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserService> _logger;

    public EmployeeService(AppDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<(bool success, string message)> CreateEmployee(string username, long passportNumber, double hours, double pricePerHours)
    {
        try
        {
            _logger.LogInformation("Створення нового працівника з паспортом: {passportNumber}", passportNumber);
            
            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.PassportId == passportNumber);

            if (existingEmployee != null)
            {
                _logger.LogWarning("Працівник з паспортом {passportNumber} вже існує.", passportNumber);
                return (false, "Працівник з таким номером паспорта вже існує.");
            }
            
            var newEmployee = new Employee
            {
                Name = username,
                PassportId = passportNumber,
                Hours = hours,
                PricePerHour = pricePerHours
            };
            
            await _context.Employees.AddAsync(newEmployee);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Працівника успішно створено з паспортом: {passportNumber}", passportNumber);
            return (true, "Працівника успішно створено.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Сталася помилка під час створення працівника з паспортом: {passportNumber}", passportNumber);
            return (false, "Сталася помилка під час створення працівника.");
        }
    }
    
    public async Task<(bool success, string message, List<Employee> employees)> GetAllEmployees()
    {
        try
        {
            _logger.LogInformation("Отримання списку всіх працівників.");
            var employees = await _context.Employees.ToListAsync();

            if (employees == null || employees.Count == 0)
            {
                _logger.LogWarning("Список працівників порожній.");
                return (false, "Працівників не знайдено.", new List<Employee>());
            }

            _logger.LogInformation("Успішне отримання списку працівників. Кількість: {count}", employees.Count);
            return (true, "Працівників успішно отримано.", employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Сталася помилка під час отримання списку працівників.");
            return (false, "Сталася помилка під час отримання працівників.", new List<Employee>());
        }
    }
    
    public async Task<(bool success, string message)> UpdateEmployee(string? username, long passportNumber, double? hours, double? pricePerHours)
{
    try
    {
        _logger.LogInformation("Оновлення працівника з паспортним номером: {passportNumber}", passportNumber);
        
        var existingEmployee = await _context.Employees
            .FirstOrDefaultAsync(e => e.PassportId == passportNumber);

        if (existingEmployee == null)
        {
            _logger.LogWarning("Працівника з паспортним номером {passportNumber} не знайдено.", passportNumber);
            return (false, "Працівника з таким номером паспорта не знайдено.");
        }
        
        if (!string.IsNullOrEmpty(username))
        {
            existingEmployee.Name = username;
        }
        else
        {
            return (false, "Ім'я не може бути порожнім");
        }

        if (hours.HasValue)
        {
            existingEmployee.Hours = hours.Value;
        }
        else
        {
            return (false, "Кількість годин не може бути порожньою");
        }

        if (pricePerHours.HasValue)
        {
            existingEmployee.PricePerHour = pricePerHours.Value;
        }
        else
        {
            return (false, "Кількість оплати за годину не може бути порожньою");
        }
        
        _context.Employees.Update(existingEmployee);
        await _context.SaveChangesAsync();

        // Перевірка оновлених даних
        var updatedEmployee = await _context.Employees
            .FirstOrDefaultAsync(e => e.PassportId == passportNumber);

        bool isUpdatedSuccessfully = !(!string.IsNullOrEmpty(username) && updatedEmployee.Name != username);

        if (hours.HasValue && updatedEmployee.Hours != hours.Value)
        {
            isUpdatedSuccessfully = false;
        }

        if (pricePerHours.HasValue && updatedEmployee.PricePerHour != pricePerHours.Value)
        {
            isUpdatedSuccessfully = false;
        }

        if (isUpdatedSuccessfully)
        {
            _logger.LogInformation("Працівник з паспортним номером {passportNumber} успішно оновлений.", passportNumber);
            return (true, "Працівника успішно оновлено.");
        }
        else
        {
            _logger.LogWarning("Працівника з паспортним номером {passportNumber} не вдалося оновити.", passportNumber);
            return (false, "Дані працівника не були оновлені.");
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Сталася помилка під час оновлення працівника з паспортом: {passportNumber}", passportNumber);
        return (false, "Сталася помилка під час оновлення працівника.");
    }
}

}
