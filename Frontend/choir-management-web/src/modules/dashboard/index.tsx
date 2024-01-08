import BrightnessLowTwoToneIcon from "@mui/icons-material/BrightnessLowTwoTone";
import { ModuleConfiguration } from "@/app.types";
import { LayoutType } from "@/layouts/types";
import {
  NavigatorItemDefinition,
  NavigatorContext,
} from "@/infrastructure/navigating";
import Dashboard from "./Dashboard";

const dashboardModule: (baseRoute: string) => ModuleConfiguration = (
  baseRoute: string
) => {
  const routes = {
    dashboard: {
      element: <Dashboard />,
      layout: LayoutType.Sidebar,
      path: baseRoute,
    },
  };

  const dashboardNavigation: NavigatorItemDefinition = (
    context: NavigatorContext
  ) => ({
    name: "Dashboard",
    children: [
      {
        name: context.authState.isLogged ? "Dashboard" : "Dashboard (no auth)",
        to: routes.dashboard.path,
        icon: <BrightnessLowTwoToneIcon />,
      },
    ],
  });

  return {
    routes: [routes.dashboard],
    navigator: [dashboardNavigation],
  };
};

export default dashboardModule;
