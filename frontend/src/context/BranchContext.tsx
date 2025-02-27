import {createContext, useContext, useState, ReactNode, FC} from "react"
import { ServerResponse } from "types/ServerResponse";
import { Branch } from "types/Branch";
import { handleApiError } from "api/handleApiError";
import axios from "axios";



interface BranchContextProps {
    branches: Branch[];
    branch: Branch | null;
    getBranches: () => Promise<ServerResponse<Branch[]>>
    getBranchById: (id: number) => Promise<ServerResponse<Branch>>
    createBranch: (branch: Omit<Branch, 'branchId'>) => Promise<ServerResponse<null>>
    updateBranch: (branch: Branch) => Promise<ServerResponse<null>>
    deleteBranch: (branch: Branch) => Promise<ServerResponse<null>>
}

const BranchContext = createContext<BranchContextProps | undefined>(undefined)

export const BranchProvider: FC<{children: ReactNode}> = ({children}) => {
    const [branches, setBranches] = useState<Branch[]>([]);
    const [branch, setBranch] = useState<Branch| null>(null);

    const getBranches = async(): Promise<ServerResponse<Branch[]>> => {
        try {
            const res = await axios.get<ServerResponse<Branch[]>>("Branches/index");
            setBranches(res.data.data)
            return res.data
        } catch(error) {
            return handleApiError<Branch[]>(error)
        }
    }

    const getBranchById = async(id: number): Promise<ServerResponse<Branch>> => {
        try {
            const res = await axios.get<ServerResponse<Branch>>(`Branches/details/${id}`);
            setBranch(res.data.data)
            return res.data
        } catch(error) {
            return handleApiError<Branch>(error)
        }
    }

    const createBranch = async (branch: Omit<Branch, 'branchId'>): Promise<ServerResponse<null>> => {
        try {
            const res = await axios.post<ServerResponse<null>>(`Branches/create`, branch);
            return res.data
        } catch(error) {
            return handleApiError<null>(error)
        }
    }

    const updateBranch= async (branch: Branch): Promise<ServerResponse<null>> =>{
        try {
            const res = await axios.post<ServerResponse<null>>(`Branches/edit`, branch);
            return res.data
        } catch(error) {
            return handleApiError<null>(error)
        }
    }

    const deleteBranch = async (branch: Branch): Promise<ServerResponse<null>> => {
        try {
            const res = await axios.post<ServerResponse<null>>(`Branches/delete`, branch);
            return res.data
        } catch(error) {
            return handleApiError<null>(error)
        }
    }

    return (
        <BranchContext.Provider value={{branches, branch, getBranches, getBranchById, createBranch, updateBranch, deleteBranch}}>
            {children}
        </BranchContext.Provider>
    )
}

export const useBranchContext = () => {
    const context = useContext(BranchContext)

    if(!context) {
        throw new Error('useBranchContext must be used within a BranchProvider')
    }

    return context;
}