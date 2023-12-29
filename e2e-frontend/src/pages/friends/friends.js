import Header from "../../components/header/header";
import React, {useEffect, useState} from 'react';
import './friends.css';
import {getAllUsers} from "../../services/userService";

const Friends = () => {
    const [users, setUsers] = useState([]);

    useEffect(() => {
        async function fetchUsers() {
            const fetchedUsers = await getAllUsers();
            setUsers(fetchedUsers);
        }
        fetchUsers();
    }, []);

    const [requests, setRequests] = useState([
        'Request 1',
        'Request 2',
        'Request 3',
    ]);

    const [searchValue, setSearchValue] = useState('');

    const filteredUsers = searchValue
        ? users.filter((user) =>
            typeof user.name === 'string' &&
            user.name.toLowerCase().includes(searchValue.trim().toLowerCase())
        )
        : users;

    const addFriend = () => {
        if (searchValue.trim() !== '') {
            setUsers([...users, searchValue]);
            setSearchValue('');
        }
    };

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
                            placeholder="Enter User name"
                            value={searchValue}
                            onChange={(e) => setSearchValue(e.target.value)}
                        />
                    </div>

                    {/* List of users */}
                    <div>
                        {users ? (
                            <ul className="friends-list">
                                {filteredUsers.map((user, index) => (
                                    <li key={index}>
                                        <span>{user.username}</span>
                                        <button onClick={() => addFriend(user)}>
                                            Add Friend
                                        </button>
                                    </li>
                                ))}
                            </ul>
                        ) : (
                            <p>No users available</p>
                        )}
                    </div>

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