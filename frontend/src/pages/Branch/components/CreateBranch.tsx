import {
    Box,
    Container,
    Typography,
    TextField,
    ButtonGroup,
    Button,
} from "@mui/material";
import { FC, useReducer } from "react";
import { Branch, BranchError } from "types/Branch";
import { useBranchContext } from "context/BranchContext";

interface CreateBranchProps {
    refreshBranches: () => void;
    onClose: () => void;
    setSuccess: (message: string) => void;
    setError: (message: string) => void;
}

interface BranchState {
    loading: boolean;
    branch: Branch;
    errors: BranchError;
}

type CreateAction =
    | { type: "SET_LOADING"; payload: boolean }
    | { type: "SET_FIELD"; payload: { field: string; value: string } }
    | {
          type: "SET_ERRORS";
          payload: { field: "name" | "description"; message: string };
      }
    | { type: "RESET" };

const initialState: BranchState = {
    loading: false,
    branch: {
        branchId: 0,
        name: "",
        description: "",
        glaccounts: [],
    },
    errors: {
        name: "",
        description: "",
    },
};

const branchReducer = (
    state: BranchState,
    action: CreateAction
): BranchState => {
    switch (action.type) {
        case "SET_LOADING":
            return { ...state, loading: action.payload };
        case "SET_FIELD":
            return {
                ...state,
                branch: {
                    ...state.branch,
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

const CreateBranch: FC<CreateBranchProps> = ({
    refreshBranches,
    onClose,
    setSuccess,
    setError,
}) => {
    const { createBranch } = useBranchContext();
    const [state, dispatch] = useReducer(branchReducer, initialState);

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

        if (!state.branch.name.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "name",
                    message: "Branch abbreviation is required.",
                },
            });
            valid = false;
        }
        if (!state.branch.description.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "description",
                    message: "Branch name is required.",
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
                const response = await createBranch(state.branch);
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
            refreshBranches();
            dispatch({ type: "SET_LOADING", payload: false });
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        dispatch({ type: "SET_FIELD", payload: { field: name, value } });
    };

    const handleCancel = () => {
        dispatch({ type: "RESET" });
        refreshBranches();
        onClose();
    };
    return (
        <Container className="drawerContainer">
            <Box className="drawerHeader">
                <Typography variant="h4">Add Branch</Typography>
            </Box>
            <Box sx={{ width: "100%", textAlign: "center" }}>
                <Box>
                    <TextField
                        name="name"
                        label="Abbreviation"
                        variant="outlined"
                        value={state.branch.name}
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
                        value={state.branch.description}
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

export default CreateBranch;
