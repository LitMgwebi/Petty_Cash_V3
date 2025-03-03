import { FC, useCallback, useEffect, useRef } from "react"
import { useBranchContext } from "context/BranchContext"
import { ServerResponse } from "types/ServerResponse";
import { Branch } from "types/Branch";
import { Container, Box } from "@mui/material";
import { DataGrid, GridColDef } from "@mui/x-data-grid";


const List: FC = () => {
    const { branches, getBranches } = useBranchContext();
    const intervalRef = useRef<NodeJS.Timeout | null>(null)

    const fetchBranches = useCallback(async () => {
        const response: ServerResponse<Branch[]> = await getBranches();
        const { success, message } = response;

        if (!success) {
            console.error(message)
        }
    }, [getBranches])

    useEffect(() => {
        fetchBranches();
        intervalRef.current = setInterval(() => {
            fetchBranches();
        }, 20000); // 20 seconds in milliseconds

        // Clean up the interval when the component unmounts
        return () => {
            if (intervalRef.current) {
                clearInterval(intervalRef.current);
            }
        };
    }, []);

    const columns: GridColDef<(typeof branches)[number]>[] = [
        { field: 'branchId', headerName: 'ID' },
        { field: 'name', headerName: 'Branch Name' },
        { field: 'description', headerName: 'Branch Description' },
    ]

    return (
        <Container>
            <Box sx={{ height: 'auto', width: '100%' }}>
                <DataGrid
                    rows={branches}
                    columns={columns}
                    initialState={{
                        pagination: {
                            paginationModel: {
                                pageSize: 5
                            }
                        }
                    }}
                    pageSizeOptions={[5]}
                    disableRowSelectionOnClick
                />
            </Box>
        </Container>
    )
}

export default List;