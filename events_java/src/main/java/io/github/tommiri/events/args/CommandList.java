package io.github.tommiri.events.args;

import com.beust.jcommander.Parameter;
import com.beust.jcommander.ParameterException;
import com.beust.jcommander.Parameters;
import io.github.tommiri.events.utils.LocalDateConverter;

import java.time.LocalDate;
import java.util.List;

/**
 * Command for listing events
 */
@Parameters(commandNames = "list", commandDescription = "List events")
public class CommandList extends CommandBase {
    @Parameter(names = "--today", description = "List events happening today")
    public boolean today;
    @Parameter(names = "--before-date", description = "List events before specified date", converter = LocalDateConverter.class)
    public LocalDate before_date;
    @Parameter(names = "--after-date", description = "List events after specified date", converter = LocalDateConverter.class)
    public LocalDate after_date;
    @Parameter(names = "--date", description = "List events on specified date", converter = LocalDateConverter.class)
    public LocalDate date;
    @Parameter(names = "--categories", description = "List events from specified categories")
    public List<String> categories;
    @Parameter(names = "--exclude", description = "Exclude specified categories")
    public boolean exclude;
    @Parameter(names = "--no-category", description = "List events with no category")
    public boolean no_category;

    /**
     * Method for validating user input for list command
     *
     * @throws ParameterException in case user tries to exclude without specifying categories
     */
    public void validateOptions() throws ParameterException {
        if (exclude && categories == null) {
            throw new ParameterException("Cannot use \"--exclude\" without \"--categories\"!");
        }
    }
}