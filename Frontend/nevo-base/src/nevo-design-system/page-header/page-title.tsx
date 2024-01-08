import React from "react";
import classNames from "classnames";

export type PageTitleProps = {} & React.HTMLAttributes<HTMLHeadingElement>;

const pageTitleClasses = "text-3xl font-semibold text-high-contrast-500";

export const PageTitle: React.FC<PageTitleProps> = ({
  className,
  children,
  ...other
}) => (
  <h3 className={classNames(pageTitleClasses, className)} {...other}>
    {children}
  </h3>
);
