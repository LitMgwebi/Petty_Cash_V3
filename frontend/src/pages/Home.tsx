import { FC, useEffect } from "react"
import { useBranchContext } from "context/BranchContext"
import { ServerResponse } from "types/ServerResponse";
import { Branch } from "types/Branch";

const Home: FC = () => {
    const { branches, getBranches } = useBranchContext();

    useEffect(() => {
        const fetchBranches = async () => {
            // Await the result of getBranches to resolve the promise
            const response: ServerResponse<Branch[]> = await getBranches();

            // Destructure the ServerResponse
            const { success, message } = response;


            console.info(message, success)
            console.info(branches)
        };

        fetchBranches();
    }, [getBranches]);

    return (
        <div>Hello mama how are you?</div>
    )
}

export default Home