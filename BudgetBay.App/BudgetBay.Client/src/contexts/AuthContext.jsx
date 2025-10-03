import { createContext, useState } from 'react';
import useLocalStorage from '../hooks/useLocalStorage';
import { loginRequest } from '../services/apiClient';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [loading, setLoading] = useState(false);
    const [token, setToken] = useLocalStorage('authToken', null);

    const login = async (email, password) => {
        setLoading(true);
        try {
            const receivedToken = await loginRequest(email, password);
            setToken(receivedToken);
            setLoading(false);
            return true;
        }
        catch (e) {
            console.error(e);
            setLoading(false);
            return false;
        }
    }
    
    const logout = () => {
        setToken(null);
    }

    const value = { token, loading, login, logout };

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
};