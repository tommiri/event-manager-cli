package io.github.tommiri.events.utils;

import java.util.List;
import java.util.function.Predicate;
import java.util.stream.Collectors;

/**
 * Utility class for filtering
 */
public class FilterUtils {
    /**
     * Generic method for composing a predicate by short-circuiting
     * logical AND
     *
     * @param previous previous predicate
     * @param next     new predicate
     * @param <T>      Type of predicates
     * @return new predicate is previously null, otherwise composed predicate
     */
    public static <T> Predicate<T> composePredicateAnd(Predicate<T> previous, Predicate<T> next) {
        if (previous == null) {
            return next;
        } else {
            // Compose new predicate from previous and next
            return previous.and(next);
        }
    }

    /**
     * Generic method for composing a predicate by short-circuiting
     * logical OR
     *
     * @param previous previous predicate
     * @param next     new predicate
     * @param <T>      Type of predicates
     * @return new predicate is previously null, otherwise composed predicate
     */
    public static <T> Predicate<T> composePredicateOr(Predicate<T> previous, Predicate<T> next) {
        if (previous == null) {
            return next;
        } else {
            // Compose new predicate from previous and next
            return previous.or(next);
        }
    }

    /**
     * Generic method for filtering a list with given predicate
     *
     * @param items     list to filter
     * @param predicate predicate to filter list with
     * @param <T>       Type of list and predicate
     * @return the list filtered
     */
    public static <T> List<T> filterList(List<T> items, Predicate<T> predicate) {
        if (predicate == null) {
            return items;
        }
        return items.stream().filter(predicate).collect(Collectors.toList());
    }
}