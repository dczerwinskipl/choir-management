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
                  "name": "NEvo.CQRS_Topology_Endpoints_Accounting_ChannelType",
                  "value": "NEvo.CQRS.Transporting.RestTransportChannel"
              },
              {
                  "name": "NEvo.CQRS_Topology_Endpoints_Accounting_Endpoint",
                  "value": "http_//accounting_5000/api"
              },
              {
                  "name": "NEvo.CQRS_Topology_Endpoints_Membership_ChannelType",
                  "value": "NEvo.CQRS.Transporting.RestTransportChannel"
              },
              {
                  "name": "NEvo.CQRS_Topology_Endpoints_Membership_Endpoint",
                  "value": "http_//membership_5000/api"
              },
              {
                  "name": "NEvo.CQRS_Topology_Topics_Accounting_ChannelType",
                  "value": "NEvo.Azure.Transporting.AzureServiceBusTransportChannel"
              },
              {
                  "name": "NEvo.CQRS_Topology_Topics_Accounting_TopicName",
                  "value": "choirmanagement.accounting"
              },
              {
                  "name": "NEvo.CQRS_Topology_Topics_Membership_ChannelType",
                  "value": "NEvo.Azure.Transporting.AzureServiceBusTransportChannel"
              },
              {
                  "name": "NEvo.CQRS_Topology_Topics_Membership_TopicName",
                  "value": "choirmanagement.membership"
              },
            ],
          },
        ],
      },
    },
  },
}