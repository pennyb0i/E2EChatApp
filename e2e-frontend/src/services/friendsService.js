import {apiConfig} from "../apiConfig";

export const getUsers = async () => {
    try {
        const response = await fetch(apiConfig.API_URL+'User');
        return response.json();
    } catch (error) {
        console.error('Error fetching users:', error);
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