import styles from './ProductEditPage.module.css';
import EditForm from '../../components/ProductEditForm/EditForm';

const ProductEditPage = () => {
    return (
        <div className={styles.productEditPage}>
            <h1>Edit Product</h1>
            <EditForm />
        </div>
    )
}

export default ProductEditPage;