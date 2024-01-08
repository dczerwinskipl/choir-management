import { ModuleConfiguration } from "@/app.types";
import ApplicationError from "./ApplicationError";
import NotFound from "./NotFound";
import { LayoutType } from "@/layouts/types";

const errorPages: () => ModuleConfiguration = () => {
  const routes = {
    notFound: {
      element: <NotFound />,
      layout: LayoutType.Fullscreen,
      allowAnonymous: true,
      path: "*",
    },
    error: {
      element: (
        <ApplicationError
          error={{ message: "Błąd", details: "Szczegóły" }}
          onReset={() => {}}
        />
      ),
      layout: LayoutType.Fullscreen,
      allowAnonymous: true,
      path: "/error",
    },
  };

  return {
    routes: [routes.notFound, routes.error],
  };
};

export default errorPages;
