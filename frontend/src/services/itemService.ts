import { api } from './api';
import type { Item, CreateItemRequest, UpdateItemRequest, ItemSearchRequest } from '../types/item';

export const itemService = {
  getAll: async (communityId?: string): Promise<Item[]> => {
    const url = communityId ? '/items?communityId=' + communityId : '/items';
    return await api.get<Item[]>(url);
  },

  getByCommunityId: async (communityId: string): Promise<Item[]> => {
    return await api.get<Item[]>(`/items?communityId=${communityId}`);
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
      params: {
        communityId: searchParams.communityId,
        searchTerm: searchParams.query,
        category: searchParams.category,
        status: searchParams.status,
        availableOnly: searchParams.availableOnly,
        pageNumber: searchParams.page || 1,
        pageSize: searchParams.pageSize || 20
      },
    });
  },

  getMyItems: async (): Promise<Item[]> => {
    return await api.get<Item[]>('/items');
  },

  updateAvailability: async (id: string, available: boolean): Promise<Item> => {
    return await api.patch<Item>(`/items/${id}/availability`, { available });
  },

  getCategories: async (): Promise<string[]> => {
    return await api.get<string[]>('/items/categories');
  },

  // Legacy methods for backward compatibility
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