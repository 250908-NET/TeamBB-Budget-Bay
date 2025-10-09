import React from 'react';
import styles from '../../pages/ProductDetailsPage/ProductDetailsPage.module.css';

const BidHistory = ({ bidsList }) => {
  return (
    <div className={styles.widget}>
      <h3 className={styles.bidTitle}>Bid History</h3>

      <div className={styles.bidListContainer}>
        {(!bidsList || bidsList.length === 0) ? (
          <p className={styles.noBids}>No bids yet.</p>
        ) : (
          <ul className={styles.bidList}>
            {bidsList.map((bid, index) => (
              <li key={index} className={styles.bidItem}>
                <span className={styles.bidUsername}>{bid.username}</span>
                <span className={styles.bidAmount}>${bid.amount.toFixed(2)}</span>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
};

export default BidHistory;