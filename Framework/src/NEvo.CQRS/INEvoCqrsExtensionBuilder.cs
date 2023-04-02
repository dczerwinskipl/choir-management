using NEvo.CQRS.Processing.Registering;
using NEvo.CQRS.Routing;

namespace NEvo.Core;

/// <summary>
/// Public interface to configure additional options of CQRS
/// </summary>
public interface INEvoCqrsExtensionBuilder
{
    INEvoCqrsExtensionBuilder UseCustomMessageHandlers(params MessageHandlerOptions[] messageHandlerOptions);
    INEvoCqrsExtensionBuilder ConfigureRoutingTopology(Action<RoutingTopologyDescription> topologyConfiguration);
    INEvoCqrsExtensionBuilder ConfigureRoutingPolicy(Action<RoutingPolicyDescription> policyConfiguration);
}
