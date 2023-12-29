import axios from "axios";
import {apiConfig} from "../apiConfig";
import {getId} from "./userService";

export const createFriendship = async (id) => {
    try {
        const respons = await axios.post(`${apiConfig.baseURL}/Friendship/${id}`,{},{
            headers: apiConfig.getHeaders(),
        });
        console.log("Data: "+ respons.data)
         return respons.data
    }catch (e) {
        console.error('Send the friend request is failed', e)
    }
}

export const getAllFriendRequests = async () => {
    try {
        const response = await fetch(`${apiConfig.baseURL}/Friendship`, {
            headers: apiConfig.getHeaders(),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const result = await response.json();
        if (result.some(request => request.senderId == getId())) {
            return [];
        } else {
            return result
        }
    } catch (error) {
        throw error;
    }
}