import { Outlet } from "react-router";
import Navbar from "./Navbar";
import { FC } from "react";

const Layout: FC = () => {
    return (
        <>
            <Navbar />
            <Outlet />
        </>
    )
}

export default Layout;