import { JobTitleProvider } from "./JobTitleContext";
import { BranchProvider } from "./BranchContext";
import { DepartmentProvider } from "./DepartmentContext";
import { FC, ReactNode } from "react";

const AppProviders: FC<{ children: ReactNode }> = ({ children }) => {
    return (
        <BranchProvider>
            <JobTitleProvider>
                <DepartmentProvider>
                    {children}
                </DepartmentProvider>
            </JobTitleProvider>
        </BranchProvider>
    )
}

export default AppProviders;