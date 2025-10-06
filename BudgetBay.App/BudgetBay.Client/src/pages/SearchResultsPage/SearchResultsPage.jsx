import { useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import styles from './SearchResultsPage.module.css';

const SearchResultsPage = () => {
    const [searchParams] = useSearchParams();
    const query = searchParams.get('q');

    useEffect(() => {
        if (query) {
            console.log("Search query parameter:", query);
        }
    }, [query]);

    return (
        <div className={styles.resultsContainer}>
            <h1>Search Results</h1>
            {query ? (
                <p>Showing results for: <strong>{query}</strong></p>
            ) : (
                <p>No query, will return all products by default.</p>
            )}
        </div>
    );
};

export default SearchResultsPage;