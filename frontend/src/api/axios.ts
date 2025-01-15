import axios from "axios";

const axiosBase = axios.create({
    baseURL:"https",
    headers: {
        'Content-Type' : 'application/json'
    }
})

export default axiosBase