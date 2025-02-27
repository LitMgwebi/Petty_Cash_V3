import axios from "axios";

axios.defaults.baseURL = "https://localhost:7190/api/";
axios.defaults.headers.common['Content-Type'] = "application/json";