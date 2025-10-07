import styles from '../../pages/DashboardPage/DashboardPage.module.css';

const UserBids = ({ bids }) => {
    return (
        <section className={styles.dashboardSection}>
            <h2 className={styles.sectionTitle}>My Bids</h2>
            {bids.length > 0 ? (
                <div className={styles.contentGrid}>
                    {bids.map((bid, index) => (
                        <div key={`${bid.productId}-${index}`} className={styles.card}>
                            <h3>Bid on Product ID: {bid.productId}</h3>
                            <p><strong>Amount:</strong> ${bid.amount.toFixed(2)}</p>
                        </div>
                    ))}
                </div>
            ) : (
                <p className={styles.empty}>You have not placed any bids.</p>
            )}
        </section>
    );
};

export default UserBids;