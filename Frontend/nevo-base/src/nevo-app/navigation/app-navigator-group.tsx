import React from "react";
import { NavigatorSectionExpandableItem } from "@nevo/design-system/navigator";
import { AppNavigationGroupProps } from "./app-navigator.types";
import { AppNavigatorLink } from "./app-navigator-link";

export const AppNavigatorGroup: React.FC<AppNavigationGroupProps> = React.memo(
  ({ items, ...rest }) => (
    <NavigatorSectionExpandableItem {...rest}>
      {items.map((link) => (
        <AppNavigatorLink key={link.title} {...link} />
      ))}
    </NavigatorSectionExpandableItem>
  )
);
