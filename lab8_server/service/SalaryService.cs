using Microsoft.EntityFrameworkCore;
using lab8_server.api;
using WorkerService1.model;

namespace WorkerService1.service;

public class SalaryService
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserService> _logger;

    public SalaryService(AppDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<(bool success, string message)> AddSalaryOrBonus(long passportNumber, double amount, string type)
    {
        try
        {
            _logger.LogInformation("Додавання зарплати або премії для працівника з паспортним номером: {passportNumber}", passportNumber);
            
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.PassportId == passportNumber);

            if (employee == null)
            {
                _logger.LogWarning("Працівника з паспортним номером {passportNumber} не знайдено.", passportNumber);
                return (false, "Працівника з таким номером паспорта не знайдено.");
            }
            
            var salary = new Salary
            {
                PassportNumber = passportNumber,
                Amount = amount,
                Type = type,
                CreatedTimeStamp = DateTime.UtcNow 
            };
            
            await _context.Salaries.AddAsync(salary);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Зарплату або премію успішно додано для працівника з паспортним номером: {passportNumber}", passportNumber);
            return (true, "Зарплату або премію успішно додано.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Сталася помилка під час додавання зарплати або премії для працівника з паспортним номером: {passportNumber}", passportNumber);
            return (false, "Сталася помилка під час додавання зарплати або премії.");
        }
    }
}
