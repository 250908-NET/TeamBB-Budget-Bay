import React, { useContext } from "react";
import { Link, useLocation } from "react-router-dom";
import { AuthContext } from "../../contexts/AuthContext";
import logo from "../../assets/full-logo.png";
import "./Header.css";

const Header = () => {
  const location = useLocation();
  const currentPath = location.pathname;
  const { user, logout } = useContext(AuthContext);

  const handleSignOut = () => {
    logout();
  };

  return (
    <header className="header">
      <div>
        <Link to="/">
          <img src={logo} alt="BudgetBay Logo" className="logo" />
        </Link>
      </div>
      <nav>
        <ul className="nav-links">
          {user ? (
            <li>
              <a onClick={handleSignOut}>Sign Out</a>
            </li>
          ) : (
            <>
              {currentPath === "/login" && (
                <li>
                  <Link to="/signup">Sign Up</Link>
                </li>
              )}
              {currentPath === "/signup" && (
                <li>
                  <Link to="/login">Login</Link>
                </li>
              )}
              {currentPath !== "/login" && currentPath !== "/signup" && (
                <>
                  <li>
                    <Link to="/login">Login</Link>
                  </li>
                  <li>
                    <Link to="/signup">Sign Up</Link>
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
