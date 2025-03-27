
using System.ComponentModel.Design;
using System.Globalization;
using Spectre.Console;

namespace Running.Tracker
{
    internal class UserInput
    {
        RunningController runningController = new();
        internal void MainMenu()
        {
            bool closeApp = false;
            while (closeApp == false)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("MAIN MENU\n" +
                    "Select Choice Below")
                    .AddChoices(new[]
                    {
                    "Close Application",
                    "View All Records",
                    "Add Record",
                    "Delete Record",
                    "Update Record",
                    }));

                switch (choice)
                {
                    case "Close Application":
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "View All Records":
                        runningController.Get();
                        break;
                    case "Add Record":
                        ProcessAdd();
                        break;
                    case "Delete Record":
                        ProcessDelete();
                        break;
                    case "Update Record":
                        ProcessUpdate();
                        break;
                }
            }
        }

        private void ProcessUpdate()
        {
            runningController.Get();

            var records = runningController.GetAll();

            if (!records.Any())
            {
                Console.WriteLine("\nNo records found");
                return;
            }

            var choices = records.Select(x => $"{x.Id}").ToList();
            choices.Add("Return to Main Menu");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose the ID of the record you want to update.")
                .AddChoices(choices)
                );

            if (choice == "Return to Main Menu")
            {
                MainMenu();
                return;
            }

            int id = int.Parse(choice);
            
            var record = runningController.GetById(id);
            if (record == null)
            {
                Console.WriteLine("\nNo record found");
                return;
            }

            bool doneUpdating = false;
            while (doneUpdating == false)
            {
                var choice2 = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Choose what to update")
                    .AddChoices(new[]
                    {
                    "Update Date",
                    "Update Duration",
                    "Update Miles",
                    "Save Updates",
                    "Return to Main Menu"
                    }));

                switch (choice2)
                {
                    case "Update Date":
                        string date = GetDateInput();
                        record.Date = date;
                        break;
                    case "Update Duration":
                        string duration = GetDurationInput();
                        record.Duration = duration;
                        break;
                    case "Update Miles":
                        double miles = GetMilesInput();
                        record.Miles = miles;
                        break;
                    case "Save Updates":
                        runningController.Update(record);
                        doneUpdating = true;
                        break;
                    case "Return to Main Menu":
                        MainMenu();
                        return;
                }
            }
        }

        private void ProcessDelete()
        {
            runningController.Get();

            Console.Write("\nEnter the ID of the record you want to delete OR 0 to return to Main Menu: ");

            string? deleteInput = Console.ReadLine();

            if (deleteInput == "0")
            {
                MainMenu();
            }

            int id;
            while (!int.TryParse(deleteInput, out id))
            {
                Console.WriteLine($"deleteInput: {deleteInput}");
                Console.Write("\nNot a valid number. Please try again: ");
                deleteInput = Console.ReadLine();
            }

            var record = runningController.GetById(id);

            if (record == null)
            {
                Console.WriteLine($"\nNo record found with ID {id}");
                ProcessDelete();
            }
            else
            {
                runningController.Delete(id);
            }
        }

        private void ProcessAdd()
        {
            string date = GetDateInput();
            string duration = GetDurationInput();
            double miles = GetMilesInput();

            //Console.WriteLine($"Date: {date}, Duration: {duration}, Miles: {miles}");
            RunningSession run = new();

            run.Date = date;
            run.Duration = duration;
            run.Miles = miles;

            runningController.Post(run);
        }

        private double GetMilesInput()
        {
            Console.WriteLine("\nPlease enter the number of miles ran OR 0 to return to Main Menu: ");
            string? milesInput = Console.ReadLine();
            double miles;

            if (milesInput == "0")
            {
                MainMenu();
            }

            while (!Double.TryParse(milesInput, out miles))
            {
                Console.WriteLine("\nInvalid number. Please enter a valid number.\n");
                milesInput = Console.ReadLine();
            }
            return miles;
        }

        private string GetDurationInput()
        {
            Console.WriteLine("\nPlease enter the duration of your run. (Format hh:mm) OR 0 to return to Main Menu: ");
            string? durationInput = Console.ReadLine();

            if (durationInput == "0")
            {
                MainMenu();
            }

            while (!TimeSpan.TryParseExact(durationInput, "hh\\:mm", CultureInfo.InvariantCulture, out _))
            {
                Console.WriteLine("\nNot a vaild time. Use the format 'hh:mm'");
                durationInput = Console.ReadLine();
            }
            return durationInput;
        }

        private string GetDateInput()
        {
            Console.WriteLine("\nPlease enter the date: (Format: mm-dd-yy) OR 0 to return to Main Menu: \n");

            string? dateInput = Console.ReadLine();

            if (dateInput == "0")
            {
                MainMenu(); 
            }

            while (!DateTime.TryParseExact(dateInput, "MM-dd-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\nInvalid date. Use the format 'mm-dd-yy'");
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }
    }
}