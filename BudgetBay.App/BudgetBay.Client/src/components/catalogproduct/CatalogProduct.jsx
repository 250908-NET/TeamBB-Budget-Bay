import { use, useState } from "react";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { GetHighestBidByProductId } from "../../services/apiClient";
import CatalogItem from "../catalogitem/CatalogItem";
import styles from "./CatalogProduct.module.css";

const CatalogProduct = ({ Products, name, onHome }) => {
  const [FilteredProducts, setFilteredProducts] = useState([]);

  useEffect(() => {
    if(!name || name.trim() === ""){
      setFilteredProducts(Products);
    } else if (Products && name) {
      const filtered = Products.filter((product) => {
        return product.name.toLowerCase().includes(name.toLowerCase());
      });
      setFilteredProducts(filtered);
    }
  }, [Products, name]);

  return (
    <div className={styles.editContainer}>
      {onHome === false ? (
        <h3 className={styles.resultCount}>
          {FilteredProducts.length} Results
        </h3>
      ):(<></>)}
      
      { FilteredProducts.length > 0 ? (
        FilteredProducts.map((product) => (
          <CatalogItem key={product.id} Product={product} />
        ))
      ) : (
        <p>No products found</p>
      )}
    </div>
  );
};

export default CatalogProduct;
