import * as React from "react";
import { CssBaseline, ThemeProvider } from "@mui/material";
import theme from "./theme";
import { QueryClient, QueryClientProvider } from "react-query";

import * as membershipApi from "./members/api/memberApi";
import MemberGrid, { AddMember } from "./members/api/MemberGrid";

membershipApi
  .getAllMembers()
  .then((res) => console.log(res))
  .catch((error) => console.log(error));

const queryClient = new QueryClient();
const MyApp = () => (
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={theme}>
        {/* CssBaseline kickstart an elegant, consistent, and simple baseline to build upon. */}
        <CssBaseline />
        <div style={{ width: "100%", height: "100%" }}>
          <AddMember />
          <MemberGrid />
        </div>
      </ThemeProvider>
    </QueryClientProvider>
  </React.StrictMode>
);

export default MyApp;
