namespace Events.Commands;

using Utils;
using Event;
using Options;

/// <summary>
/// Static class for handling commands
/// </summary>
public static class CommandHandler
{
    // Get event manager instance
    private static readonly EventManager Em = EventManager.GetInstance();
    private static List<Event> Events { get; set; } = new();

    /// <summary>
    /// Method for handling the "list" command
    /// </summary>
    /// <param name="opts">"list" options to handle</param>
    /// <returns>exit code 0 if successful</returns>
    public static int RunListAndReturnExitCode(ListOptions opts)
    {
        Setup();

        // Initialize function used for filtering to null
        Func<Event, bool>? filter = null;

        // Set filter to:
        if (opts.Today)
            // Match events happening today
            filter = Compose.And(filter, e => e.Date == DateOnly.FromDateTime(DateTime.Now));
        if (opts.Date != null)
            // Match events on specified date
            filter = Compose.And(filter, e => e.Date == opts.Date);
        if (opts.BeforeDate != null)
            // Match events before specified date
            filter = Compose.And(filter, e => e.Date < opts.BeforeDate);
        if (opts.AfterDate != null)
            // Match events after specified date
            filter = Compose.And(filter, e => e.Date > opts.AfterDate);
        if (opts.Categories != null && opts.Categories.Any())
            // Match events excluding specified categories if exclude is selected, 
            // else match events with categories matching specified categories
            filter = opts.Exclude
                ? Compose.And(filter, e => !opts.Categories.Contains(e.Category))
                : Compose.And(filter, e => opts.Categories.Contains(e.Category));
        if (opts.NoCategory)
            // Match events with no categories
            filter = Compose.And(filter, e => string.IsNullOrEmpty(e.Category));

        if (filter != null)
            // Filter local events if any options were chosen
            Events = Events.Where(filter).ToList();

        // Print local events
        EventManager.PrintEvents(Events);

        return 0;
    }

    /// <summary>
    /// Method for handling the "add" command
    /// </summary>
    /// <param name="opts">"add" options to handle</param>
    /// <returns>exit code 0 if successful</returns>
    public static int RunAddAndReturnExitCode(AddOptions opts)
    {
        Setup();
        // Insert new event into event manager with specified values
        // Date defaults to current date, category can be null and 
        // description is required from the user
        Em.InsertEvent(new Event(opts.Date, opts.Category, opts.Description!));

        // Print events from event manager
        EventManager.PrintEvents(Em.Events);
        return 0;
    }

    /// <summary>
    /// Method for handling the "delete" command
    /// </summary>
    /// <param name="opts">"delete" options to handle</param>
    /// <returns>exit code 0 if successful</returns>
    public static int RunDeleteAndReturnExitCode(DeleteOptions opts)
    {
        Setup();

        // Initialize function used for filtering to null
        Func<Event, bool>? filter = null;

        // Set filter to:
        if (opts.All)
            // Clear all local events
            Events.Clear();
        if (opts.Date != null)
            // Match events that don't match specified date
            filter = Compose.Or(filter, e => e.Date != opts.Date);
        if (opts.BeforeDate != null)
            // Match events equal to or after specified date
            filter = Compose.And(filter, e => e.Date >= opts.BeforeDate);
        if (opts.AfterDate != null)
            // Match events equal to of before specified date
            filter = Compose.And(filter, e => e.Date <= opts.AfterDate);
        if (!string.IsNullOrEmpty(opts.Category))
            // Match events with categories that don't match specified category
            filter = Compose.Or(filter, e => e.Category != opts.Category);
        if (!string.IsNullOrEmpty(opts.Description))
            // Match event descriptions that don't start with specified string
            filter = Compose.Or(filter, e => !e.Description.StartsWith(opts.Description));

        if (filter != null)
            // Filter local events if any options were chosen
            Events = Events.Where(filter).ToList();

        if (opts.DryRun)
        {
            // If user has chosen the "--dry-run" option,
            // do not modify events in event manager and file
            // and print locally changed events
            Console.WriteLine("Performing dry run...\nResult:");
            EventManager.PrintEvents(Events);
        }
        else
        {
            // Replace events in event manager and file with
            // local events and print them out
            Em.ReplaceEvents(Events);
            EventManager.PrintEvents(Em.Events);
        }

        return 0;
    }

    /// <summary>
    /// Loads events from event manager and sets them to local events
    /// </summary>
    private static void Setup()
    {
        Em.LoadEvents();
        Events = Em.Events;
    }
}