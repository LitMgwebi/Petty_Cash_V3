import {
    Box,
    Container,
    Typography,
    TextField,
    ButtonGroup,
    Autocomplete,
    Button,
} from "@mui/material";
import { FC, useReducer, ChangeEvent, useEffect } from "react";
import { Division, DivisionError } from "types/Division";
import { useDivisionContext } from "context/DivisionContext";
import { useDepartmentContext } from "context/DepartmentContext";

interface EditDivisionProps {
    division: Division | null;
    refreshDivisions: () => void;
    onClose: () => void;
    setSuccess: (message: string) => void;
    setError: (message: string) => void;
}

interface DivisionState {
    loading: boolean;
    division: Division;
    errors: DivisionError;
}
type EditAction =
    | { type: "SET_LOADING"; payload: boolean }
    | { type: "SET_FIELD"; payload: { field: string; value: string | number } }
    | {
          type: "SET_ERRORS";
          payload: { field: string; message: string };
      }
    | { type: "RESET" };

const initialState: DivisionState = {
    loading: false,
    division: {
        divisionId: 0,
        name: "",
        description: "",
        departmentId: 0,
        department: {
            departmentId: 0,
            name: "",
            description: "",
        },
    },
    errors: {
        name: "",
        description: "",
        departmentId: "",
    },
};

const divisionReducer = (
    state: DivisionState,
    action: EditAction
): DivisionState => {
    switch (action.type) {
        case "SET_LOADING":
            return { ...state, loading: action.payload };
        case "SET_FIELD":
            return {
                ...state,
                division: {
                    ...state.division,
                    [action.payload.field]: action.payload.value,
                },
            };
        case "SET_ERRORS":
            return {
                ...state,
                errors: {
                    ...state.errors,
                    [action.payload.field]: action.payload.message,
                },
            };
        case "RESET":
            return initialState;
        default:
            return state;
    }
};

const EditDivision: FC<EditDivisionProps> = ({
    division,
    refreshDivisions,
    onClose,
    setSuccess,
    setError,
}) => {
    const { updateDivision } = useDivisionContext();
    const { departments } = useDepartmentContext();
    const [state, dispatch] = useReducer(divisionReducer, initialState);

    useEffect(() => {
        if (division) {
            dispatch({
                type: "SET_FIELD",
                payload: { field: "divisionId", value: division.divisionId },
            });
            dispatch({
                type: "SET_FIELD",
                payload: { field: "name", value: division.name },
            });
            dispatch({
                type: "SET_FIELD",
                payload: { field: "description", value: division.description },
            });
            dispatch({
                type: "SET_FIELD",
                payload: {
                    field: "departmentId",
                    value: division.departmentId,
                },
            });
        }
    }, [division]);

    const validate = () => {
        let valid = true;

        dispatch({
            type: "SET_ERRORS",
            payload: { field: "name", message: "" },
        });
        dispatch({
            type: "SET_ERRORS",
            payload: { field: "description", message: "" },
        });

        if (!state.division.name.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "name",
                    message: "Abbreviation is required.",
                },
            });
            valid = false;
        }
        if (!state.division.description.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "description",
                    message: "Name is required.",
                },
            });
            valid = false;
        }
        if (state.division.departmentId === 0) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "departmentId",
                    message: "Department is required.",
                },
            });
            valid = false;
        }

        return valid;
    };

    const handleUpdate = async () => {
        dispatch({ type: "SET_LOADING", payload: true });
        try {
            if (validate()) {
                const response = await updateDivision(state.division);
                if (response.success) {
                    setSuccess(response.message);
                } else {
                    setError(response.message);
                }
                handleCancel();
                dispatch({ type: "SET_LOADING", payload: false });
            } else {
                dispatch({ type: "SET_LOADING", payload: false });
            }
        } catch (error: any) {
            setError(
                error.response?.data?.message ||
                    "System could not resolve creation action."
            );
            refreshDivisions();
            dispatch({ type: "SET_LOADING", payload: false });
        }
    };
    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        dispatch({ type: "SET_FIELD", payload: { field: name, value } });
    };

    const handleCancel = () => {
        dispatch({ type: "RESET" });
        refreshDivisions();
        onClose();
    };
    return (
        <Container className="drawerContainer">
            <Box className="drawerHeader">
                <Typography variant="h4">Edit Office</Typography>
            </Box>
            <Box sx={{ width: "100%", textAlign: "center" }}>
                <Box>
                    <TextField
                        name="name"
                        variant="outlined"
                        label="Abbreviation"
                        value={state.division.name}
                        onChange={handleChange}
                        error={!!state.errors.name}
                        helperText={state.errors.name || "TST"}
                        fullWidth
                        disabled={state.loading}
                    />
                </Box>
                <Box>
                    <TextField
                        name="description"
                        variant="outlined"
                        label="Name"
                        value={state.division.description}
                        onChange={handleChange}
                        error={!!state.errors.description}
                        helperText={state.errors.description || "Test"}
                        fullWidth
                        disabled={state.loading}
                    />
                </Box>
                <Box>
                    <Autocomplete
                        disablePortal
                        options={departments}
                        getOptionLabel={(option) => option.name}
                        isOptionEqualToValue={(option, value) =>
                            option.departmentId === value.departmentId
                        }
                        value={
                            departments.find(
                                (dep) =>
                                    dep.departmentId ===
                                    state.division.departmentId
                            ) || null
                        }
                        onChange={(event, newValue) => {
                            handleChange({
                                target: {
                                    name: "departmentId",
                                    value: newValue?.departmentId || 0,
                                },
                            } as unknown as React.ChangeEvent<HTMLInputElement>);
                        }}
                        disabled={state.loading}
                        renderInput={(params) => (
                            <TextField
                                {...params}
                                label="Departmentsa"
                                error={!!state.errors.departmentId}
                                helperText={
                                    state.errors.departmentId || "Test Dept"
                                }
                                variant="outlined"
                                fullWidth
                            />
                        )}
                    />
                </Box>
                <Box>
                    <ButtonGroup
                        variant="contained"
                        aria-label="Basic button group"
                    >
                        <Button
                            onClick={handleCancel}
                            loading={state.loading}
                            loadingPosition="start"
                        >
                            Cancel
                        </Button>
                        <Button
                            onClick={handleUpdate}
                            loading={state.loading}
                            loadingPosition="start"
                        >
                            Submit
                        </Button>
                    </ButtonGroup>
                </Box>
            </Box>
        </Container>
    );
};

export default EditDivision;
