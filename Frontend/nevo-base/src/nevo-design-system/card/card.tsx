import React from "react";
import classNames from "classnames";

export type CardProps = {};

const cardClasses = "p-8 rounded-lg bg-content-background shadow";

export const Card: React.FC<
  CardProps & React.HTMLAttributes<HTMLDivElement>
> = ({ className, children }) => (
  <div className={classNames(cardClasses, className)}>{children}</div>
);
