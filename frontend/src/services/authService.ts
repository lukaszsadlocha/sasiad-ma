import { api } from './api';
import type { LoginRequest, RegisterRequest, LoginResponse, GoogleLoginRequest } from '../types/auth';

export const authService = {
  login: async (credentials: LoginRequest): Promise<LoginResponse> => {
    const response = await api.post<LoginResponse>('/auth/login', credentials);
    return response.data;
  },

  register: async (userData: RegisterRequest): Promise<LoginResponse> => {
    const response = await api.post<LoginResponse>('/auth/register', userData);
    return response.data;
  },

  googleLogin: async (googleData: GoogleLoginRequest): Promise<LoginResponse> => {
    const response = await api.post<LoginResponse>('/auth/google-login', googleData);
    return response.data;
  },

  refreshToken: async (): Promise<LoginResponse> => {
    const response = await api.post<LoginResponse>('/auth/refresh');
    return response.data;
  },

  logout: async (): Promise<void> => {
    await api.post('/auth/logout');
  },

  confirmEmail: async (token: string): Promise<void> => {
    await api.post('/auth/confirm-email', { token });
  },

  resendConfirmation: async (email: string): Promise<void> => {
    await api.post('/auth/resend-confirmation', { email });
  },

  forgotPassword: async (email: string): Promise<void> => {
    await api.post('/auth/forgot-password', { email });
  },

  resetPassword: async (token: string, password: string): Promise<void> => {
    await api.post('/auth/reset-password', { token, password });
  },

  changePassword: async (currentPassword: string, newPassword: string): Promise<void> => {
    await api.post('/auth/change-password', { currentPassword, newPassword });
  },
};