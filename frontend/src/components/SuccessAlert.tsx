import { AlertTitle, Alert, IconButton } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";

interface SuccessAlertProps {
    message: string;
    closeAlert: () => void;
}

function SuccessAlert({ message, closeAlert }: SuccessAlertProps): JSX.Element {
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
            severity="success"
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
            <AlertTitle>Success</AlertTitle>
            {message}
        </Alert>
    );
}

export default SuccessAlert;
