export interface Community {
  id: string;
  name: string;
  description: string;
  imageUrl?: string;
  inviteCode: string;
  isPublic: boolean;
  maxMembers?: number;
  memberCount: number;
  isActive: boolean;
  createdBy: string;
  createdByName: string;
  createdAt: string;
  updatedAt: string;
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
  inviteCode: string;
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
  inviteCode: string;
  isPublic: boolean;
  maxMembers?: number;
  memberCount: number;
  isActive: boolean;
  createdBy: string;
  createdByName: string;
  isUserMember: boolean;
  isUserAdmin: boolean;
  createdAt: string;
  members?: CommunityMember[];
}
