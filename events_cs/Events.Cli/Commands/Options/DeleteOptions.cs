using System.ComponentModel.DataAnnotations;
using CommandLine;

namespace Events.Commands.Options;

/// <summary>
/// Command for deleting events
/// </summary>
[Verb("delete", HelpText = "Delete events")]
public class DeleteOptions
{
    
    [Option('D', "date", HelpText = "Delete all events with specified date", Group = "filters")]
    public DateOnly? Date { get; set; }

    [Option('b', "before-date", HelpText = "Delete all events before specified date", Group = "filters")]
    public DateOnly? BeforeDate { get; set; }

    [Option('a', "after-date", HelpText = "Delete all events after specified date", Group = "filters")]
    public DateOnly? AfterDate { get; set; }

    [Option('c', "category", HelpText = "Delete all events with specified category", Group = "filters")]
    public string? Category { get; set; }

    [Option('d', "description", HelpText = "Delete all events with descriptions starting with specified string",
        Group = "filters")]
    public string? Description { get; set; }

    [Option("all", HelpText = "Delete all events", Group = "filters")]
    public bool All { get; set; }

    [Option("dry-run", HelpText = "Display results of executing command without actually executing it")]
    public bool DryRun { get; set; }
}