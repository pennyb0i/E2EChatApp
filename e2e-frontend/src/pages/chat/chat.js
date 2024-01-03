import React, {useEffect, useState} from 'react';
import Header from "../../components/header/header";
import Message from "../chat/components/message";
import './chat.css'
import * as signalR from "@microsoft/signalr";

import {getAllUsers, getId} from "../../services/userService";

import {getMessages, getSharedSecret, sendMessage} from '../../services/chatService';

import {decryptData, encryptData} from '../../services/encryptionService';

const loggedUserId = getId();

const Chat = () => {
    const [friends, setFriends] = useState([]);
    const [messages, setMessages] = useState([]);
    const [selectedFriend, setSelectedFriend] = useState(null);
    const [inputValue, setInputValue] = useState('');
    const [loadingChat, setLoadingChat] = useState(false);
    const [loadingFriends, setLoadingFriends] = useState(false);

    const [connection, setConnection] = useState(null);

    useEffect(() => {
        loadUsers();
    }, []);


    function chooseFriend(friend) {
        setSelectedFriend(friend);
        loadChat(loggedUserId, friend);
    }

    async function loadChat(myId, friend) {
        try {
            const secret = getSharedSecret(friend);
            setLoadingChat(true);
            const messagesEncrypted = await getMessages(friend.id);

            const messagesDecrypted = await decryptMessages(messagesEncrypted, secret);

            setLoadingChat(false);

            setMessages(messagesDecrypted);
            console.log(messages);
        } catch (e) {

        }
    }

    async function decryptMessages(messagesEncrypted, secret) {
        const messages = [...messagesEncrypted];

        return messages.map(message => {

            const decryptedMessage = {...message};

            decryptedMessage.content = decryptData(message.content, secret);
            return decryptedMessage;
        });
    }

    async function loadUsers() {
        try {
            setLoadingFriends(true);
            const result = await getAllUsers();
            const filteredFriends = result.filter(user => user.id.toString() !== loggedUserId);
            setFriends(filteredFriends);
            setLoadingFriends(false);
        } catch (e) {

        }
    }

    const handleSendMessage = async () => {
        const secret = getSharedSecret(selectedFriend);

        const encryptedText = encryptData(inputValue, secret);

        try {
            const result = await sendMessage(selectedFriend.id, encryptedText);
            //send message signal
            if (result) {
                setInputValue("");
                loadChat(loggedUserId, selectedFriend);
            }

        } catch (e) {
            console.error('Error sending message:', e.message);
        }

    };

    const handleInputChange = (event) => {
        setInputValue(event.target.value);
    };

    return (
        <>
            <Header/>
            <div className="container">
                <div className="container-left">
                    <ul className="friend-list">
                        {friends.map(friend => (
                            <li key={friend.id} onClick={() => chooseFriend(friend)}
                                className={selectedFriend != null && selectedFriend.id === friend.id ? "selected" : ""}>
                                <a>{friend.username}</a>
                            </li>
                        ))}
                    </ul>
                </div>

                {loadingChat ? (
                    <div>Loading...</div>
                ) : selectedFriend === null ? (
                    <div className="select-friend-text">Select a friend to start the conversation</div>
                ) : (
                    <div className="container-right">
                        <div className="chat-container">
                            {messages.map((message, index) => (
                                <Message key={index} message={message} userId={loggedUserId.toString()}
                                         friend={selectedFriend}/>
                            ))}
                        </div>
                        <div className="input-container">
                            <input type="text" value={inputValue} onChange={handleInputChange}/>
                            <button onClick={handleSendMessage}>Send</button>
                        </div>
                    </div>
                )}


            </div>
        </>
    );
};

export default Chat;