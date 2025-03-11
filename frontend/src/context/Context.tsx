import { JobTitleProvider } from "./JobTitleContext";
import { AuthProvider } from "./AuthContext";
import { BranchProvider } from "./BranchContext";
import { DepartmentProvider } from "./DepartmentContext";
import { FC, ReactNode } from "react";

const AppProviders: FC<{ children: ReactNode }> = ({ children }) => {
    return (
        <AuthProvider>
            <BranchProvider>
                <JobTitleProvider>
                    <DepartmentProvider>{children}</DepartmentProvider>
                </JobTitleProvider>
            </BranchProvider>
        </AuthProvider>
    );
};

export default AppProviders;
