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
        'Request 1',
        'Request 2',
        'Request 3',
    ]);

    const [searchValue, setSearchValue] = useState('');

    const addFriend = () => {
        if (searchValue.trim() !== '') {
            setFriends([...friends, searchValue]);
            setSearchValue(''); // Clear the search bar after adding a friend
        }
    };
    const filteredFriends = friends.filter((friend) =>
        friend.toLowerCase().includes(searchValue.toLowerCase())
    )
    return (
        <>
            <Header />
            <div className="black-border-container">

                {/* Black-bordered window */}
                <div className="black-bordered-window">
                    <h1>List of Users</h1>

                    {/* Search bar for adding new friends */}
                    <div>
                        <input
                            type="text"
                            placeholder="Enter friend's name"
                            value={searchValue}
                            onChange={(e) => setSearchValue(e.target.value)}
                        />
                    </div>

                    {/* List of friends */}
                    <ul className="friends-list">
                        {filteredFriends.map((friend, index) => (
                            <li key={index}>
                                <span>{friend}</span>
                                <button onClick={() => addFriend(friend)}>
                                    Add Friend
                                </button>
                            </li>
                        ))}
                    </ul>

                    {/* Black line separator */}
                    <hr style={{borderTop: '2px solid black'}}/>

                    {/* Friend requests */}
                    <h1>Friend Requests</h1>
                    <ul className="request-list">
                        {requests.map((request, index) => (
                            <li key={index}>
                                {request}
                                <div className="button-container">
                                    <button onClick={() => addFriend(request)}>
                                        Accept
                                    </button>
                                    <button onClick={() => addFriend(request)}>
                                        Decline
                                    </button>
                                </div>
                            </li>
                        ))}
                    </ul>

                </div>
            </div>
        </>
    );
};


export default Friends;