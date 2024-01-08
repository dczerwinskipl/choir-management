import BrightnessLowTwoToneIcon from "@mui/icons-material/BrightnessLowTwoTone";
import List from "./Users.List";
import Create from "./Users.Create";
import Edit from "./Users.Edit";
import { ModuleConfiguration } from "@/app.types";
import {
  NavigatorItemDefinition,
  NavigatorContext,
} from "@/infrastructure/navigating";
import { LayoutType } from "@/layouts/types";

const dashboardModule: (baseRoute: string) => ModuleConfiguration = (
  baseRoute: string
) => {
  const routes = {
    list: {
      element: <List />,
      layout: LayoutType.Sidebar,
      path: baseRoute,
    },
    create: {
      element: <Create />,
      layout: LayoutType.Sidebar,
      path: baseRoute + "/Create",
    },
    edit: {
      element: <Edit />,
      layout: LayoutType.Sidebar,
      path: baseRoute + ":/userId",
    },
  };

  const usersNavigation: NavigatorItemDefinition = (
    context: NavigatorContext
  ) => ({
    name: "Users",
    displayName: "Użytkownicy",
    children: [
      {
        name: "Users",
        displayName: "Użytkownicy",
        icon: <BrightnessLowTwoToneIcon />,
        children: [
          {
            name: "Users.Create",
            displayName: "Dodaj",
            to: routes.create.path,
          },
          {
            name: "Users.List",
            displayName: "Lista",
            to: routes.list.path,
          },
        ],
      },
    ],
  });

  return {
    routes: [routes.create, routes.list, routes.list],
    navigator: [usersNavigation],
  };
};

export default dashboardModule;
