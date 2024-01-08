export type AppUserRole = string;

export type AppUser = {
  id: string;
  login: string;
  roles: AppUserRole[];
};
