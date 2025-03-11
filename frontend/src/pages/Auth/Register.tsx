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
import { Register as RegisterType, RegisterError } from "types/Register";
import SuccessAlert from "components/SuccessAlert";
import ErrorAlert from "components/ErrorAlert";
import { useAuthContext } from "context/AuthContext";
import { HomeOutlined } from "@mui/icons-material";
import { Routes } from "api/router";
import { Link } from "react-router-dom";

interface RegisterState {
    loading: boolean;
    registerModel: RegisterType;
    errors: RegisterError;
    successfulAction: boolean;
    errorAction: boolean;
    alertMessage: string;
}

type RegisterAction =
    | { type: "TOGGLE_LOADING" }
    | { type: "SET_REGISTER_MODEL"; payload: { field: string; value: any } }
    | { type: "SET_ERRORS"; payload: { field: string; message: string } }
    | { type: "RESET_ERRORS" }
    | { type: "RESET" }
    | { type: "TOGGLE_SUCCESSFUL" }
    | { type: "TOGGLE_ERROR" }
    | { type: "SET_ALERT_MESSAGE"; payload: string };

const initialState: RegisterState = {
    loading: false,
    registerModel: {
        firstName: "",
        lastName: "",
        phoneNumber: "",
        email: "",
        password: "",
        confirmPassword: "",
        divisionId: 1,
        jobTitleId: 1,
        officeId: 1,
    },
    errors: {
        firstName: "",
        lastName: "",
        phoneNumber: "",
        email: "",
        password: "",
        confirmPassword: "",
        divisionId: 1,
        jobTitleId: 1,
        officeId: 1,
    },
    successfulAction: false,
    errorAction: false,
    alertMessage: "",
};

const registerReducer = (
    state: RegisterState,
    action: RegisterAction
): RegisterState => {
    switch (action.type) {
        case "TOGGLE_LOADING":
            return { ...state, loading: !state.loading };
        case "SET_REGISTER_MODEL":
            return {
                ...state,
                registerModel: {
                    ...state.registerModel,
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

export default function Register(): ReactElement {
    const [state, dispatch] = useReducer(registerReducer, initialState);
    const { registerUser } = useAuthContext();

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

        if (!state.registerModel.firstName.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "firstName",
                    message: "Your first name is required.",
                },
            });
            valid = false;
        }
        if (!state.registerModel.lastName.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "lastName",
                    message: "Your last name is required.",
                },
            });
            valid = false;
        }
        if (!state.registerModel.phoneNumber.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "phoneNumber",
                    message: "Your phone number is required.",
                },
            });
            valid = false;
        }
        if (!state.registerModel.email.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "email",
                    message: "Your email is required.",
                },
            });
            valid = false;
        }
        if (
            !/^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/.test(
                state.registerModel.email.trim()
            )
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
        if (!state.registerModel.password.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "password",
                    message: "Your password is required.",
                },
            });
            valid = false;
        }
        if (!state.registerModel.confirmPassword.trim()) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "confirmPassword",
                    message: "Your confirm password is required.",
                },
            });
            valid = false;
        }
        if (
            state.registerModel.password.trim() !==
            state.registerModel.confirmPassword.trim()
        ) {
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "confirmPassword",
                    message:
                        "The password entered and the confirm password do not match.",
                },
            });
            dispatch({
                type: "SET_ERRORS",
                payload: {
                    field: "password",
                    message:
                        "The password entered and the confirm password do not match.",
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
                const response = await registerUser(state.registerModel);

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
            type: "SET_REGISTER_MODEL",
            payload: {
                field: name,
                value,
            },
        });
    };

    const handleCancel = () => {
        dispatch({ type: "RESET" });
    };

    const formatNumber = (e: ChangeEvent<HTMLInputElement>) => {
        const rawValue = e.target.value;
        const digits = rawValue.replace(/\D/g, "").slice(0, 10); // Remove non-digits & limit to 10 digits
        let formatted = "";

        const areaCode = digits.slice(0, 3);
        const middle = digits.slice(3, 6);
        const last = digits.slice(6, 10);

        if (digits.length <= 3) {
            formatted = `(${areaCode}`;
        } else if (digits.length <= 6) {
            formatted = `(${areaCode}) ${middle}`;
        } else {
            formatted = `(${areaCode}) ${middle}-${last}`;
        }

        dispatch({
            type: "SET_REGISTER_MODEL",
            payload: {
                field: "phoneNumber",
                value: formatted,
            },
        });
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
                        Register
                    </Typography>
                </Box>
                <Box className="pageHeaderComponent"></Box>
            </Box>
            <Box sx={{ width: "80%", textAlign: "center" }}>
                <Box>
                    <TextField
                        name="firstName"
                        label="First Name"
                        variant="outlined"
                        value={state.registerModel.firstName}
                        onChange={handleChange}
                        error={!!state.errors.firstName}
                        helperText={state.errors.firstName || "Jane"}
                    />
                </Box>
                <Box>
                    <TextField
                        name="lastName"
                        label="Last Name"
                        variant="outlined"
                        value={state.registerModel.lastName}
                        onChange={handleChange}
                        error={!!state.errors.lastName}
                        helperText={state.errors.lastName || "Doe"}
                    />
                </Box>
                <Box>
                    <TextField
                        name="email"
                        label="Email Address"
                        variant="outlined"
                        type="email"
                        value={state.registerModel.email}
                        onChange={handleChange}
                        error={!!state.errors.email}
                        helperText={
                            state.errors.email || "janedoe@pettycash.co.za"
                        }
                    />
                </Box>
                <Box>
                    <TextField
                        name="phoneNumber"
                        label="Phone Number"
                        variant="outlined"
                        value={state.registerModel.phoneNumber}
                        onChange={(e: ChangeEvent<HTMLInputElement>) => {
                            handleChange(e);
                            formatNumber(e);
                        }}
                        error={!!state.errors.phoneNumber}
                        helperText={
                            state.errors.phoneNumber || "(000) 000-0000"
                        }
                    />
                </Box>
                <Box>
                    <TextField
                        name="password"
                        label="password"
                        variant="outlined"
                        value={state.registerModel.password}
                        type="password"
                        onChange={handleChange}
                        error={!!state.errors.password}
                        helperText={state.errors.password}
                    />
                </Box>
                <Box>
                    <TextField
                        name="confirmPassword"
                        label="Confirm Password"
                        variant="outlined"
                        type="password"
                        value={state.registerModel.confirmPassword}
                        onChange={handleChange}
                        error={!!state.errors.confirmPassword}
                        helperText={state.errors.confirmPassword}
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
