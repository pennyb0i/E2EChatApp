import Header from "../../components/header/header";
import "./register.css"
import * as DiffieHellmanService from '../../services/diffieHellmanService';
const Register = () => {
    // TODO send public key to backend along with user info to complete registration
    function handleRegister() {
        const keyPair=DiffieHellmanService.generateECDHKeyPair();
        localStorage.setItem('privateKey', keyPair.privateKey);
    }

    return (
        <>
            <Header/>
        <div className="registerContainer">
            <form className="registerForm" onSubmit={handleRegister}>
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