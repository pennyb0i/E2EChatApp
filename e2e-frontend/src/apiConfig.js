export const apiConfig = {
    baseURL: "http://localhost:5001",
    getHeaders: () => ({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${getJwtToken()}`,
    }),
};
const getJwtToken = () => {
    return localStorage.getItem("jwt");
};