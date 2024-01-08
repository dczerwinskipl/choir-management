import React from "react";
import classNames from "classnames";

export type IconSize = "s" | "m" | "l";

export type IconVariant = "default" | "primary" | "secondary";

export type IconProps = {
  name: string;
  size?: IconSize;
  variant?: IconVariant;
  iconClassName?: string;
} & Omit<React.HTMLAttributes<HTMLBaseElement>, "children">;

const iconClasses = "material-icons text-center";

const pageSizeClasses = {
  s: "w-8 h-8 text-xl leading-[2rem]",
  m: "w-12 h-12 text-3xl leading-[3rem]",
  l: "w-16 h-16 text-5xl leading-[4rem]",
};

const variantClasses = {
  default: "text-inherit",
  primary: "text-primary-500",
  secondary: "text-secondary-500",
};

export const Icon: React.FC<IconProps> = ({
  className,
  name,
  size = "m",
  variant = "default",
}) => (
  <i
    className={classNames(
      iconClasses,
      pageSizeClasses[size],
      variantClasses[variant],
      className
    )}
  >
    {name}
  </i>
);
