import Header from "../../components/header/header";
import React, {useEffect, useState} from 'react';
import './friends.css';
import {createFriendship, getAllFriendRequests, getAllNotFriends} from "../../services/friendsService";

const Friends = () => {
    const [users, setUsers] = useState([]);
    const [requests, setRequests] = useState([]);
    const [searchValue, setSearchValue] = useState('');

    useEffect(() => {
        async function fetchUsers() {
            const fetchedUsers = await getAllNotFriends();
            setUsers(fetchedUsers);
        }
        fetchUsers();
    }, []);

    useEffect(() => {
        async function fetchRequests() {
            const fetchedRequests = await getAllFriendRequests();
            setRequests(fetchedRequests);
        }
        fetchRequests();
    }, []);

    const filteredUsers = searchValue
        ? users.filter((user) =>
            typeof user.email === 'string' &&
            user.email.toLowerCase().includes(searchValue.trim().toLowerCase())
        )
        : users;

    const sendFriendRequest = async (userId, setRequests, requests) => {
        try {
            await createFriendship(userId);
            console.log("Selected userID: " + userId);

        } catch (error) {
            console.error("Failed to send friend request:", error);
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
                                        <span>{user.email}</span>
                                        <button onClick={() => sendFriendRequest(user.id)}>
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
                                {request.sender.email}
                                <div className="button-container">
                                    <button onClick={() => sendFriendRequest(request.sender.id, setRequests, requests)}>
                                        Accept
                                    </button>
                                    <button onClick={() => sendFriendRequest()}>
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