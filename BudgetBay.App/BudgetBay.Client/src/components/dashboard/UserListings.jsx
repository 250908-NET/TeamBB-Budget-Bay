import { Link } from 'react-router-dom';
import styles from '../../pages/DashboardPage/DashboardPage.module.css';

const UserListings = ({ listings }) => {
    return (
        <section className={styles.dashboardSection}>
            <div className={styles.sectionHeader}>
                <h2 className={styles.sectionTitle}>My Products For Sale</h2>
                <Link to="/add-product" className={styles.actionButton}>Add New Product</Link>
            </div>
            {listings.length > 0 ? (
                <div className={styles.contentGrid}>
                    {listings.map(product => (
                        <div key={product.id} className={styles.card}>
                            <h3>{product.name}</h3>
                            <p><strong>Current Price:</strong> ${product.currentPrice?.toFixed(2) ?? 'N/A'}</p>
                            <p><strong>End Time:</strong> {new Date(product.endTime).toLocaleString()}</p>
                        </div>
                    ))}
                </div>
            ) : (
                <p className={styles.empty}>You have not listed any products yet.</p>
            )}
        </section>
    );
};

export default UserListings;