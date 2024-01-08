import React from "react";
import classNames from "classnames";
import { Icon, IconSize, IconVariant } from "@nevo/design-system/icon";

export type NavigatorIconProps = {
  name: string;
  size?: IconSize;
  variant?: IconVariant;
  iconClassName?: string;
} & Omit<React.HTMLAttributes<HTMLDivElement>, "children">;

const navigatorIconContainerClasses =
  "h-fit w-fit align-middle self-center bg-navigator-background-highlight bg-opacity-50 shadow rounded-lg";

const navigatorIconSizeClasses = {
  s: "p-0",
  m: "p-1",
  l: "p-2",
};

export const NavigatorIcon: React.FC<NavigatorIconProps> = ({
  name,
  variant = "default",
  size = "m",
  iconClassName,
  className,
  ...other
}) => {
  return (
    <div
      className={classNames(
        navigatorIconContainerClasses,
        navigatorIconSizeClasses[size],
        className
      )}
      {...other}
    >
      <Icon
        name={name}
        variant={variant}
        size={size}
        className={iconClassName}
      />
    </div>
  );
};
