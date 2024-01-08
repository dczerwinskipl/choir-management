import React from "react";
import classNames from "classnames";

export type NavigatorScrollProps = {} & React.HTMLAttributes<HTMLDivElement>;

const navigatorScrollClasses = "flex-1 overflow-auto h-min";

export const NavigatorScroll: React.FC<NavigatorScrollProps> = ({
  className,
  children,
}) => (
  <div className={classNames(navigatorScrollClasses, className)}>
    {children}
  </div>
);
