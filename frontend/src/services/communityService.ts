import { api } from './api';
import type { Community, CreateCommunityRequest, UpdateCommunityRequest, JoinCommunityRequest } from '../types/community';
import type { User } from '../types/user';

export const communityService = {
  getAll: async (): Promise<Community[]> => {
    return await api.get<Community[]>('/communities');
  },

  getById: async (id: string): Promise<Community> => {
    return await api.get<Community>(`/communities/${id}`);
  },

  create: async (communityData: CreateCommunityRequest): Promise<Community> => {
    return await api.post<Community>('/communities', communityData);
  },

  update: async (id: string, communityData: UpdateCommunityRequest): Promise<Community> => {
    return await api.put<Community>(`/communities/${id}`, communityData);
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/communities/${id}`);
  },

  join: async (joinData: JoinCommunityRequest): Promise<void> => {
    await api.post('/communities/join', joinData);
  },

  leave: async (id: string): Promise<void> => {
    await api.post(`/communities/${id}/leave`);
  },

  getMembers: async (id: string): Promise<User[]> => {
    return await api.get<User[]>(`/communities/${id}/members`);
  },

  removeMember: async (communityId: string, userId: string): Promise<void> => {
    await api.delete(`/communities/${communityId}/members/${userId}`);
  },

  updateMemberRole: async (communityId: string, userId: string, role: string): Promise<void> => {
    await api.put(`/communities/${communityId}/members/${userId}`, { role });
  },

  generateInvitationCode: async (id: string): Promise<{ invitationCode: string }> => {
    return await api.post<{ invitationCode: string }>(`/communities/${id}/invitation-code`);
  },

  uploadImage: async (id: string, file: File): Promise<{ imageUrl: string }> => {
    const formData = new FormData();
    formData.append('image', file);

    return await api.post<{ imageUrl: string }>(`/communities/${id}/image`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
  },

  getUserCommunities: async (): Promise<Community[]> => {
    return await api.get<Community[]>('/users/communities');
  },

  search: async (query: string): Promise<Community[]> => {
    return await api.get<Community[]>('/communities/search', {
      params: { q: query },
    });
  },
};