import { FC, useState } from "react";
import { Office } from "types/Office";
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
import { useOfficeContext } from "context/OfficeContext";
import { Delete, Edit, Save, Cancel } from "@mui/icons-material";

interface ListOfficeProps {
    offices: Office[];
    message: string;
    loading: boolean;
    setSuccess: (message: string) => void;
    setError: (message: string) => void;
    refreshOffices: () => void;
    setOfficeToDelete: (office: Office) => void;
}

export const OfficeList: FC<ListOfficeProps> = ({
    offices,
    message,
    loading,
    setSuccess,
    setError,
    refreshOffices,
    setOfficeToDelete,
}) => {
    const [rowModesModel, setRowsModesModel] = useState<GridRowModesModel>({});
    const { updateOffice } = useOfficeContext();

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

    const handleDeleteClick = (officeId: GridRowId) => () => {
        const office: Office | undefined = offices.find(
            (office) => office.officeId === officeId
        );

        if (office !== undefined) setOfficeToDelete(office);
        else setError("System could not resolve Branch ID when deleting.");
    };

    const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
        setRowsModesModel(newRowModesModel);
    };

    const processRowUpdate = async (
        newRow: Office,
        oldRow: Office
    ): Promise<Office> => {
        try {
            const response = await updateOffice(newRow);

            if (!response.success) {
                setError(response.message);
                return oldRow;
            }
            setSuccess(response.message);
            return newRow;
        } catch (error: any) {
            setError(
                error.response?.data?.message ||
                    "System could not resolve updating action."
            );
            return oldRow;
        } finally {
            refreshOffices();
        }
    };

    let columns: GridColDef<(typeof offices)[number]>[] = [];
    if (offices) {
        columns = [
            { field: "officeId", headerName: "ID", flex: 1 },
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
                rows={offices}
                columns={columns}
                getRowId={(row) => row.officeId}
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
