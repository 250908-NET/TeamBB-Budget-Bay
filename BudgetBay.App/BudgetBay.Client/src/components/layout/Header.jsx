import React, { useContext } from 'react';
import { Link, useLocation , useNavigate } from 'react-router-dom';
import { AuthContext } from '../../contexts/AuthContext';
import logo from '../../assets/logo-row.png';
import './Header.css';

const Header = () => {
    const location = useLocation();
    const currentPath = location.pathname;
    const navigate = useNavigate();
    const { user, logout } = useContext(AuthContext);

    const handleSignOut = () => {
        logout();
    };

    return (
        <header className='header'>
            <div>
                <Link to="/">
                    <img src={logo} alt="BudgetBay Logo" className="logo" />
                </Link>
            </div>
            <nav>
                <ul className='nav-links'>
                    {user ? (
                        <li>
                           <button onClick={handleSignOut} className="nav-btn">
                                Sign Out
                            </button>
                        </li>
                    ) : (
                        <>
                            {currentPath === '/login' && (
                                <li>
                                      <button onClick={() => navigate('/signup')} className="nav-btn">
                                        Sign Up
                                    </button>
                                </li>
                            )}
                            {currentPath === '/signup' && (
                                <li>
                                    <button onClick={() => navigate('/login')} className="nav-btn">
                                            Login
                                        </button>
                                </li>
                            )}
                            {currentPath !== '/login' && currentPath !== '/signup' && (
                                <>
                                    <li>
                                        <button onClick={() => navigate('/login')} className="nav-btn">
                                            Login
                                        </button>
                                    </li>
                                    <li>
                                        <button onClick={() => navigate('/signup')} className="nav-btn">
                                            Sign Up
                                        </button>
                                    </li>
                                </>
                            )}
                        </>
                    )}
                </ul>
            </nav>
        </header>
    );
};

export default Header;
