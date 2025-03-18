import { JobTitleProvider } from "./JobTitleContext";
import { AuthProvider } from "./AuthContext";
import { BranchProvider } from "./BranchContext";
import { DepartmentProvider } from "./DepartmentContext";
import { OfficeProvider } from "./OfficeContext";
import { DivisionProvider } from "./DivisionContext";
import { AccountSetProvider } from "./AccountSetContext";
import { FC, ReactNode } from "react";

const AppProviders: FC<{ children: ReactNode }> = ({ children }) => {
    return (
        <AuthProvider>
            <AccountSetProvider>
                <OfficeProvider>
                    <DivisionProvider>
                        <BranchProvider>
                            <JobTitleProvider>
                                <DepartmentProvider>
                                    {children}
                                </DepartmentProvider>
                            </JobTitleProvider>
                        </BranchProvider>
                    </DivisionProvider>
                </OfficeProvider>
            </AccountSetProvider>
        </AuthProvider>
    );
};

export default AppProviders;
