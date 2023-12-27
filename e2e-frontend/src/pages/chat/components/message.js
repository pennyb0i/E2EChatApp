import React from 'react';

const MessageComponent = ({ message, userId,friend }) => {
    return (
        <div className="message">
            <div className={`${userId === message.senderId? "bubble user-bubble": "bubble other-bubble"}`}>
                <span className="user">{userId === message.senderId? "You: ": friend.username}</span> {message.content}
            </div>
        </div>

    );
};

export default MessageComponent;