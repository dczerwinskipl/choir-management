﻿namespace NEvo.Processing.Registering;

public record MessageHandlerOptions(Type HandlerInterface, IMessageHandlerAdapterFactory MessageHandlerAdapterFactory);
