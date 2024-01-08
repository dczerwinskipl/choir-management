import { AppUser, AppUserContextProps } from "@nevo/app/user";

export type AppNavigatorProps = {
  home?: AppNavigatorLinkProps;
  sections?: AppNavigatorSectionProps[];
};

export type AppNavigatorSectionProps = {
  title: string;
  items: AppNavigatorItemProps[];
  isVisible?: (appUserContext: AppUserContextProps) => boolean;
};

type AppNavigatorItemBaseProps = {
  title: string;
  iconName?: string;
  isVisible?: (appUserContext: AppUserContextProps) => boolean;
};

export type AppNavigatorLinkProps = AppNavigatorItemBaseProps & {
  href: string;
};

export type AppNavigatorGroupProps = AppNavigatorItemBaseProps & {
  items: AppNavigatorLinkProps[];
};

export type AppNavigatorItemProps =
  | AppNavigatorLinkProps
  | AppNavigatorGroupProps;

export function isAppNavigatorLink(
  item: AppNavigatorItemProps
): item is AppNavigatorLinkProps {
  return (item as AppNavigatorLinkProps).href !== undefined;
}
export function isAppNavigatorGroup(
  item: AppNavigatorItemProps
): item is AppNavigatorGroupProps {
  return (item as AppNavigatorGroupProps).items !== undefined;
}

export type AppNavigationProps = {
  home?: AppNavigationLinkProps;
  sections?: AppNavigationSectionProps[];
};

export type AppNavigationSectionProps = {
  title: string;
  items: AppNavigationItemProps[];
  isVisible?: (appUserContext: AppUserContextProps) => boolean;
};

type AppNavigationItemBaseProps = {
  title: string;
  iconName?: string;
  isActive?: boolean;
};

export type AppNavigationLinkProps = AppNavigationItemBaseProps & {
  href: string;
};

export type AppNavigationGroupProps = AppNavigationItemBaseProps & {
  items: AppNavigatorLinkProps[];
};

export type AppNavigationItemProps =
  | AppNavigationLinkProps
  | AppNavigationGroupProps;

export function isAppNavigationLink(
  item: AppNavigationItemProps
): item is AppNavigationLinkProps {
  return (item as AppNavigationLinkProps).href !== undefined;
}
export function isAppNavigationGroup(
  item: AppNavigationItemProps
): item is AppNavigationGroupProps {
  return (item as AppNavigationGroupProps).items !== undefined;
}
