using System.Globalization;

namespace Events.Event;

using CsvHelper.Configuration.Attributes;
using System.Text;
using Humanizer;
using Humanizer.Localisation;

/// <summary>
/// Represents an event
/// </summary>
public class Event : IComparable<Event>
{
    /// <summary>
    /// Constructs an event with the given date, category and description
    /// </summary>
    /// <param name="date">the date of the event</param>
    /// <param name="category">the category of the event</param>
    /// <param name="description">the description of the event</param>
    public Event(DateOnly date, string? category, string description)
    {
        Date = date;
        Category = category;
        Description = description;
    }

    // Getters for class members. Also set name tags for CSVHelper
    [Name("date")] public DateOnly Date { get; }
    [Name("category")] public string? Category { get; }
    [Name("description")] public string Description { get; }

    /// <summary>
    /// Helper method to get spelled-out timespan
    /// </summary>
    /// <param name="difference">the timespan</param>
    /// <returns>the timespan as a humanized string</returns>
    public static string GetDifferenceString(TimeSpan difference)
    {
        var sb = new StringBuilder();

        // If the amount of days is zero, must be today
        if (difference.Days == 0)
        {
            sb.Append("today");
            return sb.ToString();
        }

        // Humanize the timespan to display years, months and days
        var humanizedTimespan = difference.Humanize(maxUnit: TimeUnit.Year, minUnit: TimeUnit.Day, precision: 3);

        sb.Append(humanizedTimespan);

        // Add proper pre/postposition depending on whether timespan
        // is positive or negative
        if (difference.TotalDays < 0)
            sb.Insert(0, "in ");
        else
            sb.Append(" ago");


        return sb.ToString();
    }

    /// <summary>
    /// Returns a string representation of this event.
    /// </summary>
    /// <returns>the event string</returns>
    public override string ToString()
    {
        var categoryString = string.IsNullOrEmpty(Category) ? "" : $" ({Category})";
        var isoDate = Date.ToString("o", CultureInfo.InvariantCulture);
        return $"{isoDate}: {Description}{categoryString}";
    }

    // IComparable<T> implementation:

    /// <summary>
    /// Compares this event to another based on their dates.
    /// </summary>
    /// <param name="other">the event the compare to</param>
    /// <returns>Less than zero if this instance is earlier than value.
    /// Greater than zero if this instance is later than value.
    /// Zero if this instance is the same as value</returns>
    public int CompareTo(Event? other)
    {
        return Date.CompareTo(other?.Date);
    }

    /// <summary>
    /// Compares this event's date to given date
    /// </summary>
    /// <param name="date">the date the compare to</param>
    /// <returns>Less than zero if this instance is earlier than value.
    /// Greater than zero if this instance is later than value.
    /// Zero if this instance is the same as value</returns>
    public int CompareTo(DateOnly date)
    {
        return Date.CompareTo(date);
    }
}