import React from "react";
import { assertUnreachable } from "@/infrastructure/utils";
import BaseLayout from "./BaseLayout";
import SidebarLayout from "./SidebarLayout";
import { LayoutProps, LayoutType } from "./types";

const getLayoutComponent = (layout: LayoutType): React.FC<LayoutProps> => {
  switch (layout) {
    case LayoutType.Fullscreen:
      return BaseLayout;
    case LayoutType.Sidebar:
      return SidebarLayout;
    default:
      throw assertUnreachable(layout);
  }
};

export default getLayoutComponent;
