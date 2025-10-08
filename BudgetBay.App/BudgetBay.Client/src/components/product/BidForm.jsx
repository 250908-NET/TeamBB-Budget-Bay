import { useState } from 'react';
import styles from '../../pages/ProductDetailsPage/ProductDetailsPage.module.css';

const BidForm = ({ product, isAuctionActive, error }) => {
    const [bidAmount, setBidAmount] = useState('');

    const handleBidChange = (e) => {
        setBidAmount(e.target.value);
    }

    const handleSubmit = (e) => {
        e.preventDefault();
        // Handle submission logic here
    };

    return (
        <section className={styles.widget}>
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
        </section>
    );
}

export default BidForm;