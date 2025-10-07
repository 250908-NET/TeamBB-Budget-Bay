import styles from '../../pages/DashboardPage/DashboardPage.module.css';

const WonAuctions = ({ auctions }) => {
    return (
        <section className={styles.dashboardSection}>
            <h2 className={styles.sectionTitle}>Auctions Won</h2>
            {auctions.length > 0 ? (
                <div className={styles.contentGrid}>
                    {auctions.map((auction, index) => (
                        <div key={`${auction.productId}-${index}`} className={styles.card}>
                            <h3>Won Auction for Product ID: {auction.productId}</h3>
                            <p><strong>Winning Bid:</strong> ${auction.amount.toFixed(2)}</p>
                        </div>
                    ))}
                </div>
            ) : (
                <p className={styles.empty}>You have not won any auctions yet.</p>
            )}
        </section>
    );
};

export default WonAuctions;