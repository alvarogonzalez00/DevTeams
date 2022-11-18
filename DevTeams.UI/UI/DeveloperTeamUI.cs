using static System.Console;

public class DeveloperTeamUI
{
    private readonly DeveloperTeamRepository _dTeamRepo;
    private readonly DeveloperRepository _devRepo;

    private bool isRunningDevTeamUI;


    public DeveloperTeamUI(DeveloperRepository devRepo)
    {
        _devRepo = devRepo;
        _dTeamRepo = new DeveloperTeamRepository(_devRepo);
    }

    public void Run()
    {
        RunApplication();
    }

    private void RunApplication()
    {
        isRunningDevTeamUI = true;
        while (isRunningDevTeamUI)
        {
            WriteLine("-- DeveloperTeams UI --\n" +
                  "Please make a selection:\n" +
                  "1. Add a developer team\n" +
                  "2. View all developer teams\n" +
                  "3. View developer team by ID\n" +
                  "4. Update existing developer team\n" +
                  "5. Delete existing developer team\n" +
                  "6. Add multiple developers to a team.\n" +
                  "---------------------\n" +
                  "7. Open main menu\n" +
                  "---------------------\n" +
                  "0. Close application\n");

            string userInputMenuSelection = ReadLine();

            switch (userInputMenuSelection)
            {
                case "1":
                    AddADeveloperTeam();
                    break;
                case "2":
                    ViewAllDeveloperTeams();
                    break;
                case "3":
                    ViewDeveloperTeamByID();
                    break;
                case "4":
                    UpdateAnExistingDeveloperTeam();
                    break;
                case "5":
                    DeleteAnExistingDeveloperTeam();
                    break;
                case "6":
                    AddMultiDevsToATeam();
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
        isRunningDevTeamUI = false;
        DTUtils.isRunning = false;
        WriteLine("Closing Application");
        DTUtils.PressAnyKey();
    }

    private void BackToMainMenu()
    {
        Clear();
        isRunningDevTeamUI = false;
    }

    private void AddMultiDevsToATeam()
    {
        Clear();
        WriteLine("-- Developer Team Listing --");
        GetDevTeamData();
        List<DeveloperTeam> dTeam = _dTeamRepo.GetDeveloperTeams();
        if (dTeam.Count() > 0)
        {
            WriteLine("Please Select a DevTeam by Id for Multiple Developer Addition.");
            int userInputDevTeamId = Convert.ToInt32(ReadLine());
            DeveloperTeam team = _dTeamRepo.GetDeveloperTeam(userInputDevTeamId);


            List<Developer> auxDevInDb = _devRepo.GetDevelopers();

            List<Developer> devsToAdd = new List<Developer>();

            if (team != null)
            {
                bool hasFilledPositions = false;
                while (!hasFilledPositions)
                {
                    if (auxDevInDb.Count() > 0)
                    {
                        DisplayDevelopersInDB(auxDevInDb);
                        WriteLine("Do you want to add a Developer y/n?");
                        var userInputAnyDevs = ReadLine();
                        if (userInputAnyDevs == "Y".ToLower())
                        {
                            WriteLine("Please Choose Dev by Id:");
                            int userInputDevId = int.Parse(ReadLine());
                            Developer dev = _devRepo.GetDeveloper(userInputDevId);
                            if (dev != null)
                            {
                                devsToAdd.Add(dev);
                                auxDevInDb.Remove(dev);
                            }
                            else
                            {
                                WriteLine($"The Dev with the Id: {userInputDevId} doesn't Exist.");
                                WriteLine("Press any key to continue.");
                                ReadKey();
                            }
                        }
                        else
                        {
                            hasFilledPositions = true;
                        }
                    }
                    else
                    {
                        WriteLine("There are no Developers in the Database.");
                        ReadKey();
                        break;
                    }
                }

                if (_dTeamRepo.AddMultiDevsToTeam(team.Id, devsToAdd))
                {
                    WriteLine("Success");
                }
                else
                {
                    WriteLine("Failure");
                }
            }


        }
        else
        {
            WriteLine("There aren't any available Developer Teams to Delete.");
        }
        ReadKey();
    }

    private DeveloperTeam InitializeDTeamCreation()
    {
        try
        {
            DeveloperTeam team = new DeveloperTeam();
            WriteLine("Please enter the DevTeam Name:");
            team.TeamName = ReadLine();

            bool hasFilledPositions = false;

            List<Developer> auxDevelopers = _devRepo.GetDevelopers();

            while (!hasFilledPositions)
            {
                WriteLine("Does this team have any Developers? y/n");
                string userInputAnyDevs = ReadLine();
                if (userInputAnyDevs == "Y".ToLower())
                {
                    if (auxDevelopers.Count() > 0)
                    {
                        DisplayDevelopersInDB(auxDevelopers);
                        WriteLine("Select the Dev you want on this team by DevId.");
                        var userInputDevId = int.Parse(ReadLine());
                        Developer selectedDev = _devRepo.GetDeveloper(userInputDevId);
                        if (selectedDev != null)
                        {
                            team.Developers.Add(selectedDev);
                            auxDevelopers.Remove(selectedDev);
                        }
                        else
                        {
                            WriteLine($"Sorry the Dev with the Id: {userInputDevId} does not exist.");
                        }
                    }
                    else
                    {
                        WriteLine("There are no Developers in the database.");
                        ReadKey();
                        break;
                    }
                }
                else
                {
                    hasFilledPositions = true;
                }
            }
            return team;
        }
        catch
        {
            SomeThingWentWrong();
        }
        return null;
    }

    private void DisplayDevelopersInDB(List<Developer> auxDevelopers)
    {
        foreach (var dev in auxDevelopers)
        {
            WriteLine(dev);
        }
    }

    private void SomeThingWentWrong()
    {
        WriteLine("Something went wrong.\n" +
                       "Try again\n" +
                       "Returning to Developer Menu.");
    }

    private void DeleteAnExistingDeveloperTeam()
    {
        Clear();
        WriteLine("-- Developer Team Listing --");
        GetDevTeamData();
        try
        {
            List<DeveloperTeam> dTeam = _dTeamRepo.GetDeveloperTeams();
            if (dTeam.Count() > 0)
            {
                WriteLine("Please select DevTeam by Id to delete.");
                int userInputDevTeamId = int.Parse(ReadLine());
                var team = _dTeamRepo.GetDeveloperTeam(userInputDevTeamId);
                if (team != null)
                {
                    if (_dTeamRepo.DeleteDeveloperTeam(team.Id))
                    {
                        WriteLine("Success");
                    }
                    else
                    {
                        WriteLine("Fail");
                    }
                }
                else
                {
                    WriteLine("There are no available Developer Teams to delete at this time.");
                }
            }
        }
        catch
        {
            SomeThingWentWrong();
        }
        ReadKey();
    }

    private void UpdateAnExistingDeveloperTeam()
    {
        Clear();
        WriteLine("-- Developer Team Listing --");
        GetDevTeamData();
        List<DeveloperTeam> dTeam = _dTeamRepo.GetDeveloperTeams();
        if (dTeam.Count() > 0)
        {
            WriteLine("Select a DevTeam by Id:");
            int userInputDevTeamId = int.Parse(ReadLine());

            DeveloperTeam teamInRepo = _dTeamRepo.GetDeveloperTeam(userInputDevTeamId);

            if (teamInRepo != null)
            {
                DeveloperTeam updateTeamData = InitializeDTeamCreation();

                if (_dTeamRepo.UpdateDevTeam(teamInRepo.Id, updateTeamData))
                {
                    WriteLine("Update Successful!");
                }
                else
                {
                    WriteLine("Update Failed.");
                }
            }
            else
            {
                WriteLine($"The DevTeam with the Id: {userInputDevTeamId} does not exist.");
            }
        }

        ReadKey();
    }

    private void ViewDeveloperTeamByID()
    {
        Clear();
        WriteLine("-- Developer Team Listing --");
        GetDevTeamData();
        List<DeveloperTeam> devTeam = _dTeamRepo.GetDeveloperTeams();
        if (devTeam.Count() > 0)
        {
            WriteLine("Select DevTeam by Id:");
            int userInputDevId = int.Parse(ReadLine());
            ValidateDevTeamData(userInputDevId);
        }

        ReadKey();
    }

    private void ValidateDevTeamData(int userInputDevId)
    {
        var team = _dTeamRepo.GetDeveloperTeam(userInputDevId);
        if (team != null)
        {
            DisplayDeveloperTeamData(team);
        }
        else
        {
            WriteLine($"Sorry the DevTeam with the Id: {userInputDevId} does not exist.");
        }
    }

    private void ViewAllDeveloperTeams()
    {
        Clear();
        WriteLine("-- Dev Team Listing --");
        GetDevTeamData();
        ReadKey();
    }

    private void GetDevTeamData()
    {
        foreach (DeveloperTeam team in _dTeamRepo.GetDeveloperTeams())
        {
            DisplayDeveloperTeamData(team);
        }

    }

    private void DisplayDeveloperTeamData(DeveloperTeam team)
    {
        WriteLine(team);
    }

    private void AddADeveloperTeam()
    {
        Clear();
        DeveloperTeam dTeam = InitializeDTeamCreation();
        if (_dTeamRepo.AddTeamToDb(dTeam))
        {
            WriteLine($"Team: {dTeam.TeamName} was Added.");
        }
        else
        {
            WriteLine($"Team: {dTeam.TeamName} was not added.");
        }
        ReadKey();
    }
}
