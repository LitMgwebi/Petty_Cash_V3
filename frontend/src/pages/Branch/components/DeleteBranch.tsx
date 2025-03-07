import { Box, Container, Typography, ButtonGroup, Button } from "@mui/material";
import { FC, useReducer } from "react";
import { Branch } from "types/Branch";
import { useBranchContext } from "context/BranchContext";

interface DeleteBranchProps {
    branch: Branch | null;
    refreshBranches: () => void;
    onClose: () => void;
    setSuccess: (message: string) => void;
    setError: (message: string) => void;
}

interface BranchState {
    loading: boolean;
}

type DeleteAction =
    | { type: "SET_LOADING"; payload: boolean }
    | { type: "RESET" };

const initialState: BranchState = {
    loading: false,
};

const branchReducer = (
    state: BranchState,
    action: DeleteAction
): BranchState => {
    switch (action.type) {
        case "SET_LOADING":
            return { ...state, loading: action.payload };
        case "RESET":
            return initialState;
        default:
            return state;
    }
};

const DeleteBranch: FC<DeleteBranchProps> = ({
    branch,
    refreshBranches,
    onClose,
    setSuccess,
    setError,
}) => {
    const { deleteBranch } = useBranchContext();
    const [state, dispatch] = useReducer(branchReducer, initialState);

    const handleDelete = async () => {
        try {
            if (branch != null) {
                const response = await deleteBranch(branch);
                if (response.success) {
                    setSuccess(response.message);
                } else {
                    setError(response.message);
                }
            } else {
                setError("System could not resolve branch when deleting.");
            }
            handleCancel();
            dispatch({ type: "SET_LOADING", payload: false });
        } catch (error: any) {
            setError(
                error.response?.data?.message ||
                    "System could not resolve deleting action."
            );
            refreshBranches();
            dispatch({ type: "SET_LOADING", payload: false });
        }
    };

    const handleCancel = () => {
        dispatch({ type: "RESET" });
        refreshBranches();
        onClose();
    };

    return (
        <Container className="drawerContainer">
            <Box className="drawerHeader">
                <Typography variant="h4">Confirm Branch Deletion</Typography>
            </Box>

            <Box sx={{ width: "100%", textAlign: "center" }}>
                <Box>
                    <Typography variant="h6">
                        Are you sure you want to delete {branch?.description}?
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

export default DeleteBranch;
