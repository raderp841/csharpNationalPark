using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.DAL;
using DependencyInjection.Lecture;
using System.Configuration;

namespace Capstone
{
    public class CampgroundsCLI
    {
        String[] months = { "", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        const string Command_AllParks = "1";
        const string Command_AllCampgrounds = "2";
        const string Command_CheckAvailability = "3";
        const string Command_Quit = "q";
        readonly string DatabaseConnection = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        public void RunCLI()
        {
            while (true)
            {
                PrintHeader();
                PrintMenu();
                string command = Console.ReadLine();

                Console.Clear();

                switch (command.ToLower())
                {
                    case Command_AllParks:
                        GetAllParks();
                        break;

                    case Command_AllCampgrounds:
                        GetAllCampgrounds();
                        break;


                    case Command_CheckAvailability:
                        CheckAvailability();
                        break;


                    case Command_Quit:
                        Console.WriteLine("Thank you for using the Campground Reservation Site");
                        return;
                }

            }
        }

        private string GetUtilityMessage(bool hasUtilities)
        {
            if (hasUtilities)
            {
                return "Site has utilities.";
            }
            else
            {
                return "Site does not have utilities.";
            }
        }

        private string GetAccessibilityMessage(bool isAccessible)
        {
            if (isAccessible)
            {
                return "Site is handicap accessible.";
            }
            else
            {
                return "Site is not handicap accessible.";
            }
        }

        private void PrintSites(List<Site> site)
        {
            foreach (Site s in site)
            {
                string accessible = GetAccessibilityMessage(s.Accessible);
                string utilities = GetUtilityMessage(s.Utilities);

                Console.WriteLine($"({s.Site_id})  is available!");
                Console.WriteLine($"Max occupancy of {s.Max_occupancy} people.");
                Console.WriteLine(accessible);
                Console.WriteLine(utilities);
                Console.WriteLine($"The max RV length for this site is {s.Max_rv_length} feet.");
                Console.WriteLine();

            }
        }

        private void HelpMakeReservation(int campgroundId, DateTime startDate, DateTime endDate)
        {
            CampgroundDAL cdal = new CampgroundDAL(DatabaseConnection);
            Campground reservedCampground = cdal.GetCampground(campgroundId);

            SiteDAL sdal = new SiteDAL(DatabaseConnection);

            string name = CLIHelper.GetString("Enter Name");
            int siteID = CLIHelper.GetInteger("Please select a Site");
            int reservationId = sdal.ReserveCampsite(campgroundId, siteID, startDate, endDate, name);

            int lengthOfStay = (endDate - startDate).Days;

            string cost = (reservedCampground.DailyFee * lengthOfStay).ToString("C"); //sdal.GetPrice(lengthOfStay, campgroundId);
            Console.WriteLine("You have successfully booked your Reservation");
            Console.WriteLine($"Your reservation number is {reservationId}.");
            Console.WriteLine($"Your bill is {cost}");
            Console.ReadLine();

        }


        private void CheckAvailability()
        {
            GetAllCampgrounds();

            int campgroundId = CLIHelper.GetInteger("Please select which Campground Id you would like to check the availability of : ");
            DateTime startDate = CLIHelper.GetDateTime("When would you like start your reservation?");
            DateTime endDate = CLIHelper.GetDateTime("When would you like end your reservation?");

            CampgroundDAL cdal = new CampgroundDAL(DatabaseConnection);
            Campground reservedCampground = cdal.GetCampground(campgroundId);

            if (startDate.Month < reservedCampground.OpenFromMM)
            {
                Console.WriteLine("The campground is not open yet.");
                return;
            }
            if (endDate.Month > reservedCampground.OpenToMM)
            {
                Console.WriteLine("The campground is closed for the year.");
                return;
            }


            Console.WriteLine();

            SiteDAL sdal = new SiteDAL(DatabaseConnection);
            List<Site> site = sdal.GetAvailableCampsites(campgroundId, startDate, endDate);

            PrintSites(site);

            Console.ReadLine();
            string input = CLIHelper.GetString(" Would you Like to confirm your reservation?").ToLower();
            if (input == "yes" || input == "y")
            {
                HelpMakeReservation(campgroundId, startDate, endDate);
            }
            else
            {
                Console.WriteLine("Returning to Main Menu");
                Console.ReadLine();
            }
        }


        private void GetAllCampgrounds()
        {
            ParksDAL pdal = new ParksDAL(DatabaseConnection);
            List<Park> parks = pdal.GetAllParks();

            if (parks.Count > 0)
            {
                foreach (Park park in parks)
                {
                    Console.WriteLine("(" + park.Park_id + ") " + park.Name);
                    Console.WriteLine();
                }

                CampgroundDAL cdal = new CampgroundDAL(DatabaseConnection);
                int parkId = CLIHelper.GetInteger("Please select a Park Id : ");
                Console.WriteLine();
                List<Campground> campground = cdal.GetAllCampgrounds(parkId);

                foreach (Campground camp in campground)
                {

                    Console.WriteLine("(" + camp.CampgroundId + ") " + camp.Name.PadRight(30) + "Opening Month " + months[camp.OpenFromMM].ToString().PadRight(10) + "Closing Month " + months[camp.OpenToMM].ToString().PadRight(10) + "Daily Fee " + camp.DailyFee.ToString("c"));
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }

            Console.ReadLine();
        }

        private void GetAllParks()
        {
            ParksDAL pdal = new ParksDAL(DatabaseConnection);
            List<Park> parks = pdal.GetAllParks();

            if (parks.Count > 0)
            {
                foreach (Park park in parks)
                {
                    Console.WriteLine(park.Name.PadRight(20) + "Park Id: " + park.Park_id.ToString().PadRight(20) + "Park Location: " + park.Location.PadRight(20) + "Park Establish Date: " + park.Establish_date.ToString("yyyy").PadRight(20) + "Park Area In Miles: " + park.Area.ToString("N0").PadRight(20) + "Park Visitors: " + park.Visitors.ToString("N0"));
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
            Console.ReadLine();
        }

        private void PrintHeader()
        {
            Console.WriteLine(@"     _____           _      _____                                _   _                 ");
            Console.WriteLine(@"    |  __ \         | |    |  __ \                              | | (_)                ");
            Console.WriteLine(@"    | |__) |_ _ _ __| | __ | |__) |___  ___  ___ _ ____   ____ _| |_ _  ___  _ __  ___ ");
            Console.WriteLine(@"    |  ___/ _` | '__| |/ / |  _  // _ \/ __|/ _ \ '__\ \ / / _` | __| |/ _ \| '_ \/ __|");
            Console.WriteLine(@"    | |  | (_| | |  |   <  | | \ \  __/\__ \  __/ |   \ V / (_| | |_| | (_) | | | \__ \");
            Console.WriteLine(@"    |_|   \__,_|_|  |_|\_\ |_|  \_\___||___/\___|_|    \_/ \__,_|\__|_|\___/|_| |_|___/");
            Console.WriteLine();
        }

        private void PrintMenu()
        {
            Console.WriteLine("Main Menu Please type in a command");
            Console.WriteLine(" 1 - Show all parks");
            Console.WriteLine(" 2 - Show Campgrounds for park");
            Console.WriteLine(" 3 - Search for availability at a campground");
            Console.WriteLine(" Q - Quit");
            Console.WriteLine();
        }
    }
}
