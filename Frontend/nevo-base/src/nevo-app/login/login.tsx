import { useCallback, useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useAppUserContext } from "@nevo/app/user";
import { Card, CardHeader } from "@nevo/design-system/card";
import { ContentContainer } from "@nevo/design-system/content-container";
import { TextInput } from "@nevo/design-system/text-input/text-input";
import { useMutation } from "react-query";

const contentClasses = "w-[512px]";

const delay = (time: number) =>
  new Promise((resolve) => setTimeout(resolve, time));

const apiMock = () =>
  delay(5000).then(() => ({
    id: "id",
    login: "login",
    roles: ["ADMIN"],
  }));

export const Login = () => {
  const appUserContext = useAppUserContext();
  const location = useLocation();
  const navigate = useNavigate();
  const loginMutation = useMutation({
    mutationKey: "login",
    mutationFn: apiMock,
    onSuccess: (user) => {
      if (!appUserContext.isLoggedIn) {
        appUserContext.login(user);
      }
    },
  });

  const from = location.state?.from?.pathname || "/";

  const handleLogin = useCallback(async () => {
    if (!appUserContext.isLoggedIn) {
      await loginMutation.mutateAsync();
    }
  }, []);

  useEffect(() => {
    if (appUserContext.isLoggedIn) {
      navigate(from);
    }
  }, [appUserContext.isLoggedIn]);

  console.log(appUserContext);

  return (
    <ContentContainer className="flex" contentClassName={contentClasses}>
      <Card>
        <CardHeader title="Welcome do NEvo" subTitle="Login to start" />
        <TextInput type="text" label="Username" />
        <TextInput type="password" label="Password" />
        <div className="flex flex-row">
          <Button isLoading={loginMutation.isLoading} onClick={handleLogin}>
            Login
          </Button>
          <Button isLoading={true} onClick={handleLogin}>
            Login
          </Button>
        </div>
      </Card>
    </ContentContainer>
  );
};

const LoadingIcon = () => (
  <svg
    className="animate-spin h-5 w-5 text-white mx-auto"
    xmlns="http://www.w3.org/2000/svg"
    fill="none"
    viewBox="0 0 24 24"
  >
    <circle
      className="opacity-25"
      cx="12"
      cy="12"
      r="10"
      stroke="currentColor"
      strokeWidth="4"
    ></circle>
    <path
      className="opacity-75"
      fill="currentColor"
      d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
    ></path>
  </svg>
);

const Button = ({ disabled, isLoading, onClick, children }: any) => {
  return (
    <div
      onClick={onClick}
      className={`w-full text-white text-base p-3 rounded-md transition duration-150 flex items-center justify-center cursor-pointer
                 ${
                   disabled || isLoading
                     ? "bg-gray-400 cursor-not-allowed"
                     : "bg-primary-600 hover:bg-primary-500"
                 }`}
    >
      {isLoading ? <LoadingIcon /> : children}
    </div>
  );
};
