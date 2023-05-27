# Event Manager CLI

UNIX-style command-line utility program for managing events stored in a local CSV file. This is one version of the
program made as the final project for TAMK's Advanced Programming Techniques course. The other version written in C# can
be found [here](https://github.com/tommiri/event-manager-cli/tree/main/Events_CS).

## Installation

Event Manager is written in Java and uses Maven for project management, so you will need to have both installed to
compile and run the program.

Clone the project

```
  git clone https://github.com/tommiri/event-manager-cli.git
```

Go to the project directory

```
  cd event-manager-cli/events_java
```

Clean and compile the program using Maven

```
  mvn clean package
```

Run the program

```
  java -jar target/Events.jar
```

## Usage/Examples

The program requires a CSV file to read the events to display from. Create the `~/.events` directory if it doesn't
already exist and add a file called `events.csv` inside that directory, with contents in the following format:

```
date,category,description
2020-12-15,computing,C++20 released
2023-01-10,computing,Rust 1.66.1 released
2022-09-20,computing,Java SE 19 released
2014-11-12,computing,.NET Core released
```

The format of each line should be `date,category,description` in the CSV (comma-separated value) style. Here `date` is
the date in ISO 8601 standard format `YYYY-MM-DD`, while `category` and `description` are free text.

Note that the first line in the file is a column header, and should be included as such.

Ideally, the value of the `category` column is a single word like `history`, `personal` or `computing`, but there are no
predefined categories.

NOTE: If your description contains commas, you will need to put it in quotation marks, because otherwise the commas
would mess up the CSV parsing. For example, a line with a description containing commas could be:

```
2010-02-14,personal,"Signed, sealed, and delivered"
```

### How to use

To manage the events, the user can use the `list`, `add` and `delete` commands, and their respective options. For
example, to list all events in the `computing` category, the user could run the program with
arguments `list --category computing`. To add a new event, the user could run the program with
arguments `add --date 2023-01-01 --category holiday --description "New Year 2023"`. To see all available options, the
user can run the program with no arguments or with the `-h` or `--help` flags. The help flags can also be used for
specific commands to see only their available options.

### Generated documentation

You can use the `javadoc` tool with Maven to generate documentation for the app:

```
mvn javadoc:javadoc
```

The documentation is generated in the `apidocs` directory within `target/site`.

## Technologies

- [Maven](https://maven.apache.org/)
- [JCommander](https://jcommander.org/)
- [OpenCSV](https://opencsv.sourceforge.net/)

## Acknowledgements

The Event class and most of the EventManager class was supplied by the course's
teacher [Jere Käpyaho](https://github.com/jerekapyaho). See the project this project is based
on [here](https://github.com/jerekapyaho/days_java).
