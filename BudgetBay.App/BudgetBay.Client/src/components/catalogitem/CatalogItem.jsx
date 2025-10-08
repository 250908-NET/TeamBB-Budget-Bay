import {useState} from 'react';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { GetHighestBidByProductId } from '../../services/apiClient';
import styles from './CatalogItem.module.css';


const CatalogItem = ({Product}) => {

    const navigate = useNavigate();
    
    

    return(
        <section className={styles.catalogContainer}>
            <div className={styles.elementImage}>
                <img className={styles.elementImage} src={Product.imageUrl} alt={Product.name} />
            </div>
            <div className={styles.elementData}>
                <a onClick={() => navigate(`/products/${Product.id}`)}>{Product.name}</a>
                <h4>Current Bid: ${Product.currentPrice}</h4>
                <div className={styles.DataDescription}>
                    <p className={styles.DataTitle}>Description:</p>
                    <p className={styles.DataDetail}>{Product.description}</p>
                </div>
            </div>
        </section>
    )
};

export default CatalogItem;