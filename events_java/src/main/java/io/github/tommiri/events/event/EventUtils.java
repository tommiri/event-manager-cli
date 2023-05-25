package io.github.tommiri.events.event;

import java.util.List;

/**
 * Utility class for events
 */
public class EventUtils {
    /**
     * Method for comparing an event's category to a
     * given list of categories
     *
     * @param event      event to look for
     * @param categories list of categories to check
     * @return whether the event's category was found in the list
     */
    public static boolean isEventCategoryInList(Event event, List<String> categories) {
        return categories.contains(event.getCategory());
    }

    /**
     * Method for checking if event has no category
     *
     * @param event event to check
     * @return whether the event's category didn't exist
     */
    public static boolean isEventCategoryUndefined(Event event) {
        return event.getCategory().isEmpty();
    }
}