import { FC } from "react";
import { Backdrop, CircularProgress } from "@mui/material";

interface LoadingProps {
    open: boolean;
}

const Loading: FC<LoadingProps> = ({ open }) => {
    return (
        <div>
            <Backdrop
                sx={(theme) => ({
                    color: "#fff",
                    zIndex: theme.zIndex.drawer + 1000,
                })}
                open={open}
            >
                <CircularProgress color="inherit" />
            </Backdrop>
        </div>
    );
};

export default Loading;
