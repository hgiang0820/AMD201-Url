<template>
  <div class="container">
    <h1 class="title">URL Shortener</h1>

    <div class="content">
      <!-- Enter URL Block -->
      <div class="about-box">
        <h3 class="section-title">Enter URL</h3>
        <input type="text"
               v-model="originalUrl"
               placeholder="Enter URL to shorten"
               class="input-box" />
        <div class="button-group">
          <button class="shorten-btn" @click="handleShortenUrl">Shorten</button>
          <button class="clear-btn" @click="clearInput">Clear</button>
        </div>
        <div v-if="shortUrl" class="shortened-url">
          <h4 class="shortened-title">Shortened URL:</h4>
          <a :href="shortUrl" target="_blank" class="shortened-link">{{ shortUrl }}</a>
        </div>

      </div>

      <!-- URL List -->
      <div class="url-list">
        <h3 class="section-title">URL List</h3>
        <table>
          <thead>
            <tr>
              <th>Original URL</th>
              <th>Short Code</th>
              <th>Created At</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="url in urlList" :key="url.id">
              <td>{{ url.originalUrl }}</td>
              <td>{{ url.shortCode }}</td>
              <td>{{ url.createdAt }}</td>
              <td>
                <button class="clear-btn" @click="handleDeleteUrl(url.id)">
                  Delete
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<style>
  /* Container */
  .container {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 20px;
    background-color: #f4f5fc;
    min-height: 100vh;
    font-family: Arial, sans-serif;
  }

  /* Title */
  .title {
    text-align: center;
    font-size: 2rem;
    color: #333548;
    margin-bottom: 20px;
  }

  /* Content Layout */
  .content {
    display: flex;
    justify-content: space-between;
    width: 100%;
    max-width: 1200px;
    gap: 20px;
  }

  /* Section Title */
  .section-title {
    color: #333548;
    font-size: 1.2rem; /* Slightly increased size for better readability */
    margin-bottom: 10px;
    border-bottom: 2px solid #333548;
    display: inline-block;
    padding-bottom: 5px;
  }

  /* About Box (Enter URL) */
  .about-box {
    flex: 1;
    background-color: white;
    color: #333548;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  }

  .input-box {
    width: 100%;
    padding: 12px 10px; /* Increased padding for better clickability */
    margin: 10px 0;
    border: 1px solid #ccc;
    border-radius: 4px;
    font-size: 1rem;
    box-sizing: border-box;
  }

  /* Button Group */
  .button-group {
    display: flex;
    gap: 10px;
    margin-top: 10px;
  }

  /* URL List (Right Side) */
  .url-list {
    flex: 2;
    background-color: white;
    color: #333548;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  }

  table {
    width: 100%;
    border-collapse: collapse;
    font-size: 0.9rem; /* Slightly smaller font size for table contents */
  }

    table th,
    table td {
      text-align: left;
      padding: 10px;
      border-bottom: 1px solid #ddd;
    }

    table th {
      background-color: #333548;
      color: #f4f5fc;
      font-weight: bold;
      font-size: 0.9rem; /* Made headers more prominent */
    }

    table td {
      font-size: 0.9rem; /* Table data slightly smaller than headers */
    }

  /* Buttons */
  .shorten-btn {
    background-color: #333548;
    color: white;
    border: none;
    padding: 10px 20px;
    cursor: pointer;
    border-radius: 4px;
    font-size: 0.9rem; /* Standardized font size */
  }

  .clear-btn {
    background-color: #f4f5fc;
    color: #333548;
    border: none;
    padding: 10px 20px;
    cursor: pointer;
    border-radius: 4px;
    font-size: 0.9rem; /* Standardized font size */
  }

  /* Shortened URL */
  .shortened-title {
    font-size: 1rem;
    color: #333548;
    margin-bottom: 5px;
  }

  .shortened-link {
    font-size: 0.9rem;
    color: #007bff;
    text-decoration: none;
  }
  /* General Navbar Styles */
  .navbar {
    background-color: #333; /* Dark background */
    color: white; /* White text */
    padding: 10px 20px; /* Spacing around the navbar */
    display: flex; /* Flexbox for layout */
    justify-content: space-between; /* Space between logo and links */
    align-items: center; /* Center items vertically */
    width: 100%;
  }

  /* Logo Styles */
  .navbar-logo {
    font-size: 20px; /* Larger font for the logo */
    font-weight: bold; /* Bold text */
    color: white; /* White text */
    text-decoration: none; /* Remove underline */
  }

  /* Link Styles */
  .navbar-links {
    list-style: none; /* Remove bullet points */
    display: flex; /* Horizontal layout for links */
    gap: 20px; /* Spacing between links */
    margin: 0; /* Remove default margin */
    padding: 0; /* Remove default padding */
    justify-content: flex-end;
  }

  .nav-link {
    color: white; /* White text */
    text-decoration: none; /* Remove underline */
    font-size: 16px; /* Font size */
    transition: color 0.3s ease; /* Smooth color transition */
  }

    /* Hover Effect for Links */
    .nav-link:hover {
      color: #00bcd4; /* Light blue on hover */
    }

</style>




<script>
  import { fetchUrls, shortenUrl, deleteUrl } from "../services/api";

  export default {
    data() {
      return {
        originalUrl: "",
        shortUrl: null,
        urlList: [],
      };
    },
    methods: {
      async handleShortenUrl() {
        if (!this.originalUrl) {
          alert("Please enter a valid URL");
          return;
        }
        try {
          console.log("Sending URL:", this.originalUrl);
          const data = await shortenUrl(this.originalUrl); // Adjusted for updated API
          this.shortUrl = data.shortUrl;
          this.fetchAllUrls(); // Refresh list
        } catch (error) {
          console.error("Error in handleShortenUrl:", error);
        }
      },
      async fetchAllUrls() {
        try {
          this.urlList = await fetchUrls(); // Adjusted for updated API
        } catch (error) {
          console.error("Error fetching URLs:", error);
        }
      },
      async handleDeleteUrl(id) {
        try {
          const confirmation = confirm("Do you really want to delete this URL?");
          if (!confirmation) return;
          await deleteUrl(id); // Adjusted for updated API
          this.fetchAllUrls(); // Refresh list
        } catch (error) {
          console.error("Error in handleDeleteUrl:", error);
        }
      },
      clearInput() {
        this.originalUrl = "";
        this.shortUrl = null;
      },
    },
    mounted() {
      this.fetchAllUrls(); // Fetch list on load
    },
  };
</script>


<style>
  /* Container */
  .container {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 20px;
    background-color: #f4f5fc;
    min-height: 100vh;
    font-family: Arial, sans-serif;
  }

  /* Title */
  .title {
    text-align: center;
    font-size: 2rem;
    color: #333548;
    margin-bottom: 20px;
  }

  /* Content Layout */
  .content {
    display: flex;
    justify-content: space-between;
    width: 100%;
    max-width: 1200px;
    gap: 20px;
  }

  /* Section Title */
  .section-title {
    color: #333548;
    font-size: 1.2rem; /* Slightly increased size for better readability */
    margin-bottom: 10px;
    border-bottom: 2px solid #333548;
    display: inline-block;
    padding-bottom: 5px;
  }

  /* About Box (Enter URL) */
  .about-box {
    flex: 1;
    background-color: white;
    color: #333548;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  }

  .input-box {
    width: 100%;
    padding: 12px 10px; /* Increased padding for better clickability */
    margin: 10px 0;
    border: 1px solid #ccc;
    border-radius: 4px;
    font-size: 1rem;
    box-sizing: border-box;
  }

  /* Button Group */
  .button-group {
    display: flex;
    gap: 10px;
    margin-top: 10px;
  }

  /* URL List (Right Side) */
  .url-list {
    flex: 2;
    background-color: white;
    color: #333548;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  }

  table {
    width: 100%;
    border-collapse: collapse;
    font-size: 0.9rem; /* Slightly smaller font size for table contents */
  }

    table th,
    table td {
      text-align: left;
      padding: 10px;
      border-bottom: 1px solid #ddd;
    }

    table th {
      background-color: #333548;
      color: #f4f5fc;
      font-weight: bold;
      font-size: 0.9rem; /* Made headers more prominent */
    }

    table td {
      font-size: 0.9rem; /* Table data slightly smaller than headers */
    }

  /* Buttons */
  .shorten-btn {
    background-color: #333548;
    color: white;
    border: none;
    padding: 10px 20px;
    cursor: pointer;
    border-radius: 4px;
    font-size: 0.9rem; /* Standardized font size */
  }

  .clear-btn {
    background-color: #f4f5fc;
    color: #333548;
    border: none;
    padding: 10px 20px;
    cursor: pointer;
    border-radius: 4px;
    font-size: 0.9rem; /* Standardized font size */
  }

  /* Shortened URL */
  .shortened-title {
    font-size: 1rem;
    color: #333548;
    margin-bottom: 5px;
  }

  .shortened-link {
    font-size: 0.9rem;
    color: #007bff;
    text-decoration: none;
  }
</style>
