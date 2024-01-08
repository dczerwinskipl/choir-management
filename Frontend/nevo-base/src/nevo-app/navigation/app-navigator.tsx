import React from "react";
import {
  Navigator,
  NavigatorDivider,
  NavigatorSection,
} from "@nevo/design-system/navigator";
import { AppNavigatorLink } from "./app-navigator-link";
import { AppNavigatorSection } from "./app-navigator-section";
import { useAppNavigatorContext } from "./app-navigator-context";

export const AppNavigator: React.FC = React.memo(() => {
  const {
    navigation: { home, sections },
  } = useAppNavigatorContext();
  return (
    <Navigator>
      {home && (
        <NavigatorSection>
          <AppNavigatorLink {...home} />
        </NavigatorSection>
      )}
      {home && sections && <NavigatorDivider />}
      {sections?.map((section) => (
        <AppNavigatorSection key={section.title} {...section} />
      ))}
    </Navigator>
  );
});
