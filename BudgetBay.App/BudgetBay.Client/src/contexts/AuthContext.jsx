import React, { createContext, useState } from 'react';
import useLocalStorage from '../hooks/useLocalStorage';
import { loginRequest } from '../services/apiClient';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [loading, setLoading] = useState(false);
    const [token, setToken] = useLocalStorage(null);

    const login = (email, password) => {
        setLoading(true);
        try {
            const token = loginRequest(email, password);
            setToken(token);
        }
        catch (e) {
            console.error(e);
        }
        setLoading(false);
    }
    const logout = () => setToken(null);

    return (
        <AuthContext.Provider value={{ token, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};