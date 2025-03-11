export interface Register {
    firstName: string;
    lastName: string;
    phoneNumber: string;
    email: string;
    password: string;
    confirmPassword: string;
    divisionId: number;
    jobTitleId: number;
    officeId: number;
}

export interface RegisterError {
    firstName: string;
    lastName: string;
    phoneNumber: string;
    email: string;
    password: string;
    confirmPassword: string;
    divisionId: number;
    jobTitleId: number;
    officeId: number;
}
