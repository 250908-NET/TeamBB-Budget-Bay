import { useContext, useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "./EditForm.css";
import { AuthContext } from "../../contexts/AuthContext";
import { getProductById, updateProduct } from "../../services/apiClient";


const EditForm = () => {
  const { token, user } = useContext(AuthContext);
  const { productId } = useParams();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    imageUrl: "",
    condition: "0",
    endTime: "",
    startingPrice: "",
    currentPrice: "",
  });
  const [message, setMessage] = useState("");
  const [messageType, setMessageType] = useState(""); 

  // Load existing product data
  useEffect(() => {
    const loadProduct = async () => {
      try {
        const product = await getProductById(productId);
        setFormData({
          name: product.name || "",
          description: product.description || "",
          imageUrl: product.imageUrl || "",
          condition: product.condition !== undefined ? product.condition.toString() : "0",
          endTime: product.endTime ? new Date(product.endTime).toISOString().slice(0, 16) : "",
          startingPrice: product.startingPrice ? product.startingPrice.toString() : "",
          currentPrice: product.currentPrice ? product.currentPrice.toString() : "",
        });
      } catch (error) {
        console.error("Error loading product:", error);
        setMessage("❌ Failed to load product data");
        setMessageType("error");
      } finally {
        setLoading(false);
      }
    };

    if (productId) {
      loadProduct();
    }
  }, [productId]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleDateTimeChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
    e.target.blur();
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    setMessage("");
    setMessageType("");

    try {
      // Prepare data according to UpdateProductDto
      const updateData = {
        name: formData.name || null,
        description: formData.description || null,
        imageUrl: formData.imageUrl || null,
        condition: parseInt(formData.condition) || 0, // Convert to enum value
        endTime: formData.endTime ? new Date(formData.endTime).toISOString() : null,
        startingPrice: formData.startingPrice ? parseFloat(formData.startingPrice) : null,
        currentPrice: formData.currentPrice ? parseFloat(formData.currentPrice) : null,
      };

      await updateProduct(productId, updateData, token);
      
      setMessage("✅ Product updated successfully!");
      setMessageType("success");
      
      setTimeout(() => {
        navigate(`/products/${productId}`);
      }, 2000);
      
    } catch (error) {
      console.error("Error:", error);
      setMessage("❌ Failed to update product");
      setMessageType("error");
    }
  };

  if (loading) {
    return <div className="loading">Loading product data...</div>;
  }

  return (
    <form className="Edit-form" onSubmit={handleSubmit}>
      {message && (
        <div className={`message ${messageType}`}>
          {message}
        </div>
      )}

      <label>Name</label>
      <input
        type="text"
        name="name"
        value={formData.name}
        onChange={handleChange}
      />

      <label>Description</label>
      <textarea
        name="description"
        value={formData.description}
        onChange={handleChange}
      />

      <label>Image URL</label>
      <input 
        type="url" 
        name="imageUrl" 
        value={formData.imageUrl}
        onChange={handleChange} 
        placeholder="https://example.com/image.jpg"
      />

      <label>Condition</label>
      <select
        name="condition"
        value={formData.condition}
        onChange={handleChange}
      >
        <option value="0">New</option>
        <option value="1">Used</option>
      </select>

      <label>End Time</label>
      <input
        type="datetime-local"
        name="endTime"
        value={formData.endTime}
        onChange={handleDateTimeChange}
      />

      <label>Starting Price</label>
      <input
        type="number"
        name="startingPrice"
        value={formData.startingPrice}
        onChange={handleChange}
        min="0.01"
        step="0.01"
        placeholder="0.01"
      />

      <label>Current Price</label>
      <input
        type="number"
        name="currentPrice"
        value={formData.currentPrice}
        onChange={handleChange}
        min="0.01"
        step="0.01"
        placeholder="0.01"
      />

      <button type="submit">Update Product</button>
    </form>
  );
};

export default EditForm;
