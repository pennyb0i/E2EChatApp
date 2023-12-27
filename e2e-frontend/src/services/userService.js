import {apiConfig} from "../apiConfig";
import {API_BASE_URL} from "../api";

export const getAllUsers = async () => {
    try {
        const response = await fetch(`${API_BASE_URL}/User`);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const result = await response.json();
        return result;
    } catch (error) {
        throw error;
    }
};
export const getFriendRequests = async () => {

};
export const addFriend = async () => {

};
export const acceptFriendRequest = async () => {

};
export const declineFriendRequest = async () => {

};