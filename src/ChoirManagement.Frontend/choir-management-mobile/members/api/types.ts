import { Optional } from "../../api/commonTypes";

export type Member = {
  id: string;
  name: string;
  username: string;
  email: string;
  address: MemberAddress;
};

export type MemberAddress = {};

export type GetMemberResponse = Member;

export type GetMembersReponse = Member[];

export type AddMemberRequest = Optional<Member, "id">;

export type AddMemberResponse = Member;
