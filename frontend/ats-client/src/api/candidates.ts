import apiClient from './apiClient';
import type { CreateCandidateDto } from '../types';

export const getCandidates = async () => {
  const response = await apiClient.get('/Candidates');
  return response.data;
};

export const createCandidate = async (dto: CreateCandidateDto) => {
  const response = await apiClient.post('/Candidates', dto);
  return response.data;
};

export const updateCandidateStatus = async (id: string, status: string) => {
  const response = await apiClient.patch(`/Candidates/${id}/status`, { status });
  return response.data;
};