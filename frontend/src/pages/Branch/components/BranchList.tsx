import { FC, useState } from "react";
import { Branch } from "types/Branch";
import DataGridLoading from "components/DataGridLoading";
import { Box } from "@mui/material";
import {
    DataGrid,
    GridActionsCellItem,
    GridColDef,
    GridEventListener,
    GridRowEditStopReasons,
    GridRowId,
    GridRowModes,
    GridRowModesModel,
} from "@mui/x-data-grid";
import { useBranchContext } from "context/BranchContext";
import { Delete, Edit, Save, Cancel } from "@mui/icons-material";

interface ListBranchProps {
    branches: Branch[];
    message: string;
    loading: boolean;
    setSuccess: (message: string) => void;
    setError: (message: string) => void;
    refreshBranches: () => void;
    setBranchToDelete: (branch: Branch) => void;
}

const BranchList: FC<ListBranchProps> = ({
    branches,
    message,
    loading,
    setSuccess,
    setError,
    refreshBranches,
    setBranchToDelete,
}) => {
    const [rowModesModel, setRowsModesModel] = useState<GridRowModesModel>({});
    const { updateBranch } = useBranchContext();

    const handleRowEditStop: GridEventListener<"rowEditStop"> = (
        params,
        event
    ) => {
        if (params.reason === GridRowEditStopReasons.rowFocusOut) {
            event.defaultMuiPrevented = true;
        }
    };

    const handleEditClick = (branchId: GridRowId) => () => {
        setRowsModesModel({
            ...rowModesModel,
            [branchId]: { mode: GridRowModes.Edit },
        });
    };

    const handleSaveClick = (branchId: GridRowId) => () => {
        setRowsModesModel({
            ...rowModesModel,
            [branchId]: { mode: GridRowModes.View },
        });
    };

    const handleCancelClick = (branchId: GridRowId) => () => {
        setRowsModesModel({
            ...rowModesModel,
            [branchId]: { mode: GridRowModes.View, ignoreModifications: true },
        });
    };

    const handleDeleteClick = (branchId: GridRowId) => () => {
        const branch: Branch | undefined = branches.find(
            (branch) => branch.branchId === branchId
        );

        if (branch !== undefined) setBranchToDelete(branch);
        else setError("System could not resolve Branch ID when deleting.");
    };

    const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
        setRowsModesModel(newRowModesModel);
    };

    const processRowUpdate = async (
        newRow: Branch,
        oldRow: Branch
    ): Promise<Branch> => {
        try {
            const response = await updateBranch(newRow);

            if (!response.success) {
                setError(response.message);
                refreshBranches();
                return oldRow;
            }
            setSuccess(response.message);
            refreshBranches();
            return newRow;
        } catch (error: any) {
            setError(
                error.response?.data?.message ||
                    "System could not resolve updating action."
            );
            refreshBranches();
            return oldRow;
        }
    };

    let columns: GridColDef<(typeof branches)[number]>[] = [];
    if (branches) {
        columns = [
            { field: "branchId", headerName: "ID", flex: 1 },
            {
                field: "name",
                headerName: "Abbreviation",
                flex: 2,
                editable: true,
            },
            {
                field: "description",
                headerName: "Name",
                flex: 3,
                editable: true,
            },
            {
                field: "actions",
                type: "actions",
                headerName: "Actions",
                flex: 1,
                getActions: ({ id }) => {
                    const isInEditMode =
                        rowModesModel[id]?.mode === GridRowModes.Edit;
                    return isInEditMode
                        ? [
                              <GridActionsCellItem
                                  icon={<Save />}
                                  label="Save"
                                  onClick={handleSaveClick(id)}
                              />,
                              <GridActionsCellItem
                                  icon={<Cancel />}
                                  label="Cancel"
                                  onClick={handleCancelClick(id)}
                              />,
                          ]
                        : [
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
    }

    return (
        <Box sx={{ minHeight: "50vh", width: "80%", margin: 4 }}>
            <DataGrid
                rows={branches}
                columns={columns}
                getRowId={(row) => row.branchId}
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
                editMode="row"
                rowModesModel={rowModesModel}
                onRowModesModelChange={handleRowModesModelChange}
                onRowEditStop={handleRowEditStop}
                processRowUpdate={(updatedRow, originalRow) =>
                    processRowUpdate(updatedRow, originalRow)
                }
                slots={{ loadingOverlay: DataGridLoading }}
            />
        </Box>
    );
};

export default BranchList;
