import axios, { AxiosInstance } from "axios";
import env from "../env"

let clientInstances: {[key: string]: AxiosInstance} = {}; 
const client = (api: string) => {
    if(!clientInstances[api]) {
        clientInstances[api] = axios.create({
        baseURL: env.config.apis[api].url, //TODO: throw error when there is no key
        headers: {
            "Content-type": "application/json; charset=UTF-8",
        },
        });
    }
    return clientInstances[api];
}

export default client;