import React from "react";
import {
  AppNavigationItemProps,
  isAppNavigationGroup,
  isAppNavigationLink,
} from "./app-navigator.types";
import { AppNavigatorLink } from "./app-navigator-link";
import { AppNavigatorGroup } from "./app-navigator-group";

export const AppNavigatorItem: React.FC<AppNavigationItemProps> = React.memo(
  (item) =>
    isAppNavigationLink(item) ? (
      <AppNavigatorLink {...item} />
    ) : isAppNavigationGroup(item) ? (
      <AppNavigatorGroup {...item} />
    ) : null
);
