import { redirect, Route, RouteProps } from "react-router-dom";
import { useAppUserContext } from "../user";

const PrivateRoute = (props: RouteProps) => {
  const { isLoggedIn } = useAppUserContext();

  if (!isLoggedIn) redirect("/login");

  return <Route {...props} />;
};

export default PrivateRoute;
