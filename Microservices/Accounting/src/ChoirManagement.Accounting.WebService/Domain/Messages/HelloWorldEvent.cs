﻿using NEvo.CQRS.Messaging.Events;
using NEvo.ValueObjects;

namespace ChoirManagement.Accounting.Messages;

public record HelloWorldEvent(string Message, ObjectId Source) : Event(Source);
