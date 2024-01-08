import {
  RouteObject,
  RouterProvider,
  createBrowserRouter,
} from "react-router-dom";
import { ModuleConfiguration } from "./app.types";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import React from "react";
import { SidebarProvider } from "./contexts/SidebarContext";
import { CssBaseline } from "@mui/material";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import ThemeProvider from "./theme/ThemeProvider";
import {
  NavigatorItemDefinition,
  NavigatorProvider,
} from "@/infrastructure/navigating";
import { AuthProvider } from "@/infrastructure/auth";
import { createRoute } from "@/infrastructure/routing";

const bootstrap = (modules: ModuleConfiguration[]) => {
  const queryClient = new QueryClient();
  const routes: RouteObject[] = [];
  const navigatorItems: NavigatorItemDefinition[] = [];

  modules.forEach((module) => {
    module.routes.forEach((route) => routes.push(createRoute(route)));
    module.navigator?.forEach((navigatorItem) =>
      navigatorItems.push(navigatorItem)
    );
  });
  const router = createBrowserRouter(routes);

  console.log("routes", routes);
  console.log("router", router);
  console.log("navigatorItems", navigatorItems);

  const App = () => (
    <React.StrictMode>
      <QueryClientProvider client={queryClient}>
        <SidebarProvider>
          <AuthProvider>
            <NavigatorProvider items={navigatorItems}>
              <ThemeProvider>
                <CssBaseline />
                <RouterProvider router={router} />
              </ThemeProvider>
            </NavigatorProvider>
          </AuthProvider>
        </SidebarProvider>
        <ReactQueryDevtools initialIsOpen={false} />
      </QueryClientProvider>
    </React.StrictMode>
  );

  return App;
};

export default bootstrap;
