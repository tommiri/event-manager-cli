using CommandLine;
using Events.Commands;
using Events.Commands.Options;

// Parse arguments and handle commands
return Parser.Default.ParseArguments<ListOptions, AddOptions, DeleteOptions>(args).MapResult(
    (ListOptions opts) => CommandHandler.RunListAndReturnExitCode(opts),
    (AddOptions opts) => CommandHandler.RunAddAndReturnExitCode(opts),
    (DeleteOptions opts) => CommandHandler.RunDeleteAndReturnExitCode(opts),
    _ => -1);