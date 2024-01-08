import React from "react";
import { NavigatorSectionItem } from "@nevo/design-system/navigator";
import { AppNavigationLinkProps } from "./app-navigator.types";

export const AppNavigatorLink: React.FC<AppNavigationLinkProps> = React.memo(
  ({ title, href, ...rest }) => (
    <NavigatorSectionItem
      size="s"
      onClick={() => (window.location.href = href)}
      {...rest}
    >
      {title}
    </NavigatorSectionItem>
  )
);
