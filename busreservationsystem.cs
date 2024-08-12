using System;
using System.Collections.Generic;

class Program
{
    class Bus
    {
        public int BusNumber { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public float Fare { get; set; }
    }

    class Passenger
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int SeatNumber { get; set; }
        public int BusNumber { get; set; }
    }

    class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    static void DisplayMainMenu()
    {
        Console.WriteLine("\n=== Bus Reservation System ===");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Exit");
        Console.Write("Enter your choice: ");
    }

    static void DisplayUserMenu()
    {
        Console.WriteLine("\n=== User Menu ===");
        Console.WriteLine("1. Book a Ticket");
        Console.WriteLine("2. Cancel a Ticket");
        Console.WriteLine("3. Check Bus Status");
        Console.WriteLine("4. Logout");
        Console.Write("Enter your choice: ");
    }

    static int LoginUser(List<User> users, string username, string password)
    {
        for (int i = 0; i < users.Count; i++)
        {
            if (users[i].Username == username && users[i].Password == password)
            {
                return i;
            }
        }
        return -1;
    }

    static void BookTicket(List<Bus> buses, List<Passenger> passengers, ref int numPassengers, int userId)
    {
        Console.Write("Enter Bus Number: ");
        int busNumber = int.Parse(Console.ReadLine());

        Bus bus = buses.Find(b => b.BusNumber == busNumber);

        if (bus == null)
        {
            Console.WriteLine($"Bus with Bus Number {busNumber} not found.");
        }
        else if (bus.AvailableSeats == 0)
        {
            Console.WriteLine("Sorry, the bus is fully booked.");
        }
        else
        {
            var passenger = new Passenger();
            Console.Write("Enter Passenger Name: ");
            passenger.Name = Console.ReadLine();

            Console.Write("Enter Passenger Age: ");
            passenger.Age = int.Parse(Console.ReadLine());

            passenger.SeatNumber = bus.TotalSeats - bus.AvailableSeats + 1;
            passenger.BusNumber = busNumber;

            bus.AvailableSeats--;

            passengers.Add(passenger);
            numPassengers++;

            Console.WriteLine("Ticket booked successfully!");
        }
    }

    static void CancelTicket(List<Bus> buses, List<Passenger> passengers, ref int numPassengers, int userId)
    {
        Console.Write("Enter Passenger Name: ");
        string name = Console.ReadLine();

        Passenger passenger = passengers.Find(p => p.Name == name && p.BusNumber == buses[userId].BusNumber);

        if (passenger != null)
        {
            Bus bus = buses.Find(b => b.BusNumber == passenger.BusNumber);
            bus.AvailableSeats++;

            passengers.Remove(passenger);
            numPassengers--;

            Console.WriteLine("Ticket canceled successfully!");
        }
        else
        {
            Console.WriteLine($"Passenger with name {name} not found on this bus.");
        }
    }

    static void CheckBusStatus(List<Bus> buses, int userId)
    {
        Bus bus = buses[userId];
        Console.WriteLine($"\nBus Number: {bus.BusNumber}");
        Console.WriteLine($"Source: {bus.Source}");
        Console.WriteLine($"Destination: {bus.Destination}");
        Console.WriteLine($"Total Seats: {bus.TotalSeats}");
        Console.WriteLine($"Available Seats: {bus.AvailableSeats}");
        Console.WriteLine($"Fare: {bus.Fare:F2}");
    }

    static void Main()
    {
        var users = new List<User>
        {
            new User { Username = "admin1", Password = "adminpass1" },
            new User { Username = "admin2", Password = "adminpass2" },
            new User { Username = "admin3", Password = "adminpass3" },
            new User { Username = "admin4", Password = "adminpass4" },
            new User { Username = "admin5", Password = "adminpass5" }
        };
        int numUsers = users.Count;

        var buses = new List<Bus>
        {
            new Bus { BusNumber = 101, Source = "City A", Destination = "City B", TotalSeats = 50, AvailableSeats = 50, Fare = 25.0f },
            new Bus { BusNumber = 102, Source = "City C", Destination = "City D", TotalSeats = 40, AvailableSeats = 40, Fare = 20.0f },
            new Bus { BusNumber = 103, Source = "City E", Destination = "City F", TotalSeats = 30, AvailableSeats = 30, Fare = 15.0f }
        };
        int numBuses = buses.Count;

        var passengers = new List<Passenger>();
        int numPassengers = 0;

        int loggedInUserId = -1;

        while (true)
        {
            if (loggedInUserId == -1)
            {
                DisplayMainMenu();
                int choice = int.Parse(Console.ReadLine());

                if (choice == 1)
                {
                    Console.Write("Enter Username: ");
                    string username = Console.ReadLine();
                    Console.Write("Enter Password: ");
                    string password = Console.ReadLine();

                    loggedInUserId = LoginUser(users, username, password);
                    if (loggedInUserId == -1)
                    {
                        Console.WriteLine("Login failed. Please check your username and password.");
                    }
                    else
                    {
                        Console.WriteLine($"Login successful. Welcome, {username}!");
                    }
                }
                else if (choice == 2)
                {
                    Console.WriteLine("Exiting the program.");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
            else
            {
                DisplayUserMenu();
                int userChoice = int.Parse(Console.ReadLine());

                switch (userChoice)
                {
                    case 1:
                        BookTicket(buses, passengers, ref numPassengers, loggedInUserId);
                        break;
                    case 2:
                        CancelTicket(buses, passengers, ref numPassengers, loggedInUserId);
                        break;
                    case 3:
                        CheckBusStatus(buses, loggedInUserId);
                        break;
                    case 4:
                        Console.WriteLine("Logging out.");
                        loggedInUserId = -1;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}
