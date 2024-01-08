import { Optional } from "../../infrastructure/api/commonTypes";

export type User = {
  id: string;
  name: string;
  username: string;
  email: string;
  address: UserAddress;
};

export type UserAddress = {};

export type GetUserResponse = User;

export type GetUsersReponse = User[];

export type AddUserRequest = Optional<User, "id">;

export type AddUserResponse = User;
