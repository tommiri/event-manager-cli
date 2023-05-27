namespace Events.Commands;

using Utils;
using Event;
using Options;

public class CommandHandler
{
    private static EventManager? _em = EventManager.GetInstance();
    private static List<Event> Events { get; set; }

    public static int RunListAndReturnExitCode(ListOptions opts)
    {
        Setup();

        Func<Event, bool>? filter = null;

        if (opts.Today)
            filter = Compose.And(filter, e => e.Date == DateOnly.FromDateTime(DateTime.Now));
        if (opts.Date != null)
            filter = Compose.And(filter, e => e.Date == opts.Date);
        if (opts.BeforeDate != null)
            filter = Compose.And(filter, e => e.Date < opts.BeforeDate);
        if (opts.AfterDate != null)
            filter = Compose.And(filter, e => e.Date > opts.AfterDate);
        if (opts.Categories.Any())
            filter = opts.Exclude
                ? Compose.And(filter, e => !opts.Categories.Contains(e.Category))
                : Compose.And(filter, e => opts.Categories.Contains(e.Category));

        if (filter != null)
            Events = Events.Where(filter).ToList();

        EventManager.PrintEvents(Events);

        return 0;
    }

    public static int RunAddAndReturnExitCode(AddOptions opts)
    {
        Setup();
        return 0;
    }

    public static int RunDeleteAndReturnExitCode(DeleteOptions opts)
    {
        Setup();
        return 0;
    }

    private static void Setup()
    {
        var eventsPath = _em.GetEventsPath();

        if (string.IsNullOrEmpty(eventsPath)) Environment.Exit(-1);

        var success = _em.LoadEvents(eventsPath);
        if (!success)
        {
            Console.Error.WriteLine("Error loading events!");
            Environment.Exit(-1);
        }

        Events = _em.Events;
    }
}