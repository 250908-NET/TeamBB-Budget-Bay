import { useState, useCallback } from 'react';

const useLocalStorage = (key, initialValue) => {
    const [storedValue, setStoredValue] = useState(() => {
        if (typeof window === 'undefined') {
            return initialValue;
        }
        try {
            const item = window.localStorage.getItem(key);
            return item ? JSON.parse(item) : initialValue;
        } catch (error) {
            console.error(error);
            return initialValue;
        }
    });

    const setValue = useCallback((value) => {
        try {
            // By using the functional update form of useState's setter,
            // we can ensure `setValue` itself doesn't need `storedValue` in its
            // dependency array, making the setter function stable across renders.
            setStoredValue(prevValue => {
                const valueToStore = value instanceof Function ? value(prevValue) : value;
                
                if (typeof window !== 'undefined') {
                    // A small improvement: remove the item if the value is null/undefined
                    // instead of storing the string "null".
                    if (valueToStore === null || valueToStore === undefined) {
                        window.localStorage.removeItem(key);
                    } else {
                        window.localStorage.setItem(key, JSON.stringify(valueToStore));
                    }
                }
                
                return valueToStore;
            });
        } catch (error) {
            console.error(error);
        }
    }, [key]);

    return [storedValue, setValue];
};

export default useLocalStorage;