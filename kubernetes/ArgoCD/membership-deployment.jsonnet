local utils = import './lib/utils.libsonnet';
local nEvoCQRSGlobal = {
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
};

local appName = "choirmanagement-membership";
local appImage = "localhost:5000/choir.management.membership:latest";
local httpPort = 9001;

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
                "containerPort": httpPort,
              },
            ],
            "env": [
              {
                "name": "ASPNETCORE_URLS",
                "value": "http://+:" + httpPort,
              }
            ] 
            + utils.toEnv(nEvoCQRSGlobal),
            "volumeMounts": [
              { 
                "name": "azure-service-bus-credential-secrets",
                "mountPath": "/app/secrets",
                "readonly": true
              }
            ],
          },
        ],
        "volumes": [
          { 
            "name": "azure-service-bus-credential-secrets",
            "secret": {
              "secretName": "azure-service-bus-credential-secrets",
            },
          },
        ],
      },
    },
  },
}