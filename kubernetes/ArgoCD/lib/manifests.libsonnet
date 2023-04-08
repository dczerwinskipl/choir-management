local utils = import './utils.libsonnet';

{
  deployment(service, envJsons=[], secrets=[])::
    {
      apiVersion: 'apps/v1',
      kind: 'Deployment',
      metadata: {
        name: service.name,
      },
      spec: {
        replicas: 3,
        selector: {
          matchLabels: {
            app: service.name,
          },
        },
        template: {
          metadata: {
            labels: {
              app: service.name,
            },
          },
          spec: {
            containers: [
              {
                name: service.name,
                image: service.image,
                ports: [
                  { containerPort: service.port },
                ],
                env: [
                  {
                    name: 'ASPNETCORE_URLS',
                    value: 'http://+:' + service.port,
                  },
                ] + std.flattenArrays([utils.toEnv(envJson) for envJson in envJsons]),
                volumeMounts: [] + [
                  {
                    name: name,
                    mountPath: '/app/secrets',
                    readonly: true,
                  }
                  for name in secrets
                ],
              },
            ],
            volumes: []
                     + [{
                       name: name,
                       secret:
                         {
                           secretName: name,
                         },
                     } for name in secrets],
          },
        },
      },
    },

  network(service, type='LoadBalancer')::
    {
      apiVersion: 'v1',
      kind: 'Service',
      metadata: {
        name: service.name,
      },
      spec: {
        type: type,
        ports: [
          {
            port: service.port,
          },
        ],
        selector: {
          app: service.name,
        },
      },
    },
}
