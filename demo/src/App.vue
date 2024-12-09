<template>
  <div class="container">
    <div class="form-container">
      <h2>Enter URL</h2>
      <input type="text" v-model="urlInput" placeholder="Enter a long URL" />
      <div class="button-group">
        <button class="btn btn-short" @click="shortenUrl">Shorten</button>
        <button class="btn btn-clear" @click="clearInput">Clear</button>
      </div>
    </div>
    <div class="list-container">
      <h2>URL List</h2>
      <table>
        <thead>
          <tr>
            <th>#</th>
            <th>Original URL</th>
            <th>Short URL</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(url, index) in urlList" :key="index">
            <td>{{ index + 1 }}</td>
            <td>{{ url.original }}</td>
            <td>{{ url.shortened }}</td>
            <td>
              <button @click="deleteUrl(index)">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
      <div class="pagination">
        <button class="page-btn" v-for="page in totalPages" :key="page" @click="changePage(page)">
          {{ page }}
        </button>
      </div>
    </div>
  </div>
</template>

<script>
  export default {
    name: 'App',
    data() {
      return {
        urlInput: '',
        urlList: [],
        currentPage: 1,
        urlsPerPage: 5,
      };
    },
    computed: {
      totalPages() {
        return Math.ceil(this.urlList.length / this.urlsPerPage);
      },
      paginatedUrls() {
        const start = (this.currentPage - 1) * this.urlsPerPage;
        return this.urlList.slice(start, start + this.urlsPerPage);
      },
    },
    methods: {
      shortenUrl() {
        if (this.urlInput) {
          const shortened = `short.ly/${Math.random().toString(36).substring(7)}`;
          this.urlList.push({ original: this.urlInput, shortened: shortened });
          this.urlInput = ''; // Clear input field after shortening URL
        }
      },
      clearInput() {
        this.urlInput = ''; // Clear the input field
      },
      deleteUrl(index) {
        this.urlList.splice(index, 1);
      },
      changePage(page) {
        this.currentPage = page;
      },
    },
  };
</script>

<style>
  /* General Styles */
  body {
    font-family: Arial, sans-serif;
    margin: 0;
    padding: 0;
    background-color: #f8f9fa;
  }

  .container {
    display: flex;
    justify-content: space-around;
    align-items: flex-start;
    padding: 20px;
    gap: 20px;
  }

  h2 {
    margin-bottom: 10px;
    font-size: 18px;
    font-weight: bold;
    color: #333;
  }

  /* Form Container */
  .form-container {
    background-color: #fff;
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 20px;
    width: 300px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  }

  #urlInput {
    width: 100%;
    padding: 10px;
    margin-bottom: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
    font-size: 14px;
  }

  .button-group {
    display: flex;
    justify-content: space-between;
  }

  .btn {
    padding: 10px 20px;
    font-size: 14px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    color: white;
  }

  .btn-short {
    background-color: #198754;
  }

  .btn-clear {
    background-color: #6c757d;
  }

  .btn:hover {
    opacity: 0.9;
  }

  /* URL List Container */
  .list-container {
    background-color: #fff;
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 20px;
    width: 600px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  }

  table {
    width: 100%;
    border-collapse: collapse;
    margin-bottom: 15px;
  }

  thead th {
    text-align: left;
    border-bottom: 2px solid #ddd;
    padding: 10px;
    font-size: 14px;
  }

  tbody td {
    padding: 10px;
    border-bottom: 1px solid #ddd;
    font-size: 14px;
  }

  .pagination {
    display: flex;
    justify-content: center;
    gap: 5px;
  }

  .page-btn {
    background-color: #6c757d;
    color: white;
    padding: 8px 12px;
    border: none;
    border-radius: 5px;
    font-size: 14px;
    cursor: pointer;
  }

    .page-btn:hover {
      opacity: 0.9;
    }
</style>
