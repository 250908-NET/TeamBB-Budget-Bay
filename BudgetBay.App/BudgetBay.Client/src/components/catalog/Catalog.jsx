import {useState} from 'react';
import {useNavigate} from 'react-router-dom';
import styles from './Catalog.module.css';

const Catalog = () => {

    return(
        <section className={styles.catalogContainer}>
            <div className={styles.elementImage}>
                Catalog Component Placeholder


            </div>
            <div className={styles.elementData}>
`               <h2>Product Name</h2>`
            </div>
        </section>
    )
};

export default Catalog;