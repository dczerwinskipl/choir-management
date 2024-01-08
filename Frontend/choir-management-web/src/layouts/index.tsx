import React from "react";
import ApplicationError from "@/modules/errorPages/ApplicationError";
import { FallbackProps } from "react-error-boundary";

export { default as getLayoutComponent } from "./getLayoutComponent";

const parseError = (error: any): { message: string; details: string } => {
  if (error instanceof Error) {
    return {
      message: "Error occurs.",
      details: `Message: ${error.message}${
        error.cause ? `\r\n${error.cause}` : ""
      }${error.stack ? `\r\n${error.stack}` : ""}`,
    };
  }
  if (typeof error === "string") {
    return {
      message: "Error occurs.",
      details: error,
    };
  }
  return {
    message: "Error occurs.",
    details: JSON.stringify(error, null, 2),
  };
};

export const ErrorFallbackComponent: React.FC<FallbackProps> = ({
  error,
  resetErrorBoundary,
}) => {
  const errorMessage = parseError(error);
  return <ApplicationError onReset={resetErrorBoundary} error={errorMessage} />;
};
