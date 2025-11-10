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
    public async static Task CreateTask(int projId)
    {
        Console.WriteLine("\nAdding a new Task to the Project...");
        Console.Write("Task Name: ");
        string taskName = Console.ReadLine()!;
        Console.Write("Due Date(MM-DD-YYYY): ");
        string dueStr = Console.ReadLine()!;
        DateOnly.TryParse(dueStr, out DateOnly due);

        using (var context = new ProjectTrackerContext())
        {
            // Create the new ProjectTask.
            var task = new ProjectTask
            {
                ProjectId = projId,
                TaskName = taskName,
                TaskGoalDate = due
            };
            context.Add(task);

            // Project.TotalTasks += 1.
            var proj = context.Projects.Include(p => p.ProjectTasks)
                .FirstOrDefault(p => p.Id == projId);
            if (proj == null)
            {
                Console.WriteLine($"Project with ID: {projId} not found..");
                return;
            }
            proj.TotalTasks += 1;
            await context.SaveChangesAsync();
            Console.Write("Task added to the Project...");
        }
    }

    // ########## SHOW TASKS ########## 
    public async static Task ShowTasks(int projId)
    {
        Console.WriteLine("\nTASK\t\t\tDUE_DATE\t\tCOMPLETE\t\tID");
        using (var context = new ProjectTrackerContext())
        {
            var proj = await context.Projects
                .Include(p => p.ProjectTasks)
                .SingleAsync(p => p.Id == projId);
            
            foreach (ProjectTask t in proj.ProjectTasks)
            {
                Console.WriteLine($"{t.TaskName}\t\t{t.TaskGoalDate}\t\t{t.TaskComplete}\t\t\t{t.Id}");
            }
        }
        Console.Write("\n");
    }

    // ########## COMPLETE TASK ########## 

    // ########## DELETE TASK ########## 
}