import { api } from './api';
import type { User, CreateUserRequest, UpdateUserRequest } from '../types/user';

export const userService = {
  getProfile: async (): Promise<User> => {
    const response = await api.get<User>('/users/profile');
    return response.data;
  },

  updateProfile: async (userData: UpdateUserRequest): Promise<User> => {
    const response = await api.put<User>('/users/profile', userData);
    return response.data;
  },

  getUserById: async (id: string): Promise<User> => {
    const response = await api.get<User>(`/users/${id}`);
    return response.data;
  },

  uploadAvatar: async (file: File): Promise<{ avatarUrl: string }> => {
    const formData = new FormData();
    formData.append('avatar', file);

    const response = await api.post<{ avatarUrl: string }>('/users/avatar', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  },

  deleteAccount: async (): Promise<void> => {
    await api.delete('/users/account');
  },

  updateSettings: async (settings: Record<string, any>): Promise<void> => {
    await api.put('/users/settings', settings);
  },

  getSettings: async (): Promise<Record<string, any>> => {
    const response = await api.get<Record<string, any>>('/users/settings');
    return response.data;
  },

  getUsersByIds: async (ids: string[]): Promise<User[]> => {
    const response = await api.post<User[]>('/users/batch', { ids });
    return response.data;
  },

  searchUsers: async (query: string): Promise<User[]> => {
    const response = await api.get<User[]>('/users/search', {
      params: { q: query },
    });
    return response.data;
  },
};