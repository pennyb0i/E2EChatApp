import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Home from './pages/home/home';
import Login from './pages/login/login';
import Register from './pages/register/register';
import Friends from './pages/friends/friends';
import Chat from './pages/chat/chat';

// Assume isAuthenticated is a valid function that checks user authentication
import isAuthenticated from './guards/authGuard';

const AuthenticatedComponent = ({ component: Component, ...rest }) => {
    return isAuthenticated() ? (
        <Component {...rest} />
    ) : (
        <Navigate to="/login" />
    );
};

const App = () => {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/home" element={<Home />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/friends" element={<AuthenticatedComponent component={Friends} />} />
            <Route path="/chat" element={<AuthenticatedComponent component={Chat} />} />
        </Routes>
    );
};

export default App;