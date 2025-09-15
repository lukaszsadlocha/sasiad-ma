export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  profileImageUrl?: string;
  bio?: string;
  phoneNumber?: string;
  isEmailConfirmed: boolean;
  role: UserRole;
  reputation: ReputationScore;
  isActive: boolean;
  lastLoginAt?: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateUserRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phoneNumber?: string;
  bio?: string;
}

export interface UpdateUserRequest {
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
  bio?: string;
  profileImageUrl?: string;
}

export interface UserDto {
  id: string;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  profileImageUrl?: string;
  bio?: string;
  phoneNumber?: string;
  reputation: ReputationScore;
  role: UserRole;
  isActive: boolean;
  createdAt: string;
}

export enum UserRole {
  User = 'User',
  Admin = 'Admin'
}

export interface ReputationScore {
  totalBorrows: number;
  totalLends: number;
  successfulReturns: number;
  rating: number;
}
