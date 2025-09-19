export interface Community {
  id: string;
  name: string;
  description: string;
  imageUrl?: string;
  invitationCode: string;
  address?: string;
  city?: string;
  postalCode?: string;
  latitude?: number;
  longitude?: number;
  isPublic: boolean;
  isActive: boolean;
  maxMembers: number;
  activeMembersCount: number;
  canAcceptNewMembers: boolean;
  createdAt: string;
  members: CommunityMember[];
}

export interface CreateCommunityRequest {
  name: string;
  description: string;
  isPublic: boolean;
  maxMembers?: number;
  imageUrl?: string;
}

export interface UpdateCommunityRequest {
  name?: string;
  description?: string;
  isPublic?: boolean;
  maxMembers?: number;
  imageUrl?: string;
}

export interface JoinCommunityRequest {
  invitationCode: string;
}

export interface CommunityMember {
  id: string;
  userId: string;
  userFirstName: string;
  userLastName: string;
  userEmail: string;
  userProfileImageUrl?: string;
  communityId: string;
  isAdmin: boolean;
  isActive: boolean;
  joinedAt: string;
}

export interface CommunityDto {
  id: string;
  name: string;
  description: string;
  imageUrl?: string;
  invitationCode: string;
  address?: string;
  city?: string;
  postalCode?: string;
  latitude?: number;
  longitude?: number;
  isPublic: boolean;
  isActive: boolean;
  maxMembers: number;
  activeMembersCount: number;
  canAcceptNewMembers: boolean;
  createdAt: string;
  members: CommunityMember[];
}
