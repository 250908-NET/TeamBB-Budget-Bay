import styles from './ProductCreatePage.module.css';
import CreateForm from '../../components/ProductCreateForm/CreateForm';

const ProductCreatePage = () => {
    return (
        <main>
            <div className={styles.productCreatePage}>
                <h1>Product Create </h1>
                <CreateForm />
            </div>
        </main>
    );
};

export default ProductCreatePage;