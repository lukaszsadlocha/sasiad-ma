import { api } from './api';
import type { Item, CreateItemRequest, UpdateItemRequest, ItemSearchRequest } from '../types/item';

export const itemService = {
  getAll: async (communityId?: string): Promise<Item[]> => {
    const url = communityId ? `/communities/${communityId}/items` : '/items';
    const response = await api.get<Item[]>(url);
    return response.data;
  },

  getById: async (id: string): Promise<Item> => {
    const response = await api.get<Item>(`/items/${id}`);
    return response.data;
  },

  create: async (itemData: CreateItemRequest): Promise<Item> => {
    const response = await api.post<Item>('/items', itemData);
    return response.data;
  },

  update: async (id: string, itemData: UpdateItemRequest): Promise<Item> => {
    const response = await api.put<Item>(`/items/${id}`, itemData);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/items/${id}`);
  },

  search: async (searchParams: ItemSearchRequest): Promise<Item[]> => {
    const response = await api.get<Item[]>('/items/search', {
      params: searchParams,
    });
    return response.data;
  },

  uploadImages: async (id: string, files: File[]): Promise<{ imageUrls: string[] }> => {
    const formData = new FormData();
    files.forEach(file => {
      formData.append('images', file);
    });

    const response = await api.post<{ imageUrls: string[] }>(`/items/${id}/images`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  },

  deleteImage: async (id: string, imageUrl: string): Promise<void> => {
    await api.delete(`/items/${id}/images`, {
      data: { imageUrl },
    });
  },

  getMyItems: async (): Promise<Item[]> => {
    const response = await api.get<Item[]>('/users/items');
    return response.data;
  },

  updateAvailability: async (id: string, available: boolean): Promise<Item> => {
    const response = await api.patch<Item>(`/items/${id}/availability`, { available });
    return response.data;
  },

  getCategories: async (): Promise<string[]> => {
    const response = await api.get<string[]>('/items/categories');
    return response.data;
  },

  getPopularItems: async (communityId?: string): Promise<Item[]> => {
    const url = communityId
      ? `/communities/${communityId}/items/popular`
      : '/items/popular';
    const response = await api.get<Item[]>(url);
    return response.data;
  },

  reportItem: async (id: string, reason: string): Promise<void> => {
    await api.post(`/items/${id}/report`, { reason });
  },
};