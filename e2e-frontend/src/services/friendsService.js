import axios from "axios";
import {apiConfig} from "../apiConfig";
import {getId} from "./userService";

export const createFriendship = async (id) => {
    try {
        const respons = await axios.post(`${apiConfig.baseURL}/Friendship/${id}`,{},{
            headers: apiConfig.getHeaders(),
        });
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

        const userRequests = result.filter(request =>
            request.isPending &&
            (request.sender.id !== getId() && request.receiver.id == getId())
        );

        return userRequests;

    } catch (error) {
        throw error;
    }
}


export const getAllNotFriends = async () => {
    try {
        const response = await fetch(`${apiConfig.baseURL}/api/User?friendsOnly=false`, {
            headers: apiConfig.getHeaders(),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const result = await response.json();
        return result;
    } catch (error) {
        throw error;
    }
}

export const cancelFriendship = async (id) => {
    try {
        const response = await axios.delete(`${apiConfig.baseURL}/Friendship/${id}`,{
            headers: apiConfig.getHeaders(),
        });
        return response.data
    }catch (e) {
        console.error('Cancel the friend request is failed', e)
    }
}