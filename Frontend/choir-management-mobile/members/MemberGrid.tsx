import React, { memo } from "react";
import { Button, List } from "react-native-paper";
import { View, Text } from "react-native";
import { useQuery, useQueryClient, useMutation } from "react-query";
import * as memberApi from "./api/memberApi";
import { AddMemberRequest, Member } from "./api/types";
import base from "../styles/base";

const MemberGrid = memo(() => {
  console.log("rerender memberGrid");
  const { isLoading, error, data } = useQuery(
    "membersData",
    memberApi.getAllMembers
  );

  if (isLoading) return <Text>Loading</Text>;
  if (error) return <Text>Error {JSON.stringify(error)}</Text>;
  if (!data) return <>No data</>;

  return (
    <View>
      <AddMember />
      <List.Section>
        <List.Subheader>Members list</List.Subheader>
        {data.map((item) => (
          <List.Item
            key={item.id}
            title={item.name}
            description={`#${item.id}, @${item.username}, ${item.email}}`}
            left={(props) => <List.Icon {...props} icon="account" />}
          />
        ))}
      </List.Section>
    </View>
  );
});

export default MemberGrid;
export const AddMember2 = () => <div>nothing</div>;
export const AddMember = () => {
  const queryClient = useQueryClient();

  const { mutate, isLoading, isSuccess, error, data } = useMutation({
    mutationFn: async (newMember: AddMemberRequest) =>
      await memberApi.addMember(newMember),
    onSuccess: (data) => {
      if (data) {
        queryClient.setQueryData<Member[]>("membersData", (old) => [
          ...(old ?? []),
          data,
        ]);
      }
    },
  });
  return (
    <>
      {isLoading && <Text>Dodaję</Text>}
      {isSuccess && <Text>Dodałem {data ? JSON.stringify(data) : ""}</Text>}
      {error && <Text>Error {JSON.stringify(error)}</Text>}
      <Button
        onPress={() =>
          mutate({
            name: "Dominik Czerwiński",
            username: "dczerwinski",
            address: {},
            email: "dominikczerwinski@gmail.com",
          })
        }
      >
        Add member
      </Button>
    </>
  );
};
