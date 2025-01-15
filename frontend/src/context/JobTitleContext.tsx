import {createContext, useContext, useState, ReactNode, FC} from "react"
import axiosBase from "@/api/axios"

interface JobTitle {
    jobTitleId: number;
    description: string;
}

interface JobTitleContextProps {
    jobTitles: JobTitle[];
    getJobTitles: () => Promise<void>
}

const JobTitleContext = createContext<JobTitleContextProps | undefined>(undefined)

export const JobTitleProvider: FC<{children: ReactNode}> = ({children}) => {
    const [jobTitles, setJobTitles] = useState<JobTitle[]>([]);

    const getJobTitles = async() => {
        const res = await axiosBase.get('');
        setJobTitles(res.data.data)
    }

    return (
        <JobTitleContext.Provider value={{jobTitles, getJobTitles}}>
            {children}
        </JobTitleContext.Provider>
    )
}

export const useJobTitleContext = () => {
    const context = useContext(JobTitleContext);

    if(!context) {
        throw new Error('useJobTitleContext must be used within a JobTitleProvider')
    }

    return context;
}