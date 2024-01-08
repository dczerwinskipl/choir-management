import React from "react";
import classNames from "classnames";
import { IconSize, IconVariant } from "@nevo/design-system/icon";
import { PageIcon } from "./page-icon";
import { PageTitle } from "./page-title";
import { PageSubTitle } from "./page-sub-title";

type PageHeaderProps = {
  title: string | React.ReactNode;
  subTitle?: string | React.ReactNode;
  iconName?: string;
  iconSize?: IconSize;
  iconVariant?: IconVariant;
  iconClassName?: string;
} & React.HTMLAttributes<HTMLDivElement>;

const pageHeaderClasses =
  "px-12 py-6 bg-page-header-background w-full shadow-[inset_0_2px_4px_rgba(0,0,0,0.1)]";
const rowWraperClasses = "flex flex-row gap-4";
const titlePositionClasses = "self-center";
const childrenPositionClasses = "self-center ml-auto";

export const PageHeader: React.FC<PageHeaderProps> = ({
  title,
  subTitle,
  className,
  iconName,
  iconSize,
  iconVariant,
  children,
  ...other
}) => {
  return (
    <div className={classNames(pageHeaderClasses, className)} {...other}>
      <div className={rowWraperClasses}>
        {iconName && (
          <PageIcon name={iconName} size={iconSize} variant={iconVariant} />
        )}
        <div className={titlePositionClasses}>
          <PageTitle>{title}</PageTitle>
          {subTitle && <PageSubTitle>{subTitle}</PageSubTitle>}
        </div>
        {!!children && (
          <div className={childrenPositionClasses}>{children}</div>
        )}
      </div>
    </div>
  );
};
