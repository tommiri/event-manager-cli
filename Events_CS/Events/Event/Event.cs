namespace Events.Event;

using CsvHelper.Configuration.Attributes;
using System.Text;
using Humanizer;
using Humanizer.Localisation;

public class Event : IComparable<Event>
{
    public Event(DateOnly date, string category, string description)
    {
        Date = date;
        Category = category;
        Description = description;
    }

    [Name("date")] public DateOnly Date { get; }
    [Name("category")] public string Category { get; }
    [Name("description")] public string Description { get; }

    public static string GetDifferenceString(TimeSpan difference)
    {
        var sb = new StringBuilder();

        if (difference.Days == 0)
        {
            sb.Append("today");
            return sb.ToString();
        }

        var humanizedTimespan = difference.Humanize(maxUnit: TimeUnit.Year, minUnit: TimeUnit.Day, precision: 3);

        sb.Append(humanizedTimespan);

        if (difference.TotalDays < 0)
            sb.Insert(0, "in ");
        else
            sb.Append(" ago");


        return sb.ToString();
    }

    public override string ToString()
    {
        var categoryString = string.IsNullOrEmpty(Category) ? "" : $" ({Category})";

        return $"{Date}: {Description}{categoryString}";
    }

    public int CompareTo(Event other)
    {
        return Date.CompareTo(other.Date);
    }

    public int CompareTo(DateOnly date)
    {
        return Date.CompareTo(date);
    }
}