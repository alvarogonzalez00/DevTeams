
public class Developer
{
    public Developer()
    {

    }
    public Developer(string firstName, string lastName, bool hasPluralsight)
    {
        FirstName = firstName;
        LastName = lastName;
        HasPluralsight = hasPluralsight;
    }
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName
    {
        get
        {
            return $"{FirstName} {LastName}";
        }
    }
    public bool HasPluralsight { get; set; }

    public override string ToString()
    {
        string str = $"ID: {Id.ToString()}\n" +
                    $"Name: {FullName}\n" +
                    $"Has Pluralsight: {HasPluralsight}\n" +
                    "-------------------------------\n";
        return str;
    }
}
