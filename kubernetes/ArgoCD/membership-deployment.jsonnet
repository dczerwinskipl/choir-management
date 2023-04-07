local appName = "choirmanagement-membership";
local appImage = "localhost:5000/choir.management.membership:latest";
local utils = import "./Shared/utils.jsonnet";
local sharedNEvoCQRS = import "./Shared/NEvo.CQRS.global.libsonnet";

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
              + toEnv(sharedNEvoCQRS)
            ],
          },
        ],
      },
    },
  },
}