import { JobTitleProvider } from "./JobTitleContext";
import { BranchProvider } from "./BranchContext";
import { FC, ReactNode } from "react";

const AppProviders: FC<{children: ReactNode}> = ({children}) => {
    return (
        <BranchProvider>
            <JobTitleProvider>
                {children}
            </JobTitleProvider>
        </BranchProvider>
    )
}

export default AppProviders;