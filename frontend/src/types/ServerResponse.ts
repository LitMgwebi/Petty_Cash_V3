export type ServerResponse<T> = {
    data: T;
    success: boolean;
    message: string;
}