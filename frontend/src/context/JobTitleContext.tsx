import {createContext, useContext, useState, ReactNode, FC} from "react"
import axios from "axios";

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
        try {
            const res = await axios({
                url: "api/JobTitles/index",
                method: "GET"
            });
            setJobTitles(res.data.data)
        } catch(error) {
            console.error(error)
        }
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