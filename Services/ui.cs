using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

public class UI
{
    // Global var for selected Project.
    public static int selectedProjInt { get; set; }
        
    // ########## MAIN MENU ########## 
    public async static void MainMenu()
    {
        
        
        // Show Projects.
        Console.Clear();
        Console.WriteLine("|==================================================|");
        Console.WriteLine("|                     MAIN MENU                    |");
        Console.WriteLine("|==================================================|");

        await CRUD.ShowProjects();

        // Show Options.
        Console.WriteLine("\n1) Project Menu");
        Console.WriteLine("2) New Project");
        Console.WriteLine("E) Exit");

        // Get Input.
        Console.Write("-> ");
        string input = Console.ReadLine()!;
        input = input.ToLower();

        // Logic.
        if (input == "1")
        {
            // Get user input.
            Console.Write("Selected Project ID: ");
            string? selectedProjStr = Console.ReadLine();
            selectedProjInt = Convert.ToInt32(selectedProjStr);
            
            // Run ProjectMenu.
            ProjectMenu(selectedProjInt);
        }
        else if (input == "2")
        {
            await CRUD.CreateProject();
            MainMenu();
        }
        else if (input == "e")
        {
            Console.WriteLine("\nExiting Program...\n");
            return;
        }
        else
        {
            Console.WriteLine("\n----- INVALID SELECTION -----\n");
            Console.ReadLine();
            MainMenu();
        }
    }
    
    // ########## PROJECT MENU ########## 
    public async static void ProjectMenu(int projId)
    {
        // Show Project.
        Console.Clear();
        Console.WriteLine("------------------ PROJECT MENU ------------------");
        await CRUD.ShowSingleProject(projId);

        // Show Tasks.
        await CRUD.ShowTasks(selectedProjInt);

        // Show Options.
        Console.WriteLine("1) Edit Project");
        Console.WriteLine("2) Add Task");
        Console.WriteLine("3) Complete Task");
        Console.WriteLine("4) Complete Project");
        Console.WriteLine("DT) Delete Task");
        Console.WriteLine("DP) Delete Project");
        Console.WriteLine("M) Main Menu");

        // Get Input.
        Console.Write("-> ");
        string input = Console.ReadLine()!;
        input = input.ToLower();

        // Logic.
        if (input == "1")
        {
            await CRUD.EditProject(selectedProjInt);
            Console.ReadLine();
            ProjectMenu(selectedProjInt);
        }
        else if (input == "2")
        {
            await CRUD.CreateTask(selectedProjInt);
            Console.ReadLine();
            ProjectMenu(selectedProjInt);
        }
        else if (input == "3")
        {
            await CRUD.CompleteTask();
            Console.ReadLine();
            ProjectMenu(selectedProjInt);
        }
        else if (input == "4")
        {
            await CRUD.CompleteProject(selectedProjInt);
            Console.ReadLine();
            ProjectMenu(selectedProjInt);
        }
        else if (input == "dt")
        {
            await CRUD.DeleteTask();
            Console.ReadLine();
            ProjectMenu(selectedProjInt);
        }
        else if (input == "dp")
        {
            await CRUD.DeleteProject(selectedProjInt);
            Console.ReadLine();
            MainMenu();
        }
        else if (input == "m")
        {
            MainMenu();
        }
        else
        {
            Console.WriteLine("\n----- INVALID SELECTION -----\n");
            Console.ReadLine();
            ProjectMenu(selectedProjInt);
        }
    }
}