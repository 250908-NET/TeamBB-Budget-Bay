import { useEffect } from 'react';
import { useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import { getAllProducts } from '../../services/apiClient';
import styles from './SearchResultsPage.module.css';
import CatalogItem from '../../components/catalogitem/CatalogItem';

const SearchResultsPage = () => {
    const [searchParams] = useSearchParams();
    const [Products, setProducts] = useState([]);
    const query = searchParams.get('q');

    useEffect(() => {
        if (query) {
            console.log("Search query parameter:", query);
        }
        async function fetchProducts() {
            try {
                await getAllProducts().then(data => {
                    setProducts(data);
                    console.log("Fetched products:", data);
                });

         }
            catch (error) {
                console.error("Error fetching products:", error);
            }
        }
        fetchProducts();


    }, [query]);

    return (
        <div className={styles.resultsContainer}>
            <h1>Search Results</h1>
            {query ? (
                <p>Showing results for: <strong>{query}</strong></p>
            ) : (
                <p>No query, will return all products by default.</p>
            )}
            <CatalogItem Product={Products[1]} />
        </div>
    );
};

export default SearchResultsPage;