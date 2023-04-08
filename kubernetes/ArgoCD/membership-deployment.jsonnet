local nEvoCQRS = import './env/nevo-cqrs.libsonnet';
local utils = import './lib/utils.libsonnet';

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
            + utils.toEnv(nEvoCQRS),
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