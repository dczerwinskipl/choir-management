import React from "react";
import classNames from "classnames";

export type ContentContainerProps = {
  fullscreen?: boolean;
  contentClassName?: string;
};

const contentContainerClasses =
  "p-4 bg-content-container-background w-full h-full shadow-[inset_0_2px_4px_rgba(0,0,0,0.1)]";
const fullscreenClasses = "max-w-full";
const nonFullscreenClasses = "max-w-7xl m-auto";

export const ContentContainer: React.FC<
  ContentContainerProps & React.HTMLAttributes<HTMLDivElement>
> = ({ className, contentClassName, children, fullscreen = false }) => (
  <div className={classNames(contentContainerClasses, className)}>
    <div
      className={classNames(
        fullscreen ? fullscreenClasses : nonFullscreenClasses,
        contentClassName
      )}
    >
      {children}
    </div>
  </div>
);
