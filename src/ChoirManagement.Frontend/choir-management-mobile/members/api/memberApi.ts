import axios from "axios";
import {
  AddMemberRequest,
  AddMemberResponse,
  GetMembersReponse,
  GetMemberResponse,
} from "./types";

export const membershipApi = axios.create({
  baseURL: "https://jsonplaceholder.typicode.com/",
  headers: {
    "Content-type": "application/json; charset=UTF-8",
  },
});

const resourceName = "users";

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
