<template>
  <div class="container">
    <h1>URL Shortener</h1>
    <input v-model="originalUrl" placeholder="Enter URL here..." class="input" />
    <button @click="shortenUrl" class="btn">Shorten</button>

    <div v-if="shortUrl" class="result">
      <p>Shortened URL:</p>
      <a :href="shortUrl" target="_blank">{{ shortUrl }}</a>
    </div>

    <div v-if="errorMessage" class="error">
      <p style="color: red; padding-top: 10px">{{ errorMessage }}</p>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      originalUrl: "",
      shortUrl: "",
      errorMessage: "", // Store error messages
    };
  },
  methods: {
    async shortenUrl() {
      this.shortUrl = ""; // Clear previous result
      this.errorMessage = ""; // Clear previous error message

      try {
        const response = await axios.post("http://localhost:5168/shorten", {
          originalUrl: this.originalUrl,
          shortCode: "",
        });

        this.shortUrl = `http://localhost:5168/${response.data.shortCode}`;
      } catch (error) {
        // Handle errors and display error messages
        if (error.response) {
          // Server responded with an error status (e.g., 400 Bad Request)
          this.errorMessage = error.response.data || "Invalid URL.";
        } else if (error.request) {
          // No response from server
          this.errorMessage = "No response from server. Please try again.";
        } else {
          // Other errors
          this.errorMessage = "An error occurred. Please try again.";
        }
      }
    },
  },
};
</script>