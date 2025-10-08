import styles from './ProductCreatePage.module.css';
import CreateForm from '../../components/ProductCreateForm/CreateForm';

const ProductCreatePage = () => {
    return (
        <div className={styles.productCreatePage}>
            <h1>Product Create Page</h1>
            <h2>Add New Product</h2>
            <CreateForm />
        </div>
    );
};

export default ProductCreatePage;