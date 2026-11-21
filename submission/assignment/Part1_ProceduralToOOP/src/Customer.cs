internal sealed class Customer
{
    public Customer(int id, string name, string email, string city, bool isVip)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentNullException();
        }
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException();
        }
        Id = id;
        Name = name;
        Email = email;
        City = city;
        this.IsVip = isVip;
    }

    public int Id { get; init; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string City { get; private set; }
    public bool IsVip { get; private set; }

    public override string ToString()
    {
        return $"# {this.Id} {this.Name} <{this.Email}> {this.City} {this.IsVip} \n ";
    }
}