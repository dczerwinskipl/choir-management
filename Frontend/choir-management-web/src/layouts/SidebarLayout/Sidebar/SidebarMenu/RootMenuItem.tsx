import React from "react";
import {
  ListSubheader,
  alpha,
  Box,
  List,
  styled,
  Button,
  ListItem,
} from "@mui/material";
import { Link } from "react-router-dom";
import {
  RootGroupNavigatorItem,
  RootLinkNavigatorItem,
  RootNavigatorItem,
  isRootGroupNavigatorItem,
  isRootLinkNavigatorItem,
} from "@/infrastructure/navigating";
import MenuItem from "./MenuItem";

const RootMenuWrapper = styled(Box)(
  ({ theme }) => `
    .MuiList-root {

      .MuiListItem-root {
        padding: 1px 0;

        a {
          width: 100%;
          text-decoration: none;
        }

        .MuiBadge-root {
          position: absolute;
          right: ${theme.spacing(3.2)};

          .MuiBadge-standard {
            background: ${theme.colors.primary.main};
            font-size: ${theme.typography.pxToRem(10)};
            font-weight: bold;
            text-transform: uppercase;
            color: ${theme.palette.primary.contrastText};
          }
        }
    
        .MuiButton-root {
          display: flex;
          color: ${theme.colors.alpha.trueWhite[70]};
          background-color: transparent;
          width: 100%;
          justify-content: flex-start;
          padding: ${theme.spacing(1.2, 3)};

          .MuiButton-startIcon,
          .MuiButton-endIcon {
            transition: ${theme.transitions.create(["color"])};

            .MuiSvgIcon-root {
              font-size: inherit;
              transition: none;
            }
          }

          .MuiButton-startIcon {
            color: ${theme.colors.alpha.trueWhite[30]};
            font-size: ${theme.typography.pxToRem(20)};
            margin-right: ${theme.spacing(1)};
          }
          
          .MuiButton-endIcon {
            color: ${theme.colors.alpha.trueWhite[50]};
            margin-left: auto;
            opacity: .8;
            font-size: ${theme.typography.pxToRem(20)};
          }

          &.active,
          &:hover {
            background-color: ${alpha(theme.colors.alpha.trueWhite[100], 0.06)};
            color: ${theme.colors.alpha.trueWhite[100]};

            .MuiButton-startIcon,
            .MuiButton-endIcon {
              color: ${theme.colors.alpha.trueWhite[100]};
            }
          }
        }

        &.Mui-children {
          flex-direction: column;

          .MuiBadge-root {
            position: absolute;
            right: ${theme.spacing(7)};
          }
        }

        .MuiCollapse-root {
          width: 100%;

          .MuiList-root {
            padding: ${theme.spacing(1, 0)};
          }

          .MuiListItem-root {
            padding: 1px 0;

            .MuiButton-root {
              padding: ${theme.spacing(0.8, 3)};

              .MuiBadge-root {
                right: ${theme.spacing(3.2)};
              }

              &:before {
                content: ' ';
                background: ${theme.colors.alpha.trueWhite[100]};
                opacity: 0;
                transition: ${theme.transitions.create([
                  "transform",
                  "opacity",
                ])};
                width: 6px;
                height: 6px;
                transform: scale(0);
                transform-origin: center;
                border-radius: 20px;
                margin-right: ${theme.spacing(1.8)};
              }

              &.active,
              &:hover {

                &:before {
                  transform: scale(1);
                  opacity: 1;
                }
              }
            }
          }
        }
      }
    }
`
);

const RootGroupMenuItem = React.memo(
  ({
    item,
    pathname,
    onClick,
  }: {
    pathname: string;
    item: RootGroupNavigatorItem;
    onClick: () => void;
  }) => {
    return (
      <List
        component="div"
        subheader={
          <ListSubheader component="div" disableSticky>
            {item.displayName ?? item.name}
          </ListSubheader>
        }
      >
        <RootMenuWrapper>
          <List component="div">
            {item.children.map((item, index) => (
              <MenuItem
                key={`${item.name}_${index}`}
                item={item}
                onClick={onClick}
                pathname={pathname}
              />
            ))}
          </List>
        </RootMenuWrapper>
      </List>
    );
  }
);

const RootLinkMenuItem = React.memo(
  ({
    item,
    pathname,
    onClick,
  }: {
    pathname: string;
    item: RootLinkNavigatorItem;
    onClick: () => void;
  }) => {
    return (
      <List component="div">
        <RootMenuWrapper>
          <List component="div">
            <ListItem component="div">
              <Link to={item.to}>
                <Button
                  className={pathname === item.to ? "active" : ""}
                  disableRipple
                  component="div"
                  onClick={onClick}
                  startIcon={item.icon}
                >
                  {item.displayName ?? item.name}
                </Button>
              </Link>
            </ListItem>
          </List>
        </RootMenuWrapper>
      </List>
    );
  }
);

export const RootMenuItem = React.memo(
  ({
    item,
    pathname,
    onClick,
  }: {
    pathname: string;
    item: RootNavigatorItem;
    onClick: () => void;
  }) =>
    isRootGroupNavigatorItem(item) ? (
      <RootGroupMenuItem pathname={pathname} item={item} onClick={onClick} />
    ) : isRootLinkNavigatorItem(item) ? (
      <RootLinkMenuItem pathname={pathname} item={item} onClick={onClick} />
    ) : null
);
