import {
    createContext,
    useContext,
    ReactNode,
    ReactElement,
    useReducer,
} from "react";
import { ServerResponse } from "types/ServerResponse";
import { Register } from "types/Register";
import { Login } from "types/Login";
import { handleApiError } from "api/handleApiError";
import axios from "axios";
import { Token } from "types/Token";

//#region State Management

interface AuthState {
    token: Token | null;
}

type AuthAction = { type: "SET_TOKEN"; payload: Token } | { type: "RESET" };

const initialState: AuthState = {
    token: null,
};

const authReducer = (state: AuthState, action: AuthAction): AuthState => {
    switch (action.type) {
        case "SET_TOKEN":
            localStorage.setItem("token", JSON.stringify(action.payload));
            return { ...state, token: action.payload };
        case "RESET":
            localStorage.removeItem("token");
            return initialState;
        default:
            return state;
    }
};

//#endregion

interface AuthContextProps {
    token: Token | null;
    registerUser: (user: Register) => Promise<ServerResponse<Token>>;
    loginUser: (user: Login) => Promise<ServerResponse<Token>>;
    logoutUser: () => void;
}

const AuthContext = createContext<AuthContextProps | undefined>(undefined);

export function AuthProvider({
    children,
}: {
    children: ReactNode;
}): ReactElement {
    const [state, dispatch] = useReducer(authReducer, initialState);

    async function registerUser(
        user: Register
    ): Promise<ServerResponse<Token>> {
        try {
            const res = await axios.post<ServerResponse<Token>>(``, user);

            dispatch({ type: "SET_TOKEN", payload: res.data.data });
            return res.data;
        } catch (error) {
            return handleApiError<Token>(error);
        }
    }

    async function loginUser(user: Login): Promise<ServerResponse<Token>> {
        try {
            const res = await axios.post<ServerResponse<Token>>(``, user);

            dispatch({ type: "SET_TOKEN", payload: res.data.data });
            return res.data;
        } catch (error) {
            return handleApiError<Token>(error);
        }
    }

    function logoutUser() {
        dispatch({ type: "RESET" });
    }

    return (
        <AuthContext.Provider
            value={{ token: state.token, registerUser, loginUser, logoutUser }}
        >
            {children}
        </AuthContext.Provider>
    );
}

export function useAuthContext() {
    const context = useContext(AuthContext);

    if (!context) {
        throw new Error("useAuthContext must be used within a AuthProvider");
    }

    return context;
}
