import React from "react";
import classNames from "classnames";

export type CardSubTitleProps = {};

const cardSubTitleClasses = "mb-4 text";

export const CardSubTitle: React.FC<
  CardSubTitleProps & React.HTMLAttributes<HTMLHeadingElement>
> = ({ className, children }) => (
  <h5 className={classNames(cardSubTitleClasses, className)}>{children}</h5>
);
