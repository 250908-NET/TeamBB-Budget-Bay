import { createContext, useState, useEffect, useMemo, useCallback } from 'react';
import useLocalStorage from '../hooks/useLocalStorage';
import { loginRequest } from '../services/apiClient';
import { jwtDecode } from 'jwt-decode';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [loading, setLoading] = useState(false);
    const [token, setToken] = useLocalStorage('authToken', null);
    const [user, setUser] = useState(null);

    useEffect(() => {
        if (token) {
            try {
                const decodedUser = jwtDecode(token);
                setUser(decodedUser);
            } catch (error) {
                console.error("Failed to decode token:", error);
                setToken(null); 
                setUser(null);
            }
        } else {
            setUser(null);
        }
    }, [token, setToken]);


    const login = useCallback(async (email, password) => {
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
    }, [setToken]);

    const logout = useCallback(() => {
        setToken(null); // This will also trigger the useEffect
    }, [setToken]); // Dependency: setToken

    const value = useMemo(() => ({
        token,
        user,
        loading,
        login,
        logout
    }), [token, user, loading, login, logout]);

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
};