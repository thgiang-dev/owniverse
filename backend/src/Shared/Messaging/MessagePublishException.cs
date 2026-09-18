namespace Owniverse.Shared.Messaging;

public sealed class MessagePublishException() : Exception(
    "Message publication was not confirmed. Delivery outcome may be unknown.");
