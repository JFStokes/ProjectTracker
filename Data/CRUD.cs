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

    // ########## SHOW PROJECTS ########## 
    public static async Task ShowProjects()
    {
        using (var context = new ProjectTrackerContext())
        {
            var proj = await context.Projects
                .Where(p => p.ProjectComplete == false)
                .ToListAsync();

            Console.WriteLine("PROJECT\t\tSTART-DATE\t\tGOAL-DATE\n");
            foreach (Project p in proj)
            {
                Console.WriteLine($"{p.ProjectName}\t\t{p.StartDate}\t\t{p.GoalDate}");
                await Utilities.ProgressBar(p);
                Console.Write("\n");
            }
        }
    }

    // ########## SHOW SINGLE PROJECT ########## 
    public static async Task ShowSingleProject(int id)
    {
        using (var context = new ProjectTrackerContext())
        {
            var proj = await context.Projects.SingleAsync(p => p.Id == id);
            Console.WriteLine("PROJECT\t\tSTART-DATE\t\tGOAL-DATE\n");
            Console.WriteLine($"{proj.ProjectName}\t\t{proj.StartDate}\t\t{proj.GoalDate}");
            await Utilities.ProgressBar(proj);
            Console.Write("\n");
        }
    }

    // ########## EDIT PROJECT ########## 

    // ########## COMPLETE PROJECT ########## 

    // ########## DELETE PROJECT ########## 

    // ########## NEW TASK ########## 

    // ########## SHOW TASKS ########## 

    // ########## COMPLETE TASK ########## 

    // ########## DELETE TASK ########## 
}