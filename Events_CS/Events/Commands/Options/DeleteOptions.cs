using CommandLine;

namespace Events.Commands.Options;

[Verb("delete", HelpText = "Delete events")]
public class DeleteOptions
{
    [Option('D', "date", HelpText = "Delete all events with specified date")]
    public DateOnly? Date { get; set; }

    [Option('b', "before-date", HelpText = "Delete all events before specified date")]
    public DateOnly? BeforeDate { get; set; }

    [Option('a', "before-date", HelpText = "Delete all events after specified date")]
    public DateOnly? AfterDate { get; set; }

    [Option('c', "category", HelpText = "Delete all events with specified category")]
    public string? Category { get; set; }

    [Option('d', "description", HelpText = "Delete all events with descriptions starting with specified string")]
    public string? Description { get; set; }

    [Option('a', "all", HelpText = "Delete all events")]
    public bool All { get; set; }

    [Option("dry-run", HelpText = "Display results of executing command without actually executing it")]
    public bool DryRun { get; set; }
}