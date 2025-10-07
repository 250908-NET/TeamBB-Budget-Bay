import styles from '../../pages/DashboardPage/DashboardPage.module.css';

const UserAddress = ({ 
    userInfo, 
    isEditing, 
    addressForm, 
    error, 
    onEdit, 
    onCancel, 
    onChange, 
    onSubmit 
}) => {
    // Renders the address form when in editing mode
    if (isEditing) {
        return (
            <section className={styles.dashboardSection}>
                <h2 className={styles.sectionTitle}>My Address</h2>
                <form onSubmit={onSubmit} className={styles.addressForm}>
                    <input type="text" name="streetNumber" value={addressForm.streetNumber || ''} onChange={onChange} placeholder="Street Number" required />
                    <input type="text" name="streetName" value={addressForm.streetName || ''} onChange={onChange} placeholder="Street Name" required />
                    <input type="text" name="aptNumber" value={addressForm.aptNumber || ''} onChange={onChange} placeholder="Apt/Suite (Optional)" />
                    <input type="text" name="city" value={addressForm.city || ''} onChange={onChange} placeholder="City" required />
                    <input type="text" name="state" value={addressForm.state || ''} onChange={onChange} placeholder="State" required />
                    <input type="text" name="zipCode" value={addressForm.zipCode || ''} onChange={onChange} placeholder="Zip Code" required />
                    {error && <p className={styles.errorMessage}>{error}</p>}
                    <div className={styles.formActions}>
                        <button type="button" onClick={onCancel} className={`${styles.actionButton} ${styles.cancelButton}`}>Cancel</button>
                        <button type="submit" className={styles.actionButton}>Save Address</button>
                    </div>
                </form>
            </section>
        );
    }

    // Renders the address display or add prompt
    return (
        <section className={styles.dashboardSection}>
            <h2 className={styles.sectionTitle}>My Address</h2>
            {userInfo?.address ? (
                <div className={styles.addressDisplay}>
                    <p>{userInfo.address.streetNumber} {userInfo.address.streetName} {userInfo.address.aptNumber || ''}</p>
                    <p>{userInfo.address.city}, {userInfo.address.state} {userInfo.address.zipCode}</p>
                    <button onClick={onEdit} className={styles.actionButton} style={{ marginTop: '15px' }}>Update Address</button>
                </div>
            ) : (
                <div>
                    <p className={styles.empty}>You have not added an address yet.</p>
                    <button onClick={onEdit} className={styles.actionButton}>Add Address</button>
                </div>
            )}
        </section>
    );
};

export default UserAddress;