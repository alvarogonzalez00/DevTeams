using static System.Console;

public class DeveloperUI
{
    private DeveloperRepository _devRepo;
    private bool isRunningDevUI;
    private DeveloperTeamUI _dtUI;

    public DeveloperUI()
    {
        _devRepo = new DeveloperRepository();
    }

    public void Run()
    {
        RunApplication();
    }

    private void RunApplication()
    {
        isRunningDevUI = true;
        while (isRunningDevUI)
        {
            Clear();
            WriteLine("-- Komodo DevTeams Developer UI --\n" +
                "Please make a selection:\n" +
                "1. Add a developer\n" +
                "2. View all developers\n" +
                "3. View developer by ID\n" +
                "4. Update existing developer\n" +
                "5. Delete existing developer\n" +
                "6. View all developers with a Pluralsight Account\n" +
                "-------------------------------\n" +
                "7. Back To Main Menu\n" +
                "-------------------------------\n" +
                "0. Close Application\n");

            string userInputMenuSelection = ReadLine();
            switch (userInputMenuSelection)
            {
                case "1":
                    AddADeveloper();
                    break;
                case "2":
                    ViewAllDevelopers();
                    break;
                case "3":
                    ViewDeveloperByID();
                    break;
                case "4":
                    UpdateAnExistingDeveloper();
                    break;
                case "5":
                    DeleteAnExistingDeveloper();
                    break;
                case "6":
                    ViewDevsWithPluralsight();
                    break;
                case "7":
                    BackToMainMenu();
                    break;
                case "0":
                    CloseApplication();
                    break;
                default:
                    WriteLine("Invalid Selection");
                    DTUtils.PressAnyKey();
                    break;
            }
        }
    }

    private void CloseApplication()
    {
        isRunningDevUI = false;
        DTUtils.isRunning = false;
        WriteLine("Closing Application");
        DTUtils.PressAnyKey();
    }

    private void BackToMainMenu()
    {
        Clear();
        isRunningDevUI = false;
    }

    private void ViewDevsWithPluralsight()
    {
        Clear();
        WriteLine("-- Devs Without Pluralsight --\n");
        ShowDevsWithOutPs();
        ReadKey();
    }

    private void ShowDevsWithOutPs()
    {
        List<Developer> devsWoPS = _devRepo.DevsWithOutPluralsight();
        if (devsWoPS.Count > 0)
        {
            foreach (var Developer in devsWoPS)
            {
                DisplayDevData(Developer);
            }
        }
        else
        {
            WriteLine("There are no developers without a Pluralsight Membership.");
        }
    }

    private void DeleteAnExistingDeveloper()
    {
        Clear();
        ShowEnlistedDevs();
        WriteLine("--------------\n");
        try
        {
            WriteLine("Select developer by Id.");
            int userInputDevId = int.Parse(ReadLine());
            ValidateDeveloperInDatabase(userInputDevId);
            WriteLine("Do you want to Delete this Developer? y/n?");
            string userInputDeleteDev = ReadLine();
            if (userInputDeleteDev == "Y".ToLower())
            {
                if (_devRepo.DeleteDeveloperData(userInputDevId))
                {
                    WriteLine($" The Developer with the Id: {userInputDevId}, was Successfully Deleted.");
                }
                else
                {
                    WriteLine($"The Developer with the Id: {userInputDevId}, was NOT Deleted.");
                }
            }
        }
        catch
        {
            SomethingWentWrong();
        }

        ReadKey();
    }

