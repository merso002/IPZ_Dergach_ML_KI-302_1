namespace lab8.Entity;

public class Worker
{
    public int Id { get; set; }
    public int PassportId { get; set; }
    public string Name { get; set; }
    public int Hours { get; set; }
    public int PricePerHour { get; set; }

    public Worker(int id, int passportId, string name, int hours, int pricePerHour)
    {
        Id = id;
        PassportId = passportId;
        Name = name;
        Hours = hours;
        PricePerHour = pricePerHour;
    }
}