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
                
                // Check if token is expired
                const currentTime = Date.now() / 1000; // Convert to seconds
                if (decodedUser.exp && decodedUser.exp < currentTime) {
                    console.warn("Token is expired, clearing authentication");
                    setToken(null);
                    setUser(null);
                    return;
                }
                
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

    // Utility function to check if token is expired
    const isTokenExpired = useCallback((token) => {
        if (!token) return true;
        try {
            const decodedToken = jwtDecode(token);
            const currentTime = Date.now() / 1000;
            return decodedToken.exp && decodedToken.exp < currentTime;
        } catch (error) {
            return true;
        }
    }, []);

    const value = useMemo(() => ({
        token,
        user,
        loading,
        login,
        logout,
        isTokenExpired
    }), [token, user, loading, login, logout, isTokenExpired]);

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
};