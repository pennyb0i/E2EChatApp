const isAuthenticated = () => {
    const jwtToken = localStorage.getItem('jwt');

    return !!jwtToken;


};

export default isAuthenticated;