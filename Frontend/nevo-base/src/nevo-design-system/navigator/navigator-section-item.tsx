import React from "react";
import classNames from "classnames";
import { NavigatorIcon } from "./navigator-icon";
import { Icon } from "@nevo/design-system/icon";

export type NavigatorSectionItemProps = {
  iconName?: string;
  isActive?: boolean;
  size?: "s" | "m";
  expandable?: boolean;
  expanded?: boolean;
  onExpand?: () => void;
} & React.HTMLAttributes<HTMLDivElement>;

const navigatorSectionItemContainerClasses =
  "group flex flex-row my-px rounded-lg cursor-pointer";

const sizeContainerClasses = {
  s: "p-1",
  m: "p-2",
};

const navigatorSectionItemTextClasses =
  "text-sm font-semibold text-navigator-high-contrast bg-transparent";

const navigatorSectionItemHoverClasses =
  "hover:text-navigator-high-contrast-highlight hover:bg-navigator-background-highlight transition-all duration-100 ease-in-out";

const navigatorSectionItemClasses = classNames(
  navigatorSectionItemContainerClasses,
  navigatorSectionItemTextClasses,
  navigatorSectionItemHoverClasses
);

const iconLeftClasses =
  "mr-4 self-center text-center w-8 h-8 flex items-center justify-center";
const dotClasses =
  "w-2 h-2 rounded-full bg-navigator-high-contrast-highlight bg-opacity-5 group-hover:bg-opacity-100 transition-all duration-100 ease-in-out";
const activeDotClasses = "bg-primary-500";

const contentClasses = "self-center";
const activeContentClasses = "text-primary-500";

const iconRightClasses = "ml-auto self-center w-8";

export const NavigatorSectionItem: React.FC<NavigatorSectionItemProps> = ({
  className,
  isActive = false,
  children,
  iconName,
  size = "m",
  expanded = false,
  onExpand,
}) => (
  <div
    className={classNames(
      navigatorSectionItemClasses,
      sizeContainerClasses[size],
      className
    )}
    onClick={onExpand}
  >
    {iconName ? (
      <NavigatorIcon
        variant={isActive ? "primary" : "default"}
        className={iconLeftClasses}
        name={iconName}
        size="s"
      />
    ) : (
      <div className={iconLeftClasses}>
        <div
          className={classNames(dotClasses, isActive ? activeDotClasses : null)}
        />
      </div>
    )}
    <div
      className={classNames(
        contentClasses,
        isActive ? activeContentClasses : null
      )}
    >
      {children}
    </div>
    {onExpand && (
      <Icon
        className={iconRightClasses}
        size="s"
        name={expanded ? "expand_less" : "expand_more"}
      />
    )}
  </div>
);
