import { useState, useContext } from "react";
import { AuthContext } from "../../contexts/AuthContext";

const LoginPage = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const {login} = useContext(AuthContext);

    const onLogin = () => {
        login(email, password);
    }

    return (
        <div>
            <h1>Login</h1>
            <label for="email">Email</label>
            <input id="email" name="email" type="text" value={email} onChange={(e)=>setEmail(e.target.value)}/>
            <label for="password">Password</label>
            <input id="password" name="password" type="password" value={password} onChange={(e)=>setPassword(e.target.value)}/>
            <button onClick={onLogin}>login</button>
        </div>
    );
}

export default LoginPage;