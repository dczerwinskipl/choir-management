import { Login } from "@nevo/app/login/login";
import { useAppUserContext } from "@nevo/app/user";
import React from "react";
import { Navigate, Route, Routes, useLocation } from "react-router-dom";
import Layout from "./Layout";

const App = () => (
  <Routes>
    <Route path="/login" element={<Login />} />
    <Route
      path="*"
      element={
        <RequireAuth>
          <Layout />
        </RequireAuth>
      }
    />
  </Routes>
);
export default App;

const RequireAuth: React.FC<React.PropsWithChildren> = ({ children }) => {
  const appUserContext = useAppUserContext();
  const location = useLocation();

  if (!appUserContext.isLoggedIn) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return <>{children}</>;
};
