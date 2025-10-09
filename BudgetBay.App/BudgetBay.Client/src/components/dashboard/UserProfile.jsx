import styles from '../../pages/DashboardPage/DashboardPage.module.css';

const UserProfile = ({ userInfo, onFileChange, isUploading, uploadError }) => {
    return (
        <section className={styles.dashboardSection}>
            <h2 className={styles.sectionTitle}>My Info</h2>
            <div className={`${styles.card} ${styles.profileCard}`}>
                
                <div className={styles.profileImageContainer}>
                    <input 
                        type="file"
                        id="profilePicInput"
                        style={{ display: 'none' }}
                        onChange={onFileChange}
                        accept="image/png, image/jpeg"
                        disabled={isUploading}
                    />
                    <label htmlFor="profilePicInput" className={styles.profileImageLabel}>
                        <img 
                            className={styles.profileImage}
                            src={userInfo?.profilePictureUrl || 'https://st3.depositphotos.com/6672868/13701/v/450/depositphotos_137014128-stock-illustration-user-profile-icon.jpg'} 
                            alt={`${userInfo?.username}'s profile`} 
                        />
                        <div className={styles.profileImageOverlay}>
                            <span>Change Photo</span>
                        </div>
                        {isUploading && (
                            <div className={styles.uploadingOverlay}>
                                <div className={styles.spinner}></div>
                            </div>
                        )}
                    </label>
                </div>

                <p><strong>Username:</strong> {userInfo?.username}</p>
                <p><strong>Email:</strong> {userInfo?.email}</p>
                {uploadError && <p className={`${styles.errorMessage} ${styles.uploadError}`}>{uploadError}</p>}
            </div>
        </section>
    );
};

export default UserProfile;