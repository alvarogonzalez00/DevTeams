
public class DeveloperTeam
{
    public DeveloperTeam()
    {
    }
    public DeveloperTeam(string teamName, List<Developer> developers)
    {
        TeamName = teamName;
        Developers = developers;
    }
    public int Id { get; set; }
    public string TeamName { get; set; }
    public List<Developer> Developers { get; set; } = new List<Developer>();

    public override string ToString()
    {
        var str = $"TeamId: {Id}\n" +
                $"TeamName: {TeamName}\n" +
                $"--------  Team Members -------------\n";
        foreach (Developer dev in Developers)
        {
            str += dev + "\n";
        }

        return str;
    }
}
