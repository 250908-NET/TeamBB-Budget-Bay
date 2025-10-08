import React, { useState, useEffect, useCallback } from 'react';
import { useParams } from 'react-router-dom';
import { getProductById } from '../../services/apiClient';
import ProductDetails from '../../components/product/ProductDetails';
import BidHistory from '../../components/product/BidHistory';
import styles from './ProductDetailsPage.module.css';

const ProductDetailsPage = () => {
    const { productId } = useParams();
    const [product, setProduct] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');
    const [bidError, setBidError] = useState('');

    const fetchProduct = useCallback(async () => {
        try {
            setError('');
            setIsLoading(true);
            const data = await getProductById(productId);
            setProduct(data);
        } catch (err) {
            setError(err.message || 'Failed to fetch product details.');
        } finally {
            setIsLoading(false);
        }
    }, [productId]);

    useEffect(() => {
        fetchProduct();
    }, [fetchProduct]);

    const handleBidSubmit = (bidAmount) => {
        console.log(`Bid placed for ${bidAmount} on product ${productId}`);
        setBidError(''); // clear previous errors
    };

    if (isLoading) {
        return <div className={styles.centeredMessage}>Loading product details...</div>;
    }

    if (error) {
        return <div className={styles.centeredMessage}>Error: {error}</div>;
    }

    if (!product) {
        return <div className={styles.centeredMessage}>Product not found.</div>;
    }

    const isAuctionActive = new Date(product.endTime) > new Date();

    return (
        <main className={styles.productDetailsContainer}>
            <div className={styles.layoutGrid}>
                <div className={styles.mainContent}>
                     <ProductDetails
                        product={product}
                        isAuctionActive={isAuctionActive}
                        onBidSubmit={handleBidSubmit}
                        error={bidError}
                    />
                </div>
                <aside className={styles.sidebarContent}>
                    <BidHistory bids={product.bids || []} />
                </aside>
            </div>
        </main>
    );
};

export default ProductDetailsPage;