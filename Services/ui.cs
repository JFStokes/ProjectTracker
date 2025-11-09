public class UI
{
    // ########## MAIN MENU ########## 
    public async static void MainMenu()
    {
        // Show Projects.
        Console.Clear();
        Console.WriteLine("----- MAIN MENU -----");
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
            ProjectMenu();
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
    public async static void ProjectMenu()
    {
        // Show Project.
        Console.Clear();
        Console.WriteLine("----- PROJECT MENU -----");
        Console.WriteLine(">>> SHOW PROJECT HERE <<<");

        // Show Tasks.
        Console.WriteLine("\n>>> SHOW TASKS HERE <<<\n");

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
            Console.WriteLine(">>> EDIT PROJECT HERE <<<");
            Console.ReadLine();
            ProjectMenu();
        }
        else if (input == "2")
        {
            Console.WriteLine(">>> ADD TASK HERE <<<");
            Console.ReadLine();
            ProjectMenu();
        }
        else if (input == "3")
        {
            Console.WriteLine(">>> COMPLETE TASK HERE <<<");
            Console.ReadLine();
            ProjectMenu();
        }
        else if (input == "4")
        {
            Console.WriteLine(">>> COMPLETE PROJECT HERE <<<");
            Console.ReadLine();
            ProjectMenu();
        }
        else if (input == "dt")
        {
            Console.WriteLine(">>> DELETE TASK HERE <<<");
            Console.ReadLine();
            ProjectMenu();
        }
        else if (input == "dp")
        {
            Console.WriteLine(">>> DELETE PROJECT HERE <<<");
            Console.ReadLine();
            ProjectMenu();
        }
        else if (input == "m")
        {
            MainMenu();
        }
        else
        {
            Console.WriteLine("\n----- INVALID SELECTION -----\n");
            Console.ReadLine();
            ProjectMenu();
        }
    }
}