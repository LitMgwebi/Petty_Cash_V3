import { FC, useCallback, useEffect, useRef, useReducer } from "react";
import { useOfficeContext } from "context/OfficeContext";
import { ServerResponse } from "types/ServerResponse";
import { Office } from "types/Office";
import {
    Container,
    Typography,
    Box,
    IconButton,
    Tooltip,
    Fade,
    SwipeableDrawer,
} from "@mui/material";
import { Routes } from "api/router";
import { Link } from "react-router-dom";
import { AddCircleOutline, HomeOutlined } from "@mui/icons-material";
import SuccessAlert from "components/SuccessAlert";
import ErrorAlert from "components/ErrorAlert";
import { OfficeList } from "./components/OfficeList";
import { CreateOffice } from "./components/CreateOffice";
import { DeleteOffice } from "./components/DeleteOffice";

//#region State and Type Management

interface OfficeIndexState {
    loading: boolean;
    message: string;
    alertMessage: string;
    openCreate: boolean;
    openDelete: boolean;
    successfulAction: boolean;
    errorAction: boolean;
    officeToDelete: Office | null;
}

type OfficeIndexAction =
    | { type: "SET_OFFICE_DELETE"; payload: Office }
    | { type: "SET_LOADING"; payload: boolean }
    | { type: "SET_MESSAGE"; payload: string }
    | { type: "SET_ALERT_MESSAGE"; payload: string }
    | { type: "TOGGLE_CREATE" }
    | { type: "TOGGLE_DELETE" }
    | { type: "TOGGLE_SUCCESSFUL" }
    | { type: "TOGGLE_ERROR" };

const initialState: OfficeIndexState = {
    loading: false,
    message: "",
    alertMessage: "",
    openCreate: false,
    openDelete: false,
    successfulAction: false,
    errorAction: false,
    officeToDelete: null,
};

const officeIndexReducer = (
    state: OfficeIndexState,
    action: OfficeIndexAction
): OfficeIndexState => {
    switch (action.type) {
        case "SET_OFFICE_DELETE":
            return { ...state, officeToDelete: action.payload };
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

//#endregion

const OfficeIndex: FC = () => {
    const { offices, getOffices } = useOfficeContext();
    const [state, dispatch] = useReducer(officeIndexReducer, initialState);

    //#region Fetching data

    const intervalRef = useRef<NodeJS.Timeout | null>(null);
    const firstFetch = useRef(true);

    const fetchOffices = useCallback(async () => {
        if (firstFetch.current)
            dispatch({ type: "SET_LOADING", payload: true });

        const response: ServerResponse<Office[]> = await getOffices();
        const { success, message } = response;

        if (!success) {
            dispatch({ type: "SET_MESSAGE", payload: message });
        }

        if (firstFetch.current) {
            dispatch({ type: "SET_LOADING", payload: false });
            firstFetch.current = false;
        }
    }, [getOffices]);

    const fetchOfficesRef = useRef(fetchOffices);

    useEffect(() => {
        fetchOfficesRef.current = fetchOffices;
    }, [fetchOffices]);

    useEffect(() => {
        fetchOfficesRef.current();

        intervalRef.current = setInterval(() => {
            fetchOfficesRef.current();
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

    const toggleSuccessAlert = (message: string) => {
        dispatch({ type: "TOGGLE_SUCCESSFUL" });
        dispatch({ type: "SET_ALERT_MESSAGE", payload: message });
    };

    const toggleErrorAlert = (message: string) => {
        dispatch({ type: "TOGGLE_ERROR" });
        dispatch({ type: "SET_ALERT_MESSAGE", payload: message });
    };

    const deleteOffice = (branch: Office) => {
        dispatch({ type: "SET_OFFICE_DELETE", payload: branch });
        toggleDelete();
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
                        Offices
                    </Typography>
                </Box>

                <Box className="pageHeaderComponent">
                    <Tooltip
                        title="Add Office"
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
                <CreateOffice
                    refreshOffices={() => fetchOfficesRef.current?.()}
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
                <DeleteOffice
                    office={state.officeToDelete}
                    refreshOffices={() => fetchOfficesRef.current?.()}
                    onClose={toggleDelete}
                    setSuccess={(message) => toggleSuccessAlert(message)}
                    setError={(message) => toggleErrorAlert(message)}
                />
            </SwipeableDrawer>
            <OfficeList
                offices={offices}
                loading={state.loading}
                message={state.message}
                setOfficeToDelete={(office: Office) => deleteOffice(office)}
                setSuccess={(message) => toggleSuccessAlert(message)}
                setError={(message) => toggleErrorAlert(message)}
                refreshOffices={() => fetchOfficesRef.current?.()}
            />
        </Container>
    );
};

export default OfficeIndex;
