import { createContext, useContext, useState, ReactNode, FC } from "react"
import { ServerResponse } from "types/ServerResponse";
import { handleApiError } from "api/handleApiError";
import { Department } from "types/Department";
import axios from "axios";

interface DepartmentContextProps {
    departments: Department[];
    department: Department | null;
    getDepartments: () => Promise<ServerResponse<Department[]>>
    getDepartmentById: (id: number) => Promise<ServerResponse<Department>>
}

const DepartmentContext = createContext<DepartmentContextProps | undefined>(undefined)

export const DepartmentProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [departments, setDepartments] = useState<Department[]>([]);
    const [department, setDepartment] = useState<Department | null>(null);

    const getDepartments = async (): Promise<ServerResponse<Department[]>> => {
        try {
            const res = await axios.get<ServerResponse<Department[]>>("Departments/index");
            setDepartments(res.data.data)
            return res.data
        } catch (error) {
            return handleApiError<Department[]>(error)
        }
    }

    const getDepartmentById = async (id: number): Promise<ServerResponse<Department>> => {
        try {
            const res = await axios.get<ServerResponse<Department>>(`Branches/Departments/${id}`);
            setDepartment(res.data.data)
            return res.data
        } catch (error) {
            return handleApiError<Department>(error)
        }
    }

    return (
        <DepartmentContext.Provider value={{ departments, department, getDepartments, getDepartmentById }}>
            {children}
        </DepartmentContext.Provider>
    )
}

export const useDepartmentContext = () => {
    const context = useContext(DepartmentContext);

    if (!context) {
        throw new Error('useDepartmentContext must be used within a DepartmentProvider')
    }

    return context;
}