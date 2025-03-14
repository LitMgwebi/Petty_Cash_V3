import { FC } from "react";
import { Division } from "types/Division";
import DataGridLoading from "components/DataGridLoading";
import { Box } from "@mui/material";
import {
    DataGrid,
    GridActionsCellItem,
    GridColDef,
    GridRowId,
} from "@mui/x-data-grid";
import { Delete, Edit } from "@mui/icons-material";

interface ListDivisionProps {
    divisions: Division[];
    message: string;
    loading: boolean;
    setSuccess: (message: string) => void;
    setError: (message: string) => void;
    refreshDivisions: () => void;
    setDivisionToDelete: (division: Division) => void;
    setDivisionToEdit: (division: Division) => void;
}

export const DivisionList: FC<ListDivisionProps> = ({
    divisions,
    message,
    loading,
    setError,
    setDivisionToDelete,
    setDivisionToEdit,
}) => {
    const handleEditClick = (divisionId: GridRowId) => () => {
        const division: Division | undefined = divisions.find(
            (division) => division.divisionId === divisionId
        );

        if (division !== undefined) setDivisionToEdit(division);
        else setError("System could not resolve Division ID when deleting.");
    };

    const handleDeleteClick = (divisionId: GridRowId) => () => {
        const division: Division | undefined = divisions.find(
            (division) => division.divisionId === divisionId
        );

        if (division !== undefined) setDivisionToDelete(division);
        else setError("System could not resolve Division ID when deleting.");
    };

    let columns: GridColDef<Division>[] = [
        { field: "divisionId", headerName: "ID", flex: 1 },
        {
            field: "name",
            headerName: "Abbreviation",
            flex: 2,
        },
        {
            field: "description",
            headerName: "Name",
            flex: 2,
        },
        {
            field: "department",
            headerName: "Department",
            flex: 2,
            valueGetter: (params: { name: string }) => params.name || "",
        },
        {
            field: "actions",
            type: "actions",
            headerName: "Actions",
            flex: 1,
            getActions: ({ id }) => {
                return [
                    <GridActionsCellItem
                        icon={<Edit />}
                        label="Edit"
                        onClick={handleEditClick(id)}
                    />,
                    <GridActionsCellItem
                        icon={<Delete />}
                        label="Delete"
                        onClick={handleDeleteClick(id)}
                    />,
                ];
            },
        },
    ];
    return (
        <Box sx={{ minHeight: "50vh", width: "80%", margin: 4 }}>
            <DataGrid
                rows={divisions}
                columns={columns}
                getRowId={(row) => row.divisionId}
                initialState={{
                    pagination: {
                        paginationModel: {
                            pageSize: 5,
                        },
                    },
                }}
                pageSizeOptions={[5]}
                disableRowSelectionOnClick
                localeText={{
                    noRowsLabel: message,
                }}
                loading={loading}
                slots={{ loadingOverlay: DataGridLoading }}
            />
        </Box>
    );
};

export default DivisionList;
