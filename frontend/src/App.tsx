import { RouterProvider } from "react-router-dom";
import { Suspense } from "react";
import { router } from "api/router"
import Loading from "components/Loading";
import './App.css';

function App() {
    return (
        <div className="App">
            <Suspense fallback={<Loading open={true} />}>
                <RouterProvider router={router} />
            </Suspense>
        </div>
    );
}

export default App;
