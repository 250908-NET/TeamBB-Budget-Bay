import React from "react";
import "./Footer.css";
import { Link } from 'react-router-dom';
import logo from '../../assets/logo-row.png';

const Footer = () => {
  const currentYear = new Date().getFullYear();

  return (
    <footer className="footer">
      <div className="footer-content">
           <div className="footer-logo-section">
                <Link to="/">
                    <img src={logo} alt="BudgetBay Logo" className="logo" />
                </Link>
                <div className="copyright">
                    <p>&copy; {currentYear} Husankhuja Nizomkhujaev, Kush Gandhi, Ledya Bakloug, Matthew Schade, Matthew Sims, & Stephen Alvarez. All rights reserved.</p>
                </div>
            </div>
      </div>
    </footer>
  );
};

export default Footer;
