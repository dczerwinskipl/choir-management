import { LayoutType } from "@/layouts/types";
import { Permission } from "../auth/types";
import { RouteObject } from "react-router-dom";

//może gdzie indziej?

export type RouteConfiguration = {
  allowAnonymous?: boolean;
  permission?: Permission[]; // one of
  layout: LayoutType;
  index?: true | false;
} & Omit<RouteObject, "ErrorBoundary" | "hasErrorBoundary" | "index">;
