import { useContext } from "react";
import { Link } from "react-router-dom";
import { AuthContext } from "../../contexts/AuthContext";
import styles from './HomePage.module.css';
import SearchBar from "../../components/common/SearchBar";

const HomePage = () => {
    const { token, logout } = useContext(AuthContext);

    return (
        <main>
        <div className={styles.homepageContainer}>
            {token ? (
                <div>
                    <h1>Welcome Back!</h1>
                    <p>You are logged in.</p>
                    <Link to="/dashboard" className={styles.dashboardLink}>
                        <button>Go to Dashboard</button>
                    </Link>
                    <button onClick={logout}>Logout</button>
                </div>
            ) : (
                <div>
                    <h1>Welcome to BudgetBay</h1>
                    <p>Please log in to manage your budget.</p>
                    <Link to="/login">
                        <button>Login</button>
                    </Link>
                </div>
            )}
        <SearchBar />
        </div>
        </main>
    )
}

export default HomePage;