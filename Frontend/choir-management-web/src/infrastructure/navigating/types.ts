export type BaseNavigatorItem = {
  name: string;
  displayName?: string;
};

export type BaseLinkNavigatorItem = BaseNavigatorItem & {
  to: string;
};

export type BaseIconNavigatorItem = BaseNavigatorItem & {
  icon?: JSX.Element;
};

export type BaseGroupNavigatorItem<T> = BaseNavigatorItem & {
  children: T[];
};

export type SubLinkNavigatorItem = BaseLinkNavigatorItem;
export type LinkNavigatorItem = BaseLinkNavigatorItem & BaseIconNavigatorItem;
export type RootLinkNavigatorItem = BaseLinkNavigatorItem &
  BaseIconNavigatorItem;

export type GroupNavigatorItem = BaseGroupNavigatorItem<SubNavigatorItem> &
  BaseIconNavigatorItem;
export type RootGroupNavigatorItem = BaseGroupNavigatorItem<NavigatorItem>;

export type SubNavigatorItem = SubLinkNavigatorItem;
export type NavigatorItem = LinkNavigatorItem | GroupNavigatorItem;
export type RootNavigatorItem = RootLinkNavigatorItem | RootGroupNavigatorItem;

export type ContextDependentNavigatorItem = (
  context: NavigatorContext
) => RootNavigatorItem | null;

export type NavigatorItemDefinition =
  | RootNavigatorItem
  | ContextDependentNavigatorItem;

export type NavigatorContext = {
  authState: {
    isLogged: boolean;
  };
};

export type NavigatorDefinition = {
  items: NavigatorItemDefinition[];
};

export const isNavigatorItemDefinition = (
  item: NavigatorItemDefinition
): item is ContextDependentNavigatorItem => typeof item === "function";

export const isRootGroupNavigatorItem = (
  item: RootNavigatorItem
): item is BaseGroupNavigatorItem<SubNavigatorItem> =>
  (item as BaseGroupNavigatorItem<SubNavigatorItem>).children !== undefined;

export const isRootLinkNavigatorItem = (
  item: RootNavigatorItem
): item is RootLinkNavigatorItem => !isRootGroupNavigatorItem(item);

export const isGroupNavigatorItem = (
  item: NavigatorItem
): item is BaseGroupNavigatorItem<SubNavigatorItem> =>
  (item as BaseGroupNavigatorItem<SubNavigatorItem>).children !== undefined;

export const isLinkNavigatorItem = (
  item: NavigatorItem
): item is LinkNavigatorItem => !isGroupNavigatorItem(item);
