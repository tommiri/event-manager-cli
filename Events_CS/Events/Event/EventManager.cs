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
    public static EventManager? GetInstance()
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
    /// <returns>the path if found, otherwise null</returns>
    public string? GetEventsPath()
    {
        var userHomeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (string.IsNullOrEmpty(userHomeDirectory))
        {
            Console.Error.WriteLine("Unable to determine user home directory");
            return null;
        }

        var eventsDir = Path.GetFullPath($"{userHomeDirectory}/.events");
        if (!Directory.Exists(eventsDir))
        {
            Console.Error.WriteLine($"{eventsDir} directory does not exist, please create it.");
            return null;
        }

        var eventsPath = Path.Combine(eventsDir, "events.csv");
        if (!File.Exists(eventsPath))
        {
            Console.Error.WriteLine($"{eventsPath} file not found!");
            return null;
        }

        return eventsPath;
    }

    /// <summary>
    /// Loads the events from the file given in eventsPath
    /// </summary>
    /// <param name="eventsPath">the path to the events.csv file</param>
    /// <returns>true is successful, otherwise false</returns>
    public bool LoadEvents(string? eventsPath)
    {
        try
        {
            using var reader = new StreamReader(eventsPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var newEvents = csv.GetRecords<Event>().ToList();
            Events = newEvents;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.InnerException is not null ? $"Error: {e.InnerException.Message}" : e.Message);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Save the events to the file at `eventsPath`
    /// </summary>
    /// <param name="eventsPath">the path to the events.csv file</param>
    /// <returns>true if successful, otherwise false</returns>
    public bool SaveEvents(string? eventsPath)
    {
        // If there are no events, there is nothing to save,
        // but it can still be considered a success.
        if (!Events.Any()) return true;

        // Sort events on save so added or deleted events don't 
        // just get appended to the end
        Events.Sort();

        try
        {
            using var writer = new StreamWriter(eventsPath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(Events);
            csv.Flush();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.InnerException is not null ? $"Error: {e.InnerException.Message}" : e.Message);
            return false;
        }

        return true;
    }
}