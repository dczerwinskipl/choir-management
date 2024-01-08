import { FC, ReactNode } from "react";
import PropTypes from "prop-types";
import { Box } from "@mui/material";
import { LayoutProps } from "../types";

const BaseLayout: FC<LayoutProps> = ({ children }) => {
  return (
    <Box
      sx={{
        display: "flex",
        flex: 1,
        height: "100%",
      }}
    >
      {children}
    </Box>
  );
};

BaseLayout.propTypes = {
  children: PropTypes.node,
};

export default BaseLayout;
