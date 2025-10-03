import { useState, useContext } from "react";
import { useNavigate, Navigate, Link } from "react-router-dom";
import { AuthContext } from "../../contexts/AuthContext";
import styles from './LoginPage.module.css';
// import logo from '../../assets/logo.svg'; 

const LoginPage = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const { token, login, loading } = useContext(AuthContext);
    const navigate = useNavigate();

    if (token) {
        return <Navigate to="/" replace />;
    }

    const handleLogin = async (e) => {
        e.preventDefault();
        setError("");
        const success = await login(email, password);
        if (success) {
            navigate("/");
        } else {
            setError("Login failed. Please check your credentials.");
        }
    };

    return (
        <div className={styles.loginContainer}>
            <div className={styles.leftPanel}></div>

            <div className={styles.rightPanel}>
                <div className={styles.loginContent}>
                    {/* <img src={logo} alt="Budget Bay Logo" className={styles.logo} /> */}
                    <h1>BUDGET BAY</h1>
                    <p className={styles.subtitle}>Login to start bidding</p>
                    
                    <form className={styles.loginForm} onSubmit={handleLogin}>
                        <div className={styles.formGroup}>
                            <input 
                                id="email" 
                                type="email" 
                                placeholder="Email"
                                value={email} 
                                onChange={(e) => setEmail(e.target.value)}
                                required
                            />
                        </div>
                        <div className={styles.formGroup}>
                            <input 
                                id="password" 
                                type="password" 
                                placeholder="Password"
                                value={password} 
                                onChange={(e) => setPassword(e.target.value)}
                                required
                            />
                        </div>
                        
                        {error && <p className={styles.errorMessage}>{error}</p>}
                        
                        <button type="submit" className={styles.loginButton} disabled={loading}>
                            {loading ? 'Logging in...' : 'Login'}
                        </button>
                    </form>

                    <p className={styles.signupLink}>
                        Click Here to <Link to="/signup">Sign Up</Link>
                    </p>
                </div>
            </div>
        </div>
    );
};

export default LoginPage;