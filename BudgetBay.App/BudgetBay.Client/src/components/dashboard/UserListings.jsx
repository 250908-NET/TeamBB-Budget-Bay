import { Link } from 'react-router-dom';
import styles from '../../pages/DashboardPage/DashboardPage.module.css';

const UserListings = ({ listings }) => {
    return (
        <section className={styles.dashboardSection}>
            <div className={styles.sectionHeader}>
                <h2 className={styles.sectionTitle}>My Products For Sale</h2>
                <Link to="/products/create" className={styles.actionButton}>Add New Product</Link>
            </div>
            {listings.length > 0 ? (
                    <div className={styles.contentGrid}>
                        {listings.map(product => (
                            <div key={product.id} className={styles.card}>
                                <div>
                                    <Link to={`/products/${product.id}`}>
                                        <h3>{product.name}</h3>
                                    </Link>
                                    <p><strong>Current Price:</strong> ${product.currentPrice?.toFixed(2) ?? 'N/A'}</p>
                                    <p><strong>End Time:</strong> {new Date(product.endTime).toLocaleString()}</p>
                                </div>
                                <div className={styles.cardActions}>
                                    <Link to={`/products/edit/${product.id}`} className={styles.editButton}>
                                        Edit
                                    </Link>
                                </div>
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