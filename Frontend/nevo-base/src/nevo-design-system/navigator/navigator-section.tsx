import React from "react";
import classNames from "classnames";
import { NavigatorSectionHeader } from "./navigator-section-header";

export type NavigatorSectionProps = {
  title?: string;
} & React.HTMLAttributes<HTMLDivElement>;

const navigatorSectionClasses = "flex flex-col p-3";

export const NavigatorSection: React.FC<NavigatorSectionProps> = ({
  title,
  className,
  children,
}) => (
  <div className={classNames(navigatorSectionClasses, className)}>
    {title && <NavigatorSectionHeader>{title}</NavigatorSectionHeader>}
    {children}
  </div>
);
