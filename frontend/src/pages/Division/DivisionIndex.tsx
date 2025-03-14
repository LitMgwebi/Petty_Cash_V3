import { FC, useCallback, useEffect, useRef, useReducer } from "react";
import { useDivisionContext } from "context/DivisionContext";
import { useDepartmentContext } from "context/DepartmentContext";
import { ServerResponse } from "types/ServerResponse";
import { Division } from "types/Division";
import { Department } from "types/Department";
import {
    Container,
    Typography,
    Box,
    IconButton,
    Tooltip,
    Fade,
    SwipeableDrawer,
    Dialog,
} from "@mui/material";
import { Routes } from "api/router";
import { Link } from "react-router-dom";
import { AddCircleOutline, HomeOutlined } from "@mui/icons-material";
import SuccessAlert from "components/SuccessAlert";
import ErrorAlert from "components/ErrorAlert";
import DivisionList from "./components/DivisionList";
import CreateDivision from "./components/CreateDivision";
import DeleteDivision from "./components/DeleteDivision";
import EditDivision from "./components/EditDivision";

interface DivisionIndexState {
    loading: boolean;
    message: string;
    alertMessage: string;
    openCreate: boolean;
    openDelete: boolean;
    openEdit: boolean;
    successfulAction: boolean;
    errorAction: boolean;
    divisionToDelete: Division | null;
    divisionToEdit: Division | null;
}

type DivisionIndexAction =
    | { type: "SET_DIVISION_DELETE"; payload: Division }
    | { type: "SET_DIVISION_EDIT"; payload: Division }
    | { type: "SET_LOADING"; payload: boolean }
    | { type: "SET_MESSAGE"; payload: string }
    | { type: "SET_ALERT_MESSAGE"; payload: string }
    | { type: "TOGGLE_CREATE" }
    | { type: "TOGGLE_DELETE" }
    | { type: "TOGGLE_EDIT" }
    | { type: "TOGGLE_SUCCESSFUL" }
    | { type: "TOGGLE_ERROR" };

const initialState: DivisionIndexState = {
    loading: false,
    message: "",
    alertMessage: "",
    openCreate: false,
    openDelete: false,
    openEdit: false,
    successfulAction: false,
    errorAction: false,
    divisionToDelete: null,
    divisionToEdit: null,
};

const divisionIndexReducer = (
    state: DivisionIndexState,
    action: DivisionIndexAction
): DivisionIndexState => {
    switch (action.type) {
        case "SET_DIVISION_DELETE":
            return { ...state, divisionToDelete: action.payload };
        case "SET_DIVISION_EDIT":
            return { ...state, divisionToEdit: action.payload };
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
        case "TOGGLE_EDIT":
            return { ...state, openEdit: !state.openEdit };
        case "TOGGLE_SUCCESSFUL":
            return { ...state, successfulAction: !state.successfulAction };
        case "TOGGLE_ERROR":
            return { ...state, errorAction: !state.errorAction };
        default:
            return state;
    }
};

