package io.github.tommiri.events.event;

import com.opencsv.CSVReaderHeaderAware;
import com.opencsv.CSVWriterBuilder;
import com.opencsv.ICSVParser;
import com.opencsv.ICSVWriter;

import java.io.IOException;
import java.io.Reader;
import java.io.Writer;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.time.LocalDate;
import java.time.Period;
import java.time.format.DateTimeParseException;
import java.util.*;

/**
 * Singleton class for handling events.
 */
public class EventManager {
    // The singleton instance, created as necessary.
    private static EventManager instance = null;
    private List<Event> events;

    // Private constructor to prevent instantiation.
    private EventManager() {
        // Ensure that the event list will never be null.
        // Effective Java 2nd Ed, Item 43: "Return empty arrays
        // or collections, not nulls"
        this.events = new ArrayList<Event>();
    }

    /**
     * Static method to get the singleton instance.
     * Creates the instance if it does not already exist.
     *
     * @return the instance
     */
    public static EventManager getInstance() {
        if (instance == null) {
            instance = new EventManager();
        }
        return instance;
    }

    /**
     * Prints given list of events
     *
     * @param events list of events to print
     */
    public static void printEvents(List<Event> events) {
        LocalDate today = LocalDate.now();

        if (events.isEmpty()) {
            System.out.println("No events found!");
        } else {
            for (Event event : events) {
                System.out.print(event + " -- ");

                Period difference = Period.between(event.getDate(), today);

                System.out.println(Event.getDifferenceString(difference));
            }
        }
    }

    /**
     * Gets the path to the user's events file.
     *
     * @return the path, or null if not found
     */
    public Path getEventsPath() {
        String userHomeDirectory = System.getProperty("user.home");
        if (userHomeDirectory.isBlank()) {
            System.err.println("Unable to determine user home directory");
            return null;
        }

        Path eventsDir = Paths.get(userHomeDirectory, ".events");
        if (Files.notExists(eventsDir)) {
            System.err.println(eventsDir + " directory does not exist, please create it");
            return null;
        }
        Path eventsPath = eventsDir.resolve("events.csv");
        if (Files.notExists(eventsPath)) {
            System.err.println(eventsPath + " file not found");
            return null;
        }

        return eventsPath;
    }

    /**
     * Loads the events from the file given in eventsPath.
     *
     * @param eventsPath the path to the events file
     * @return true if successful, false if there was an error
     */
    public boolean loadEvents(Path eventsPath) {
        List<Event> newEvents = new ArrayList<Event>();

        // Read all the lines from the events file using
        // our helper based on opencsv.
        try {
            List<Map<String, String>> lineMaps = readLineByLine(eventsPath);

            for (Map<String, String> map : lineMaps) {
                LocalDate date;
                String dateString = map.get("date");
                try {
                    date = LocalDate.parse(dateString);

                    Event event = new Event(
                            date,
                            map.get("category"),
                            map.get("description")
                    );

                    newEvents.add(event);
                } catch (DateTimeParseException dtpe) {
                    System.err.println("bad date: " + dateString);
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }

        // We got here, so loading and parsing succeeded.
        // It should now be safe to update the event list.
        // We'll just construct a new list and let the old
        // one be garbage collected.
        this.events = new ArrayList<Event>(newEvents);

        return true;
    }

    // Read the CSV file one line at a time using a header-aware
    // CSV reader from the opencsv library. Discards invalid lines.
    private List<Map<String, String>> readLineByLine(Path filePath)
            throws Exception {
        List<Map<String, String>> list = new ArrayList<>();
        try (Reader reader = Files.newBufferedReader(filePath)) {
            try (CSVReaderHeaderAware csvReader = new CSVReaderHeaderAware(reader)) {
                Map<String, String> map;
                while ((map = csvReader.readMap()) != null) {
                    list.add(map);
                }
            }
        }
        return list;
    }

    /**
     * Saves the events to the file at `eventsPath`.
     *
     * @param eventsPath the path to the events file
     * @return true if successful, false if there was an error
     */
    public boolean saveEvents(Path eventsPath) {
        // If there are no events, there is nothing to save,
        // but it can still be considered a success.
        if (this.events.size() == 0) {
            return true;
        }

        // Sort events on save so added or deleted events don't just get
        // appended to the end
        Collections.sort(this.events);

        try {
            Writer writer = Files.newBufferedWriter(
                    eventsPath,
                    StandardCharsets.UTF_8
            );

            ICSVWriter csvWriter = new CSVWriterBuilder(writer)
                    .withSeparator(ICSVParser.DEFAULT_SEPARATOR)
                    .withQuoteChar(ICSVParser.DEFAULT_QUOTE_CHARACTER)
                    .withEscapeChar(ICSVParser.DEFAULT_ESCAPE_CHARACTER)
                    .withLineEnd(ICSVWriter.DEFAULT_LINE_END)
                    .build();

            // Write the header row
            csvWriter.writeNext(
                    new String[]{
                            "date",
                            "category",
                            "description"
                    }
            );

            // Write the events to the CSV file
            for (Event event : this.events) {
                String[] entries = new String[]{
                        event.getDate().toString(),
                        event.getCategory(),
                        event.getDescription()
                };
                csvWriter.writeNext(entries);
            }

            csvWriter.close();
        } catch (IOException ioe) {
            ioe.printStackTrace();
            return false;
        }

        return true;
    }

    /**
     * Gets a sorted list of events.
     *
     * @return the event list
     */
    public List<Event> getEvents() {
        return this.events;
    }

    /**
     * Inserts new event into existing event list.
     *
     * @param event event to insert
     */
    public void insertEvent(Event event) {
        this.events.add(event);
        if (!this.saveEvents(this.getEventsPath())) {
            System.err.println("ERROR: Failed to save events to file!");
            System.exit(-1);
        }
        System.out.println("Successfully added new event!");
    }

    /**
     * Replaces existing events with given events.
     *
     * @param events events to replace existing events with
     */
    public void replaceEvents(List<Event> events) {
        int previousSize = this.events.size();

        this.events = events;
        if (!this.saveEvents(this.getEventsPath())) {
            System.err.println("ERROR: Failed to save events to file!");
            System.exit(-1);
        }

        // If new events are the same length as previously, no items were deleted
        if (previousSize == events.size()) {
            System.out.println("No events affected!");
        } else {
            System.out.println("Successfully removed event(s)!");
        }
    }

    /**
     * Gets a sorted list of all the categories across all events.
     *
     * @return the category list
     */
    public List<String> getCategories() {
        // Each category string may be added to the set
        // multiple times, but there will be only one of each.
        Set<String> categories = new HashSet<String>();
        for (Event event : this.events) {
            categories.add(event.getCategory());
        }

        // Create a new list based on the set of categories,
        // then sort it.
        List<String> result = new ArrayList<String>(categories);
        Collections.sort(result);
        return result;
    }
}