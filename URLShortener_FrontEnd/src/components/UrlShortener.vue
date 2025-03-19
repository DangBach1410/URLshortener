<template>
  <div class="container">
    <h1>URL Shortener</h1>
    <input v-model="originalUrl" placeholder="Enter URL here..." class="input" />
    <button @click="shortenUrl" class="btn">Shorten</button>
    <div v-if="shortUrl" class="result">
      <p>Shortened URL:</p>
      <a :href="shortUrl" target="_blank">{{ shortUrl }}</a>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      originalUrl: "",
      shortUrl: "",
    };
  },
  methods: {
    async shortenUrl() {
      try {
        const response = await axios.post("http://localhost:5168/shorten", 
        { 
          originalUrl: this.originalUrl  ,
          shortCode: ""
        });
        this.shortUrl = `http://localhost:5168/${response.data.shortCode}`;
      } catch (error) {
        alert("Error shortening URL:", error);
      }
    }
  }
};
</script>
