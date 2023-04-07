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
                  "name": "NEvo.CQRS__Topology__Endpoints__Accounting__ChannelType",
                  "value": "NEvo.CQRS.Transporting.RestTransportChannel"
              },
              {
                  "name": "NEvo.CQRS__Topology__Endpoints__Accounting__Endpoint",
                  "value": "http__//accounting__5000/api"
              },
              {
                  "name": "NEvo.CQRS__Topology__Endpoints__Membership__ChannelType",
                  "value": "NEvo.CQRS.Transporting.RestTransportChannel"
              },
              {
                  "name": "NEvo.CQRS__Topology__Endpoints__Membership__Endpoint",
                  "value": "http__//membership__5000/api"
              },
              {
                  "name": "NEvo.CQRS__Topology__Topics__Accounting__ChannelType",
                  "value": "NEvo.Azure.Transporting.AzureServiceBusTransportChannel"
              },
              {
                  "name": "NEvo.CQRS__Topology__Topics__Accounting__TopicName",
                  "value": "choirmanagement.accounting"
              },
              {
                  "name": "NEvo.CQRS__Topology__Topics__Membership__ChannelType",
                  "value": "NEvo.Azure.Transporting.AzureServiceBusTransportChannel"
              },
              {
                  "name": "NEvo.CQRS__Topology__Topics__Membership__TopicName",
                  "value": "choirmanagement.membership"
              },
            ],
          },
        ],
      },
    },
  },
}