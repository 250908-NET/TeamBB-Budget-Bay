const BASE = 'http://localhost:5192/api';

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
        
        return response;

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
            const errorData = await response.json().catch(() => ({ message: response.statusText }));
            throw new Error(errorData.message || 'Registration failed');
        }

        return true;

    } catch (error) {
        throw error;
    }
};