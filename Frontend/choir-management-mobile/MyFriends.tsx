import { useIsFocused, useNavigation } from "@react-navigation/native";
import ContextualActionBar from "./components/ContextualActionBar";
import React, { useCallback, useEffect, useState } from "react";
import { View } from "react-native";
import { FAB, List, Portal, Title } from "react-native-paper";
import base from "./styles/base";

interface IMyFriendsProps {}

const MyFriends: React.FunctionComponent<IMyFriendsProps> = (props) => {
  const isScreenFocused = useIsFocused();
  const [fabIsOpen, setFabIsOpen] = useState(false);
  const [cabIsOpen, setCabIsOpen] = useState(false);

  const navigation = useNavigation();
  const openHeader = useCallback(
    (str: string) => {
      setSelectedItemName(str);
      setCabIsOpen(!cabIsOpen);
    },
    [cabIsOpen]
  );

  const [selectedItemName, setSelectedItemName] = useState("");
  const closeHeader = useCallback(() => {
    setCabIsOpen(false);
    setSelectedItemName("");
  }, []);

  useEffect(() => {
    if (cabIsOpen) {
      navigation.setOptions({
        header: (props: any) => (
          <ContextualActionBar
            {...props}
            title={selectedItemName}
            close={closeHeader}
          />
        ),
      });
    } else {
      navigation.setOptions({ header: undefined });
    }
  }, [cabIsOpen, selectedItemName]);

  return (
    <View style={base.centered}>
      <Title>MyFriends</Title>
      <List.Item
        title="Friend #1"
        description="Mar 18 | 3:31 PM"
        style={{ width: "100%" }}
        onPress={() => {}}
        onLongPress={() => openHeader("Friend #1")}
      />
      <Portal>
        <FAB.Group
          visible={isScreenFocused}
          open={fabIsOpen}
          onStateChange={({ open }) => setFabIsOpen(open)}
          icon={fabIsOpen ? "close" : "account-multiple"}
          actions={[
            {
              icon: "plus",
              label: "Add new friend",
              onPress: () => {},
            },
            {
              icon: "file-export",
              label: "Export friend list",
              onPress: () => {},
            },
          ]}
        />
      </Portal>
    </View>
  );
};
export default MyFriends;
