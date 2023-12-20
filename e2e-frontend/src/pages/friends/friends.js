import Header from "../../components/header/header";
import React, { useState } from 'react';
import './friends.css'

const Friends = () => {
    const [friends, setFriends] = useState([
        'Friend 1',
        'Friend 2',
        'Friend 3',
    ]);

    const [requests, setRequests] = useState([
        'Requests 1',
        'Requests 2',
        'Requests 3',
    ]);

    const [searchValue, setSearchValue] = useState('');

    const addFriend = () => {
        if (searchValue.trim() !== '') {
            setFriends([...friends, searchValue]);
            setSearchValue(''); // Clear the search bar after adding a friend
        }
    };
    return (
        <>
            <Header />
            <div className="black-border-container">
                <h1>My Friends</h1>

                {/* Search bar for adding new friends */}
                <div>
                    <input
                        type="text"
                        placeholder="Enter friend's name"
                        value={searchValue}
                        onChange={(e) => setSearchValue(e.target.value)}
                    />
                    <button onClick={addFriend}>Add Friend</button>
                </div>

                {/* Black-bordered window */}
                <div className="black-bordered-window">
                    {/* List of friends */}
                    <ul>
                        {friends.map((friend, index) => (
                            <li key={index}>{friend}</li>
                        ))}
                    </ul>

                    {/* Black line separator */}
                    <hr style={{ borderTop: '2px solid black' }} />

                    {/* Friend requests */}
                    <h1>Friend Requests</h1>
                    <ul>
                        {requests.map((requests, index) => (
                            <li key={index}>{requests}</li>
                        ))}
                    </ul>
                </div>
            </div>
            </>
    );
};


export default Friends;