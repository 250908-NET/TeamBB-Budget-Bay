import styles from '../../pages/ProductDetailsPage/ProductDetailsPage.module.css';

const AuctionInfo = ({ product, isAuctionActive }) => {
    return (
        <section className={styles.widget}>
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
        </section>
    );
}

export default AuctionInfo;