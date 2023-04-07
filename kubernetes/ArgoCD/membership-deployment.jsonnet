local appName = "choirmanagement-membership";
local appImage = "localhost:5000/choir.management.membership:latest";
local testObject = {
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

local buildArray(prefix, value) = 
  if std.isObject(value) then 
    std.flatten([ buildArray(prefix + "__" + key, value[key]) for key in value]) 
  else 
    [{ name: prefix, value: value }];


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
              }
            ] + buildArray(testObject)
          },
        ],
      },
    },
  },
}