import styles from "./ProductEditPage.module.css";
import EditForm from "../../components/ProductEditForm/EditForm";

const ProductEditPage = () => {
  return (
    <main>
      <div className={styles.productEditPage}>
        <h1>Edit Product</h1>
        <EditForm />
      </div>
    </main>
  );
};

export default ProductEditPage;
