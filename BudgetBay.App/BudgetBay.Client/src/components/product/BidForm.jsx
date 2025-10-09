/*C:\Users\Husan\Projects\Revature\TeamBB-Budget-Bay\BudgetBay.App\BudgetBay.Client\src\components\product\BidForm.jsx*/
import styles from '../../pages/ProductDetailsPage/ProductDetailsPage.module.css';

const BidForm = ({ 
    product, 
    isAuctionActive, 
    error, 
    onSubmit,
    bidAmount,
    onBidChange,
    isBidding,
    isLoggedIn
}) => {

    return (
        <section className={styles.widget}>
            {isAuctionActive ? (
                <form className={styles.bidForm} onSubmit={onSubmit}>
                    <div className={styles.bidInputGroup}>
                        <span className={styles.currencySymbol}>$</span>
                        <input
                            type="number"
                            className={styles.bidInput}
                            value={bidAmount}
                            onChange={onBidChange}
                            placeholder={`Min bid $${(product.currentPrice + 0.01).toFixed(2)}`}
                            step="0.01"
                            min={product.currentPrice + 0.01}
                            required
                            disabled={!isLoggedIn || isBidding}
                        />
                    </div>
                    <button 
                        type="submit" 
                        className={styles.bidButton} 
                        disabled={isBidding || !isLoggedIn || !bidAmount}
                    >
                        {isBidding 
                            ? 'Placing Bid...' 
                            : isLoggedIn ? 'Place Bid' : 'Login to Bid'
                        }
                    </button>
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