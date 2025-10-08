import {useState} from 'react';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { GetHighestBidByProductId } from '../../services/apiClient';
import styles from './CatalogItem.module.css';


const CatalogItem = ({Product}) => {

    
    

    return(
        <section className={styles.catalogContainer}>
            <div className={styles.elementImage}>
                Catalog Component Placeholder
            </div>
            <div className={styles.elementData}>
                <h2>{Product.Name}</h2>
                <h4>Current Bid: {Product.CurrentBid}</h4>
                <div className={styles.DataDescription}>
                    <p className={styles.DataTitle}>Description:</p>
                    <p className={styles.DataDetial}>{Product.Description}</p>
                </div>
            </div>
        </section>
    )
};

export default CatalogItem;