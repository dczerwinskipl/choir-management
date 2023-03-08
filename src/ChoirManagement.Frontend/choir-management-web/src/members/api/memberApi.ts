import axios from "axios";
import { env } from "../../env";
import {
  AddMemberRequest,
  AddMemberResponse,
  GetMembersReponse,
  GetMemberResponse,
} from "./types";

export const membershipApi = axios.create({
  baseURL: env.REACT_APP_MEMBERSHIP_API_URL,
  headers: {
    "Content-type": "application/json; charset=UTF-8",
  },
});

const resourceName = "userss";

export const getAllMembers = async () => {
  const response = await membershipApi.get<GetMembersReponse>(
    `${resourceName}`
  );
  return response.data;
};

export const getMember = async (id: string) => {
  const response = await membershipApi.get<GetMemberResponse>(
    `${resourceName}/${id}`
  );
  return response.data;
};

export const addMember = async (data: AddMemberRequest) => {
  const response = await membershipApi.post<AddMemberResponse>(
    `${resourceName}`,
    data
  );
  return response.data;
};
