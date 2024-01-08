import React, { useEffect, useState } from "react";
import { Button, Container } from "@mui/material";
import PageTitleWrapper from "@/components/PageTitleWrapper";
import PageTitle from "@/components/PageTitle";
import AddTwoToneIcon from "@mui/icons-material/AddTwoTone";
import { Link } from "react-router-dom";
import { useGetUsers } from "./usersApi";
import { DataGrid, GridColDef } from "@mui/x-data-grid";

const usersGridColDef: GridColDef[] = [
  { field: "id", headerName: "Id", width: 150 },
  { field: "name", headerName: "Name", width: 150 },
  { field: "username", headerName: "Username", width: 150 },
];

function delay(ms: number) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

const UsersList = () => {
  const { isLoading, data } = useGetUsers();
  const [cnt, setCnt] = useState(0);
  if (cnt === 1) throw Error("some error");

  useEffect(() => {
    delay(5000).then(() => setCnt(1));
  });

  return (
    <>
      <PageTitleWrapper>
        <PageTitle heading="Użytkownicy">
          <Link to="./Create">
            <Button
              sx={{ mt: { xs: 2, md: 0 } }}
              variant="contained"
              startIcon={<AddTwoToneIcon fontSize="small" />}
            >
              Dodaj użytkownika
            </Button>
          </Link>
        </PageTitle>
      </PageTitleWrapper>
      <Container maxWidth="lg">
        <DataGrid
          style={{ width: "100%", height: "500px" }}
          columns={usersGridColDef}
          rows={data ?? []}
          loading={isLoading}
        />
      </Container>
    </>
  );
};

export default UsersList;
