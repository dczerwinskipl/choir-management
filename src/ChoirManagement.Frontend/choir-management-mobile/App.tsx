import React from "react";
import { createDrawerNavigator } from "@react-navigation/drawer";
import { StatusBar } from "expo-status-bar";
import { SafeAreaProvider } from "react-native-safe-area-context";
import MyFriends from "./MyFriends";
import Profile from "./Profile";
import MembersGrid from "./members/MemberGrid";
import MenuIcon from "./components/MenuIcon";
import MenuContent from "./components/MenuContent";
import { QueryClientProvider, QueryClient } from "react-query";
import { Provider } from "react-native-paper";
import { NavigationContainer, Theme } from "@react-navigation/native";
import { combineThemes } from "./styles/theme";

export default function App() {
  console.log("1");
  const theme = combineThemes("light");
  console.log("2");
  const Drawer = createDrawerNavigator();
  console.log("3");
  const queryClient = new QueryClient();
  console.log("4");
  return (
    <SafeAreaProvider>
      <Provider theme={theme as ReactNativePaper.Theme}>
        <QueryClientProvider client={queryClient}>
          <NavigationContainer theme={theme as Theme}>
            <Drawer.Navigator
              screenOptions={{
                headerShown: true,
                headerLeft: () => <MenuIcon />,
              }}
              drawerContent={(props) => <MenuContent {...props} />}
            >
              <Drawer.Screen name="My Friends" component={MyFriends} />
              <Drawer.Screen name="Profile" component={Profile} />
              <Drawer.Screen name="Members" component={MembersGrid} />
            </Drawer.Navigator>
          </NavigationContainer>
        </QueryClientProvider>
        <StatusBar style="auto" />
      </Provider>
    </SafeAreaProvider>
  );
}
