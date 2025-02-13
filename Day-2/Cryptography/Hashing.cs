using System;
using System.Collections.Generic;
using BCrypt.Net;
 
// Model
class Admin
{
    public string Username { get; set; }
    public string HashedPasscode { get; set; }
}
 
class Patient
{
    public string ID { get; set; }
    public string Name { get; set; }
}
 
// Service
class AuthService
{
    private Dictionary<string, string> admins = new Dictionary<string, string>();
    private int failedAttempts = 0;
    private const int maxFailedAttempts = 3;
 
    public bool RegisterAdmin(string username, string passcode)
    {
        if (admins.ContainsKey(username)) return false;
        admins[username] = BCrypt.Net.BCrypt.HashPassword(passcode);
        return true;
    }
 
    public bool LoginAdmin(string username, string passcode)
    {
        if (failedAttempts >= maxFailedAttempts)
        {
            Console.WriteLine("Too many failed attempts. Try again later.");
            return false;
        }
 
        if (admins.ContainsKey(username) && BCrypt.Net.BCrypt.Verify(passcode, admins[username]))
        {
            Console.WriteLine("Login successful!");
            failedAttempts = 0;
            return true;
        }
        else
        {
            failedAttempts++;
            Console.WriteLine("Invalid credentials. Attempts left: " + (maxFailedAttempts - failedAttempts));
            return false;
        }
    }
}
 
class PatientService
{
    private Dictionary<string, string> patients = new Dictionary<string, string>();
 
    public void AddPatient(string id, string name)
    {
        patients[id] = name;
        Console.WriteLine("Patient added successfully!");
    }
 
    public void ViewPatients()
    {
        Console.WriteLine("Patient List:");
        foreach (var entry in patients)
        {
            Console.WriteLine($"ID: {entry.Key}, Name: {entry.Value}");
        }
    }
}
 
// View
class Program
{
    static AuthService authService = new AuthService();
    static PatientService patientService = new PatientService();
 
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1. Register\n2. Login\n3. Exit");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    RegisterAdmin();
                    break;
                case "2":
                    if (LoginAdmin())
                        ManagePatients();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }
 
    static void RegisterAdmin()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter passcode: ");
        string passcode = Console.ReadLine();
        if (authService.RegisterAdmin(username, passcode))
            Console.WriteLine("Admin registered successfully!");
        else
            Console.WriteLine("Username already exists!");
    }
 
    static bool LoginAdmin()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter passcode: ");
        string passcode = Console.ReadLine();
        return authService.LoginAdmin(username, passcode);
    }
 
    static void ManagePatients()
    {
        while (true)
        {
            Console.WriteLine("1. Add Patient\n2. View Patients\n3. Logout");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddPatient();
                    break;
                case "2":
                    patientService.ViewPatients();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }
 
    static void AddPatient()
    {
        Console.Write("Enter Patient ID: ");
        string id = Console.ReadLine();
        Console.Write("Enter Patient Name: ");
        string name = Console.ReadLine();
        patientService.AddPatient(id, name);
    }
}
