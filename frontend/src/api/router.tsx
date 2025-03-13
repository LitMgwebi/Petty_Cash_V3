import { createBrowserRouter } from "react-router-dom";
import { lazy } from "react";
import Layout from "components/Layout";
import { PublicRoute } from "components/PublicRoute";
import { ProtectedRoute } from "components/ProtectedRoute";
import NotFound from "pages/NotFound";

export const Routes = {
    Home: "/",
    Branch: "/branch",
    Dashboard: "/dashboard",
    Login: "/login",
    Register: "/register",
    Office: "/office",
};

export const router = createBrowserRouter([
    {
        path: Routes.Home,
        element: <Layout />,
        children: [
            {
                index: true,
                Component: lazy(() => import("pages/Home")),
            },
            {
                element: <PublicRoute />,
                children: [
                    {
                        path: Routes.Login,
                        Component: lazy(() => import("pages/Auth/Login")),
                    },
                    {
                        path: Routes.Register,
                        Component: lazy(() => import("pages/Auth/Register")),
                    },
                ],
            },
            {
                element: <ProtectedRoute />,
                children: [
                    {
                        path: Routes.Dashboard,
                        Component: lazy(() => import("pages/Dashboard")),
                    },
                    {
                        path: Routes.Branch,
                        Component: lazy(
                            () => import("pages/Branch/BranchIndex")
                        ),
                    },
                    {
                        path: Routes.Office,
                        Component: lazy(
                            () => import("pages/Office/OfficeIndex")
                        ),
                    },
                ],
            },
            {
                path: "*",
                element: <NotFound />,
            },
        ],
    },
]);
