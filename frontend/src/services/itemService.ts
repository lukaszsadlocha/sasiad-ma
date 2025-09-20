import { api } from './api';
import type { Item, CreateItemRequest, UpdateItemRequest, ItemSearchRequest } from '../types/item';

export const itemService = {
  getAll: async (communityId?: string): Promise<Item[]> => {
    const url = communityId ? `/communities/${communityId}/items` : '/items';
    return await api.get<Item[]>(url);
  },

  getById: async (id: string): Promise<Item> => {
    return await api.get<Item>(`/items/${id}`);
  },

  create: async (itemData: CreateItemRequest): Promise<Item> => {
    return await api.post<Item>('/items', itemData);
  },

  update: async (id: string, itemData: UpdateItemRequest): Promise<Item> => {
    return await api.put<Item>(`/items/${id}`, itemData);
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/items/${id}`);
  },

  search: async (searchParams: ItemSearchRequest): Promise<Item[]> => {
    return await api.get<Item[]>('/items/search', {
      params: searchParams,
    });
  },

  uploadImages: async (id: string, files: File[]): Promise<{ imageUrls: string[] }> => {
    const formData = new FormData();
    files.forEach(file => {
      formData.append('images', file);
    });

    return await api.post<{ imageUrls: string[] }>(`/items/${id}/images`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
  },

  deleteImage: async (id: string, imageUrl: string): Promise<void> => {
    await api.delete(`/items/${id}/images`, {
      data: { imageUrl },
    });
  },

  getMyItems: async (): Promise<Item[]> => {
    return await api.get<Item[]>('/users/items');
  },

  updateAvailability: async (id: string, available: boolean): Promise<Item> => {
    return await api.patch<Item>(`/items/${id}/availability`, { available });
  },

  getCategories: async (): Promise<string[]> => {
    return await api.get<string[]>('/items/categories');
  },

  getPopularItems: async (communityId?: string): Promise<Item[]> => {
    const url = communityId
      ? `/communities/${communityId}/items/popular`
      : '/items/popular';
    return await api.get<Item[]>(url);
  },

  reportItem: async (id: string, reason: string): Promise<void> => {
    await api.post(`/items/${id}/report`, { reason });
  },
};