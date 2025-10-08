import { useContext, useState } from "react";
import "./CreateForm.css";
import { AuthContext } from "../../contexts/AuthContext";
import { BASE } from "../../services/apiClient";

const CreateForm = () => {
  const { token } = useContext(AuthContext);
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    image: "",
    condition: "",
    startTime: "",
    endTime: "",
    startingPrice: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

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
        alert("✅ Product created successfully!");
        setFormData({
          id: "",
          name: "",
          description: "",
          image: "",
          condition: "",
          startTime: "",
          endTime: "",
          startingPrice: "",
        });
      } else {
        alert("❌ Failed to create product");
      }
    } catch (error) {
      console.error("Error:", error);
    }
  };

  return (
    <form className="create-form" onSubmit={handleSubmit}>
      <h2>Create Product</h2>

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

      <label>Image</label>
      <input type="text" name="image" onChange={handleChange} />

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
        onChange={handleChange}
        required
      />

      <label>End Time</label>
      <input
        type="datetime-local"
        name="endTime"
        value={formData.endTime}
        onChange={handleChange}
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