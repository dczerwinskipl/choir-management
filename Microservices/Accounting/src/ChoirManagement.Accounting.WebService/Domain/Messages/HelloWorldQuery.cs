﻿using NEvo.CQRS.Messaging.Queries;

namespace ChoirManagement.Accounting.Messages;
public record HelloWorldQuery() : Query<string>;
