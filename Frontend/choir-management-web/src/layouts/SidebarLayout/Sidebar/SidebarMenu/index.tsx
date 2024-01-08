import React, { useContext } from "react";
import { Box, styled } from "@mui/material";
import { useLocation } from "react-router-dom";
import { SidebarContext } from "@/contexts/SidebarContext";
/*
import DesignServicesTwoToneIcon from '@mui/icons-material/DesignServicesTwoTone';
import BrightnessLowTwoToneIcon from '@mui/icons-material/BrightnessLowTwoTone';
import MmsTwoToneIcon from '@mui/icons-material/MmsTwoTone';
import TableChartTwoToneIcon from '@mui/icons-material/TableChartTwoTone';
import AccountCircleTwoToneIcon from '@mui/icons-material/AccountCircleTwoTone';
import BallotTwoToneIcon from '@mui/icons-material/BallotTwoTone';
import BeachAccessTwoToneIcon from '@mui/icons-material/BeachAccessTwoTone';
import EmojiEventsTwoToneIcon from '@mui/icons-material/EmojiEventsTwoTone';
import FilterVintageTwoToneIcon from '@mui/icons-material/FilterVintageTwoTone';
import HowToVoteTwoToneIcon from '@mui/icons-material/HowToVoteTwoTone';
import LocalPharmacyTwoToneIcon from '@mui/icons-material/LocalPharmacyTwoTone';
import RedeemTwoToneIcon from '@mui/icons-material/RedeemTwoTone';
import SettingsTwoToneIcon from '@mui/icons-material/SettingsTwoTone';
import TrafficTwoToneIcon from '@mui/icons-material/TrafficTwoTone';
import CheckBoxTwoToneIcon from '@mui/icons-material/CheckBoxTwoTone';
import ChromeReaderModeTwoToneIcon from '@mui/icons-material/ChromeReaderModeTwoTone';
import WorkspacePremiumTwoToneIcon from '@mui/icons-material/WorkspacePremiumTwoTone';
import CameraFrontTwoToneIcon from '@mui/icons-material/CameraFrontTwoTone';
import DisplaySettingsTwoToneIcon from '@mui/icons-material/DisplaySettingsTwoTone';*/
import { useNavigator } from "@/infrastructure/navigating";
import { RootMenuItem } from "./RootMenuItem";

const MenuWrapper = styled(Box)(
  ({ theme }) => `
  .MuiList-root {
    padding: ${theme.spacing(1)};

    & > .MuiList-root {
      padding: 0 ${theme.spacing(0)} ${theme.spacing(1)};
    }
  }

    .MuiListSubheader-root {
      text-transform: uppercase;
      font-weight: bold;
      font-size: ${theme.typography.pxToRem(12)};
      color: ${theme.colors.alpha.trueWhite[50]};
      padding: ${theme.spacing(0, 2.5)};
      line-height: 1.4;
    }
`
);

const SidebarMenu = () => {
  const { closeSidebar } = useContext(SidebarContext);
  const navigator = useNavigator();
  const { pathname } = useLocation();
  return (
    <>
      <MenuWrapper>
        {navigator.map((item, index) => (
          <RootMenuItem
            key={`${item.name}_${index}`}
            onClick={closeSidebar}
            item={item}
            pathname={pathname}
          />
        ))}
      </MenuWrapper>
    </>
  );
};

export default SidebarMenu;

