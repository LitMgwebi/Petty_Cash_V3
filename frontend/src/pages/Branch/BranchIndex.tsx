import { FC, useCallback, useEffect, useRef, useReducer } from "react";
import { useBranchContext } from "context/BranchContext";
import { ServerResponse } from "types/ServerResponse";
import { Branch } from "types/Branch";
import {
    Container,
    Typography,
    Box,
    IconButton,
    Tooltip,
    Fade,
    SwipeableDrawer,
} from "@mui/material";
import List from "./components/BranchList";
import { Routes } from "api/router";
import { Link } from "react-router-dom";
import { AddCircleOutline, HomeOutlined } from "@mui/icons-material";
import CreateBranch from "./components/CreateBranch";
import DeleteBranch from "./components/DeleteBranch";
import SuccessAlert from "components/SuccessAlert";
import ErrorAlert from "components/ErrorAlert";

interface BranchIndexState {
    loading: boolean;
    message: string;
    alertMessage: string;
    openCreate: boolean;
    openDelete: boolean;
    successfulAction: boolean;
    errorAction: boolean;
    branchToDelete: Branch | null;
}

type BranchIndexAction =
    | { type: "SET_BRANCH_DELETE"; payload: Branch }
    | { type: "SET_LOADING"; payload: boolean }
    | { type: "SET_MESSAGE"; payload: string }
    | { type: "SET_ALERT_MESSAGE"; payload: string }
    | { type: "TOGGLE_CREATE" }
    | { type: "TOGGLE_DELETE" }
    | { type: "TOGGLE_SUCCESSFUL" }
    | { type: "TOGGLE_ERROR" };

const initialState: BranchIndexState = {
    loading: false,
    message: "",
    alertMessage: "",
    openCreate: false,
    openDelete: false,
    successfulAction: false,
    errorAction: false,
    branchToDelete: null,
};

const branchIndexReducer = (
    state: BranchIndexState,
    action: BranchIndexAction
): BranchIndexState => {
    switch (action.type) {
        case "SET_BRANCH_DELETE":
            return { ...state, branchToDelete: action.payload };
        case "SET_LOADING":
            return { ...state, loading: action.payload };
        case "SET_MESSAGE":
            return { ...state, message: action.payload };
        case "SET_ALERT_MESSAGE":
            return { ...state, alertMessage: action.payload };
        case "TOGGLE_CREATE":
            return { ...state, openCreate: !state.openCreate };
        case "TOGGLE_DELETE":
            return { ...state, openDelete: !state.openDelete };
        case "TOGGLE_SUCCESSFUL":
            return { ...state, successfulAction: !state.successfulAction };
        case "TOGGLE_ERROR":
            return { ...state, errorAction: !state.errorAction };
        default:
            return state;
    }
};

