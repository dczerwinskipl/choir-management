local nEvoCQRS = import './env/nevo-cqrs.libsonnet';
local services = import './services.libsonnet';

local manifests = import './lib/manifests.libsonnet';

manifests.deployment(
  services['members-registration'],
  [
    nEvoCQRS,
  ],
  [
    'azure-service-bus-credential-secrets',
  ]
)