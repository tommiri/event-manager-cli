package io.github.tommiri.events.args;

import com.beust.jcommander.Parameter;
import com.beust.jcommander.ParameterException;
import com.beust.jcommander.Parameters;
import io.github.tommiri.events.utils.LocalDateConverter;

import java.time.LocalDate;

/**
 * Command for deleting events
 */
@Parameters(commandNames = "delete", commandDescription = "Delete events")
public class CommandDelete extends CommandBase {
    @Parameter(names = "--date", description = "Deletes all events with specified date", converter =
            LocalDateConverter.class)
    public LocalDate date;
    @Parameter(names = "--before-date", description = "Delete all events before specified date", converter =
            LocalDateConverter.class)
    public LocalDate before_date;
    @Parameter(names = "--after-date", description = "Delete all events after specified date", converter =
            LocalDateConverter.class)
    public LocalDate after_date;
    @Parameter(names = "--category", description = "Delete all events with specified category")
    public String category;
    @Parameter(names = "--description", description = "Delete all events with descriptions starting with specified " +
            "string")
    public String description;
    @Parameter(names = "--all", description = "Delete all events")
    public boolean all;
    @Parameter(names = "--dry-run", description = "Display results of executing command without actually executing " +
            "it")
    public boolean dry_run;

    /**
     * Method for validating user input for delete command
     *
     * @throws ParameterException user input is invalid
     */
    public void validateOptions() throws ParameterException {
        if (date == null && before_date == null && after_date == null && category == null && description == null && !all) {
            if (dry_run) {
                throw new ParameterException("\"--dry-run\" requires at least one other option!");
            }
            throw new ParameterException("At least one option is required!");
        }

        if (all && (date != null || before_date != null || after_date != null || category != null || description != null)) {
            throw new ParameterException("Cannot have other options with \"--all\"!");
        }
    }

}