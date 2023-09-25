using Microsoft.Extensions.DependencyInjection;
using SimpleTaskList.Interface;
using SimpleTaskList.Service;

class Program
{
    static void Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<ITaskListService, TaskListService>() // Use EmailService as the default implementation
            .BuildServiceProvider();

        var tlm = serviceProvider.GetRequiredService<ITaskListService>();

        while (true)
        {
            Console.Clear(); // Clear the console screen

            // Display the menu options
            Console.WriteLine("Welcome to Simple Task List App");
            Console.WriteLine("Menu:");
            Console.WriteLine("1. View Task");
            Console.WriteLine("2. Add Task");
            Console.WriteLine("3. Marking Task");
            Console.WriteLine("4. Delete Task");
            Console.WriteLine("5. Exit");

            // Get user input
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            // Process the user's choice
            switch (choice)
            {
                case "1":
                    tlm.ViewTask();
                    break;
                case "2":
                    tlm.AddTask();
                    break;
                case "3":
                    tlm.MarkTask();
                    break;
                case "4":
                    tlm.DeleteTask();
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            // Pause to let the user see the result before clearing the screen
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}