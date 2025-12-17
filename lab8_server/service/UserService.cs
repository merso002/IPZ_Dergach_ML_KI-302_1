using Microsoft.EntityFrameworkCore;
using lab8_server.api;
using WorkerService1.model;

namespace WorkerService1.service;

public class UserService
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(AppDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<(bool success, string message)> RegisterUser(string username, string email, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Email == email))
        {
            _logger.LogWarning("Реєстрація не вдалася. Користувач {Email} вже існує.", email);
            return (false, "Користувач вже існує.");
        }

        var user = new User 
        { 
            Username = username, 
            Email = email,
            Password = password
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Користувач {Email} успішно зареєстрований.", email);
        return (true, "Користувач успішно зареєстрований.");
    }


    public async Task<(bool success, string message)> LoginUser(string email, string password)
    {
        _logger.LogInformation("Спроба входу для користувача: {Email}", email);

        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);

        if (user == null)
        {
            _logger.LogWarning("Вхід не вдався. Користувача {Email} не знайдено або пароль невірний.", email);
            throw new InvalidOperationException("Користувача не знайдено або пароль невірний.");
        }

        _logger.LogInformation("Користувач {Email} успішно увійшов.", email);
        return (true, "Користувач успішно залогінений.");
    }
}