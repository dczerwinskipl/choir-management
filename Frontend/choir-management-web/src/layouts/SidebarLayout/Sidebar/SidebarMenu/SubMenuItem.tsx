import React from "react";
import { SubLinkNavigatorItem } from "@/infrastructure/navigating";
import { Button, ListItem } from "@mui/material";
import { Link } from "react-router-dom";

const SubLinkMenuItem = React.memo(
  ({
    item,
    pathname,
    onClick,
  }: {
    pathname: string;
    item: SubLinkNavigatorItem;
    onClick: () => void;
  }) => {
    return (
      <ListItem component="div">
        <Link to={item.to}>
          <Button
            className={pathname === item.to ? "active" : ""}
            disableRipple
            component="div"
            onClick={onClick}
          >
            {item.displayName ?? item.name}
          </Button>
        </Link>
      </ListItem>
    );
  }
);

export default SubLinkMenuItem;
