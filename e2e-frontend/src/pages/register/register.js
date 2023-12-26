import Header from "../../components/header/header";
import "./register.css"
import * as DiffieHellmanService from '../../services/diffieHellmanService';
import React from 'react';
import {useNavigate} from "react-router-dom";
import * as EncryptionService from '../../services/encryptionService';
import {signUp} from '../../services/authService'


const Register = () => {
    let navigate = useNavigate();
        async function handleRegister (event) {
            event.preventDefault();

            const username = event.target.elements.username.value;
            const email = event.target.elements.email.value;
            const password = event.target.elements.password.value;

            const response = await signUp(email,password,username);

            if(response){
                if(response.message === "ok"){
                    navigate('/login');
                }
            }

        }

        return (
            <>
                <Header/>
                <div className="registerContainer">
                    <form className="registerForm" onSubmit={handleRegister}>
                        <h2>Register</h2>
                        <label htmlFor="username">Username:</label>
                        <input type="text" id="username" name="username"/>

                        <label htmlFor="email">Email:</label>
                        <input type="email" id="email" name="email"/>

                        <label htmlFor="password">Password:</label>
                        <input type="password" id="password" name="password"/>

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