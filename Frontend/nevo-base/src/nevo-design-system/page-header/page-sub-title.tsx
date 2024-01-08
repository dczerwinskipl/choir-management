import React from "react";
import classNames from "classnames";

export type PageSubTitleProps = {} & React.HTMLAttributes<HTMLHeadingElement>;

const pageSubTitleClasses = "text-lg text-low-contrast-500";

export const PageSubTitle: React.FC<PageSubTitleProps> = ({
  className,
  children,
  ...other
}) => (
  <h2 className={classNames(pageSubTitleClasses, className)} {...other}>
    {children}
  </h2>
);
