{
  "NEvo.CQRS": {
    "Topology": {
      "Endpoints": {
        "Membership": {
          "ChannelType": "NEvo.CQRS.Transporting.RestTransportChannel",
          "Endpoint": "http://membership:5000/api"
        },
        "Accounting": {
          "ChannelType": "NEvo.CQRS.Transporting.RestTransportChannel",
          "Endpoint": "http://accounting:5000/api"
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