using CommandLine;

namespace Events.Commands.Options;

[Verb("list", HelpText = "List events")]
public class ListOptions
{
    [Option('t', "today", HelpText = "List events happening today")]
    public bool Today { get; set; }

    [Option('b', "before-date", HelpText = "List events before specified date")]
    public DateOnly? BeforeDate { get; set; }

    [Option('a', "after-date", HelpText = "List events after specified date")]
    public DateOnly? AfterDate { get; set; }

    [Option('D', "date", HelpText = "List events on specified date")]
    public DateOnly? Date { get; set; }

    [Option('c', "categories", HelpText = "List events from specified categories")]
    public IEnumerable<string>? Categories { get; set; }

    [Option('e', "exclude", HelpText = "Exclude specified categories")]
    public bool Exclude { get; set; }

    [Option('n', "no-category", HelpText = "List events with no category")]
    public bool NoCategory { get; set; }
}