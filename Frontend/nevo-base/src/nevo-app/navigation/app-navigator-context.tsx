import React, {
  useContext,
  createContext,
  useState,
  useMemo,
  useCallback,
} from "react";
import {
  AppNavigationGroupProps,
  AppNavigationItemProps,
  AppNavigationLinkProps,
  AppNavigationProps,
  AppNavigatorGroupProps,
  AppNavigatorItemProps,
  AppNavigatorLinkProps,
  AppNavigatorSectionProps,
  isAppNavigatorGroup,
} from "./app-navigator.types";
import {
  AppUserContextProps,
  useAppUserContext,
} from "../user/app-user-context";
import { AppUser } from "../user";

const baseNavigation = {
  home: {
    title: "Home",
    iconName: "home",
    href: "http://google.pl",
  },
} as const;

type AppNavigatorContextProps = {
  navigation: AppNavigationProps;
  setSection: (
    key: string,
    section: AppNavigatorSectionProps,
    isVisible: boolean
  ) => void;
};

const defaultAppNavigatorContext = {
  navigation: baseNavigation,
  setSection: (
    key: string,
    section: AppNavigatorSectionProps,
    isVisible: boolean
  ) => {
    throw new Error();
  },
};

const AppNavigatorContext = createContext<AppNavigatorContextProps>(
  defaultAppNavigatorContext
);

const buildNavigationLink = (
  navigatorItem: AppNavigatorLinkProps
): AppNavigationLinkProps => ({
  ...navigatorItem,
  isActive: false, //TODO: check by router link?
});

const buildNavigationGroup =
  (appUSerContext: AppUserContextProps) =>
  (navigatorItem: AppNavigatorGroupProps): AppNavigationGroupProps => {
    const items = navigatorItem.items
      .filter(
        (subItem) => !subItem.isVisible || subItem.isVisible(appUSerContext)
      )
      .map(buildNavigationLink);
    return {
      ...navigatorItem,
      items,
      isActive: items.some((item) => item.isActive),
    };
  };

const buildNavigationItem =
  (appUserContext: AppUserContextProps) =>
  (navigatorItem: AppNavigatorItemProps): AppNavigationItemProps =>
    isAppNavigatorGroup(navigatorItem)
      ? buildNavigationGroup(appUserContext)(navigatorItem)
      : buildNavigationLink(navigatorItem);

const buildNavigationItems =
  (appUserContext: AppUserContextProps) =>
  (navigatorItems: AppNavigatorItemProps[]) =>
    navigatorItems
      .filter((item) => !item.isVisible || item.isVisible(appUserContext))
      .map(buildNavigationItem(appUserContext));

const buildNavigationSections =
  (appUserContext: AppUserContextProps) =>
  (navigatorSections: AppNavigatorSectionProps[]) =>
    navigatorSections
      .filter(
        (section) => !section.isVisible || section.isVisible(appUserContext)
      )
      .map((section) => ({
        ...section,
        items: buildNavigationItems(appUserContext)(section.items),
      }));

const useAppNavigator = () => {
  const [sectionsDict, setSectionsDict] = useState<
    Record<string, AppNavigatorSectionProps>
  >({});

  const appUserContext = useAppUserContext();

  const navigation = useMemo<AppNavigationProps>(
    () => ({
      home: buildNavigationLink(baseNavigation.home),
      sections: buildNavigationSections(appUserContext)(
        Object.values(sectionsDict)
      ),
    }),
    [sectionsDict, appUserContext]
  );

  const setSection = useCallback<AppNavigatorContextProps["setSection"]>(
    (key, section, isVisible) => {
      setSectionsDict(({ [key]: _, ...rest }) =>
        isVisible ? { ...rest, [key]: section } : rest
      );
    },
    []
  );

  return {
    navigation,
    setSection,
  };
};

export const useAppNavigatorContext = () => useContext(AppNavigatorContext);

export const AppNavigatorProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const navigator = useAppNavigator();

  return (
    <AppNavigatorContext.Provider value={navigator}>
      {children}
    </AppNavigatorContext.Provider>
  );
};
