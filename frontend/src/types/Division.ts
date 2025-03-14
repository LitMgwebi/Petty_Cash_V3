import { Department } from "./Department";

export interface DivisionAppend {
    name: string;
    description: string;
    departmentId: number;
}

export interface Division {
    divisionId: number;
    name: string;
    description: string;
    departmentId: number;
    department: Department;
}

export interface DivisionError {
    name: string;
    description: string;
    departmentId: string;
}
