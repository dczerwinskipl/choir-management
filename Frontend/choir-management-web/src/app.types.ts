import { NavigatorItemDefinition } from "@/infrastructure/navigating";
import { RouteConfiguration } from "@/infrastructure/routing";

/** gdzieś to wywalić do commonów albo czegoś takieg */
export type ModuleConfiguration = {
  routes: RouteConfiguration[];
  navigator?: NavigatorItemDefinition[];
};
export type Navigator = {};
