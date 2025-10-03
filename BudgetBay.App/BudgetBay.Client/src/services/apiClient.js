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
            throw new Error('Login failed: ' + response.statusText);
        }
        
        return response;

    } catch (error) {
        throw error;
    }
};

