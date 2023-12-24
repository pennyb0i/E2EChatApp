import crypto from "crypto-browserify";
import {Buffer} from 'buffer';

const initialize = () => {
    if (typeof window !== 'undefined') {
        window.Buffer = Buffer;
    }
};

const generateECDHKeyPair = () => {
    const ecdh = crypto.createECDH('secp256k1');
    const publicKey = ecdh.generateKeys('hex', 'compressed');
    const privateKey = ecdh.getPrivateKey('hex');
    return { publicKey, privateKey };
};

const computeSharedSecret = (yourPrivateKey, otherPublicKey) => {
    const ecdh = crypto.createECDH('secp256k1');
    const yourPrivateKeyBuffer = Buffer.from(yourPrivateKey, 'hex');
    const otherPublicKeyBuffer = Buffer.from(otherPublicKey, 'hex');

    ecdh.setPrivateKey(yourPrivateKeyBuffer);

    const sharedSecret = ecdh.computeSecret(otherPublicKeyBuffer, 'hex', 'hex');

    return sharedSecret;
};

initialize();

export {generateECDHKeyPair,computeSharedSecret };