import React, { useEffect, useRef, useState } from "react";
import classNames from "classnames";
import { NavigatorSectionItem } from "./navigator-section-item";

export type NavigatorSectionExpandableItemProps = {
  title: string;
  isActive?: boolean;
  iconName?: string;
} & React.HTMLAttributes<HTMLDivElement>;

const containerClasses =
  "transition-max-height duration-300 ease-in-out overflow-hidden";
const expandedContainerClasses = "h-fit";
const colapsedContainerClasses = "max-h-0";

export const NavigatorSectionExpandableItem: React.FC<
  NavigatorSectionExpandableItemProps
> = ({ className, title, isActive = false, iconName, children }) => {
  const [expanded, setExpanded] = useState(isActive);

  const [height, setHeight] = useState<number | undefined>(undefined);
  const wrapper = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (wrapper.current) {
      var { height: wrapperHeight } = wrapper.current.getBoundingClientRect();
      setHeight(wrapperHeight);
    }
  }, [wrapper.current]);

  return (
    <>
      <NavigatorSectionItem
        className={className}
        iconName={iconName}
        expanded={expanded}
        onExpand={() => setExpanded((current) => !current)}
      >
        {title}
      </NavigatorSectionItem>
      <div
        className={classNames(
          containerClasses,
          expanded ? expandedContainerClasses : colapsedContainerClasses
        )}
        style={{ maxHeight: expanded ? height : undefined }}
      >
        <div className={expandedContainerClasses} ref={wrapper}>
          {children}
        </div>
      </div>
    </>
  );
};
