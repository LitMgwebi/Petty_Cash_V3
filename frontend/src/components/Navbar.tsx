import { FC } from "react";
import MenuIcon from '@mui/icons-material/Menu';
import { IconButton, Typography, Toolbar, Box, AppBar, Container } from "@mui/material"
import { Link } from "react-router-dom";
import { Routes } from "api/router"

const Navbar: FC = () => {

    return (
        <AppBar position="static">
            <Container>
                <Box sx={{ flexGrow: 1 }}>
                    <Toolbar>
                        <IconButton
                            size="large"
                            edge="start"
                            color="inherit"
                            aria-label="menu"
                            sx={{ mr: 2 }}
                        >
                            <MenuIcon />
                        </IconButton>
                        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                            Petty Cash
                        </Typography>
                        <Typography>
                            <Link to={Routes.Branch}>Branch</Link>
                        </Typography>
                    </Toolbar>
                </Box>
            </Container >
        </AppBar >
    );
}

export default Navbar;