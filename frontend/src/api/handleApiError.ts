import { ServerResponse } from "types/ServerResponse";
import { AxiosError } from "axios";

export const handleApiError = <T>(err: unknown): ServerResponse<T> => {
    const error = err as AxiosError<ServerResponse<T>>;
    return {
        data: error.response?.data?.data ?? null as unknown as T,
        success: false,
        message: error.response?.data?.message || "An unexpected error occurred",
    };
};