import * as z from "zod";

const environmentSchema = z.object({
  NODE_ENV: z
    .enum(["development", "production", "test"])
    .default("development"),
  REACT_APP_MEMBERSHIP_API_URL: z.string().min(1),
});
type EnvType = ReturnType<typeof environmentSchema.parse>;

let env: EnvType;
const loadEnv = () => {
  env = environmentSchema.parse(process.env);
};
loadEnv();
export { env, loadEnv };
