
public class DeveloperRepository
{
    private readonly List<Developer> _devDb = new List<Developer>();
    private int _count;

    public DeveloperRepository()
    {
        SeedData();
    }

    public bool AddDevToDb(Developer dev)
    {
        return (dev is null) ? false : AddToDatabase(dev);
    }

    private bool AddToDatabase(Developer dev)
    {
        AssignId(dev);
        _devDb.Add(dev);
        return true;
    }

    private void AssignId(Developer dev)
    {
        _count++;
        dev.Id = _count;
    }

    public List<Developer> GetDevelopers()
    {
        return _devDb;
    }

    public Developer GetDeveloper(int id)
    {
        foreach (Developer dev in _devDb)
        {
            if(dev.Id ==id)
            return dev;
        }
        return null;
    }

    public bool UpdateDeveloperData(int devId, Developer updatedData)
    {
        Developer devInDb = GetDeveloper(devId);

        if (devInDb != null)
        {
            devInDb.FirstName = updatedData.FirstName;
            devInDb.LastName = updatedData.LastName;
            devInDb.HasPluralsight = updatedData.HasPluralsight;
            return true;
        }
        return false;
    }

    public bool DeleteDeveloperData(int devId)
    {
        Developer devInDb = GetDeveloper(devId);
        return _devDb.Remove(devInDb);
    }
    public List<Developer> DevsWithOutPluralsightLINQ()
    {
        return _devDb.Where(dev => dev.HasPluralsight == false).ToList();
    }

    public List<Developer> DevsWithOutPluralsight()
    {
        List<Developer> devsWithoutPs = new List<Developer>();
        foreach (Developer dev in _devDb)
        {
            if (dev.HasPluralsight == false)
            {
                devsWithoutPs.Add(dev);
            }
        }
        return devsWithoutPs;
    }

    private void SeedData()
    {
        var devA = new Developer("Cristiano", "n/a", false);
        var devB = new Developer("Lionel", "n/a", true);
        var devC = new Developer("Kylian", "Mbappe", true);
        var devD = new Developer("Mohamed", "Salah", false);
        var devE = new Developer("Harry", "n/a", false);
        var devF = new Developer("Paul", "n/a", false);
        var devG = new Developer("Eden", "n/a", false);
        var devH = new Developer("Diego", "n/a", false);
        var devI = new Developer("Pele", "n/a", true);

        AddDevToDb(devA);
        AddDevToDb(devB);
        AddDevToDb(devC);
        AddDevToDb(devD);
        AddDevToDb(devE);
        AddDevToDb(devF);
        AddDevToDb(devG);
        AddDevToDb(devH);
        AddDevToDb(devI);
    }
}
