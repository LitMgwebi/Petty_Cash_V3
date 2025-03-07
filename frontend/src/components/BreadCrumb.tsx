import { Breadcrumbs, Link } from "@mui/material";
import { FC } from "react";
import { Link as RouterLink } from "react-router-dom";

interface BreadCrumbProps {
    items: { label: string, link: string, disabled: boolean }[]
}

const BreadCrumb: FC<BreadCrumbProps> = ({ items }) => {
    return (
        <div role="presentation">
            <Breadcrumbs aria-label="breadcrumb">
                {items.map((item, index) => (
                    <Link
                        key={index} // Ensure unique key for each item
                        component={item.disabled ? "span" : RouterLink}
                        to={item.disabled ? undefined : item.link}
                        aria-current="page"
                        sx={{
                            textDecoration: "none",
                            color: item.disabled ? "grey" : "blue",
                            pointerEvents: item.disabled ? "none" : "auto",
                            cursor: item.disabled ? "not-allowed" : "pointer",
                        }}
                    >
                        {item.label}
                    </Link>
                ))}
            </Breadcrumbs>
        </div>
    )
}

export default BreadCrumb;