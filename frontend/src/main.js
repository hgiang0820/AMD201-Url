import { createApp } from 'vue'
import App from './App.vue'

//Vue.config.productionTip = false;

new { createApp }({
  render: (h) => h(App), // Render the App.vue
}).$mount('#app'); // Mount the app to the #app div
