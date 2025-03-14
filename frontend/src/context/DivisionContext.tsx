import { createContext, useContext, useState, ReactNode, FC } from "react";
import { ServerResponse } from "types/ServerResponse";
import { Division, DivisionAppend } from "types/Division";
import { handleApiError } from "api/handleApiError";
import axios from "axios";

interface DivisionContextProps {
    divisions: Division[];
    division: Division | null;
    getDivisions: () => Promise<ServerResponse<Division[]>>;
    getDivisionById: (id: number) => Promise<ServerResponse<Division>>;
    createDivision: (division: DivisionAppend) => Promise<ServerResponse<null>>;
    updateDivision: (division: Division) => Promise<ServerResponse<null>>;
    deleteDivision: (division: Division) => Promise<ServerResponse<null>>;
}

const DivisionContext = createContext<DivisionContextProps | undefined>(
    undefined
);

export const DivisionProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [divisions, setDivisions] = useState<Division[]>([]);
    const [division, setDivision] = useState<Division | null>(null);

    const getDivisions = async (): Promise<ServerResponse<Division[]>> => {
        try {
            const res =
                await axios.get<ServerResponse<Division[]>>("Divisions/index");
            setDivisions(res.data.data);
            return res.data;
        } catch (error) {
            return handleApiError<Division[]>(error);
        }
    };

    const getDivisionById = async (
        id: number
    ): Promise<ServerResponse<Division>> => {
        try {
            const res = await axios.get<ServerResponse<Division>>(
                `Divisions/details/${id}`
            );
            setDivision(res.data.data);
            return res.data;
        } catch (error) {
            return handleApiError<Division>(error);
        }
    };

    const createDivision = async (
        division: DivisionAppend
    ): Promise<ServerResponse<null>> => {
        try {
            const res = await axios.post<ServerResponse<null>>(
                `Divisions/create`,
                division
            );
            return res.data;
        } catch (error) {
            return handleApiError<null>(error);
        }
    };

    const updateDivision = async (
        division: Division
    ): Promise<ServerResponse<null>> => {
        try {
            const res = await axios.put<ServerResponse<null>>(
                `Divisions/edit`,
                division
            );
            return res.data;
        } catch (error) {
            return handleApiError<null>(error);
        }
    };

    const deleteDivision = async (
        division: Division
    ): Promise<ServerResponse<null>> => {
        try {
            const res = await axios.delete<ServerResponse<null>>(
                `Divisions/delete`,
                { data: division }
            );
            return res.data;
        } catch (error) {
            return handleApiError<null>(error);
        }
    };

    return (
        <DivisionContext.Provider
            value={{
                divisions,
                division,
                getDivisions,
                getDivisionById,
                createDivision,
                updateDivision,
                deleteDivision,
            }}
        >
            {children}
        </DivisionContext.Provider>
    );
};

export const useDivisionContext = () => {
    const context = useContext(DivisionContext);

    if (!context) {
        throw new Error(
            "useDivisionContext must be used within a DivisionProvider"
        );
    }

    return context;
};
