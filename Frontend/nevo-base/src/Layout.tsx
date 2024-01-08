import { AppNavigator, useAppNavigatorContext } from "@nevo/app/navigation";
import { hasRole, useAppUserContext } from "@nevo/app/user";
import { Card, CardHeader } from "@nevo/design-system/card";
import { ContentContainer } from "@nevo/design-system/content-container";
import { PageHeader } from "@nevo/design-system/page-header";
import { TopBar } from "@nevo/design-system/top-bar";
import React, { useEffect, useState } from "react";
import { Route, Routes } from "react-router-dom";

const Layout: React.FC = () => {
  const context = useAppUserContext();
  const handleLogout = async () => {
    if (context.isLoggedIn) {
      await context.logout();
    }
  };
  return (
    <div className="flex flex-row h-full w-full">
      <AppNavigator />
      <div className="flex-1 flex flex-col h-full">
        <TopBar>
          Something <button onClick={handleLogout}>Logout</button>
        </TopBar>
        <Routes>
          <Route path="/" element={<Page />} />
          <Route path="/User/" element={<Page />} />
          <Route path="*" element={<>404</>} />
        </Routes>
      </div>
    </div>
  );
};
export default Layout;

export const Page = () => {
  const [toggle, setToggle] = useState(true);
  const { setSection } = useAppNavigatorContext();
  useEffect(() => {
    setSection(
      "PageToggle",
      {
        title: "Identity",
        items: [
          {
            title: "Users",
            iconName: "manage_accounts",
            items: [
              {
                title: "User list",
                href: "#",
                isVisible: hasRole("ADMIN"),
              },
              {
                title: "User groups",
                href: "#",
              },
            ],
          },
          {
            title: "Roles",
            iconName: "assignment_ind",
            isVisible: hasRole("ADMIN"),
            items: [
              {
                title: "Role list",
                href: "#",
              },
              {
                title: "Role relations",
                href: "#",
              },
            ],
          },
        ],
      },
      toggle
    );
  }, [toggle, setSection]);
  return (
    <>
      <PageHeader
        title="Page 1"
        subTitle="Some description"
        iconName="home"
        iconSize="s"
      />
      <ContentContainer className="flex-1">
        <Card>
          <CardHeader title="Hello world!" subTitle="Show must go on" />
          Some other text
          <div>
            <input
              type={"checkbox"}
              checked={toggle}
              onChange={(evt) => setToggle(evt.target.checked)}
            />{" "}
            Show menu
          </div>
        </Card>
      </ContentContainer>
    </>
  );
};
