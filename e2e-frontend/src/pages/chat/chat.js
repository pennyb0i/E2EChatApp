import React, {useEffect, useState} from 'react';
import Header from "../../components/header/header";
import Message from "../chat/components/message";
import './chat.css'

import { getAllUsers} from "../../services/userService";

import {getMessages, sendMessage} from '../../services/chatService';

const friendsList = [
    {id: 4, name: 'Albert'},
    {id: 5, name: 'Vlad'},
]

const messageList = [
    {senderId: 3, receiverId: 4, content: 'Hello Faustas'},
    {senderId: 5, receiverId: 3, content: 'Hey Tomas!'},
]

const myId = 8;


const Chat = () => {
    const [friends, setFriends] = useState([]);
    const [messages, setMessages] = useState(messageList);
    const [selectedFriend, setSelectedFriend] = useState(null);
    const [inputValue, setInputValue] = useState('');

    const [loadingChat, setLoadingChat] = useState(false);
    const [loadingFriends, setLoadingFriends] = useState(false);

    useEffect(() => {
        loadUsers();
    }, []);

    function chooseFriend(friend) {
        setSelectedFriend(friend);
        loadChat(myId, friend);
    }

    async function loadChat(myId,friend) {
        try {
            setLoadingChat(true);
            const result = await getMessages(myId, friend.id);
            setLoadingChat(false);
            setMessages(result);
        } catch (e) {

        }
    }

    async function loadUsers() {
        try {
            setLoadingFriends(true);
            const result = await getAllUsers();
            const filteredFriends = result.filter(user => user.id !== myId);
            setFriends(filteredFriends);
            setLoadingFriends(false);
        } catch (e) {

        }
    }

    const handleSendMessage = async () => {
        try{
            const result = await sendMessage(myId, selectedFriend.id, inputValue);
            if(result){
                setInputValue("");
                loadChat(myId,selectedFriend);
            }

        }catch (e) {
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
                                <Message key={index} message={message} userId={myId.toString()} friend={selectedFriend} />
                            ))}
                        </div>
                        <div className="input-container">
                            <input type="text" value={inputValue} onChange={handleInputChange} />
                            <button onClick={handleSendMessage}>Send</button>
                        </div>
                    </div>
                )}


            </div>
        </>
    );
};

export default Chat;