import React from "react";
import { NavigatorSection } from "@nevo/design-system/navigator";
import { AppNavigationSectionProps } from "./app-navigator.types";
import { AppNavigatorItem } from "./app-navigator-item";

export const AppNavigatorSection: React.FC<AppNavigationSectionProps> =
  React.memo(({ title, items }) => (
    <NavigatorSection title={title}>
      {items.map((item) => (
        <AppNavigatorItem key={item.title} {...item} />
      ))}
    </NavigatorSection>
  ));
