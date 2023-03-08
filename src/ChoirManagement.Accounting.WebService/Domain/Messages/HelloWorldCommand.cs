using NEvo.Messaging.Commands;

namespace ChoirManagement.Accounting.Messages;

public record HelloWorldCommand(string Message) : Command;
