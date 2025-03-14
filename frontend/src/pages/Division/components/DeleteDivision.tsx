import { Box, Container, Typography, ButtonGroup, Button } from "@mui/material";
import { FC, useReducer } from "react";
import { Division } from "types/Division";
import { useDivisionContext } from "context/DivisionContext";

interface DeleteDivisionProps {
    division: Division | null;
    refreshDivisions: () => void;
    onClose: () => void;
    setSuccess: (message: string) => void;
    setError: (message: string) => void;
}
interface OfficeState {
    loading: boolean;
}

type DeleteAction =
    | { type: "SET_LOADING"; payload: boolean }
    | { type: "RESET" };

const initialState: OfficeState = {
    loading: false,
};

const branchReducer = (
    state: OfficeState,
    action: DeleteAction
): OfficeState => {
    switch (action.type) {
        case "SET_LOADING":
            return { ...state, loading: action.payload };
        case "RESET":
            return initialState;
        default:
            return state;
    }
};
const DeleteDivision: FC<DeleteDivisionProps> = ({
    division,
    refreshDivisions,
    onClose,
    setSuccess,
    setError,
}) => {
    const [state, dispatch] = useReducer(branchReducer, initialState);
    const { deleteDivision } = useDivisionContext();

    const handleDelete = async () => {
        try {
            if (division != null) {
                const response = await deleteDivision(division);
                if (response.success) {
                    setSuccess(response.message);
                } else {
                    setError(response.message);
                }
            } else {
                setError("System could not resolve branch when deleting.");
            }
            dispatch({ type: "SET_LOADING", payload: false });
        } catch (error: any) {
            setError(
                error.response?.data?.message ||
                    "System could not resolve deleting action."
            );
            dispatch({ type: "SET_LOADING", payload: false });
        } finally {
            handleCancel();
        }
    };

    const handleCancel = () => {
        dispatch({ type: "RESET" });
        refreshDivisions();
        onClose();
    };

    return (
        <Container className="drawerContainer">
            <Box className="drawerHeader">
                <Typography variant="h4">Confirm Division Deletion</Typography>
            </Box>

            <Box sx={{ width: "100%", textAlign: "center" }}>
                <Box>
                    <Typography variant="h6">
                        Are you sure you want to delete {division?.description}?
                    </Typography>
                </Box>

                <Box sx={{ margin: 2 }}>
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
                            onClick={handleDelete}
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

export default DeleteDivision;
