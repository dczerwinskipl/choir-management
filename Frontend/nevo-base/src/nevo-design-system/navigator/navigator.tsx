import React from "react";
import classNames from "classnames";

export type NavigatorProps = {} & React.HTMLAttributes<HTMLDivElement>;

const navigatorClasses =
  "flex flex-col w-72 h-full p-1 bg-navigator-background text-navigator-high-contrast";

export const Navigator: React.FC<NavigatorProps> = ({
  className,
  children,
}) => <div className={classNames(navigatorClasses, className)}>{children}</div>;
