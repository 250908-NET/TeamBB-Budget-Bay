import React, { useState, useEffect, useCallback } from 'react';
import { useParams } from 'react-router-dom';
import { getProductById } from '../../services/apiClient';
import ProductDetails from '../../components/product/ProductDetails';
import BidForm from '../../components/product/BidForm';
import AuctionInfo from '../../components/product/AuctionInfo';
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
            // Sort bids from highest to lowest before setting state
            if (data.bids) {
                data.bids.sort((a, b) => b.amount - a.amount);
            }
            console.log(data);
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
        // Add actual bid submission logic here
        // After successful bid, call fetchProduct() to refresh data
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
                    {/* Removed unused props onBidSubmit and error */}
                    <ProductDetails product={product} />
                </div>
                <div className={styles.sidebarContent}>
                    <AuctionInfo product={product} isAuctionActive={isAuctionActive} />
                    <BidForm 
                        product={product} 
                        isAuctionActive={isAuctionActive} 
                        onSubmit={handleBidSubmit} 
                        error={bidError}
                    />
                    {/* Pass the actual bids from the product object */}
                    <BidHistory bidsList={product.bids || [
                        {username: "bobby", amount: 1.20},
                        {username: "bobby", amount: 1.20},
                        {username: "bobby", amount: 1.20},
                        {username: "bobby", amount: 1.20},
                        {username: "bobby", amount: 1.20},
                        {username: "bobby", amount: 1.20},
                        {username: "bobby", amount: 1.20},
                        {username: "bobby", amount: 1.20},
                        {username: "bobby", amount: 1.20},
                    ]} />
                </div>
            </div>
        </main>
    );
};

export default ProductDetailsPage;