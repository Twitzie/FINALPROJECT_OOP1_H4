using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace finalproj
{
    public class Pet
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string OwnerName { get; set; }
        public string OwnerContact { get; set; }

        public Pet(string type, string name, int age, string ownerName, string ownerContact)
        {
            Type = type;
            Name = name;
            Age = age;
            OwnerName = ownerName;
            OwnerContact = ownerContact;
        }
    }

    public class PetService : Pet
    {
        public string Grooming { get; set; }
        public string SpecialFeeding { get; set; }
        public string MedicalServices { get; set; }

        public PetService(string type, string name, int age, string ownerName, string ownerContact,
                      string grooming, string specialFeeding, string medicalServices)
        : base(type, name, age, ownerName, ownerContact)
        {
            Grooming = grooming;
            SpecialFeeding = specialFeeding;
            MedicalServices = medicalServices;
        }

        public decimal GetTotalServiceFee()
        {
            decimal total = 0;

            if (Grooming.Equals("yes", StringComparison.OrdinalIgnoreCase)) total += 100;
            if (SpecialFeeding.Equals("yes", StringComparison.OrdinalIgnoreCase)) total += 75;
            if (MedicalServices.Equals("yes", StringComparison.OrdinalIgnoreCase)) total += 150;

            return total;
        }
    }

    public static class SalesReport
    {
        public static void GenerateWeeklyReport(List<Reservation> reservations, DateTime now)
        {
            try
            {
                var startOfWeek = now.StartOfWeek(DayOfWeek.Monday);
                var endOfWeek = startOfWeek.AddDays(7);
                decimal totalEarnings = 0;

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"\n\n\n                                       >  Weekly Sales Report from {startOfWeek.ToShortDateString()} to {endOfWeek.ToShortDateString()}  <");
                Console.ResetColor();

                foreach (var reservation in reservations)
                {
                    if (reservation.StartTime >= startOfWeek && reservation.StartTime < endOfWeek)
                    {
                        Console.WriteLine("\n");
                        totalEarnings += reservation.Service.GetTotalServiceFee();
                        reservation.DisplayReservation();
                        Console.WriteLine();
                    }
                }

                Console.WriteLine($"\n                               Total Earnings for the week: {totalEarnings:C}");
                SaveSalesReport("weekly_sales_report.txt", totalEarnings, startOfWeek, endOfWeek);
                Console.WriteLine("\n                               Sales report saved to weekly_sales_report.txt");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\n\n\n\n                                                Press any key to return to the menu...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while generating the weekly report: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static void GenerateMonthlyReport(List<Reservation> reservations, DateTime now)
        {
            try
            {
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1);
                decimal totalEarnings = 0;

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"\n\n\n                                            >  Monthly Sales Report for {now.ToString("MMMM yyyy")}  <");
                Console.ResetColor();

                foreach (var reservation in reservations)
                {
                    if (reservation.StartTime >= startOfMonth && reservation.StartTime < endOfMonth)
                    {
                        Console.WriteLine("\n");
                        totalEarnings += reservation.Service.GetTotalServiceFee();
                        reservation.DisplayReservation();
                        Console.WriteLine();
                    }
                }

                Console.WriteLine($"\n                               Total Earnings for the month: {totalEarnings:C}");
                SaveSalesReport("monthly_sales_report.txt", totalEarnings, startOfMonth, endOfMonth);
                Console.WriteLine("\n                               Sales report saved to monthly_sales_report.txt");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\n\n\n\n                                                Press any key to return to the menu...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while generating the weekly report: {ex.Message}");
                Console.ResetColor();
            }
        }
        private static void SaveSalesReport(string fileName, decimal totalEarnings, DateTime start, DateTime end)
        {
            try
            {
                using (var writer = new StreamWriter(fileName, true))
                {
                    writer.WriteLine($"Report from {start.ToShortDateString()} to {end.ToShortDateString()}");
                    writer.WriteLine($"Total Earnings: {totalEarnings:C}");
                    writer.WriteLine("----------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while saving the sales report: {ex.Message}");
                Console.ResetColor();
            }
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startOfWeek)
        {
            int diff = dateTime.DayOfWeek - startOfWeek;
            if (diff < 0) diff += 7;
            return dateTime.AddDays(-1 * diff).Date;
        }
    }
    public class Reservation
    {
        public Pet Pet { get; set; }
        public PetService Service { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Reservation(Pet pet, PetService service, DateTime startTime, DateTime endTime)
        {
            Pet = pet;
            Service = service;
            StartTime = startTime;
            EndTime = endTime;

            string format = "MM-dd-yyyy hh:mm tt";
            var provider = System.Globalization.CultureInfo.InvariantCulture;
        }

        public void DisplayReservation()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"| {Pet.Name,-10} | {Pet.Type,-10} | {Pet.Age,-5} | {Pet.OwnerName,-15} | {Pet.OwnerContact,-15} | {Service.Grooming,-10} | {Service.SpecialFeeding,-10} | {Service.MedicalServices,-10} | {StartTime,-20} | {EndTime,-20} |");
            Console.ResetColor();
        }

        public string GetReservationDetails()
        {
            return $"{Pet.Type},{Pet.Name},{Pet.Age},{Pet.OwnerName},{Pet.OwnerContact}," +
                   $"{Service.Grooming},{Service.SpecialFeeding},{Service.MedicalServices}," +
                   $"{StartTime},{EndTime}";
        }

        public static Reservation ParseReservation(string line)
        {
            try
            {
                var parts = line.Split(',');

                var service = new PetService(
                    parts[0],
                    parts[1],
                    int.Parse(parts[2]),
                    parts[3],
                    parts[4],
                    parts[5],
                    parts[6],
                    parts[7]
                );

                var startTime = DateTime.Parse(parts[8]);
                var endTime = DateTime.Parse(parts[9]);

                return new Reservation((Pet)service, service, startTime, endTime);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while parsing the reservation: {ex.Message}");
                Console.ResetColor();
                return null;
            }
        }
    }
    public class Program
    {
        private static List<Reservation> reservations = new List<Reservation>();

        public static void Main(string[] args)
        {
            LoadReservations();
            DisplayRoleSelection();
        }

        private static void DisplayRoleSelection()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n.....................................................................................................................................");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(@"
                                             _____       _                                         
                                            |  __ \     | |                                        
                                            | |__) |___ | |_                                       
                                            |  ___// _ \| __|                                      
                                            | |   |  __/| |_                                       
                                            |_|    \___| \__|                                      
                                     ____                          _  _               
                                    |  _ \                        | |(_)              
                                    | |_) |  ___    __ _  _ __  __| | _  _ __    __ _ 
                                    |  _ <  / _ \  / _` || '__|/ _` || || '_ \  / _` |
                                    | |_) || (_) || (_| || |  | (_| || || | | || (_| |
                                    |____/  \___/  \__,_||_|   \__,_||_||_| |_| \__, |
                                               _____              _              __/ |
                                              / ____|            | |            |___/ 
                                             | (___   _   _  ___ | |_  ___  _ __ ___  
                                              \___ \ | | | |/ __|| __|/ _ \| '_ ` _ \ 
                                              ____) || |_| |\__ \| |_|  __/| | | | | |
                                             |_____/  \__, ||___/ \__|\___||_| |_| |_|
                                                       __/ |                          
                                                      |___/                           ");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n\n                                                Welcome to our Pet Boarding System!");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("                              We're excited to help you manage your pet’s stay with ease and care.");
                Console.WriteLine("                                      Let’s make your pet’s visit comfortable and enjoyable! :)");
                Console.ResetColor();
                Console.WriteLine("\n");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n.....................................................................................................................................");
                Console.ResetColor();
                Console.ReadKey();

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n                                                           Select your role");
                Console.ResetColor();
                Console.WriteLine("                                     ----------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\n\n                                         1 - OWNER ");
                Console.Write("\n                                         2 - CUSTOMER ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n\n\n                                         Enter here:  ");
                Console.ResetColor();
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    DisplayOwnerMenu();
                }
                else if (choice == "2")
                {
                    DisplayCustomerMenu();
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                    DisplayRoleSelection();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while displaying role selection: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static void DisplayOwnerMenu()
        {
            try
            {
                Console.Clear();
                const string ownerPassword = "tweetybird";
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n                                  Enter the owner password: ");
                Console.ResetColor();
                string inputPassword = Console.ReadLine();

                if (inputPassword != ownerPassword)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\n                                  Incorrect password. Access denied.");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\n\n                                                Press any key to exit...");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

                int selectedIndex = 0;
                string[] menuOptions =
                {
            "View all reservations",
            "Search reservations",
            "~ Weekly sales report ~",
            "~ Monthly sales report ~",
            "Exit"
        };

                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n                                                       OWNER MENU");
                    Console.ResetColor();
                    Console.WriteLine("                                     ---------------------------------------------------");

                    for (int i = 0; i < menuOptions.Length; i++)
                    {
                        if (i == selectedIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"==> {menuOptions[i]}");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine($"  {menuOptions[i]}");
                        }
                    }

                    Console.ResetColor();
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.UpArrow)
                    {
                        selectedIndex = (selectedIndex - 1 + menuOptions.Length) % menuOptions.Length;
                    }
                    else if (keyInfo.Key == ConsoleKey.DownArrow)
                    {
                        selectedIndex = (selectedIndex + 1) % menuOptions.Length;
                    }
                    else if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        switch (selectedIndex)
                        {
                            case 0:
                                ViewReservations();
                                break;
                            case 1:
                                SearchReservationsByOwner();
                                break;
                            case 2:
                                SalesReport.GenerateWeeklyReport(reservations, DateTime.Now);
                                break;
                            case 3:
                                SalesReport.GenerateMonthlyReport(reservations, DateTime.Now);
                                break;
                            case 4:
                                DisplayRoleSelection();
                                return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while displaying role selection: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static void DisplayCustomerMenu()
        {
            try
            {
                int selectedIndex = 0;
                string[] menuOptions =
                    {
                "Make a reservation",
                "View all reservations",
                "Search reservations",
                "Update a reservation",
                "Delete a reservation",
                "Exit"
            };

                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n                                                       CUSTOMER MENU");
                    Console.ResetColor();
                    Console.WriteLine("                                     ----------------------------------------------------");

                    for (int i = 0; i < menuOptions.Length; i++)
                    {
                        if (i == selectedIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"==> {menuOptions[i]}");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine($"  {menuOptions[i]}");
                        }
                    }

                    Console.ResetColor();
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.UpArrow)
                    {
                        selectedIndex = (selectedIndex - 1 + menuOptions.Length) % menuOptions.Length;
                    }
                    else if (keyInfo.Key == ConsoleKey.DownArrow)
                    {
                        selectedIndex = (selectedIndex + 1) % menuOptions.Length;
                    }
                    else if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        Program program = new Program();
                        switch (selectedIndex)
                        {
                            case 0:
                                MakeReservation();
                                break;
                            case 1:
                                ViewReservations();
                                break;
                            case 2:
                                SearchReservationsByOwner();
                                break;
                            case 3:
                                UpdateReservation();
                                break;
                            case 4:
                                DeleteReservation();
                                break;
                            case 5:
                                SaveReservations();
                                DisplayRoleSelection();
                                return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while displaying role selection: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void MakeReservation()
        {
            try
            {
                Program program = new Program();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n                                                ------------------------------------");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("                                                          RESERVATION FORM");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n                                                ------------------------------------");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n                                         > Enter pet type or 'exit' to cancel: ");
                Console.ResetColor();
                string type = Console.ReadLine();
                if (type.ToLower() == "exit") return;

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n                                    ..........................................................");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\n                                    PET DETAILS");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n\n                                         > Pet Name: ");
                Console.ResetColor();
                string name = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n                                         > Pet Age (in months): ");
                Console.ResetColor();
                int age;
                while (!int.TryParse(Console.ReadLine(), out age))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("                                    Invalid input. Please enter a valid age: ");
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n                                    ..........................................................");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\n                                   OWNER DETAILS");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n\n                                         > Owner Name: ");
                Console.ResetColor();
                string ownerName = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n                                         > Owner Contact Number: ");
                Console.ResetColor();
                string ownerContact = Console.ReadLine();

                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n                                                       ADDITIONAL SERVICES");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("                                        Does your pet's stay include the following? (yes/no)");
                Console.ResetColor();

                Console.WriteLine("\n\n");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n                                    ..........................................................");
                Console.ResetColor();

                Console.WriteLine("\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("                                                     Grooming         - ");
                Console.ResetColor();
                string grooming = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n                                    ..........................................................");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n\n                                                Specialized Feeding   - ");
                Console.ResetColor();
                string specialFeeding = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\n                                    ..........................................................");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n\n                                                  Medical Services    - ");
                Console.ResetColor();
                string medicalServices = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\n                                    ..........................................................");
                Console.ResetColor();

                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n                                                          PET'S STAY DURATION");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("                             Please follow the date and time format to avoid an error in the system. Thank you!");
                Console.ResetColor();

                Console.WriteLine("\n\n");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n                                    ..........................................................");
                Console.ResetColor();

                Console.WriteLine("\n");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("                                        DROP - OFF TIME");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n                                             (MM-DD-YYYY) (hh:mm AM/PM): ");
                Console.ResetColor();
                DateTime startTime = DateTime.Parse(Console.ReadLine());

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n                                    ..........................................................");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("\n\n                                        PICK - UP TIME");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n                                             (MM-DD-YYYY) (hh:mm AM/PM): ");
                Console.ResetColor();
                DateTime endTime = DateTime.Parse(Console.ReadLine());

                var service = new PetService(type, name, age, ownerName, ownerContact, grooming, specialFeeding, medicalServices);
                var reservation = new Reservation((Pet)service, service, startTime, endTime);

                reservations.Add(reservation);
                SaveReservations();
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n                                                ************************************");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("                                                    Reservation made successfully!");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("                                                ************************************");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\n\n\n                                                Press any key to return to the menu...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while displaying role selection: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void ViewReservations()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n     > CURRENT RESERVATIONS < ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------");
                Console.ResetColor();
                Console.WriteLine("| Pet Name | Pet Type | Age | Owner Name  | Owner Contact | Grooming | Special Feeding | Medical Services | Start Time          | End Time            |");
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------");
                foreach (var reservation in reservations)
                {
                    reservation.DisplayReservation();
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\n\n                                                Press any key to return to the menu...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while displaying role selection: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void SearchReservationsByOwner()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\n\n\n                                         Enter Owner's Name to search: ");
                Console.ResetColor();
                string ownerName = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n\n\n         >  SEARCH RESULTS  < ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("--------------------------------------------------------------------");
                Console.ResetColor();

                bool found = false;
                foreach (var reservation in reservations)
                {
                    if (reservation.Pet.OwnerName.Equals(ownerName, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine();
                        reservation.DisplayReservation();
                        found = true;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("No reservations found for this owner.");
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\n\n                                                Press any key to return to the menu...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while displaying role selection: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void UpdateReservation()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\n\n\n                                         Enter Owner's Name to UPDATE reservation: ");
                Console.ResetColor();
                string ownerName = Console.ReadLine();

                Reservation reservationToUpdate = null;
                foreach (var reservation in reservations)
                {
                    if (reservation.Pet.OwnerName.Equals(ownerName, StringComparison.OrdinalIgnoreCase))
                    {
                        reservationToUpdate = reservation;
                        break;
                    }
                }

                if (reservationToUpdate == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\n                                                No reservation found for this owner.");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\n\n                                                Press any key to return to the menu...");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" > UPDATING RESERVATION FOR :");
                Console.ResetColor();
                Console.Write("\n");
                reservationToUpdate.DisplayReservation();
                Program program = new Program();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\n\n> Please enter new details (click enter to keep existing)");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\nPET DETAILS");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n     New Pet Type (current: " + reservationToUpdate.Pet.Type + "): ");
                string newType = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newType)) reservationToUpdate.Pet.Type = newType;

                Console.Write("\n     New Pet Name (current: " + reservationToUpdate.Pet.Name + "): ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName)) reservationToUpdate.Pet.Name = newName;

                Console.Write("\n     New Pet Age (current: " + reservationToUpdate.Pet.Age + "): ");
                string ageInput = Console.ReadLine();
                if (int.TryParse(ageInput, out int newAge)) reservationToUpdate.Pet.Age = newAge;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\nOWNER DETAILS");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n     New Owner Name (current: " + reservationToUpdate.Pet.OwnerName + "): ");
                string newOwnerName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newOwnerName)) reservationToUpdate.Pet.OwnerName = newOwnerName;
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n     New Owner Contact (current: " + reservationToUpdate.Pet.OwnerContact + "): ");
                string newOwnerContact = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newOwnerContact)) reservationToUpdate.Pet.OwnerContact = newOwnerContact;
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\nADDITIONAL SERVICES");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n     New Grooming (current: " + reservationToUpdate.Service.Grooming + "): ");
                string newGrooming = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newGrooming)) reservationToUpdate.Service.Grooming = newGrooming;
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n     New Special Feeding (current: " + reservationToUpdate.Service.SpecialFeeding + "): ");
                string newSpecialFeeding = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newSpecialFeeding)) reservationToUpdate.Service.SpecialFeeding = newSpecialFeeding;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n     New Medical Services (current: " + reservationToUpdate.Service.MedicalServices + "): ");
                string newMedicalServices = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newMedicalServices)) reservationToUpdate.Service.MedicalServices = newMedicalServices;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\nSTAY DURATION");

                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n     Enter new Start Time (current: " + reservationToUpdate.StartTime + "): ");
                string startTimeInput = Console.ReadLine();
                if (DateTime.TryParse(startTimeInput, out DateTime newStartTime)) reservationToUpdate.StartTime = newStartTime;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n     Enter new End Time (current: " + reservationToUpdate.EndTime + "): ");
                string endTimeInput = Console.ReadLine();
                if (DateTime.TryParse(endTimeInput, out DateTime newEndTime)) reservationToUpdate.EndTime = newEndTime;
                Console.ResetColor();

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n                                                ************************************");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("                                                  Reservation updated successfully!");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("                                                ************************************");
                Console.ResetColor();
                SaveReservations();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\n\n\n                                                Press any key to return to the menu...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while displaying role selection: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void DeleteReservation()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\n\n\n                                         Enter Owner's Name to DELETE reservation: ");
                Console.ResetColor();
                string ownerName = Console.ReadLine();

                Reservation reservationToDelete = null;
                foreach (var reservation in reservations)
                {
                    if (reservation.Pet.OwnerName.Equals(ownerName, StringComparison.OrdinalIgnoreCase))
                    {
                        reservationToDelete = reservation;
                        break;
                    }
                }

                if (reservationToDelete == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\n                                                No reservation found for this owner.");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\n\n                                                Press any key to return to the menu...");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" > UPDATING RESERVATION FOR :");
                Console.ResetColor();
                Console.Write("\n");
                reservationToDelete.DisplayReservation();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\n\n\n          Are you sure you want to delete this reservation? (yes/no): ");
                Console.ResetColor();
                string confirmation = Console.ReadLine();
                Program program = new Program();
                if (confirmation.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    reservations.Remove(reservationToDelete);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("                                             ******************************************");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("                                                 Reservation deleted successfully!");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("                                             ******************************************");
                    Console.ResetColor();
                    SaveReservations();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\n\n\n                                                        Deletion cancelled.");
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\n\n                                                Press any key to return to the menu...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while displaying role selection: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void SaveReservations()
        {
            try
            {
                string fileName = "all_reservations.txt";
                using (var writer = new StreamWriter(fileName, false))
                {
                    foreach (var reservation in reservations)
                    {
                        writer.WriteLine(reservation.GetReservationDetails());
                    }
                }
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n\n\n                                               All reservations saved successfully!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while displaying role selection: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void LoadReservations()
        {
            string fileName = "all_reservations.txt";
            if (File.Exists(fileName))
            {
                var lines = File.ReadAllLines(fileName);
                foreach (var line in lines)
                {
                    var reservation = Reservation.ParseReservation(line);
                    reservations.Add(reservation);
                }
            }
        }
    }
}