import React from "react";
import PageTitleWrapper from "@/components/PageTitleWrapper";
import PageTitle from "@/components/PageTitle";
import { Container } from "@mui/material";

type UsersCreateProps = {};
const UsersCreate = (props: UsersCreateProps) => {
  return (
    <>
      <PageTitleWrapper>
        <PageTitle heading="Dodaj użytkownika" />
      </PageTitleWrapper>
      <Container maxWidth="lg"></Container>
    </>
  );
};

export default UsersCreate;
