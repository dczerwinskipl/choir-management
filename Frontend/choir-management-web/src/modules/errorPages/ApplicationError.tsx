import React from "react";
import {
  Box,
  Typography,
  Container,
  Button,
  Grid,
  styled,
} from "@mui/material";
import RefreshTwoToneIcon from "@mui/icons-material/RefreshTwoTone";
import LoadingButton from "@mui/lab/LoadingButton";

const MainContent = styled(Box)(
  () => `
    height: 100%;
    display: flex;
    flex: 1;
    overflow: auto;
    flex-direction: column;
    align-items: center;
    justify-content: center;
`
);

const ApplicationError = ({
  onReset: reset,
  error: { message, details },
}: {
  onReset: () => void;
  error: { message: string; details: string };
}) => {
  return (
    <>
      <MainContent>
        <Grid
          container
          sx={{ height: "100%" }}
          alignItems="stretch"
          spacing={0}
        >
          <Grid
            xs={12}
            md={12}
            alignItems="center"
            display="flex"
            justifyContent="center"
            item
          >
            <Container maxWidth="sm">
              <Box textAlign="center">
                <img alt="500" height={260} src="/images/500.svg" />
                <Typography variant="h2" sx={{ my: 2 }}>
                  {message}
                </Typography>
                <Typography
                  variant="h4"
                  color="text.secondary"
                  fontWeight="normal"
                  sx={{ mb: 4 }}
                >
                  {details}
                </Typography>
                <LoadingButton
                  onClick={reset}
                  variant="outlined"
                  color="primary"
                  startIcon={<RefreshTwoToneIcon />}
                >
                  Refresh view
                </LoadingButton>
                <Button href="/" variant="contained" sx={{ ml: 1 }}>
                  Go back
                </Button>
              </Box>
            </Container>
          </Grid>
        </Grid>
      </MainContent>
    </>
  );
};

export default ApplicationError;
