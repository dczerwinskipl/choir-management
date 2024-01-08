import dashboardModule from "@/modules/dashboard";
import usersModule from "@/modules/users";
import errorPages from "@/modules/errorPages";
import bootstrap from "./bootstrap";

export default bootstrap([
  dashboardModule("/"),
  usersModule("/users"),
  errorPages(),
]);
