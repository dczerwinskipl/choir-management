import React from "react";
import { Appbar } from "react-native-paper";

interface IContextualActionBarProps {
  title: string;
  close: () => void;
}

const ContextualActionBar: React.FunctionComponent<
  IContextualActionBarProps
> = (props) => {
  return (
    <Appbar.Header {...props} style={{ width: "100%" }}>
      <Appbar.Action icon="close" onPress={props.close} />
      <Appbar.Content title={props.title} />
      <Appbar.Action icon="delete" onPress={() => {}} />
      <Appbar.Action icon="content-copy" onPress={() => {}} />
      <Appbar.Action icon="magnify" onPress={() => {}} />
      <Appbar.Action icon="dots-vertical" onPress={() => {}} />
    </Appbar.Header>
  );
};
export default ContextualActionBar;
