import {use, useState} from 'react';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { GetHighestBidByProductId } from '../../services/apiClient';
import CatalogItem from '../catalogitem/CatalogItem';
import styles from './CatalogProduct.module.css';


const CatalogProduct = ({Products,name}) => {
    const [FilteredProducts, setFilteredProducts] = useState([]);

    useEffect(() => {
        if (Products && name) {
            const filtered = Products.filter(product => {
                return product.name.toLowerCase().includes(name.toLowerCase());
            });
            setFilteredProducts(filtered);
        }

    },[Products, name]);

    return(
        <div className={styles.editContainer}>
            <h3 className={styles.resultCount}>{FilteredProducts.length} Results</h3>
            {FilteredProducts.length > 0 ? (
                FilteredProducts.map((product) => {
                    return (
                        <CatalogItem Product={product} />
                    );
                })

            ) : 
            (
                <p>No products found</p>
            )}
        </div>
    )

}


export default CatalogProduct;