import './header.css'
const Header = () => {

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
                    <li className="loginButton">
                        <a href="/login" className="mainNavLink">Login</a>
                    </li>
                </ul>
            </nav>
        </header>
    );
};

export default Header;


/*
<div>
    <p>
        <nav>
            <button>Home</button>
            <button>Friendz</button>
            <button>Login</button>
        </nav>
    </p>
</div>*/
