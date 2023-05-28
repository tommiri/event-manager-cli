# Event Manager CLI

UNIX-style command-line utility program for managing events stored in a local CSV file. This is one version of the
program made as the final project for TAMK's Advanced Programming Techniques course. The other version written in Java can
be found [here](https://github.com/tommiri/event-manager-cli/tree/main/events_java).

## Installation

Event Manager is a .NET app written in C#, so you will need to have .NET installed locally to
build the app.

Clone the project

```
  git clone https://github.com/tommiri/event-manager-cli.git
```

Go to the project directory

```
  cd event-manager-cli/events_cs/Events
```

### Building

The next steps will vary depending on whether you want to install the app globally as a command-line tool, or if you just want to build and run it.

#### To install it as a global command-line tool:

Pack the project
```
  dotnet pack
```

Install the tool globally
```
  dotnet tool install --global --add-source ./nupkg/ events.cli
```

Run the program from anywhere
```
  events [command] [options]
```

If you want to uninstall the tool you can run
```
  dotnet tool uninstall -g events.cli
```

#### To build and run the program locally:

Build the project using .NET

```
  dotnet build
```

Run the program

```
  dotnet bin/Debug/net7.0/Events.Cli.dll [command] [options]
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
arguments `list -c computing`. To add a new event, the user could run the program with
arguments `add -D 2023-01-01 -c holiday -d "New Year 2023"`. To see all available commands, the
user can run the program with no arguments or with the `--help` flag. The help flag can also be used for
specific commands to see only their available options.

### Generated documentation

The project is documented using the XML comment format, which means that it's ready for auto-generating documentation using tools such as:
- [DocFX](https://dotnet.github.io/docfx/)
- [Sandcastle](https://github.com/EWSoftware/SHFB)
- [Doxygen](https://github.com/doxygen/doxygen)

## Technologies

- [.NET](https://dotnet.microsoft.com/en-us/)
- [CommandLineParser](https://github.com/commandlineparser/commandline)
- [CsvHelper](https://joshclose.github.io/CsvHelper/)
- [Humanizer](https://github.com/Humanizr/Humanizer)
