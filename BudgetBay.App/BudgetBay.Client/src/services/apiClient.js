export const BASE = 'http://localhost:5192/api';

// Helper for GET requests
const get = async (endpoint) => {
    const response = await fetch(`${BASE}${endpoint}`, {
        method: 'GET'
    });
    if (!response.ok) {
        const errorData = await response.json().catch(() => ({ message: response.statusText }));
        throw new Error(errorData.message || 'Failed to fetch data');
    }
    return response.json();
};

// Helper for multipart/form-data POST requests (for file uploads)
const postMultipartWithAuth = async (endpoint, formData, token) => {
    const response = await fetch(`${BASE}${endpoint}`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
        },
        body: formData,
    });
    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || 'Upload failed');
    }
    return true; // Endpoint returns 200 OK on success
};

// Helper for authenticated GET requests
const getWithAuth = async (endpoint, token) => {
    const response = await fetch(`${BASE}${endpoint}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
        },
    });
    if (!response.ok) {
        if (response.status === 401) {
            throw new Error('AUTHENTICATION_EXPIRED');
        }
        const errorData = await response.json().catch(() => ({ message: response.statusText }));
        throw new Error(errorData.message || 'Failed to fetch data');
    }
    return response.json();
};
const getWithoutAuth = async (endpoint) => {
    const response = await fetch(`${BASE}${endpoint}`, {
        method: 'GET',
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
        if (response.status === 401) {
            throw new Error('AUTHENTICATION_EXPIRED');
        }
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



// --- Product Details Functions ---
export const getProductById =  (productId) => get(`/Product/${productId}`);
export const updateProduct = (productId, productData, token) => postOrPutWithAuth(`/Product/${productId}`, 'PUT', productData, token);
export const placeBid = (productId, bidData, token) => postOrPutWithAuth(`/Products/${productId}/bids`, 'POST', bidData, token);

// --- Dashboard Functions ---
export const getUserById = (userId, token) => getWithAuth(`/Users/${userId}`, token);
export const getUserAddress = (userId, token) => getWithAuth(`/Users/${userId}/address`, token);
export const getUserProducts = (userId, token) => getWithAuth(`/Users/${userId}/products`, token);
export const getUserBids = (userId, token) => getWithAuth(`/Users/${userId}/bids`, token);
export const getWonAuctions = (userId, token) => getWithAuth(`/Users/${userId}/won-auctions`, token);
export const updateUserAddress = (userId, addressData, token) => postOrPutWithAuth(`/Users/${userId}/address`, 'PUT', addressData, token);
export const createUserAddress = (userId, addressData, token) => postOrPutWithAuth(`/Users/${userId}/address`, 'POST', addressData, token);

// --- Profile Functions ---
export const uploadProfilePicture = (userId, file, token) => {
    const formData = new FormData();
    formData.append('File', file);
    formData.append('UserId', userId);
    return postMultipartWithAuth('/Profile/upload', formData, token);
};

// --- Catalog Functions ---
export const getAllProducts = () => getWithoutAuth(`/Product`);
export const GetHighestBidByProductId = (productId) => getWithAuth(`/Products/${productId}/bids/highest`);