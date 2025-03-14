import { JobTitleProvider } from "./JobTitleContext";
import { AuthProvider } from "./AuthContext";
import { BranchProvider } from "./BranchContext";
import { DepartmentProvider } from "./DepartmentContext";
import { OfficeProvider } from "./OfficeContext";
import { DivisionProvider } from "./DivisionContext";
import { FC, ReactNode } from "react";

const AppProviders: FC<{ children: ReactNode }> = ({ children }) => {
    return (
        <AuthProvider>
            <OfficeProvider>
                <DivisionProvider>
                    <BranchProvider>
                        <JobTitleProvider>
                            <DepartmentProvider>{children}</DepartmentProvider>
                        </JobTitleProvider>
                    </BranchProvider>
                </DivisionProvider>
            </OfficeProvider>
        </AuthProvider>
    );
};

export default AppProviders;
