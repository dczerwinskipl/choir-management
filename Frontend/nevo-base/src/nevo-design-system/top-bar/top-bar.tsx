import React from "react";
import classNames from "classnames";

export type TopBarProps = {
  fullscreen?: boolean;
};

const topBarClasses = "p-4 bg-content-background w-full h-12";

export const TopBar: React.FC<
  TopBarProps & React.HTMLAttributes<HTMLDivElement>
> = ({ className, children }) => (
  <div className={classNames(topBarClasses, className)}>{children}</div>
);
