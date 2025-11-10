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
        // int daysPast = 2; // Place Filler for real var.
        DrawBar(daysPast, projectDaysBetween);
        int intPercent = (int)Math.Round((double)daysPast / projectDaysBetween * 100.0);
        Console.Write($"Days: {daysPast}/{projectDaysBetween}\t{intPercent}%\n");

        // Draw Completed Bar.
        DrawBar(proj.CompletedTasks, proj.TotalTasks);
        intPercent = (int)Math.Round((double)proj.CompletedTasks / proj.TotalTasks * 100.0);
        Console.Write($"Tasks: {proj.CompletedTasks}/{proj.TotalTasks}\t{intPercent}%\n");

        Console.Write("\n");
    }

    // ########## DRAW BAR ##########
    public async static void DrawBar(int num, int den)
    {
        int intPercent = (int)Math.Round((double)num / den * 100.0);
        intPercent = (int)Math.Round((double)intPercent / 2.0);

        Console.Write("|");
        for (int i = 0; i < intPercent; i++)
        {
            Console.Write("X");
        }

        int intRemaining = 50 - intPercent;
        for (int i = 0; i < intRemaining; i++)
        {
            Console.Write("_");
        }
        Console.Write("|");
    }

    // ########## HIGHLIGHT WHITE ##########
    // ########## HIGHLIGHT GREEN ##########
    // ########## HIGHLIGHT RED ##########
}