import React, { memo } from "react";
import { DataGrid, GridColDef } from "@mui/x-data-grid";
import { useGetMembers, useAddMember } from "./memberApi";
import Button from "@mui/material/Button";

const memberGridColDef: GridColDef[] = [
  { field: "id", headerName: "Id", width: 150 },
  { field: "name", headerName: "Name", width: 150 },
  { field: "username", headerName: "Username", width: 150 },
];

const MemberGrid = memo(() => {
  console.log("rerender memberGrid");
  const { isLoading, error, data } = useGetMembers();

  if (isLoading) return <>Loading</>;
  if (error)
    return (
      <>
        Error <pre>{JSON.stringify(error)}</pre>
      </>
    );
  if (!data) return <>No data</>;

  return <DataGrid rows={data} columns={memberGridColDef} />;
});

export default MemberGrid;
export const AddMember2 = () => <div>nothing</div>;
export const AddMember = () => {
  const { mutate, isLoading, isSuccess, error, data } = useAddMember();
  return (
    <>
      {isLoading && <>Dodaję</>}
      {isSuccess && (
        <>
          Dodałem <pre>{data ? JSON.stringify(data) : ""}</pre>
        </>
      )}
      {error && (
        <>
          Error <pre>{JSON.stringify(error)}</pre>
        </>
      )}
      <Button
        onClick={() =>
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
