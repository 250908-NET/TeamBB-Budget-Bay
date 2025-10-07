import styles from '../../pages/DashboardPage/DashboardPage.module.css';

const UserProfile = ({ userInfo }) => {
    return (
        <section className={styles.dashboardSection}>
            <h2 className={styles.sectionTitle}>My Info</h2>
            <div className={styles.card}>
                <p><strong>Username:</strong> {userInfo?.username}</p>
                <p><strong>Email:</strong> {userInfo?.email}</p>
            </div>
        </section>
    );
};

export default UserProfile;