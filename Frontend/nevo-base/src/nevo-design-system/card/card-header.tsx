import React from "react";
import classNames from "classnames";

type CardTitleProps = {};

const cardTitleClasses = "text-2xl text-high-contrast-500";

export const CardTitle: React.FC<
  CardTitleProps & React.HTMLAttributes<HTMLHeadingElement>
> = ({ className, children, ...other }) => (
  <h4 className={classNames(cardTitleClasses, className)} {...other}>
    {children}
  </h4>
);

type CardSubTitleProps = {};

const cardSubTitleClasses = "text-m text-low-contrast-500";

const CardSubTitle: React.FC<
  CardSubTitleProps & React.HTMLAttributes<HTMLHeadingElement>
> = ({ className, children, ...other }) => (
  <h5 className={classNames(cardSubTitleClasses, className)} {...other}>
    {children}
  </h5>
);

type CardHeaderProps = {
  title: string;
  subTitle?: string;
};

const cardHeaderClasses = "mb-2";

export const CardHeader: React.FC<
  CardHeaderProps & Omit<React.HTMLAttributes<HTMLHeadingElement>, "children">
> = ({ title, subTitle, className, ...other }) => (
  <div className={classNames(cardHeaderClasses, className)} {...other}>
    <CardTitle>{title}</CardTitle>
    {subTitle && <CardSubTitle>{subTitle}</CardSubTitle>}
  </div>
);
