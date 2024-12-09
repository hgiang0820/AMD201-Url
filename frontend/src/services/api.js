import axios from "axios";

const apiClient = axios.create({
  baseURL: "https://localhost:8888", // Corrected to https to match gateway configuration
  headers: {
    "Content-Type": "application/json",
  },
});

// Shorten URL (Post request to /createUrl)
export const shortenUrl = async (originalUrl) => {
  try {
    // Make sure the request body is an object with 'originalUrl' key
    const response = await apiClient.post("/createUrl", { originalUrl });
    return response.data;
  } catch (error) {
    console.error("Error shortening URL:", error);
    throw error;
  }
};


// Fetch all URLs (Get request to /getUrl)
export const fetchUrls = async () => {
  try {
    const response = await apiClient.get("/getUrl"); // Correct path to match gateway
    return response.data; // Ensure it returns the full list of URLs
  } catch (error) {
    console.error("Error fetching URLs:", error);
    throw error;
  }
};

// Delete a URL by ID (Delete request to /deleteUrl/{id})
export const deleteUrl = async (id) => {
  try {
    await apiClient.delete(`/deleteUrl/${id}`); // Correct path to match gateway
  } catch (error) {
    console.error("Error deleting URL:", error);
    throw error;
  }
};

// Login (Post request to /login)
export const login = async (credentials) => {
  try {
    const response = await apiClient.post("/login", credentials); // Correct path to match gateway
    return response.data;
  } catch (error) {
    console.error("Error logging in:", error);
    throw error;
  }
};

// Register (Post request to /register)
export const register = async (userDetails) => {
  try {
    const response = await apiClient.post("/register", userDetails); // Correct path to match gateway
    return response.data;
  } catch (error) {
    console.error("Error registering:", error);
    throw error;
  }
};
