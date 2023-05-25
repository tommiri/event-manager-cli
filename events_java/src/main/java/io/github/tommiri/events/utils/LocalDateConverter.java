package io.github.tommiri.events.utils;

import com.beust.jcommander.ParameterException;
import com.beust.jcommander.converters.BaseConverter;

import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.time.format.DateTimeParseException;

/**
 * Converter class for converting string to LocalDate
 */
public class LocalDateConverter extends BaseConverter<LocalDate> {
    // Format for desired date pattern
    private static final SimpleDateFormat DATE_FORMAT = new SimpleDateFormat("yyyy-MM-dd");

    /**
     * Public constructor that invokes BaseConverter constructor
     *
     * @param optionName desired option
     */
    public LocalDateConverter(String optionName) {
        super(optionName);
    }

    /**
     * Method for converting string to LocalDate
     *
     * @param value string to parse
     * @return the LocalDate parsed from the string
     */
    public LocalDate convert(String value) {
        try {
            return LocalDate.parse(value);
        } catch (DateTimeParseException var3) {
            throw new ParameterException(this.getErrorString(value, String.format("an ISO-8601 formatted date (%s)", DATE_FORMAT.toPattern())));
        }
    }
}