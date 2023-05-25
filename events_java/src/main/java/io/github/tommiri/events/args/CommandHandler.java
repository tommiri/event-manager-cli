package io.github.tommiri.events.args;

import io.github.tommiri.events.event.Event;
import io.github.tommiri.events.event.EventManager;
import io.github.tommiri.events.event.EventUtils;

import java.nio.file.Path;
import java.time.LocalDate;
import java.util.Collections;
import java.util.List;
import java.util.function.Predicate;

import static io.github.tommiri.events.event.EventUtils.isEventCategoryInList;
import static io.github.tommiri.events.utils.FilterUtils.*;

/**
 * Singleton class for handling command line arguments.
 */
public class CommandHandler {
    private static CommandHandler instance = null;
    final private EventManager em;
    private List<Event> events;

    // Private constructor to prevent instantiation.
    private CommandHandler() {
        this.em = EventManager.getInstance();
        Path eventsPath = em.getEventsPath();

        if (eventsPath == null) {
            System.exit(-1);
        }

        boolean success = em.loadEvents(eventsPath);
        if (!success) {
            System.err.println("Error loading events");
            System.exit(-1);
        }

        // Assign current events to local member
        this.events = em.getEvents();
    }

    /**
     * Static method to get the singleton instance.
     * Creates the instance if it does not already exist.
     *
     * @return the instance
     */
    public static CommandHandler getInstance() {
        if (instance == null) {
            instance = new CommandHandler();
        }
        return instance;
    }


    /**
     * Method for handling the "list" command
     *
     * @param cmd "list" command to handle
     */
    public void handleListCommand(CommandList cmd) {
        // Validate user input
        cmd.validateOptions();

        // Initialize predicate used for filtering to null
        Predicate<Event> filterPredicate = null;

        if (cmd.today) {
            // Set predicate to true if event's day matches today's date
            filterPredicate = composePredicateAnd(filterPredicate, event -> event.compareTo(LocalDate.now()) == 0);
        }
        if (cmd.date != null) {
            // Set predicate to true if event's day matches given date
            filterPredicate = composePredicateAnd(filterPredicate, event -> event.compareTo(cmd.date) == 0);
        }
        if (cmd.before_date != null) {
            // Set predicate to true if event's day is before given date
            filterPredicate = composePredicateAnd(filterPredicate, event -> event.compareTo(cmd.before_date) < 0);
        }
        if (cmd.after_date != null) {
            // Set predicate to true if event's day is after given date
            filterPredicate = composePredicateAnd(filterPredicate, event -> event.compareTo(cmd.after_date) > 0);
        }
        if (cmd.categories != null) {
            if (cmd.exclude) {
                // Set predicate to true if event's category is not in list
                filterPredicate = composePredicateAnd(filterPredicate, event -> !isEventCategoryInList(event, cmd.categories));
            } else {
                // Set predicate to true if event's category is in list
                filterPredicate = composePredicateAnd(filterPredicate, event -> isEventCategoryInList(event, cmd.categories));
            }
        }
        if (cmd.no_category) {
            // Set predicate to true if event has no category
            filterPredicate = composePredicateAnd(filterPredicate, EventUtils::isEventCategoryUndefined);
        }

        // Set local events to events filtered with predicate
        // No changes are made to file
        this.events = filterList(this.events, filterPredicate);

        // Print local events
        EventManager.printEvents(this.events);
    }

    /**
     * Method for handling "add" command
     *
     * @param cmd "add" command to handle
     */
    public void handleAddCommand(CommandAdd cmd) {
        // Insert new event to event manager and file
        em.insertEvent(new Event(cmd.date, cmd.category, cmd.description));

        // Print events from event manager
        EventManager.printEvents(em.getEvents());
    }

    /**
     * Method for handling "delete" command
     *
     * @param cmd "delete" command to handle
     */
    public void handleDeleteCommand(CommandDelete cmd) {
        // Validate user input
        cmd.validateOptions();

        // Initialize predicate used for filtering to null
        Predicate<Event> filterPredicate = null;

        if (cmd.all) {
            // Clear local events
            events.clear();
        }
        if (cmd.date != null) {
            // Set predicate to true if event's date doesn't match given date
            filterPredicate = composePredicateOr(filterPredicate, event -> event.compareTo(cmd.date) != 0);
        }
        if (cmd.before_date != null) {
            // Set predicate to true if event's date isn't before given date
            filterPredicate = composePredicateAnd(filterPredicate, event -> event.compareTo(cmd.before_date) >= 0);
        }
        if (cmd.after_date != null) {
            // Set predicate to true if event's date isn't after given date
            filterPredicate = composePredicateAnd(filterPredicate, event -> event.compareTo(cmd.after_date) <= 0);
        }
        if (cmd.category != null) {
            // Set predicate to true if event's category doesn't match given category
            filterPredicate = composePredicateOr(filterPredicate, event -> !isEventCategoryInList(event,
                    Collections.singletonList(cmd.category)));
        }
        if (cmd.description != null) {
            // Set predicate to true if event's description doesn't start with given string
            filterPredicate = composePredicateOr(filterPredicate,
                    event -> !event.getDescription().startsWith(cmd.description));
        }

        // Filter local events with predicate
        events = filterList(events, filterPredicate);

        if (cmd.dry_run) {
            // If user has chosen the "--dry-run" option,
            // do not modify events in event manager and file
            // and print locally changed events
            System.out.println("Performing dry run...\nResult:");
            EventManager.printEvents(events);
        } else {
            // Replace events in event manager and file with
            // local events and print them out
            em.replaceEvents(events);
            EventManager.printEvents(em.getEvents());
        }
    }

    /**
     * Method for checking if the help flag on a command
     * has been triggered
     *
     * @param commands command objects to check
     * @return true if user has specified the "-h" flag, otherwise false
     */
    public boolean isHelpRequested(CommandBase... commands) {
        for (CommandBase cmd : commands) {
            if (cmd.help) {
                return true;
            }
        }
        return false;
    }
}