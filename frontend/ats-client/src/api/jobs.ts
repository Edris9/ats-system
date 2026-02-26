import apiClient from './apiClient';
import type { CreateJobDto } from '../types';

export const getJobs = async () => {
  const response = await apiClient.get('/Jobs');
  return response.data;
};

export const createJob = async (dto: CreateJobDto) => {
  const response = await apiClient.post('/Jobs', dto);
  return response.data;
};

export const deleteJob = async (id: string) => {
  const response = await apiClient.delete(`/Jobs/${id}`);
  return response.data;
};