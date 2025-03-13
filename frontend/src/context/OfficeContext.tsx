import { createContext, useContext, useState, ReactNode, FC } from "react";
import { ServerResponse } from "types/ServerResponse";
import { Office } from "types/Office";
import { handleApiError } from "api/handleApiError";
import axios from "axios";

interface OfficeContextProps {
    offices: Office[];
    office: Office | null;
    getOffices: () => Promise<ServerResponse<Office[]>>;
    getOfficeById: (id: number) => Promise<ServerResponse<Office>>;
    createOffice: (
        office: Omit<Office, "officeId">
    ) => Promise<ServerResponse<null>>;
    updateOffice: (office: Office) => Promise<ServerResponse<null>>;
    deleteOffice: (office: Office) => Promise<ServerResponse<null>>;
}

const OfficeContext = createContext<OfficeContextProps | undefined>(undefined);

export const OfficeProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [offices, setOffices] = useState<Office[]>([]);
    const [office, setOffice] = useState<Office | null>(null);

    const getOffices = async (): Promise<ServerResponse<Office[]>> => {
        try {
            const res =
                await axios.get<ServerResponse<Office[]>>("Offices/index");
            setOffices(res.data.data);
            return res.data;
        } catch (error: any) {
            return handleApiError<Office[]>(error);
        }
    };

    const getOfficeById = async (
        id: number
    ): Promise<ServerResponse<Office>> => {
        try {
            const res = await axios.get<ServerResponse<Office>>(
                `Offices/details/${id}`
            );
            setOffice(res.data.data);
            return res.data;
        } catch (error: any) {
            return handleApiError<Office>(error);
        }
    };

    const createOffice = async (
        office: Omit<Office, "officeId">
    ): Promise<ServerResponse<null>> => {
        try {
            const res = await axios.post<ServerResponse<null>>(
                `Offices/create`,
                office
            );
            return res.data;
        } catch (error: any) {
            return handleApiError<null>(error);
        }
    };

    const updateOffice = async (
        office: Office
    ): Promise<ServerResponse<null>> => {
        try {
            const res = await axios.put<ServerResponse<null>>(
                `Offices/edit`,
                office
            );
            return res.data;
        } catch (error: any) {
            return handleApiError<null>(error);
        }
    };

    const deleteOffice = async (
        office: Office
    ): Promise<ServerResponse<null>> => {
        try {
            const res = await axios.delete<ServerResponse<null>>(
                `Offices/delete`,
                { data: office }
            );
            return res.data;
        } catch (error: any) {
            return handleApiError<null>(error);
        }
    };

    return (
        <OfficeContext.Provider
            value={{
                offices,
                office,
                getOffices,
                getOfficeById,
                createOffice,
                updateOffice,
                deleteOffice,
            }}
        >
            {children}
        </OfficeContext.Provider>
    );
};

export const useOfficeContext = () => {
    const context = useContext(OfficeContext);

    if (!context) {
        throw new Error(
            "useOfficeContext must be used within a OfficeProvider"
        );
    }

    return context;
};
