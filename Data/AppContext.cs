using Microsoft.EntityFrameworkCore;

public class ProjectTrackerContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> projectTasks { get; set; }

    public string DbPath { get; }

    public ProjectTrackerContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "todo.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Project
{
    public int Id { get; set; }
    public string ProjectName { get; set; } = "EMPTY";
    public bool ProjectComplete { get; set; }
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly GoalDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public int CompletedTasks { get; set; } = 0;
    public int TotalTasks { get; set; } = 0;
    public ICollection<ProjectTask> ProjectTasks { get; } = new List<ProjectTask>();
}

public class ProjectTask
{
    public int Id { get; set; }
    public string TaskName { get; set; } = "EMPTY";
    public DateOnly TaskGoalDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public bool TaskComplete { get; set; } = false;
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
}