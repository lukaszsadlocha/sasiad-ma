import { api } from './api';
import type { BorrowRequest, CreateBorrowRequest, UpdateBorrowRequest } from '../types/borrow';

export const borrowService = {
  getAll: async (): Promise<BorrowRequest[]> => {
    const response = await api.get<BorrowRequest[]>('/borrows');
    return response.data;
  },

  getById: async (id: string): Promise<BorrowRequest> => {
    const response = await api.get<BorrowRequest>(`/borrows/${id}`);
    return response.data;
  },

  create: async (borrowData: CreateBorrowRequest): Promise<BorrowRequest> => {
    const response = await api.post<BorrowRequest>('/borrows', borrowData);
    return response.data;
  },

  update: async (id: string, borrowData: UpdateBorrowRequest): Promise<BorrowRequest> => {
    const response = await api.put<BorrowRequest>(`/borrows/${id}`, borrowData);
    return response.data;
  },

  approve: async (id: string): Promise<BorrowRequest> => {
    const response = await api.put<BorrowRequest>(`/borrows/${id}/approve`);
    return response.data;
  },

  reject: async (id: string, reason?: string): Promise<BorrowRequest> => {
    const response = await api.put<BorrowRequest>(`/borrows/${id}/reject`, { reason });
    return response.data;
  },

  markAsReturned: async (id: string): Promise<BorrowRequest> => {
    const response = await api.put<BorrowRequest>(`/borrows/${id}/return`);
    return response.data;
  },

  cancel: async (id: string): Promise<BorrowRequest> => {
    const response = await api.put<BorrowRequest>(`/borrows/${id}/cancel`);
    return response.data;
  },

  getMyBorrowRequests: async (): Promise<BorrowRequest[]> => {
    const response = await api.get<BorrowRequest[]>('/users/borrow-requests');
    return response.data;
  },

  getMyLendRequests: async (): Promise<BorrowRequest[]> => {
    const response = await api.get<BorrowRequest[]>('/users/lend-requests');
    return response.data;
  },

  getBorrowHistory: async (userId?: string): Promise<BorrowRequest[]> => {
    const url = userId ? `/users/${userId}/borrow-history` : '/users/borrow-history';
    const response = await api.get<BorrowRequest[]>(url);
    return response.data;
  },

  extendBorrow: async (id: string, newReturnDate: string): Promise<BorrowRequest> => {
    const response = await api.put<BorrowRequest>(`/borrows/${id}/extend`, {
      newReturnDate,
    });
    return response.data;
  },

  addRating: async (id: string, rating: number, comment?: string): Promise<void> => {
    await api.post(`/borrows/${id}/rating`, { rating, comment });
  },

  getOverdueItems: async (): Promise<BorrowRequest[]> => {
    const response = await api.get<BorrowRequest[]>('/borrows/overdue');
    return response.data;
  },

  sendReminder: async (id: string): Promise<void> => {
    await api.post(`/borrows/${id}/reminder`);
  },
};