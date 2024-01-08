import {
  UseMutationResult,
  UseQueryResult,
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";
import client from "./client";
import { AxiosInstance } from "axios";

type CrudApiOptions<K> = {
  resourceName: string;
  endpoints: {
    get(key: K): string;
    getAll: string;
    add: string;
    update: string;
  };
};
const defaultOptions = <K>(resourceName: string) => ({
  resourceName,
  endpoints: {
    get: (key: K) => `/${resourceName}/${key}`,
    getAll: `/${resourceName}`,
    add: `/${resourceName}`,
    update: `/${resourceName}`,
    delete: `/${resourceName}`,
  },
});

type CrudApi<T, K, AT extends Partial<T> = T, UT extends Partial<T> = AT> = {
  useGet: (key: K) => UseQueryResult<T, any>;
  useGetAll: () => UseQueryResult<T[], any>;
  useAdd: () => UseMutationResult<T, any, AT>;
  useUpdate: (input: UT) => any;
  useDelete: (key: K) => any;
};

const crudApi =
  <T, AT extends Partial<T> = T, UT extends Partial<T> = AT>() =>
  <K, R extends string>(
    apiName: R,
    keySelector: (data: T) => K,
    options: Partial<CrudApiOptions<K>> = {}
  ): CrudApi<T, K, AT, UT> => {
    const apiOptions = {
      ...defaultOptions<K>(options.resourceName ?? apiName),
      ...options,
    };

    const get = (clientInstance: AxiosInstance) => async (key: K) => {
      const response = await clientInstance.get<T>(
        apiOptions.endpoints.get(key)
      );
      return response.data;
    };

    const getAll = (clientInstance: AxiosInstance) => async () => {
      const response = await clientInstance.get<T>(apiOptions.endpoints.getAll);
      await new Promise((resolve) => setTimeout(resolve, 5000));
      return response.data;
    };

    const add = (clientInstance: AxiosInstance) => async (data: AT) => {
      const response = await clientInstance.post<T>(
        apiOptions.endpoints.add,
        data
      );
      return response.data;
    };

    return {
      useGet: (id: K) =>
        useQuery([apiName, id], async () => await get(client(apiName))(id)),
      useGetAll: () =>
        useQuery([apiName], async () => await getAll(client(apiName))()),
      useAdd: () => {
        const clientInstance = client(apiName);
        const queryClient = useQueryClient();
        return useMutation({
          mutationFn: add(clientInstance),
          onSuccess: (newData) => {
            if (newData) {
              const id = keySelector(newData);
              queryClient.setQueryData<T>([apiName, id], (oldData) => ({
                ...oldData,
                ...newData,
              }));
              queryClient.setQueryData<T[]>([apiName], (old) => [
                ...(old ?? []),
                newData,
              ]);
            }
          },
        });
      },
      useUpdate: (_: UT) => null,
      useDelete: (_: K) => null,
    };
  };

export default crudApi;
