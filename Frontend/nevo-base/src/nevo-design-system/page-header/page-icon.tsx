import React from "react";
import classNames from "classnames";
import { Icon, IconSize, IconVariant } from "@nevo/design-system/icon";

export type PageIconProps = {
  name: string;
  size?: IconSize;
  variant?: IconVariant;
  iconClassName?: string;
} & Omit<React.HTMLAttributes<HTMLDivElement>, "children">;

const pageIconContainerClasses =
  "align-middle self-center h-fit w-fit bg-content-background shadow rounded-lg";

const pageIconSizeClasses = {
  xs: "p-1",
  s: "p-2",
  m: "p-4",
  l: "p-6",
};

export const PageIcon: React.FC<PageIconProps> = ({
  name,
  size = "m",
  variant = "primary",
  iconClassName,
  className,
  ...other
}) => {
  return (
    <div
      className={classNames(
        pageIconContainerClasses,
        pageIconSizeClasses[size],
        className
      )}
      {...other}
    >
      <Icon
        name={name}
        size={size}
        variant={variant}
        className={iconClassName}
      />
    </div>
  );
};