const DivisionIndex: FC = () => {
    const { divisions, getDivisions } = useDivisionContext();
    const { getDepartments } = useDepartmentContext();
    const [state, dispatch] = useReducer(divisionIndexReducer, initialState);

    //#region Fetching data

    const intervalRef = useRef<NodeJS.Timeout | null>(null);
    const firstFetch = useRef(true);
    const hasFetchedDept = useRef(false);

    const fetchDivisions = useCallback(async () => {
        if (firstFetch.current)
            dispatch({ type: "SET_LOADING", payload: true });

        const response: ServerResponse<Division[]> = await getDivisions();

        if (!hasFetchedDept.current) {
            hasFetchedDept.current = true;
            const response: ServerResponse<Department[]> =
                await getDepartments();

            const { success, message } = response;

            if (!success) {
                dispatch({ type: "SET_MESSAGE", payload: message });
            }
        }
        const { success, message } = response;

        if (!success) {
            dispatch({ type: "SET_MESSAGE", payload: message });
        }

        if (firstFetch.current) {
            dispatch({ type: "SET_LOADING", payload: false });
            firstFetch.current = false;
        }
    }, [getDivisions, getDepartments]);

    const fetchDivisionsRef = useRef(fetchDivisions);

    useEffect(() => {
        fetchDivisionsRef.current = fetchDivisions;
    }, [fetchDivisions]);

    useEffect(() => {
        fetchDivisionsRef.current();

        intervalRef.current = setInterval(() => {
            fetchDivisionsRef.current();
        }, 20000);

        return () => clearInterval(intervalRef.current!);
    }, []);

    //#endregion

    //#region Toggling Functions

    const toggleCreate = () => {
        dispatch({ type: "TOGGLE_CREATE" });
    };

    const toggleDelete = () => {
        dispatch({ type: "TOGGLE_DELETE" });
    };

    const toggleEdit = () => {
        dispatch({ type: "TOGGLE_EDIT" });
    };

    const toggleSuccessAlert = (message: string) => {
        dispatch({ type: "TOGGLE_SUCCESSFUL" });
        dispatch({ type: "SET_ALERT_MESSAGE", payload: message });
    };

    const toggleErrorAlert = (message: string) => {
        dispatch({ type: "TOGGLE_ERROR" });
        dispatch({ type: "SET_ALERT_MESSAGE", payload: message });
    };

    const deleteDivision = (division: Division) => {
        dispatch({ type: "SET_DIVISION_DELETE", payload: division });
        toggleDelete();
    };

    const editDivision = (division: Division) => {
        dispatch({ type: "SET_DIVISION_EDIT", payload: division });
        toggleEdit();
    };

    //#endregion

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
                        Division
                    </Typography>
                </Box>

                <Box className="pageHeaderComponent">
                    <Tooltip
                        title="Add Division"
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
                <CreateDivision
                    refreshDivisions={() => fetchDivisionsRef.current?.()}
                    onClose={toggleCreate}
                    setSuccess={(message) => toggleSuccessAlert(message)}
                    setError={(message) => toggleErrorAlert(message)}
                />
            </SwipeableDrawer>
            <SwipeableDrawer
                anchor="right"
                open={state.openEdit}
                onClose={toggleEdit}
                onOpen={toggleEdit}
                sx={{
                    "& .MuiDrawer-paper": {
                        width: "40%", // Custom width for the drawer
                        height: "60vh", // Height of the drawer
                        top: "20vh", // Position it vertically to center on the screen
                        bottom: "auto", // Make sure the drawer does not extend beyond the bottom
                        transform: "none", // Remove any transform if it was being applied
                        borderRadius: "15px 0 0 15px",
                    },
                }}
            >
                <EditDivision
                    division={state.divisionToEdit}
                    refreshDivisions={() => fetchDivisionsRef.current?.()}
                    onClose={toggleEdit}
                    setSuccess={(message) => toggleSuccessAlert(message)}
                    setError={(message) => toggleErrorAlert(message)}
                />
            </SwipeableDrawer>
            <Dialog open={state.openDelete} keepMounted onClose={toggleDelete}>
                <DeleteDivision
                    division={state.divisionToDelete}
                    refreshDivisions={() => fetchDivisionsRef.current?.()}
                    onClose={toggleDelete}
                    setSuccess={(message) => toggleSuccessAlert(message)}
                    setError={(message) => toggleErrorAlert(message)}
                />
            </Dialog>
            <DivisionList
                divisions={divisions}
                loading={state.loading}
                message={state.message}
                setDivisionToDelete={(division: Division) =>
                    deleteDivision(division)
                }
                setDivisionToEdit={(division: Division) =>
                    editDivision(division)
                }
                setSuccess={(message) => toggleSuccessAlert(message)}
                setError={(message) => toggleErrorAlert(message)}
                refreshDivisions={() => fetchDivisionsRef.current?.()}
            />
        </Container>
    );
};

export default DivisionIndex;
