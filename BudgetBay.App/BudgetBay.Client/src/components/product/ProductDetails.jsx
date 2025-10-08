import React, { useState } from 'react';
import styles from '../../pages/ProductDetailsPage/ProductDetailsPage.module.css';

const formatDate = (dateString) => {
    return new Date(dateString).toLocaleString();
};

const ProductDetails = ({ product, isAuctionActive, onBidSubmit, error }) => {
    const [bidAmount, setBidAmount] = useState('');

    const handleBidChange = (e) => {
        setBidAmount(e.target.value);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onBidSubmit(bidAmount);
        setBidAmount('');
    };

    return (
        <section className={styles.productDetailsSection}>
            <div className={styles.detailsLayout}>
                <div className={styles.imageContainer}>
                    <img src={product.imageUrl || 'https://via.placeholder.com/500'} alt={product.name} />
                </div>

                <div className={styles.infoContainer}>
                    <h1 className={styles.productName}>{product.name}</h1>
                    <p className={styles.productDescription}>{product.description}</p>

                    <div className={styles.infoGrid}>
                        <div>
                            <span className={styles.infoLabel}>Starting Price</span>
                            <p className={styles.infoValue}>${product.startingPrice.toFixed(2)}</p>
                        </div>
                        <div>
                            <span className={styles.infoLabel}>Auction Start</span>
                            <p className={styles.infoValue}>{formatDate(product.startTime)}</p>
                        </div>
                        <div>
                            <span className={styles.infoLabel}>Auction End</span>
                            <p className={styles.infoValue}>{formatDate(product.endTime)}</p>
                        </div>
                    </div>

                    <div className={styles.auctionInfo}>
                        <div className={styles.priceContainer}>
                            <span className={styles.infoLabel}>Current Bid</span>
                            <p className={styles.currentPrice}>${product.currentPrice.toFixed(2)}</p>
                        </div>
                        {isAuctionActive && (
                            <div className={styles.timerContainer}>
                                <span className={styles.infoLabel}>Time Left</span>
                                {/* A real timer component would go here */}
                                <p className={styles.timer}>--:--:--</p>
                            </div>
                        )}
                    </div>

                    {isAuctionActive ? (
                        <form className={styles.bidForm} onSubmit={handleSubmit}>
                            <div className={styles.bidInputGroup}>
                                <span className={styles.currencySymbol}>$</span>
                                <input
                                    type="number"
                                    className={styles.bidInput}
                                    value={bidAmount}
                                    onChange={handleBidChange}
                                    placeholder={`Min bid $${(product.currentPrice + 0.01).toFixed(2)}`}
                                    step="0.01"
                                    min={product.currentPrice + 0.01}
                                    required
                                />
                            </div>
                            <button type="submit" className={styles.bidButton}>Place Bid</button>
                        </form>
                    ) : (
                        <div className={styles.auctionEndedMessage}>
                            This auction has ended.
                        </div>
                    )}
                     {error && <p className={styles.errorMessage}>{error}</p>}
                </div>
            </div>
        </section>
    );
};

export default ProductDetails;