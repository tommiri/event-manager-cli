using CommandLine;
using CsvHelper.Configuration.Attributes;

namespace Events.Commands.Options;

[Verb("add", HelpText = "Add new events")]
public class AddOptions
{
    [Option('D', "date", HelpText = "Specify date for new event")]
    public DateOnly? Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Option('c', "category", HelpText = "Specify category for new event")]
    public string? Category { get; set; }

    [Option('d', "description", HelpText = "Specify description for new event", Required = true)]
    public string? Description { get; set; }
}