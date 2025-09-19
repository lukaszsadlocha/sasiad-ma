import { api } from './api';
import type { Notification, CreateNotificationRequest } from '../types/notification';

export const notificationService = {
  getAll: async (): Promise<Notification[]> => {
    return await api.get<Notification[]>('/notifications');
  },

  getUnread: async (): Promise<Notification[]> => {
    const response = await api.get<Notification[]>('/notifications/unread');
    return response.data;
  },

  getById: async (id: string): Promise<Notification> => {
    const response = await api.get<Notification>(`/notifications/${id}`);
    return response.data;
  },

  create: async (notificationData: CreateNotificationRequest): Promise<Notification> => {
    const response = await api.post<Notification>('/notifications', notificationData);
    return response.data;
  },

  markAsRead: async (id: string): Promise<void> => {
    await api.put(`/notifications/${id}/read`);
  },

  markAllAsRead: async (): Promise<void> => {
    await api.put('/notifications/mark-all-read');
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/notifications/${id}`);
  },

  deleteAll: async (): Promise<void> => {
    await api.delete('/notifications');
  },

  getSettings: async (): Promise<Record<string, boolean>> => {
    return await api.get<Record<string, boolean>>('/notifications/settings');
  },

  updateSettings: async (settings: Record<string, boolean>): Promise<void> => {
    await api.put('/notifications/settings', settings);
  },

  subscribe: async (subscription: PushSubscription): Promise<void> => {
    await api.post('/notifications/subscribe', { subscription });
  },

  unsubscribe: async (endpoint: string): Promise<void> => {
    await api.post('/notifications/unsubscribe', { endpoint });
  },

  testNotification: async (): Promise<void> => {
    await api.post('/notifications/test');
  },

  getUnreadCount: async (): Promise<{ count: number }> => {
    const response = await api.get<{ count: number }>('/notifications/unread-count');
    return response.data;
  },
};