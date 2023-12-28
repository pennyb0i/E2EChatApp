import {API_BASE_URL} from "../api"
import {computeSharedSecret, getPrivateKey} from "../services/diffieHellmanService";

export const getMessages = async (firstUserId, secondUserId) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Message/${firstUserId}/${secondUserId}`,{
            headers: {
                Authorization: `Bearer ${localStorage.getItem('jwt')}`,
                'Content-Type': 'application/json',
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

export const sendMessage = async (senderId, receiverId, content) => {
    try {
        const response = await fetch(`${API_BASE_URL}/message/send`, {
            method: 'POST',
            headers: {
                Authorization: `Bearer ${localStorage.getItem('jwt')}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                senderId,
                receiverId,
                content,
            }),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        return true;
    } catch (error) {
        throw error;
    }
};

export const getSharedSecret = (friend) => {
    const myPrivateKey = getPrivateKey();
    const friendPublicKey = friend.publicKey;

    return computeSharedSecret(myPrivateKey, friendPublicKey);
}