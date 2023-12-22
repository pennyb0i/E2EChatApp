const URL = 'http://localhost:5112';
export const getUsers = async () => {
    try {
        const response = await fetch(`${URL}/User`);
        if (!response.ok) {
            throw new Error('Failed to fetch users');
        }
        return response.json();
    } catch (error) {
        console.error('Error fetching users:', error);
        return [];
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