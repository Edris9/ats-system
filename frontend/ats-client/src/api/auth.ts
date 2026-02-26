import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'http://localhost:5121/api',
});

apiClient.interceptors.request.use((config) => {
  const user = localStorage.getItem('user');
  if (user) {
    const { token } = JSON.parse(user);
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default apiClient;