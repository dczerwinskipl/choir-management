import { useContext, createContext, useState } from "react";

const AuthContext = createContext({
  authState: { isLogged: false },
  logIn: () => {},
  logOut: () => {}
});

export const useAuth = () => {
    const auth =  useContext(AuthContext)
    return auth;
}

const defaultAuthState = {
  isLogged: false
};

export const AuthProvider = ({ children } : { children: any }) => {
    const [authState, setCurrentSettings] = useState(defaultAuthState);
  
    const logIn = () => {
      setCurrentSettings({ isLogged: true })
    };

    const logOut = () => {
      setCurrentSettings({ isLogged: false })
    };
  
    return (
      <AuthContext.Provider
        value={{ authState, logIn, logOut }}
      >
        {children}
      </AuthContext.Provider>
    );
  };