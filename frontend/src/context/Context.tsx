import { JobTitleProvider } from "./JobTitleContext";
import { AuthProvider } from "./AuthContext";
import { BranchProvider } from "./BranchContext";
import { DepartmentProvider } from "./DepartmentContext";
import { OfficeProvider } from "./OfficeContext";
import { FC, ReactNode } from "react";

const AppProviders: FC<{ children: ReactNode }> = ({ children }) => {
    return (
        <AuthProvider>
            <OfficeProvider>
                <BranchProvider>
                    <JobTitleProvider>
                        <DepartmentProvider>{children}</DepartmentProvider>
                    </JobTitleProvider>
                </BranchProvider>
            </OfficeProvider>
        </AuthProvider>
    );
};

export default AppProviders;
