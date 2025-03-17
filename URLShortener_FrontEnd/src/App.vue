<template>
  <UrlShortener />
</template>

<script>
import UrlShortener from "./components/UrlShortener.vue";

export default {
  components: {
    UrlShortener
  }
};
</script>
<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

html, body {
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #f1f1f1;
  font-family: Arial, sans-serif;
}

#app {
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
}

.container {
  width: 90vw;
  padding: 20px;
  background: #fff;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  text-align: center;
  justify-content: center;
  align-items: center;
}

h1 {
  color: #333;
  font-size: 24px;
  margin-bottom: 20px;
}

.input {
  width: 90%;
  flex-grow: 1;
  padding: 12px;
  margin-right: 10px; 
  border: 2px solid #ccc;
  border-radius: 5px;
  font-size: 16px;
  min-width: 200px;
}

.btn {
  margin-top: 12px;
  padding: 12px 20px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;
  transition: background 0.3s;
  min-width: 100px;
}

.btn:hover {
  background-color: #0056b3;
}

.result {
  margin-top: 20px;
  padding: 10px;
  background: #e9f5ff;
  border-radius: 5px;
}

.result p {
  font-size: 16px;
  font-weight: bold;
  color: #333333
}

.result a {
  color: #007bff;
  text-decoration: none;
  font-size: 16px;
  font-weight: bold;
}

.result a:hover {
  text-decoration: underline;
}
@media (max-width: 785px) {
  .input {
    width: 95%;
    margin-right: 0px; 
  }
}
@media (max-width: 480px) {
  .container {
    width: 100%;
    padding: 15px;
  }

  .input {
    font-size: 14px;
    padding: 10px;
  }

  .btn {
    font-size: 14px;
    padding: 10px 15px;
  }

  .result a {
    font-size: 14px;
  }
}
</style>