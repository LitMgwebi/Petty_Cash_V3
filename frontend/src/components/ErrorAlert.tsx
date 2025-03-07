import { AlertTitle, Alert, IconButton } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";

interface ErrorAlertProps {
    message: string;
    closeAlert: () => void;
}
function ErrorAlert({ message, closeAlert }: ErrorAlertProps): JSX.Element {
    function handleClose() {
        closeAlert();
    }
    return (
        <Alert
            sx={{
                position: "absolute",
                top: 0,
                left: 0,
                width: "100%",
                zIndex: 10,
            }}
            severity="error"
            action={
                <IconButton
                    aria-label="close"
                    color="inherit"
                    size="small"
                    onClick={handleClose}
                >
                    <CloseIcon fontSize="inherit" />
                </IconButton>
            }
        >
            <AlertTitle>Error</AlertTitle>
            {message}
        </Alert>
    );
}

export default ErrorAlert;
