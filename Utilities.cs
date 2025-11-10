using System.Runtime.InteropServices;

public class Utilities
{
    // ########## PROGRESS BAR ##########
    public async static Task ProgressBar(Project proj)
    {
        // Get int percent completed.
        DateOnly today = DateOnly.FromDateTime(DateTime.Now); // Gets Today's date.
        int projectDaysBetween = (proj.GoalDate.ToDateTime(TimeOnly.MinValue) - proj.StartDate.ToDateTime(TimeOnly.MinValue)).Days; // Gets total days of project.
        int timeDaysBetween = (proj.GoalDate.ToDateTime(TimeOnly.MinValue) - today.ToDateTime(TimeOnly.MinValue)).Days; // Gets number of days from Today to end of project.
        int daysPast = projectDaysBetween - timeDaysBetween; // Gets Days since project started.
        DrawBar(daysPast, projectDaysBetween, "days", 0, 0);
        int intPercentDays = (int)Math.Round((double)daysPast / projectDaysBetween * 100.0);
        Console.Write($"Days: {daysPast}/{projectDaysBetween}\t{intPercentDays}%\n");

        // Draw Completed Bar.
        int intPercentTasks = (int)Math.Round((double)proj.CompletedTasks / proj.TotalTasks * 100.0);
        DrawBar(proj.CompletedTasks, proj.TotalTasks, "tasks", intPercentDays, intPercentTasks);
        Console.Write($"Tasks: {proj.CompletedTasks}/{proj.TotalTasks}\t{intPercentTasks}%\n");

        Console.Write("\n");
    }

    // ########## DRAW BAR ##########
    public async static void DrawBar(int num, int den, string type, int percDay, int percTask)
    {
        int intPercent = (int)Math.Round((double)num / den * 100.0);
        intPercent = (int)Math.Round((double)intPercent / 2.0);

        Console.Write("|");
        for (int i = 0; i < intPercent; i++)
        {
            HighlightBar(type, percDay, percTask);
            Console.ResetColor();
        }

        int intRemaining = 50 - intPercent;
        for (int i = 0; i < intRemaining; i++)
        {
            Console.Write("_");
        }
        Console.Write("|");
    }

    // ########## HIGHLIGHT BAR ##########
    static void HighlightBar(string type, int percDay, int percTask)
    {
        if (type == "days")
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(" ");
        }
        else if (type == "tasks" && percTask >= percDay)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(" ");
        }
        else if (type == "tasks" && percTask < percDay)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(" ");
        }
    }
}