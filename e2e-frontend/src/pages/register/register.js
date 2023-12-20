import Header from "../../components/header/header";
import "./register.css"

const Register = () => {
    return (
        <>
            <Header/>
        <div className="registerContainer">
            <form className="registerForm">
                <h2>Register</h2>
                <label htmlFor="username">Username:</label>
                <input type="text" id="username" name="username" />

                <label htmlFor="email">Email:</label>
                <input type="email" id="email" name="email" />

                <label htmlFor="password">Password:</label>
                <input type="password" id="password" name="password" />

                <button type="submit">Register</button>

                <p>
                    Already have an account?
                    <a href="/login" className="loginLink">Login here</a>
                </p>
            </form>
        </div>
        </>
    );
};

export default Register;