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
        // This effect correctly synchronizes the user state with the token state.
        if (token) {
            try {
                const decodedUser = jwtDecode(token);
                setUser(decodedUser);
            } catch (error) {
                console.error("Failed to decode token:", error);
                // Clear out invalid token from storage
                setToken(null); 
                setUser(null);
            }
        } else {
            setUser(null);
        }
        // setToken is guaranteed to be stable by the useLocalStorage hook (if implemented correctly)
        // or by useState, so it's safe to include.
    }, [token, setToken]);

    // useCallback ensures the login function reference doesn't change on every render,
    // preventing unnecessary re-renders in consumer components that only use this function.
    const login = useCallback(async (email, password) => {
        setLoading(true);
        try {
            const receivedToken = await loginRequest(email, password);
            setToken(receivedToken); // This will trigger the useEffect above
            setLoading(false);
            return true;
        }
        catch (e) {
            console.error(e);
            setLoading(false);
            return false;
        }
    }, [setToken]); // Dependency: setToken

    // useCallback for logout for the same reason.
    const logout = useCallback(() => {
        setToken(null); // This will also trigger the useEffect
    }, [setToken]); // Dependency: setToken

    // useMemo creates a stable context value object. It will only be recreated
    // if one of its dependencies (token, user, loading, login, logout) changes.
    // This prevents all consumers of the context from re-rendering just because
    // the AuthProvider parent re-rendered for an unrelated reason.
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