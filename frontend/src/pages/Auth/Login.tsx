import { ReactElement, useReducer, ChangeEvent } from "react";
import {
    Box,
    Container,
    Typography,
    TextField,
    ButtonGroup,
    Button,
    IconButton,
    Tooltip,
    Fade,
} from "@mui/material";
import { Login as LoginType } from "types/Login";
import { useAuthContext } from "context/AuthContext";
import { HomeOutlined } from "@mui/icons-material";
import { Routes } from "api/router";
import { Link } from "react-router-dom";
import SuccessAlert from "components/SuccessAlert";
import ErrorAlert from "components/ErrorAlert";

interface LoginState {
    loading: boolean;
    loginModel: LoginType;
    errors: LoginType;
    successfulAction: boolean;
    errorAction: boolean;
    alertMessage: string;
}

type LoginAction =
    | { type: "TOGGLE_LOADING" }
    | { type: "SET_LOGIN_MODEL"; payload: { field: string; value: any } }
    | { type: "SET_ERRORS"; payload: { field: string; message: string } }
    | { type: "RESET_ERRORS" }
    | { type: "RESET" }
    | { type: "TOGGLE_SUCCESSFUL" }
    | { type: "TOGGLE_ERROR" }
    | { type: "SET_ALERT_MESSAGE"; payload: string };

const initialState: LoginState = {
    loading: false,
    loginModel: {
        email: "",
        password: "",
    },
    errors: {
        email: "",
        password: "",
    },
    successfulAction: false,
    errorAction: false,
    alertMessage: "",
};

const loginReducer = (state: LoginState, action: LoginAction): LoginState => {
    switch (action.type) {
        case "TOGGLE_LOADING":
            return { ...state, loading: !state.loading };
        case "SET_LOGIN_MODEL":
            return {
                ...state,
                loginModel: {
                    ...state.loginModel,
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
        case "RESET_ERRORS":
            return {
                ...state,
                errors: { ...initialState.errors },
            };
        case "TOGGLE_SUCCESSFUL":
            return { ...state, successfulAction: !state.successfulAction };
        case "TOGGLE_ERROR":
            return { ...state, errorAction: !state.errorAction };
        case "SET_ALERT_MESSAGE":
            return { ...state, alertMessage: action.payload };
        case "RESET":
            return initialState;
        default:
            return state;
    }
};

export default function Login(): ReactElement {
    const [state, dispatch] = useReducer(loginReducer, initialState);
    const { loginUser } = useAuthContext();

    const toggleSuccessAlert = (message: string) => {
        dispatch({ type: "TOGGLE_SUCCESSFUL" });
        dispatch({ type: "SET_ALERT_MESSAGE", payload: message });
    };

    const toggleErrorAlert = (message: string) => {
        dispatch({ type: "TOGGLE_ERROR" });
        dispatch({ type: "SET_ALERT_MESSAGE", payload: message });
    };

    const validate = () => {
        let valid = true;

        dispatch({
            type: "RESET_ERRORS",
        });

        if (
            !/^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/.test(state.loginModel.email.trim())
        ) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "email",
                    message: "Please enter a valid email address.",
                },
            });
            valid = false;
        }
        if (!state.loginModel.email.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "email",
                    message: "Your email is required.",
                },
            });
            valid = false;
        }
        if (!state.loginModel.password.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "password",
                    message: "Your password is required.",
                },
            });
            valid = false;
        }

        return valid;
    };

    const handleCreate = async () => {
        dispatch({ type: "TOGGLE_LOADING" });

        try {
            if (validate()) {
                const response = await loginUser(state.loginModel);

                response.success
                    ? toggleSuccessAlert(response.message)
                    : toggleErrorAlert(response.message);

                dispatch({ type: "TOGGLE_LOADING" });
            } else {
                dispatch({ type: "TOGGLE_LOADING" });
            }
        } catch (error: any) {
            toggleErrorAlert(
                error.response?.data?.message ||
                    "System could not resolve creation action."
            );
            dispatch({ type: "TOGGLE_LOADING" });
        }
    };

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        dispatch({
            type: "SET_LOGIN_MODEL",
            payload: {
                field: name,
                value,
            },
        });
    };

    const handleCancel = () => {
        dispatch({ type: "RESET" });
    };
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
                        Login
                    </Typography>
                </Box>
                <Box className="pageHeaderComponent"></Box>
            </Box>
            <Box sx={{ width: "80%", textAlign: "center" }}>
                <Box>
                    <TextField
                        name="email"
                        label="Email Address"
                        variant="outlined"
                        type="email"
                        value={state.loginModel.email}
                        onChange={handleChange}
                        error={!!state.errors.email}
                        helperText={state.errors.email}
                    />
                </Box>
                <Box>
                    <TextField
                        name="password"
                        label="password"
                        variant="outlined"
                        value={state.loginModel.password}
                        type="password"
                        onChange={handleChange}
                        error={!!state.errors.password}
                        helperText={state.errors.password}
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
}
