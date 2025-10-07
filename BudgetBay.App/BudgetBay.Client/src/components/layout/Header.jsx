import { Link, useLocation } from 'react-router-dom';
import logo from '../../assets/full-logo.png';

const Header = () => {
    const location = useLocation();
    const currentPath = location.pathname;

    return (
        <header className='p-4 flex justify-between items-center'>
            <div>
                <Link to="/">
                    <img src={logo} alt="BudgetBay Logo" className="h-8" />
                </Link>
            </div>
            <nav>
                <ul className='flex space-x-4'>
                    {currentPath === '/login' && (
                        <li>
                            <Link to="/signup">Sign Up</Link>
                        </li>
                    )}
                    {currentPath === '/signup' && (
                        <li>
                            <Link to="/login">Login</Link>
                        </li>
                    )}
                    {currentPath !== '/login' && currentPath !== '/signup' && (
                        <>
                            <li>
                                <Link to="/login">Login</Link>
                            </li>
                            <li>
                                <Link to="/signup">Sign Up</Link>
                            </li>
                        </>
                    )}
                </ul>
            </nav>
        </header>
    );
};

export default Header;
