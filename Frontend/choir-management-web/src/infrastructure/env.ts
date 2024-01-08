import * as z from "zod";

type AppConfig = {
  apis: { [name: string]: { url: string } }
}

const environmentSchema = z.object({
  NODE_ENV: z
    .enum(["development", "production", "test"])
    .default("development"),
  REACT_APP_MEMBERSHIP_API_URL: z.string().min(1),
  REACT_APP_USERS_API_URL: z.string().min(1),
});
type EnvType = ReturnType<typeof environmentSchema.parse>;

let instance: AppConfig | null = null;
const buildConfigFromEnv = (env: EnvType): AppConfig => ({
  apis: {
    membership: {
      url: env.REACT_APP_MEMBERSHIP_API_URL
    },
    users: {
      url: env.REACT_APP_USERS_API_URL
    }
  }
})

class Env 
{
  get config () {
    if(instance == null)
      instance = buildConfigFromEnv(environmentSchema.parse(process.env));
    return instance;
  };
}
const env = new Env();

export default env;