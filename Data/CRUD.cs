using System.Security.Cryptography.X509Certificates;
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
                Console.WriteLine($"{p.ProjectName}\t\t{p.StartDate}\t\t{p.GoalDate}\tID: {p.Id}");
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
            try
            {
                var proj = await context.Projects.SingleAsync(p => p.Id == id);
                Console.WriteLine("PROJECT\t\tSTART-DATE\t\tGOAL-DATE\n");
                Console.WriteLine($"{proj.ProjectName}\t\t{proj.StartDate}\t\t{proj.GoalDate}");
                await Utilities.ProgressBar(proj);
                Console.Write("\n");
            }
            catch (InvalidOperationException)
            {
                Console.Clear();
                Console.WriteLine("----------------------ERROR-----------------------");
                Console.Write("INVALID ID...");
                Console.ReadLine();
                UI.MainMenu();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("----------------------ERROR-----------------------");
                Console.WriteLine($"Something went wrong: {ex.Message}");
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
                Console.ReadLine();
                UI.MainMenu();
            }
        }
    }

    // ########## EDIT PROJECT ########## 
    public async static Task EditProject(int projId)
    {
        using (var context = new ProjectTrackerContext())
        {
            var proj = await context.Projects.SingleAsync(p => p.Id == projId);

            // Edit Name.
            Console.Write($"Project Name from {proj.ProjectName} to: ");
            string inputStr = Console.ReadLine()!;
            proj.ProjectName = inputStr;

            // Edit Start Date.
            Console.Write($"StartDate from {proj.StartDate} to (MM-DD-YYYY): ");
            inputStr = Console.ReadLine()!;
            DateOnly.TryParse(inputStr, out DateOnly startD);
            proj.StartDate = startD;

            // Edit Goal Date.
            Console.Write($"GoalDate from {proj.GoalDate} to (MM-DD-YYYY): ");
            inputStr = Console.ReadLine()!;
            DateOnly.TryParse(inputStr, out DateOnly goalD);
            proj.GoalDate = goalD;

            await context.SaveChangesAsync();
            Console.Write("Edits complete...");
        }
    }

    // ########## COMPLETE PROJECT ########## 

    // ########## DELETE PROJECT ########## 
    public async static Task DeleteProject(int projId)
    {
        Console.WriteLine("Enter 'DELETE' if you are sure you want to delete this project.");
        Console.Write("-> ");
        string inputStr = Console.ReadLine()!;

        if (inputStr == "DELETE")
        {
            using (var context = new ProjectTrackerContext())
            {
                var proj = await context.Projects.SingleAsync(p => p.Id == projId);
                string projName = proj.ProjectName;
                // Remove any tasks that reference this project first to avoid foreign-key constraint failures
                var tasks = await context.projectTasks
                    .Where(t => t.ProjectId == projId)
                    .ToListAsync();

                if (tasks.Count > 0)
                {
                    context.projectTasks.RemoveRange(tasks);
                }

                context.Projects.Remove(proj);
                await context.SaveChangesAsync();
                Console.Write($"{projName} (ID: {projId}) has been DELETED...");
            }
        }
        else
        {
            Console.Write("\nInvalid entry...");
        }
    }

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
            Console.Write("\n");
        }
    }

    // ########## COMPLETE TASK ########## 
    public async static Task CompleteTask()
    {
        // Get user input.
        Console.WriteLine("\nMarking Task COMPLETE...");
        Console.Write("Task ID: ");
        string? taskIdStr = Console.ReadLine();
        int taskIdInt = Convert.ToInt32(taskIdStr);

        
        using (var context = new ProjectTrackerContext())
        {
            // Get the correct ProjectTask object.
            var projTask = await context.Set<ProjectTask>()
                .Include(t => t.Project)
                .SingleOrDefaultAsync(t => t.Id == taskIdInt);

            if (projTask == null)
            {
                Console.WriteLine($"Task with ID: {taskIdInt} was not found...");
                return;
            }

            // Mark the Task Complete.
            projTask.TaskComplete = true;

            // The Task's Project.CompletedTasks += 1.
            if (projTask.Project != null)
            {
                projTask.Project.CompletedTasks += 1;
            }

            // Save changes to the Database.
            await context.SaveChangesAsync();
        }
    }

    // ########## DELETE TASK ########## 
    public async static Task DeleteTask()
    {
        Console.Write("\nDELETE Task ID: ");
        string inputStr = Console.ReadLine()!;
        int inputInt = Convert.ToInt32(inputStr);
        Console.WriteLine("Enter 'DELETE' if you are sure you want to delete this task.");
        Console.Write("-> ");
        inputStr = Console.ReadLine()!;

        try
            {
                if (inputStr == "DELETE")
                {
                    using (var context = new ProjectTrackerContext())
                    {
                        var projTask = await context.projectTasks
                            .Include(t => t.Project)
                            .SingleAsync(t => t.Id == inputInt);
                        string projTaskName = projTask.TaskName;

                        if (projTask.TaskComplete && projTask.Project != null)
                        {
                            projTask.Project.CompletedTasks -= 1;
                            projTask.Project.TotalTasks -= 1;
                            Console.WriteLine($"Project: {projTask.Project.ProjectName} minus 1 completed task and 1 total task.");
                        }
                        else if (projTask.Project != null)
                        {
                            projTask.Project.TotalTasks -= 1;
                            Console.WriteLine($"Task NOT completed. Project: {projTask.Project.ProjectName} minus 1 total task.");
                        }

                        context.projectTasks.Remove(projTask);
                        await context.SaveChangesAsync();
                        Console.WriteLine($"{projTaskName} (ID: {inputInt}) has been DELETED...");
                    }
                }
                else
                {
                    Console.Write("\nInvalid entry...");
                }
            }
            catch (InvalidOperationException)
            {
                Console.Clear();
                Console.WriteLine("----------------------ERROR-----------------------");
                Console.Write("INVALID ID...");
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("----------------------ERROR-----------------------");
                Console.WriteLine($"Something went wrong: {ex.Message}");
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
            }
    }
}