/*
<List
          component="div"
          subheader={
            <ListSubheader component="div" disableSticky>
              Dashboards
            </ListSubheader>
          }
        >
          <SubMenuWrapper>
            <List component="div">
              <ListItem component="div">
                <Link to="/dashboards/tasks">
                  <Button
                    className={
                      pathname === '/dashboards/tasks' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<BrightnessLowTwoToneIcon />}
                  >
                    Manage Tasks
                  </Button>
                </Link>
              </ListItem>
              <ListItem component="div">
                <Link to="/applications/messenger">
                  <Button
                    className={
                      pathname === '/applications/messenger' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<MmsTwoToneIcon />}
                  >
                    Messenger
                  </Button>
                </Link>
              </ListItem>
            </List>
          </SubMenuWrapper>
        </List>
        <List
          component="div"
          subheader={
            <ListSubheader component="div" disableSticky>
              Management
            </ListSubheader>
          }
        >
          <SubMenuWrapper>
            <List component="div">
              <ListItem component="div">
                <Link to="/management/transactions">
                  <Button
                    className={
                      pathname === '/management/transactions'
                        ? 'active'
                        : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<TableChartTwoToneIcon />}
                  >
                    Transactions List
                  </Button>
                </Link>
              </ListItem>
            </List>
          </SubMenuWrapper>
        </List>
        <List
          component="div"
          subheader={
            <ListSubheader component="div" disableSticky>
              Accounts
            </ListSubheader>
          }
        >
          <SubMenuWrapper>
            <List component="div">
              <ListItem component="div">
                <Link to="/management/profile">
                  <Button
                    className={
                      pathname === '/management/profile' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<AccountCircleTwoToneIcon />}
                  >
                    User Profile
                  </Button>
                </Link>
              </ListItem>
              <ListItem component="div">
                <Link to="/management/profile/settings">
                  <Button
                    className={
                      pathname === '/management/profile/settings'
                        ? 'active'
                        : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<DisplaySettingsTwoToneIcon />}
                  >
                    Account Settings
                  </Button>
                </Link>
              </ListItem>
            </List>
          </SubMenuWrapper>
        </List>
        <List
          component="div"
          subheader={
            <ListSubheader component="div" disableSticky>
              Components
            </ListSubheader>
          }
        >
          <SubMenuWrapper>
            <List component="div">
              <ListItem component="div">
                <Link to="/components/buttons">
                  <Button
                    className={
                      pathname === '/components/buttons' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<BallotTwoToneIcon />}
                  >
                    Buttons
                  </Button>
                </Link>
              </ListItem>
              <ListItem component="div">
                <Link to="/components/modals">
                  <Button
                    className={
                      pathname === '/components/modals' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<BeachAccessTwoToneIcon />}
                  >
                    Modals
                  </Button>
                </Link>
              </ListItem>
              <ListItem component="div">
                <Link to="/components/accordions">
                  <Button
                    className={
                      pathname === '/components/accordions' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<EmojiEventsTwoToneIcon />}
                  >
                    Accordions
                  </Button>
                </Link>
              </ListItem>
              <ListItem component="div">
                <Link to="/components/tabs">
                  <Button
                    className={
                      pathname === '/components/tabs' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<FilterVintageTwoToneIcon />}
                  >
                    Tabs
                  </Button>
                </Link>
              </ListItem>
              <ListItem component="div">
                <Link to="/components/badges">
                  <Button
                    className={
                      pathname === '/components/badges' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<HowToVoteTwoToneIcon />}
                  >
                    Badges
                  </Button>
                </Link>
              </ListItem>
              <ListItem component="div">
                <Link to="/components/tooltips">
                  <Button
                    className={
                      pathname === '/components/tooltips' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<LocalPharmacyTwoToneIcon />}
                  >
                    Tooltips
                  </Button>
                </Link>
              </ListItem>
              <ListItem component="div">
                <Link to="/components/avatars">
                  <Button
                    className={
                      pathname === '/components/avatars' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<RedeemTwoToneIcon />}
                  >
                    Avatars
                  </Button>
                </Link>
              </ListItem>
              <ListItem component="div">
                <Link to="/components/cards">
                  <Button
                    className={
                      pathname === '/components/cards' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<SettingsTwoToneIcon />}
                  >
                    Cards
                  </Button>
                </Link>
              </ListItem>
              <ListItem component="div">
                <Link to="/components/forms">
                  <Button
                    className={
                      pathname === '/components/forms' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<TrafficTwoToneIcon />}
                  >
                    Forms
                  </Button>
                </Link>
              </ListItem>
            </List>
          </SubMenuWrapper>
        </List>
        <List
          component="div"
          subheader={
            <ListSubheader component="div" disableSticky>
              Extra Pages
            </ListSubheader>
          }
        >
          <SubMenuWrapper>
            <List component="div">
              <ListItem component="div">
                <Link to="/status/404">
                  <Button
                    className={pathname === '/status/404' ? 'active' : ''}
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<CheckBoxTwoToneIcon />}
                  >
                    Error 404
                  </Button>
                </Link>
              </ListItem>
              <ListItem component="div">
                <Link to="/status/500">
                  <Button
                    className={pathname === '/status/500' ? 'active' : ''}
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<CameraFrontTwoToneIcon />}
                  >
                    Error 500
                  </Button>
                </Link>
              </ListItem>
              <ListItem component="div">
                <Link to="/status/coming-soon">
                  <Button
                    className={
                      pathname === '/status/coming-soon' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<ChromeReaderModeTwoToneIcon />}
                  >
                    Coming Soon
                  </Button>
                </Link>
              </ListItem>
              <ListItem component="div">
                <Link to="/status/maintenance">
                  <Button
                    className={
                      pathname === '/status/maintenance' ? 'active' : ''
                    }
                    disableRipple
                    component="div"
                    onClick={closeSidebar}
                    startIcon={<WorkspacePremiumTwoToneIcon />}
                  >
                    Maintenance
                  </Button>
                </Link>
              </ListItem>
            </List>
          </SubMenuWrapper>
        </List>
        */
