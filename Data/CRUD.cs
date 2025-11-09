using Microsoft.EntityFrameworkCore;

public class CRUD
{
    // ########## NEW PROJECT ##########
    public static async Task CreateProject()
    {
        using var context = new ProjectTrackerContext();
        Console.Write("Project Name: ");
        string? nameofProject = Console.ReadLine();
        Console.Write("DueDate(MM-DD-YYYY): ");
        string? dueStr = Console.ReadLine();
        DateOnly.TryParse(dueStr, out DateOnly due);
        context.Add(new Project
        {
            ProjectName = nameofProject!,
            ProjectComplete = false,
            GoalDate = due,
        });
        await context.SaveChangesAsync();
        Console.WriteLine($"New Project ({nameofProject} has been CREATED)");
        Console.ReadLine();
    }

    // ########## SHOW PROJECT ########## 

    // ########## EDIT PROJECT ########## 

    // ########## COMPLETE PROJECT ########## 

    // ########## DELETE PROJECT ########## 

    // ########## NEW TASK ########## 

    // ########## SHOW TASKS ########## 

    // ########## COMPLETE TASK ########## 

    // ########## DELETE TASK ########## 
}