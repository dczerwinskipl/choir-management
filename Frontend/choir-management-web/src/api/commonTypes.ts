export type ApiError = {};

export type ApiValidationError = {};

export type ApiErrorResponse = {
  type: "ERROR";
} & ApiError;

export type ApiValidationErrorResponse = {
  type: "VALIDATION";
} & ApiValidationError;

export type Optional<T, K extends keyof T> = Omit<T, K> & Partial<Pick<T, K>>;
