import { useContext, useState } from "react";
import "./CreateForm.css";
import { AuthContext } from "../../contexts/AuthContext";
import { BASE } from "../../services/apiClient";

const CreateForm = () => {
  const { token, user, logout } = useContext(AuthContext);
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    imageUrl: "",
    condition: "",
    startTime: "",
    endTime: "",
    sellerId: "",
    startingPrice: "0.01",
    currentPrice: "0.01",
  });
  const [message, setMessage] = useState("");
  const [messageType, setMessageType] = useState(""); 

  const handleChange = (e) => {
    const { name, value } = e.target;
    const updatedFormData = {
      ...formData,
      sellerId: user ? user.sub : "",
      [name]: value,
    };
    
    // If starting price changes, update current price to match
    if (name === "startingPrice") {
      updatedFormData.currentPrice = value;
    }
    
    setFormData(updatedFormData);
  };

  const handleDateTimeChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      sellerId: user ? user.sub : "",
      [name]: value,
    });
    // Close the datetime picker after selection
    e.target.blur();
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    // Clear any existing messages
    setMessage("");
    setMessageType("");

    try {
      const response = await fetch(`${BASE}/product`, {
        method: "POST",
        body: JSON.stringify(formData),
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.ok) {
        setMessage("✅ Product created successfully!");
        setMessageType("success");
        // Reset form after successful submission
        setFormData({
          name: "",
          description: "",
          imageUrl: "",
          condition: "",
          startTime: "",
          endTime: "",
          sellerId: user ? user.sub : "",
          startingPrice: "0.01",
          currentPrice: "0.01",
        });
        
        // Clear success message after 5 seconds
        setTimeout(() => {
          setMessage("");
          setMessageType("");
        }, 5000);
      } else {
        setMessage("❌ Failed to create product");
        setMessageType("error");
      }
    } catch (error) {
      console.error("Error:", error);
      if (error.message === 'AUTHENTICATION_EXPIRED') {
        setMessage("❌ Your session has expired. Please log in again.");
        setMessageType("error");
        logout(); // Clear the expired token
      } else {
        setMessage("❌ An error occurred while creating the product");
        setMessageType("error");
      }
    }
  };

  return (
    <form className="create-form" onSubmit={handleSubmit}>
      

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
        required
      />

      <label>Description</label>
      <textarea
        name="description"
        value={formData.description}
        onChange={handleChange}
        required
      />

      <label>ImageUrl</label>
      <input 
        type="text" 
        name="imageUrl" 
        value={formData.imageUrl}
        onChange={handleChange} 
      />

      <label>Condition</label>
      <select
        name="condition"
        value={formData.condition}
        onChange={handleChange}
        required
      >
        <option value="">Select Condition</option>
        <option value="New">New</option>
        <option value="Used">Used</option>
      </select>

      <label>Start Time</label>
      <input
        type="datetime-local"
        name="startTime"
        value={formData.startTime}
        onChange={handleDateTimeChange}
        required
      />

      <label>End Time</label>
      <input
        type="datetime-local"
        name="endTime"
        value={formData.endTime}
        onChange={handleDateTimeChange}
        required
      />

      <label>Starting Price</label>
      <input
        type="number"
        name="startingPrice"
        value={formData.startingPrice}
        onChange={handleChange}
        required
      />

      <button type="submit">Create Product</button>
    </form>
  );
};

export default CreateForm;
