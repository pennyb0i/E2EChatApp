import Header from "../../components/header/header";
import "./login.css"

import {signIn} from '../../services/authService'
import {useNavigate} from "react-router-dom";

const Login = () => {
    let navigate = useNavigate();
    async function handleLogin(event) {
        event.preventDefault();

        const email = event.target.elements.email.value;
        const password = event.target.elements.password.value;

        const response = await signIn(email,password);

        if(response){
            if(response.message === "ok"){
                navigate('/home');
                console.log("logged in")
            }
        }
    }

    return (
        <>
            <Header/>
            <div className="loginContainer">
                <form className="loginForm" onSubmit={handleLogin}>
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