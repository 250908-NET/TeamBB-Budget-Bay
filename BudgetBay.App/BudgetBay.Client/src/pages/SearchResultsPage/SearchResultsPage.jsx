import { useEffect, useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import { getAllProducts } from '../../services/apiClient';
import styles from './SearchResultsPage.module.css';
import CatalogProduct from '../../components/catalogproduct/CatalogProduct';
import SearchBar from '../../components/common/SearchBar';

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
                const allProducts = await getAllProducts();
                console.log("Fetched products:", allProducts);
                setProducts(allProducts);

         }
            catch (error) {
                console.error("Error fetching products:", error);
            }
        }
        fetchProducts();


    }, [query]);

    return (
        <main className={styles.mainbackground}>
            <div className={styles.resultsContainer}>
                <SearchBar/>
                {/* ✅ Wait until Products is populated */}
                {Products.length > 0 ? (
                    <CatalogProduct Products={Products} name={query} />
                ) : (
                    <p>Loading products...</p>
                )}
            </div>
        </main>
    );
};

export default SearchResultsPage;