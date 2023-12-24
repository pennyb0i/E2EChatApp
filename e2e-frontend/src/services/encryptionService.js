import CryptoJS from 'crypto-js';

const encryptData = (data, encryptionKey) => {
    try {
        const ciphertext = CryptoJS.AES.encrypt(JSON.stringify(data), encryptionKey).toString();
        return ciphertext;
    } catch (error) {
        console.error('Encryption error:', error.message);
    }
};

const decryptData = (ciphertext, encryptionKey) => {
    try {
        const bytes = CryptoJS.AES.decrypt(ciphertext, encryptionKey);
        const decryptedData = JSON.parse(bytes.toString(CryptoJS.enc.Utf8));
        return decryptedData;
    } catch (error) {
        console.error('Decryption error:', error.message);
    }
};

export { encryptData, decryptData };