import crudApi from "../../infrastructure/api/crudApi";
import { AddUserRequest, User } from "./types";

const api = crudApi<User, AddUserRequest>()("users", (user) => user.id);

export const useGetUser = api.useGet;
export const useGetUsers = api.useGetAll;
export const useAddUser = api.useAdd;