    private void UpdateAnExistingDeveloper()
    {
        Clear();
        ShowEnlistedDevs();
        WriteLine("------\n");
        try
        {
            WriteLine("Select developer by Id.");
            int userInputDevId = int.Parse(ReadLine());
            Developer devInDb = GetDeveloperDataFromDb(userInputDevId);
            bool isValidated = ValidateDeveloperInDatabase(devInDb.Id);

            if (isValidated)
            {
                WriteLine("Do you want to Update this Developer? y/n?");
                string userInputDeleteDev = ReadLine();
                if (userInputDeleteDev == "Y".ToLower())
                {
                    Developer updatedDevData = InitialDevCreationSetup();

                    if (_devRepo.UpdateDeveloperData(devInDb.Id, updatedDevData))
                    {
                        WriteLine($" The Developer {updatedDevData.FullName}, was Successfully Updated.");
                    }
                    else
                    {
                        WriteLine($"The Developer {updatedDevData.FullName}, was NOT Updated.");
                    }
                }
                else
                {
                    WriteLine("Returning to Developer Menu.");
                }
            }
        }
        catch
        {
            SomethingWentWrong();
        }
        ReadKey();
    }

    private void ViewDeveloperByID()
    {
        Clear();
        ShowEnlistedDevs();
        WriteLine("----------\n");
        try
        {
            WriteLine("Select developer by Id.");
            int userInputDevId = int.Parse(ReadLine());
            ValidateDeveloperInDatabase(userInputDevId);
        }
        catch
        {
            SomethingWentWrong();
        }
        ReadKey();
    }

    private void SomethingWentWrong()
    {
        WriteLine("Something went wrong.\n" +
                    "Try again\n" +
                    "Returning to Developer Menu.");
    }

    private bool ValidateDeveloperInDatabase(int userInputDevId)
    {
        Developer dev = GetDeveloperDataFromDb(userInputDevId);
        if (dev != null)
        {
            Clear();
            DisplayDevData(dev);
            return true;
        }
        else
        {
            WriteLine($"The Developer with the Id: {userInputDevId} does not exist");
            return false;
        }
    }

    private Developer GetDeveloperDataFromDb(int userInputDevId)
    {
        return _devRepo.GetDeveloper(userInputDevId);
    }

    private void ShowEnlistedDevs()
    {
        Clear();
        WriteLine("-- Developer Listing --");
        List<Developer> devsInDb = _devRepo.GetDevelopers();
        ValidateDeveloperDatabaseData(devsInDb);
    }

    private void ViewAllDevelopers()
    {
        Clear();
        ShowEnlistedDevs();
        ReadKey();
    }

    private void ValidateDeveloperDatabaseData(List<Developer> devsInDb)
    {
        if (devsInDb.Count > 0)
        {
            Clear();
            foreach (var dev in devsInDb)
            {
                DisplayDevData(dev);
            }
        }
        else
        {
            WriteLine("There are no developers in the database.");
        }
    }

    private void DisplayDevData(Developer dev)
    {
        WriteLine(dev);
    }

    private void AddADeveloper()
    {
        Clear();
        try
        {
            Developer dev = InitialDevCreationSetup();
            if (_devRepo.AddDevToDb(dev))
            {
                WriteLine($"Successfully Added {dev.FullName} to the Database!");
            }
            else
            {
                SomethingWentWrong();
            }
        }
        catch
        {

            SomethingWentWrong();
        }
        ReadKey();
    }

    private Developer InitialDevCreationSetup()
    {
        Developer dev = new Developer();

        WriteLine("-- Add Develper Menu --");

        WriteLine("What is the Developers first name?");
        dev.FirstName = ReadLine();

        WriteLine("What is the Developers last name?");
        dev.LastName = ReadLine();

        bool hasMadeSelection = false;

        while (!hasMadeSelection)
        {
            WriteLine("Does this Developer have a Pluralsight Account?\n" +
                "1. yes\n" +
                "2. no\n");

            string userInputPsAcct = ReadLine();

            switch (userInputPsAcct)
            {
                case "1":
                    dev.HasPluralsight = true;
                    hasMadeSelection = true;
                    break;

                case "2":
                    dev.HasPluralsight = false;
                    hasMadeSelection = true;
                    break;

                default:
                    WriteLine("Invalid Selection");
                    DTUtils.PressAnyKey();
                    break;
            }
        }

        return dev;
    }


}
