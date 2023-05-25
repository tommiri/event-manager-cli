package io.github.tommiri.events.args;

import com.beust.jcommander.JCommander;
import com.beust.jcommander.ParameterException;

/**
 * Singleton class for handling command line arguments.
 */
public class ArgsHandler {
    private static ArgsHandler instance = null;

    // Private constructor to prevent instantiation.
    private ArgsHandler() {
    }

    /**
     * Static method to get the singleton instance.
     * Creates the instance if it does not already exist.
     *
     * @return the instance
     */
    public static ArgsHandler getInstance() {
        if (instance == null) {
            instance = new ArgsHandler();
        }
        return instance;
    }

    /**
     * Method for handling command line arguments.
     * Displays program usage if specified or if
     * input is invalid.
     *
     * @param args command-line arguments
     */
    public void handleArgs(String... args) {
        // Initialize variables for all commands
        CommandBase baseCmd = new CommandBase();
        CommandList listCmd = new CommandList();
        CommandAdd addCmd = new CommandAdd();
        CommandDelete deleteCmd = new CommandDelete();

        // Build JCommander with commands
        JCommander jc = JCommander.newBuilder()
                .addObject(baseCmd)
                .addCommand("list", listCmd)
                .addCommand("add", addCmd)
                .addCommand("delete", deleteCmd)
                .build();

        // Set program name for usage information
        jc.setProgramName("java -jar Events.jar");

        try {
            jc.parse(args);
            String parsedCommand = jc.getParsedCommand();

            CommandHandler ch = CommandHandler.getInstance();

            // Display usage and exit if no command was
            // entered or user specifies the "-h" flag
            if (parsedCommand == null) {
                displayUsage(jc);
            } else if (ch.isHelpRequested(listCmd, addCmd, deleteCmd)) {
                displayUsage(jc, parsedCommand);
            }

            // Handle parsed command based on user input
            switch (parsedCommand) {
                case "list" -> ch.handleListCommand(listCmd);
                case "add" -> ch.handleAddCommand(addCmd);
                case "delete" -> ch.handleDeleteCommand(deleteCmd);
            }

        } catch (ParameterException e) {
            System.err.println(e.getMessage());
            displayUsage(jc);
        }

    }

    /**
     * Display usage for specific command
     *
     * @param jc      JCommander instance
     * @param command command to show usage for
     */
    private void displayUsage(JCommander jc, String command) {
        jc.getUsageFormatter().usage(command);
        System.exit(0);
    }

    /**
     * Display usage for whole program
     *
     * @param jc JCommander instance
     */
    private void displayUsage(JCommander jc) {
        jc.usage();
        System.exit(0);
    }
}