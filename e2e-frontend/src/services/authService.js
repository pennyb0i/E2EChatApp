import axios from 'axios';
import * as DiffieHellmanService from "./diffieHellmanService";

export const signIn = async (email, password) => {
    try {
        const response = await axios.post('http://localhost:5001/api/auth/login', {
            email,
            password,
        });

        const result = response.data;

        localStorage.setItem('jwt', result.jwt);
        return result;
    } catch (error) {
        console.error('Login failed', error);
    }
};
export const signUp = async (email, password, username) => {
    try {
        const keyPair = DiffieHellmanService.generateECDHKeyPair();

        const privateKey = keyPair.privateKey;
        const publicKey = keyPair.publicKey;

        const response = await axios.post('http://localhost:5001/api/auth/register', {
            email,
            password,
            username,
            publicKey,
        });

        localStorage.setItem('privateKey', privateKey);
        return response.data;
    } catch (error) {
        console.error('Registration failed', error);
    }
};

export const logout = () => {
    localStorage.removeItem('jwt');
};

export const getJwt = () => {
    localStorage.getItem('jwt');
};