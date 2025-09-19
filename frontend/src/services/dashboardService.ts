import { api } from './api';

export interface DashboardStats {
  communitiesCount: number;
  itemsCount: number;
  activeBorrowsCount: number;
  unreadNotificationsCount: number;
}

export const dashboardService = {
  getStats: async (): Promise<DashboardStats> => {
    return await api.get<DashboardStats>('/dashboard/stats');
  },
};