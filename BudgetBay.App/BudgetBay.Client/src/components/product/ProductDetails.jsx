import React from 'react';
import styles from '../../pages/ProductDetailsPage/ProductDetailsPage.module.css';

const formatDate = (dateString) => {
    return new Date(dateString).toLocaleString();
};

const ProductDetails = ({ product }) => {
    return (
        <section className={styles.widget}>
            <div className={styles.detailsLayout}>
                <div className={styles.imageContainer}>
                    <img src={product.imageUrl || 'https://via.placeholder.com/500'} alt={product.name} />
                </div>

                <div className={styles.infoContainer}>
                    <h1 className={styles.productName}>{product.name}</h1>
                    <p className={styles.productDescription}>{product.description}</p>

                    <div className={styles.infoGrid}>
                        <div>
                            <span className={styles.infoLabel}>Auction Start</span>
                            <p className={styles.infoValue}>{formatDate(product.startTime)}</p>
                        </div>
                        <div>
                            <span className={styles.infoLabel}>Auction End</span>
                            <p className={styles.infoValue}>{formatDate(product.endTime)}</p>
                        </div>
                        <div>
                            <span className={styles.infoLabel}>Starting Price</span>
                            <p className={styles.infoValue}>${product.startingPrice.toFixed(2)}</p>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
};

export default ProductDetails;