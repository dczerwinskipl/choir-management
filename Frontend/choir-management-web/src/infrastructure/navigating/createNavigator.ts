import {
  NavigatorContext,
  NavigatorItem,
  NavigatorItemDefinition,
  isNavigatorItemDefinition,
} from "./types";

export const createNavigator = (
  itemsDefinition: NavigatorItemDefinition[],
  context: NavigatorContext
) => {
  const items = itemsDefinition
    .map((definitionItem) =>
      isNavigatorItemDefinition(definitionItem)
        ? definitionItem(context)
        : definitionItem
    )
    .filter((item) => !!item);

  return items as NavigatorItem[];
};
