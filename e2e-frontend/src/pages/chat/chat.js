import React, { useState, useEffect } from 'react';
import * as signalR from '@microsoft/signalr';
import Header from '../../components/header/header';
import './chat.css';

const Chat = () => {
    const [connection, setConnection] = useState(null);
    const [messages, setMessages] = useState([]);
    const [inputMessage, setInputMessage] = useState('');

    useEffect(() => {
        // Create a connection to the SignalR hub
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl('https://your-signalr-hub-url/chatHub') // Replace with your SignalR hub URL
            .build();

        // Set up event handlers
        newConnection.on('ReceiveMessage', (senderId, content) => {
            setMessages((prevMessages) => [...prevMessages, { senderId, content }]);
        });

        // Start the connection
        newConnection
            .start()
            .then(() => console.log('Connection started'))
            .catch((err) => console.error('Error starting connection:', err));

        setConnection(newConnection);

        // Cleanup function
        return () => {
            if (newConnection) {
                newConnection.stop();
            }
        };
    }, []);

    const sendMessage = () => {
        // Replace 'receiverId' with the actual receiver's ID
        const receiverId = 'receiverId';
        connection.invoke('SendMessage', 'yourSenderId', receiverId, inputMessage).catch((err) => console.error(err));
        setInputMessage('');
    };

    return (
        <>
            <Header />
            <div className="container">
                <div className="container-left">
                    {/* Your friend list goes here */}
                </div>
                <div className="container-right">
                    <div className="chat-container">
                        {messages.map((message, index) => (
                            <div key={index} className={`message ${message.senderId === 'yourSenderId' ? 'user' : 'other'}`}>
                                <div className="bubble">
                                    <span className="user">{message.senderId}:</span> {message.content}
                                </div>
                            </div>
                        ))}
                    </div>
                    <div className="input-container">
                        <input
                            type="text"
                            value={inputMessage}
                            onChange={(e) => setInputMessage(e.target.value)}
                        />
                        <button onClick={sendMessage}>Send</button>
                    </div>
                </div>
            </div>
        </>
    );
};

export default Chat;