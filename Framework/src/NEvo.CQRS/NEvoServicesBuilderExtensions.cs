﻿using Microsoft.Extensions.Configuration;
using NEvo.CQRS.Processing.Registering;

namespace NEvo.Core;

public static class NEvoServicesBuilderExtensions
{
    public static NEvoServicesBuilder AddCqrs(this NEvoServicesBuilder builder, params MessageHandlerOptions[] messageHandlerOptions)
    {
        builder.UseExtension(new NEvoCqrsExtensionBuilder(messageHandlerOptions));
        return builder;
    }
    public static NEvoServicesBuilder AddCqrs(this NEvoServicesBuilder builder, Action<INEvoCqrsExtensionBuilder>? configureCqrs = null)
        => AddCqrs(builder, null, configureCqrs);

    public static NEvoServicesBuilder AddCqrs(this NEvoServicesBuilder builder, IConfiguration? configuration, Action<INEvoCqrsExtensionBuilder>? configureCqrs = null)
    {
        var cqrsBuilder = new NEvoCqrsExtensionBuilder();
        if (configuration is not null)
            cqrsBuilder.UseConfiguration(configuration);

        configureCqrs?.Invoke(cqrsBuilder);
        builder.UseExtension(cqrsBuilder);

        return builder;
    }
}
