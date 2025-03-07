export interface Branch {
    branchId: number;
    name: string;
    description: string;
    glaccounts: [];
}

export interface BranchError {
    name: string;
    description: string;
}
