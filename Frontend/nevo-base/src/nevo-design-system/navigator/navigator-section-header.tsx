import React from "react";
import classNames from "classnames";

export type NavigatorSectionHeaderProps =
  {} & React.HTMLAttributes<HTMLDivElement>;

const navigatorSectionHeaderClasses =
  "my-2 text-xs text-navigator-low-contrast font-bold uppercase";

export const NavigatorSectionHeader: React.FC<NavigatorSectionHeaderProps> = ({
  title,
  className,
  children,
}) => (
  <h4 className={classNames(navigatorSectionHeaderClasses, className)}>
    {children}
  </h4>
);
