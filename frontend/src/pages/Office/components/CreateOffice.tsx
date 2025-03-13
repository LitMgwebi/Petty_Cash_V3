import {
    Box,
    Container,
    Typography,
    TextField,
    ButtonGroup,
    Button,
} from "@mui/material";
import { FC, useReducer } from "react";
import { Office, OfficeErrors } from "types/Office";
import { useOfficeContext } from "context/OfficeContext";

interface CreateOfficeProps {
    refreshOffices: () => void;
    onClose: () => void;
    setSuccess: (message: string) => void;
    setError: (message: string) => void;
}

interface OfficeState {
    loading: boolean;
    office: Office;
    errors: OfficeErrors;
}
type CreateAction =
    | { type: "SET_LOADING"; payload: boolean }
    | { type: "SET_FIELD"; payload: { field: string; value: string } }
    | {
          type: "SET_ERRORS";
          payload: { field: "name" | "description"; message: string };
      }
    | { type: "RESET" };

const initialState: OfficeState = {
    loading: false,
    office: {
        officeId: 0,
        name: "",
        description: "",
    },
    errors: {
        name: "",
        description: "",
    },
};

const officeReducer = (
    state: OfficeState,
    action: CreateAction
): OfficeState => {
    switch (action.type) {
        case "SET_LOADING":
            return { ...state, loading: action.payload };
        case "SET_FIELD":
            return {
                ...state,
                office: {
                    ...state.office,
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

export const CreateOffice: FC<CreateOfficeProps> = ({
    refreshOffices,
    onClose,
    setSuccess,
    setError,
}) => {
    const { createOffice } = useOfficeContext();
    const [state, dispatch] = useReducer(officeReducer, initialState);

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

        if (!state.office.name.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "name",
                    message: "Abbreviation is required.",
                },
            });
            valid = false;
        }
        if (!state.office.description.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "description",
                    message: "Name is required.",
                },
            });
            valid = false;
        }

        return valid;
    };

    const handleCreate = async () => {
        dispatch({ type: "SET_LOADING", payload: true });
        try {
            if (validate()) {
                const response = await createOffice(state.office);
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
            refreshOffices();
            dispatch({ type: "SET_LOADING", payload: false });
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        dispatch({ type: "SET_FIELD", payload: { field: name, value } });
    };

    const handleCancel = () => {
        dispatch({ type: "RESET" });
        refreshOffices();
        onClose();
    };
    return (
        <Container className="drawerContainer">
            <Box className="drawerHeader">
                <Typography variant="h4">Add Office</Typography>
            </Box>
            <Box sx={{ width: "100%", textAlign: "center" }}>
                <Box>
                    <TextField
                        name="name"
                        label="Abbreviation"
                        variant="outlined"
                        value={state.office.name}
                        onChange={handleChange}
                        error={!!state.errors.name}
                        helperText={state.errors.name || "TST"}
                        fullWidth
                    />
                </Box>
                <Box>
                    <TextField
                        name="description"
                        label="Name"
                        variant="outlined"
                        value={state.office.description}
                        onChange={handleChange}
                        error={!!state.errors.description}
                        helperText={state.errors.description || "Test"}
                        fullWidth
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
                            onClick={handleCreate}
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