const Index: FC = () => {
    const { branches, getBranches } = useBranchContext();
    const [state, dispatch] = useReducer(branchIndexReducer, initialState);

    const intervalRef = useRef<NodeJS.Timeout | null>(null);
    const firstFetch = useRef(true);

    const toggleCreate = () => {
        dispatch({ type: "TOGGLE_CREATE" });
    };

    const toggleDelete = () => {
        dispatch({ type: "TOGGLE_DELETE" });
    };

    const toggleSuccessAlert = (message: string) => {
        dispatch({ type: "TOGGLE_SUCCESSFUL" });
        dispatch({ type: "SET_ALERT_MESSAGE", payload: message });
    };

    const toggleErrorAlert = (message: string) => {
        dispatch({ type: "TOGGLE_ERROR" });
        dispatch({ type: "SET_ALERT_MESSAGE", payload: message });
    };

    const deleteBranch = (branch: Branch) => {
        dispatch({ type: "SET_BRANCH_DELETE", payload: branch });
        toggleDelete();
    };

    const fetchBranches = useCallback(async () => {
        if (firstFetch.current)
            dispatch({ type: "SET_LOADING", payload: true });

        const response: ServerResponse<Branch[]> = await getBranches();
        const { success, message } = response;

        if (!success) {
            dispatch({ type: "SET_MESSAGE", payload: message });
        }

        if (firstFetch.current) {
            dispatch({ type: "SET_LOADING", payload: false });
            firstFetch.current = false;
        }
    }, [getBranches]);

    const fetchBranchesRef = useRef(fetchBranches);

    useEffect(() => {
        fetchBranchesRef.current = fetchBranches;
    }, [fetchBranches]);

    useEffect(() => {
        fetchBranchesRef.current();

        intervalRef.current = setInterval(() => {
            fetchBranchesRef.current();
        }, 20000);

        return () => clearInterval(intervalRef.current!);
    }, []);

    return (
        <Container className="pageContainer" sx={{ position: "relative" }}>
            {state.successfulAction && (
                <SuccessAlert
                    message={state.alertMessage}
                    closeAlert={() => toggleSuccessAlert("")}
                />
            )}
            {state.errorAction && (
                <ErrorAlert
                    message={state.alertMessage}
                    closeAlert={() => toggleErrorAlert("")}
                />
            )}
            <Box className="pageHeader">
                <Box className="pageHeaderComponent">
                    <Tooltip
                        title="Home"
                        placement="left"
                        slots={{
                            transition: Fade,
                        }}
                        slotProps={{
                            transition: { timeout: 600 },
                        }}
                    >
                        <IconButton>
                            <Link to={Routes.Home}>
                                <HomeOutlined fontSize="large" />
                            </Link>
                        </IconButton>
                    </Tooltip>
                </Box>
                <Box className="pageHeaderComponent">
                    <Typography variant="h2" gutterBottom>
                        Branches
                    </Typography>
                </Box>

                <Box className="pageHeaderComponent">
                    <Tooltip
                        title="Add Branch"
                        placement="right"
                        slots={{
                            transition: Fade,
                        }}
                        slotProps={{
                            transition: { timeout: 600 },
                        }}
                    >
                        <IconButton onClick={toggleCreate}>
                            <AddCircleOutline fontSize="large" />
                        </IconButton>
                    </Tooltip>
                </Box>
            </Box>
            <SwipeableDrawer
                anchor="left"
                open={state.openCreate}
                onClose={toggleCreate}
                onOpen={toggleCreate}
                sx={{
                    "& .MuiDrawer-paper": {
                        width: "40%", // Custom width for the drawer
                        height: "60vh", // Height of the drawer
                        top: "20vh", // Position it vertically to center on the screen
                        bottom: "auto", // Make sure the drawer does not extend beyond the bottom
                        transform: "none", // Remove any transform if it was being applied
                        borderRadius: "0 15px 15px 0",
                    },
                }}
            >
                <CreateBranch
                    refreshBranches={() => fetchBranchesRef.current?.()}
                    onClose={toggleCreate}
                    setSuccess={(message) => toggleSuccessAlert(message)}
                    setError={(message) => toggleErrorAlert(message)}
                />
            </SwipeableDrawer>
            <SwipeableDrawer
                anchor="right"
                open={state.openDelete}
                onClose={toggleDelete}
                onOpen={toggleDelete}
                sx={{
                    "& .MuiDrawer-paper": {
                        width: "35%", // Custom width for the drawer
                        height: "30vh", // Height of the drawer
                        top: "40vh", // Position it vertically to center on the screen
                        bottom: "auto", // Make sure the drawer does not extend beyond the bottom
                        transform: "none", // Remove any transform if it was being applied
                        borderRadius: "15px 0 0 15px",
                    },
                }}
            >
                <DeleteBranch
                    branch={state.branchToDelete}
                    refreshBranches={() => fetchBranchesRef.current?.()}
                    onClose={toggleDelete}
                    setSuccess={(message) => toggleSuccessAlert(message)}
                    setError={(message) => toggleErrorAlert(message)}
                />
            </SwipeableDrawer>
            <List
                branches={branches}
                loading={state.loading}
                message={state.message}
                setBranchToDelete={(branch: Branch) => deleteBranch(branch)}
                setSuccess={(message) => toggleSuccessAlert(message)}
                setError={(message) => toggleErrorAlert(message)}
                refreshBranches={() => fetchBranchesRef.current?.()}
            />
        </Container>
    );
};

export default Index;
