namespace Events.Event;

using System.Globalization;
using CsvHelper;

/// <summary>
/// Singleton class for handling events.
/// </summary>
public class EventManager
{
    // The singleton instance, created as necessary.
    private static EventManager? _instance;
    public List<Event> Events { get; private set; }

    /// <summary>
    /// Private constructor to prevent instantiation.
    /// </summary>
    private EventManager()
    {
        // Ensure the event list will never be null
        Events = new List<Event>();
    }

    /// <summary>
    /// Static method to get the singleton instance.
    /// Creates the instance if it does not already exist.
    /// </summary>
    /// <returns>the instance</returns>
    public static EventManager GetInstance()
    {
        return _instance ??= new EventManager();
    }

    /// <summary>
    /// Prints given list of events
    /// </summary>
    /// <param name="events">list of events to print</param>
    public static void PrintEvents(List<Event> events)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        if (!events.Any()) Console.WriteLine("No events found!");
        else
            events.ForEach(e =>
            {
                Console.Write($"{e} -- ");

                var difference = TimeSpan.FromDays(today.DayNumber - e.Date.DayNumber);

                Console.WriteLine(Event.GetDifferenceString(difference));
            });
    }

    /// <summary>
    /// Get the path to the user's events.csv file
    /// </summary>
    /// <returns>the path if found</returns>
    private string GetEventsPath()
    {
        var userHomeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (string.IsNullOrEmpty(userHomeDirectory))
        {
            Console.Error.WriteLine("Unable to determine user home directory");
            Environment.Exit(-1);
        }

        var eventsDir = Path.GetFullPath($"{userHomeDirectory}/.events");
        if (!Directory.Exists(eventsDir))
        {
            Console.Error.WriteLine($"{eventsDir} directory does not exist, please create it.");
            Environment.Exit(-1);
        }

        var eventsPath = Path.Combine(eventsDir, "events.csv");
        if (!File.Exists(eventsPath))
        {
            Console.Error.WriteLine($"{eventsPath} file not found!");
            Environment.Exit(-1);
        }

        return eventsPath;
    }

    /// <summary>
    /// Loads the events from file
    /// </summary>
    public void LoadEvents()
    {
        try
        {
            using var reader = new StreamReader(GetEventsPath());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var newEvents = csv.GetRecords<Event>().ToList();
            Events = newEvents;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("ERROR: Failed to load events!");
            Console.Error.WriteLine(e.InnerException is not null ? e.InnerException.Message : e.Message);
            Environment.Exit(-1);
        }
    }

    /// <summary>
    /// Save the events to file
    /// </summary>
    private void SaveEvents()
    {
        // If there are no events, there is nothing to save,
        // but it can still be considered a success.
        if (!Events.Any()) return;

        // Sort events on save so added or deleted events don't 
        // just get appended to the end
        Events.Sort();

        try
        {
            using var writer = new StreamWriter(GetEventsPath());
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            // Force ISO 8601 format for dates
            csv.Context.TypeConverterOptionsCache.GetOptions<DateOnly>().Formats = new[] { "o" };

            csv.WriteRecords(Events);
            csv.Flush();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("ERROR: Failed to save events to file!");
            Console.Error.WriteLine(e.InnerException is not null ? e.InnerException.Message : e.Message);
            Environment.Exit(-1);
        }
    }

    /// <summary>
    /// Insert new event into existing event list
    /// </summary>
    /// <param name="e">event to insert</param>
    public void InsertEvent(Event e)
    {
        Events.Add(e);
        SaveEvents();
        Console.WriteLine("Successfully added new event!");
    }

    /// <summary>
    /// Replace existing events with given events
    /// </summary>
    /// <param name="events">events to replace existing events with</param>
    public void ReplaceEvents(List<Event> events)
    {
        var previousSize = Events.Count;

        Events = events;
        SaveEvents();
        Console.WriteLine(previousSize == events.Count ? "No events affected!" : "Successfully removed event(s)!");
    }
}