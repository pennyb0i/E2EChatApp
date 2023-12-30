import {API_BASE_URL} from "../api";

import { jwtDecode } from "jwt-decode";

export const getAllUsers = async () => {
    try {
        const response = await fetch(`${API_BASE_URL}/User`, {
            headers: {
                Authorization: `Bearer ${getJwtToken()}`,
            },
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const result = await response.json();
        return result;
    } catch (error) {
        throw error;
    }
};

export const getAllFriends = async () => {
    try {
        const response = await fetch(`${API_BASE_URL}/User?friendsOnly=true`, {
            headers: {
                Authorization: `Bearer ${getJwtToken()}`,
            },
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const result = await response.json();
        return result;
    } catch (error) {
        throw error;
    }
};

const getJwtToken = () => {
    return localStorage.getItem("jwt");
}

export const getId = () => {
try{
    const token = getJwtToken();
    const decoded = jwtDecode(token);
    const { UserId } = decoded;
    return UserId;
}catch (e){
    console.error('JWT decoding failed:', e.message);
}
}

export const getUsername = () => {
    try{
        const token = getJwtToken();
        const decoded = jwtDecode(token);
        const { Username } = decoded;
        return Username;
    }catch (e){
        console.error('JWT decoding failed:', e.message);
    }
}