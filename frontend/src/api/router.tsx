import { createBrowserRouter } from "react-router-dom";
import { lazy } from "react";
import Layout from "components/Layout";

export const Routes = {
    Home: "/",
    Branch: "/branch"
}

export const router = createBrowserRouter([
    {
        path: Routes.Home,
        element: <Layout />,
        children: [
            {
                path: Routes.Home,
                Component: lazy(() => import("pages/Home")),
            },
            {
                path: "/branch",
                Component: lazy(() => import("pages/Branch/BranchIndex"))
            },
        ]
    }
]);