import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { registerRequest } from "../../services/apiClient";
import styles from './RegisterPage.module.css';

const RegisterPage = () => {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);
    const [successMessage, setSuccessMessage] = useState("");
    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        setError("");
        setSuccessMessage("");
        setLoading(true);
        try {
            await registerRequest(username, email, password);
            setSuccessMessage("Registration successful! Redirecting to login...");
            // Redirect to login page after a short delay
            setTimeout(() => {
                navigate("/login");
            }, 2000);
        } catch (err) {
            setError(err.message || "Registration failed. Please try again.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className={styles.registerContainer}>
            <div className={styles.leftPanel}></div>

            <div className={styles.rightPanel}>
                <div className={styles.registerContent}>
                    <h1>BUDGET BAY</h1>
                    <p className={styles.subtitle}>Create your account to start bidding</p>
                    
                    <form className={styles.registerForm} onSubmit={handleRegister}>
                        <div className={styles.formGroup}>
                            <input 
                                id="username" 
                                type="text" 
                                placeholder="Username"
                                value={username} 
                                onChange={(e) => setUsername(e.target.value)}
                                required
                            />
                        </div>
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
                        {successMessage && <p className={styles.successMessage}>{successMessage}</p>}
                        
                        <button type="submit" className={styles.registerButton} disabled={loading || successMessage}>
                            {loading ? 'Registering...' : 'Sign Up'}
                        </button>
                    </form>

                    <p className={styles.loginLink}>
                        Already have an account? <Link to="/login">Login</Link>
                    </p>
                </div>
            </div>
        </div>
    );
};

export default RegisterPage;