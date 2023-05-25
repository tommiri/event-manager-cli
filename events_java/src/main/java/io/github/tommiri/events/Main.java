package io.github.tommiri.events;

import io.github.tommiri.events.args.ArgsHandler;

public class Main {
    /**
     * Main program.
     *
     * @param args command-line arguments
     */
    public static void main(String... args) {
        ArgsHandler argsHandler = ArgsHandler.getInstance();
        argsHandler.handleArgs(args);
    }
}