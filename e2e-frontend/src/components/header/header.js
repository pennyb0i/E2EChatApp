import './header.css'
import isAuthenticated from '../../guards/authGuard';
import {logout} from '../../services/authService'
import {Navigate, useNavigate} from "react-router-dom";

import { getUsername} from "../../services/userService"


const Header = () => {
    let navigate = useNavigate();
    const loggedIn = isAuthenticated();

    function handleLogout() {
        logout();
    }

    return (
        <header className="mainHeader">
            <nav>
                <ul>
                    <li>
                        <a href="/home" className="mainNavLink">Home</a>
                    </li>
                    <li>
                        <a href="/friends" className="mainNavLink">Friends</a>
                    </li>
                    <li>
                        <a href="/chat" className="mainNavLink">Chat</a>
                    </li>
                    <li className="LoggedInText">

                        {loggedIn ? (
                            <span className="loggedInText">Logged in as <span className="usernameText">{getUsername()}</span></span>
                        ) : (
                            <div></div>
                        )}
                    </li>
                    <li className="loginButton">
                        {loggedIn ? (
                            <>
                            <a href="/login" className="mainNavLink" onClick={handleLogout}>
                                Logout</a>
                            </>
                        ) : (
                            <a href="/login" className="mainNavLink">
                                Login
                            </a>
                        )}
                    </li>
                </ul>
            </nav>
        </header>
    );
};

export default Header;