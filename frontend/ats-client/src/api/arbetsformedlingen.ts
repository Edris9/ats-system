import apiClient from './apiClient';

export const searchJobs = async (query: string) => {
  const response = await apiClient.get('/Arbetsformedlingen/search', {
    params: { q: query },
  });
  return response.data;
};