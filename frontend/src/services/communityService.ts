import { api } from './api';
import type { Community, CreateCommunityRequest, UpdateCommunityRequest, JoinCommunityRequest } from '../types/community';
import type { User } from '../types/user';

export const communityService = {
  getAll: async (): Promise<Community[]> => {
    const response = await api.get<Community[]>('/communities');
    return response.data;
  },

  getById: async (id: string): Promise<Community> => {
    const response = await api.get<Community>(`/communities/${id}`);
    return response.data;
  },

  create: async (communityData: CreateCommunityRequest): Promise<Community> => {
    const response = await api.post<Community>('/communities', communityData);
    return response.data;
  },

  update: async (id: string, communityData: UpdateCommunityRequest): Promise<Community> => {
    const response = await api.put<Community>(`/communities/${id}`, communityData);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/communities/${id}`);
  },

  join: async (joinData: JoinCommunityRequest): Promise<void> => {
    await api.post(`/communities/${joinData.communityId}/join`, {
      invitationCode: joinData.invitationCode,
    });
  },

  leave: async (id: string): Promise<void> => {
    await api.post(`/communities/${id}/leave`);
  },

  getMembers: async (id: string): Promise<User[]> => {
    const response = await api.get<User[]>(`/communities/${id}/members`);
    return response.data;
  },

  removeMember: async (communityId: string, userId: string): Promise<void> => {
    await api.delete(`/communities/${communityId}/members/${userId}`);
  },

  updateMemberRole: async (communityId: string, userId: string, role: string): Promise<void> => {
    await api.put(`/communities/${communityId}/members/${userId}`, { role });
  },

  generateInvitationCode: async (id: string): Promise<{ invitationCode: string }> => {
    const response = await api.post<{ invitationCode: string }>(`/communities/${id}/invitation-code`);
    return response.data;
  },

  uploadImage: async (id: string, file: File): Promise<{ imageUrl: string }> => {
    const formData = new FormData();
    formData.append('image', file);

    const response = await api.post<{ imageUrl: string }>(`/communities/${id}/image`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  },

  getUserCommunities: async (): Promise<Community[]> => {
    const response = await api.get<Community[]>('/users/communities');
    return response.data;
  },

  search: async (query: string): Promise<Community[]> => {
    const response = await api.get<Community[]>('/communities/search', {
      params: { q: query },
    });
    return response.data;
  },
};