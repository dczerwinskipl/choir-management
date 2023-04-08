local services = import '../services.libsonnet';

{
  "NEvo.CQRS": {
    "Topology": {
      "Endpoints": {
        "Membership": {
          "ChannelType": "NEvo.CQRS.Transporting.RestTransportChannel",
          "Endpoint": "http://" + services.membership.name + ":" + services.membership.port + "/api"
        },
        "Accounting": {
          "ChannelType": "NEvo.CQRS.Transporting.RestTransportChannel",
          "Endpoint": "http://" + services.accounting.name + ":" + services.accounting.port + "/api"
        }
      },
      "Topics": {
        "Membership": {
          "ChannelType": "NEvo.Azure.Transporting.AzureServiceBusTransportChannel",
          "TopicName": "choirmanagement.membership"
        },
        "Accounting": {
          "ChannelType": "NEvo.Azure.Transporting.AzureServiceBusTransportChannel",
          "TopicName": "choirmanagement.accounting"
        }
      }
    }
  }
}