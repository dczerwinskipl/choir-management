export const assertUnreachable = (_: never): Error => {
  return new Error("Didn't expect to get here");
};
