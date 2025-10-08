export const BASE = 'http://localhost:5192/api';

// Helper for authenticated GET requests
const getWithAuth = async (endpoint, token) => {
    const response = await fetch(`${BASE}${endpoint}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
        },
    });
    if (!response.ok) {
        const errorData = await response.json().catch(() => ({ message: response.statusText }));
        throw new Error(errorData.message || 'Failed to fetch data');
    }
    return response.json();
};

// Helper for authenticated POST/PUT requests
const postOrPutWithAuth = async (endpoint, method, body, token) => {
    const response = await fetch(`${BASE}${endpoint}`, {
        method: method,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(body),
    });
    if (!response.ok) {
        if (response.status === 409) {
             throw new Error("User already has an address. Use update instead.");
        }
        const errorText = await response.text();
        throw new Error(errorText || 'Request failed');
    }
    if (response.status === 204 || response.status === 201) return true;
    return response.json();
};


export const loginRequest = async (email, password) => {
    try {
        const response = await fetch(BASE + '/Auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email, password }),
        });
        if (!response.ok) {
            const errorData = await response.json().catch(() => ({ message: response.statusText }));
            throw new Error(errorData.message || 'Login failed');
        }
        return await response.text();

    } catch (error) {
        throw error;
    }
};

export const registerRequest = async (username, email, password) => {
    try {
        const response = await fetch(BASE + '/Auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, email, password }),
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText || 'Registration failed');
        }

        return true;

    } catch (error) {
        throw error;
    }
};

// --- Dashboard Functions ---
export const getUserById = (userId, token) => getWithAuth(`/Users/${userId}`, token);
export const getUserAddress = (userId, token) => getWithAuth(`/Users/${userId}/address`, token);
export const getUserProducts = (userId, token) => getWithAuth(`/Users/${userId}/products`, token);
export const getUserBids = (userId, token) => getWithAuth(`/Users/${userId}/bids`, token);
export const getWonAuctions = (userId, token) => getWithAuth(`/Users/${userId}/won-auctions`, token);
export const updateUserAddress = (userId, addressData, token) => postOrPutWithAuth(`/Users/${userId}/address`, 'PUT', addressData, token);
export const createUserAddress = (userId, addressData, token) => postOrPutWithAuth(`/Users/${userId}/address`, 'POST', addressData, token);