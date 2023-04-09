local services = import './services.libsonnet';
local manifests = import './lib/manifests.libsonnet';

manifests.network(
  services['members-registration'],
  'LoadBalancer',
)
