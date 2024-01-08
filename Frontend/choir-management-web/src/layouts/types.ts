import { ReactNode } from "react";

export enum LayoutType {
  Fullscreen,
  Sidebar,
}

export type LayoutProps = {
  children?: ReactNode;
};
