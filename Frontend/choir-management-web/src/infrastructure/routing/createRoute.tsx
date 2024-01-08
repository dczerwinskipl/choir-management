import { RouteObject } from "react-router-dom";
import { RouteConfiguration } from "./types";
import { getLayoutComponent, ErrorFallbackComponent } from "@/layouts";
import { ErrorBoundary } from "react-error-boundary";

const createRoute = ({
  layout,
  allowAnonymous,
  permission,
  index,
  children,
  element: routeElement,
  Component: RouteComponent,
  ...rest
}: RouteConfiguration): RouteObject => {
  let Component: React.ComponentType | null = null;
  let element: React.ReactNode = null;

  if (RouteComponent) {
    const LayoutComponent = getLayoutComponent(layout);
    Component = () => (
      <LayoutComponent>
        <ErrorBoundary
          key={rest.path}
          FallbackComponent={ErrorFallbackComponent}
        >
          <RouteComponent />
        </ErrorBoundary>
      </LayoutComponent>
    );
  }

  if (routeElement) {
    const LayoutComponent = getLayoutComponent(layout);
    element = (
      <LayoutComponent>
        <ErrorBoundary
          key={rest.path}
          FallbackComponent={ErrorFallbackComponent}
        >
          {routeElement}
        </ErrorBoundary>
      </LayoutComponent>
    );
  }

  const route = {
    hasErrorBoundary: false,
    ErrorBoundary: undefined,
    Component,
    element,
    ...rest,
  };

  if (index && !children) {
    return {
      ...route,
      index: true,
    };
  } else {
    return {
      ...route,
      index: false,
      children: undefined,
    };
  }
};

export default createRoute;
