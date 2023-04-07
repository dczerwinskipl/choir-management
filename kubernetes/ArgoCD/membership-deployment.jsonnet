local appName = "choirmanagement-membership";
local appImage = "localhost:5000/choir.management.membership:latest";

{
  "apiVersion": "apps/v1",
  "kind": "Deployment",
  "metadata": {
    "name": appName,
  },
  "spec": {
    "replicas": 3,
    "selector": {
      "matchLabels": {
        "app": appName,
      },
    },
    "template": {
      "metadata": {
        "labels": {
          "app": appName,
        },
      },
      "spec": {
        "containers": [
          {
            "name": appName,
            "image": appImage,
            "ports": [
              {
                "containerPort": 8002,
              },
            ],
            "env": [
              {
                "name": "ASPNETCORE_URLS",
                "value": "http://+:8002",
              },
              {
                  "name": "NEvo.CQRS:Topology:Endpoints:Accounting:ChannelType",
                  "value": "NEvo.CQRS.Transporting.RestTransportChannel"
              },
              {
                  "name": "NEvo.CQRS:Topology:Endpoints:Accounting:Endpoint",
                  "value": "http://accounting:5000/api"
              },
              {
                  "name": "NEvo.CQRS:Topology:Endpoints:Membership:ChannelType",
                  "value": "NEvo.CQRS.Transporting.RestTransportChannel"
              },
              {
                  "name": "NEvo.CQRS:Topology:Endpoints:Membership:Endpoint",
                  "value": "http://membership:5000/api"
              },
              {
                  "name": "NEvo.CQRS:Topology:Topics:Accounting:ChannelType",
                  "value": "NEvo.Azure.Transporting.AzureServiceBusTransportChannel"
              },
              {
                  "name": "NEvo.CQRS:Topology:Topics:Accounting:TopicName",
                  "value": "choirmanagement.accounting"
              },
              {
                  "name": "NEvo.CQRS:Topology:Topics:Membership:ChannelType",
                  "value": "NEvo.Azure.Transporting.AzureServiceBusTransportChannel"
              },
              {
                  "name": "NEvo.CQRS:Topology:Topics:Membership:TopicName",
                  "value": "choirmanagement.membership"
              },
            ],
          },
        ],
      },
    },
  },
}