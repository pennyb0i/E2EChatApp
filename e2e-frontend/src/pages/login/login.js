import Header from "../../components/header/header";
import "./login.css"

const Login = () => {
    return (
        <>
            <Header/>
            <div className="loginContainer">
                <form className="loginForm">
                    <h2>Login</h2>
                    <label htmlFor="email">Email:</label>
                    <input type="text" id="email" name="email" />

                    <label htmlFor="password">Password:</label>
                    <input type="password" id="password" name="password" />

                    <button type="submit">Login</button>

                    <p>
                        Don't have an account?
                        <a href="/register" className="registerLink">Register here</a>
                    </p>
                </form>
            </div>
        </>
    );
};

export default Login;