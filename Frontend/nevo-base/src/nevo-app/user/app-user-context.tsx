import { createContext, useContext, useState } from "react";
import { AppUser } from "./app-user.types";

type AppUserAuthorizedContextProps = {
  isLoggedIn: true;
  user: AppUser;
  logout: () => void | Promise<void>;
};

type AppUserUnAuthorizedContextProps = {
  isLoggedIn: false;
  login: (appUser: AppUser) => void | Promise<void>;
};

const delay = (time: number) =>
  new Promise((resolve) => setTimeout(resolve, time));

export type AppUserContextProps =
  | AppUserAuthorizedContextProps
  | AppUserUnAuthorizedContextProps;

export const AppUserContext = createContext<AppUserContextProps>({
  isLoggedIn: false,
  login: (_: AppUser) => {
    throw new Error("Missing AppUserContextProvider");
  },
});

export const useAppUserContext = () => useContext(AppUserContext);

export const AppUserContextProvider: React.FC<React.PropsWithChildren> = ({
  children,
}) => {
  const [user, setUser] = useState<AppUser | undefined>(undefined);
  const props: AppUserContextProps = user
    ? {
        isLoggedIn: true,
        user,
        logout: () => delay(5000).then(() => setUser(undefined)),
      }
    : {
        isLoggedIn: false,
        login: setUser,
      };
  return (
    <AppUserContext.Provider value={props}>{children}</AppUserContext.Provider>
  );
};

export const hasRole =
  (role: string) => (appUserContext: AppUserContextProps) =>
    appUserContext.isLoggedIn && appUserContext.user.roles.includes(role);
