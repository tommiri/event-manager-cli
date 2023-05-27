namespace Events.Event;

using CsvHelper.Configuration;

public class EventMap : ClassMap<Event>
{
    public EventMap()
    {
        Parameter("date").Name("date");
        Parameter("category").Name("category");
        Parameter("description").Name("description");
    }
}