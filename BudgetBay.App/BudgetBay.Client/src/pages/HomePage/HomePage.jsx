import { useContext, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { AuthContext } from "../../contexts/AuthContext";
import styles from "./HomePage.module.css";
import SearchBar from "../../components/common/SearchBar";
import CatalogProduct from "../../components/catalogproduct/CatalogProduct";
import { getAllProducts } from "../../services/apiClient";

const HomePage = () => {
  const { token, logout } = useContext(AuthContext);
  const [ products, setProducts] = useState([]);

useEffect(() => {
  const fetchProducts = async () => {
    const data = await getAllProducts();
    setProducts(data);
  };
  fetchProducts();
}, []);

  return (
    <div className={styles.homepageContainer}>
        <div className={styles.headerBlock}>
            
        </div>
      {token ? (
        <div>
          <h1>Welcome Back!</h1>
          <p>You are logged in.</p>
        </div>
      ) : (
        <div>
          <h1>Welcome to BudgetBay</h1>
          <p>Please log in to manage your budget.</p>
        </div>
      )}
      <div className={styles.searchBarContainer}>
        <SearchBar />
      </div>
      <div className={styles.productListContainer}>
        <CatalogProduct onHome={true} Products={products}/>
      </div>
    </div>
  );
};

export default HomePage;
