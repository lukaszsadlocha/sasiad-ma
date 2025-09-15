// API Configuration
export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api';

// App Configuration
export const APP_NAME = 'Sąsiad Ma';
export const APP_DESCRIPTION = 'Neighborhood sharing platform';

// Storage Keys
export const STORAGE_KEYS = {
  AUTH_TOKEN: 'authToken',
  REFRESH_TOKEN: 'refreshToken',
  USER: 'user',
} as const;

// Routes
export const ROUTES = {
  HOME: '/',
  LOGIN: '/login',
  REGISTER: '/register',
  DASHBOARD: '/dashboard',
  COMMUNITIES: '/communities',
  ITEMS: '/items',
  PROFILE: '/profile',
  NOTIFICATIONS: '/notifications',
} as const;

// Item Categories
export const ITEM_CATEGORIES = [
  'Tools',
  'Electronics',
  'Books',
  'Sports',
  'Kitchen',
  'Garden',
  'Home & Living',
  'Baby & Kids',
  'Automotive',
  'Other',
] as const;

// Item Conditions
export const ITEM_CONDITIONS = [
  'New',
  'Like New',
  'Good',
  'Fair',
  'Poor',
] as const;

// User Roles
export const USER_ROLES = {
  USER: 'User',
  ADMIN: 'Admin',
} as const;

// Pagination
export const DEFAULT_PAGE_SIZE = 20;
export const MAX_PAGE_SIZE = 100;
