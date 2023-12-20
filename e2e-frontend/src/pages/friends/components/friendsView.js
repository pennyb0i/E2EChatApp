import "./friendsView.css"
import React, { useState } from 'react';
import styled from 'styled-components';

const Friends = () => {
    const [friends, setFriends] = useState([
        'Friend 1',
        'Friend 2',
        'Friend 3',
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
            <div className="App">
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

                {/* List of friends */}
                <ul>
                    {friends.map((friend, index) => (
                        <li key={index}>{friend}</li>
                    ))}
                </ul>
            </div>
        </>
    );
};

export default Friends;