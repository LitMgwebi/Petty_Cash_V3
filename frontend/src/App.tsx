import {
    BrowserRouter as Router,
    Routes,
    Route
} from "react-router-dom";
import Home from './pages/Home';
import BranchIndex from "pages/Branch/BranchIndex"
import Navbar from "components/Navbar";
import './App.css';

function App() {
    return (
        <div className="App">
            <Router>
                <Navbar />
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/branch" element={<BranchIndex />} />
                    <Route path="*" element={<h2>404 - Not Found</h2>} />
                </Routes>
            </Router>
        </div>
    );
}

export default App;
