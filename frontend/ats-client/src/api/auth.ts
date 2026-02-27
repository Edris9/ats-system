import apiClient from './apiClient';
import type { LoginDto, CreateUserDto } from '../types';

export const login = async (dto: LoginDto) => {
  const response = await apiClient.post('/Auth/login', dto);
  return response.data;
};

export const createUser = async (dto: CreateUserDto) => {
  const response = await apiClient.post('/Auth/create-user', dto);
  return response.data;
};