import React, { useContext, useMemo } from "react";
import { useAuth } from "@/infrastructure/auth";
import { NavigatorDefinition, NavigatorItemDefinition } from "./types";
import { createNavigator } from "./createNavigator";

const emptyDefinition: NavigatorDefinition = {
  items: [],
};

const NavigatorDefinitionContext = React.createContext(emptyDefinition);

export const NavigatorProvider = (props: {
  items: NavigatorItemDefinition[];
  children: any;
}) => (
  <NavigatorDefinitionContext.Provider
    value={{ ...emptyDefinition, items: props.items }}
  >
    {props.children}
  </NavigatorDefinitionContext.Provider>
);

export const useNavigator = () => {
  const { items } = useContext(NavigatorDefinitionContext);
  const { authState } = useAuth();
  const context = useMemo(() => ({ authState }), [authState]);
  const navigator = useMemo(
    () => createNavigator(items, context),
    [items, context]
  );
  return navigator;
};
