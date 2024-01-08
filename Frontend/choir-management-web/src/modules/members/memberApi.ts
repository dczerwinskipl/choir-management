import crudApi from "../../infrastructure/api/crudApi";
import { AddMemberRequest, Member } from "./types";

const api = crudApi<Member, AddMemberRequest>()("users", (user) => user.id)

export const useGetMember = api.useGet;
export const useGetMembers = api.useGetAll;
export const useAddMember = api.useAdd;
