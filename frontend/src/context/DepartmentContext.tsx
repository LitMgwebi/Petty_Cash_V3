import {createContext, useContext, useState, ReactNode, FC} from "react"
import axios from "axios";

interface Department {
    departmentId: number;
    name: string;
    description: string;
    divisions: [];
}

interface DepartmentContextProps {
    departments: Department[];
    getDepartments: () => Promise<void>;
}

const DepartmentContext = createContext<DepartmentContextProps | undefined>(undefined)

// export const DepartmentProvider: FC<{children: ReactNode}> = ({children}) => {
//     const [departments, setDepartments] = useState<Department[]>([]);

//     const getDepartments = async() => {
//         try {

//         } catch(error) {
//             console.error(error)
//         }
//     }
// }