import React from 'react';
import './BidHistory.css';
const BidHistory = ({ bidsList }) => {
  return (
    <div className="bid-card">
      <h3 className="bid-title">Bid History</h3>

      <div className="bid-list-container">
        {(!bidsList || bidsList.length === 0) ? (
          <p className="no-bids">No bids yet.</p>
        ) : (
          <ul className="bid-list">
            {bidsList.map((bid, index) => (
              <li key={index} className="bid-item">
                <span className="bid-username">{bid.username}</span>
                <span className="bid-amount">${bid.amount.toFixed(2)}</span>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
};

export default BidHistory;
