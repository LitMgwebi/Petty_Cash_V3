import { createContext, useContext, useState, ReactNode, FC } from "react"
import { ServerResponse } from "types/ServerResponse";
import { handleApiError } from "api/handleApiError";
import { JobTitle } from "types/JobTitle";
import axios from "axios";

interface JobTitleContextProps {
    jobTitles: JobTitle[];
    jobTitle: JobTitle | null;
    getJobTitles: () => Promise<ServerResponse<JobTitle[]>>
    getJobTitleById: (id: number) => Promise<ServerResponse<JobTitle>>
}

const JobTitleContext = createContext<JobTitleContextProps | undefined>(undefined)

export const JobTitleProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [jobTitles, setJobTitles] = useState<JobTitle[]>([]);
    const [jobTitle, setJobTitle] = useState<JobTitle | null>(null);

    const getJobTitles = async (): Promise<ServerResponse<JobTitle[]>> => {
        try {
            const res = await axios.get<ServerResponse<JobTitle[]>>("JobTitles/index");
            setJobTitles(res.data.data)
            return res.data
        } catch (error) {
            return handleApiError<JobTitle[]>(error)
        }
    }

    const getJobTitleById = async (id: number): Promise<ServerResponse<JobTitle>> => {
        try {
            const res = await axios.get<ServerResponse<JobTitle>>(`Branches/JobTitles/${id}`);
            setJobTitle(res.data.data)
            return res.data
        } catch (error) {
            return handleApiError<JobTitle>(error)
        }
    }

    return (
        <JobTitleContext.Provider value={{ jobTitles, jobTitle, getJobTitles, getJobTitleById }}>
            {children}
        </JobTitleContext.Provider>
    )
}

export const useJobTitleContext = () => {
    const context = useContext(JobTitleContext);

    if (!context) {
        throw new Error('useJobTitleContext must be used within a JobTitleProvider')
    }

    return context;
}