import { Navigate, Outlet } from "react-router-dom";
import { useAuthContext } from "context/AuthContext";
import { ReactElement } from "react";
import { Routes } from "api/router";

export function PublicRoute(): ReactElement {
    const { token } = useAuthContext();

    if (token) {
        return <Navigate to={Routes.Dashboard} replace />;
    }

    return <Outlet />;
}
