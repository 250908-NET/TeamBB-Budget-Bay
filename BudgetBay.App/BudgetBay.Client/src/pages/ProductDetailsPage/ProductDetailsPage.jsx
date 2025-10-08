/*C:\Users\Husan\Projects\Revature\TeamBB-Budget-Bay\BudgetBay.App\BudgetBay.Client\src\pages\ProductDetailsPage\ProductDetailsPage.jsx*/
import React, { useState, useEffect, useCallback, useContext } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import { getProductById, placeBid } from '../../services/apiClient';
import { AuthContext } from '../../contexts/AuthContext';
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
    const [bidAmount, setBidAmount] = useState('');
    const [isBidding, setIsBidding] = useState(false);

    const { user, token } = useContext(AuthContext);
    const navigate = useNavigate();
    const location = useLocation();

    const fetchProduct = useCallback(async () => {
        try {
            setError('');
            setIsLoading(true);
            const data = await getProductById(productId);
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

    const handleBidSubmit = async (e) => {
        e.preventDefault();
        setBidError('');

        if (!user || !token) {
            navigate('/login', { state: { from: location } });
            return;
        }

        const bidValue = parseFloat(bidAmount);
        if (isNaN(bidValue) || bidValue <= product.currentPrice) {
            setBidError(`Your bid must be higher than $${product.currentPrice.toFixed(2)}.`);
            return;
        }

        setIsBidding(true);
        try {
            const bidData = {
                amount: bidValue,
                bidderId: user.sub,
            };
            
            await placeBid(productId, bidData, token);

            setBidAmount(''); // Clear input on success
            await fetchProduct(); // Refresh product data to show new bid
        } catch (err) {
            setBidError(err.message || 'Failed to place bid. Please try again.');
        } finally {
            setIsBidding(false);
        }
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
                    <ProductDetails product={product} />
                </div>
                <div className={styles.sidebarContent}>
                    <AuctionInfo product={product} isAuctionActive={isAuctionActive} />
                    <BidForm 
                        product={product} 
                        isAuctionActive={isAuctionActive} 
                        onSubmit={handleBidSubmit} 
                        error={bidError}
                        bidAmount={bidAmount}
                        onBidChange={(e) => setBidAmount(e.target.value)}
                        isBidding={isBidding}
                        isLoggedIn={!!user}
                    />
                    <BidHistory bidsList={product.bids || []} />
                </div>
            </div>
        </main>
    );
};

export default ProductDetailsPage;