export interface Item {
  id: string;
  name: string;
  description: string;
  category: ItemCategory;
  condition: ItemCondition;
  status: ItemStatus;
  imageUrls: string[];
  communityId: string;
  communityName: string;
  ownerId: string;
  ownerName: string;
  ownerProfileImageUrl?: string;
  isAvailable: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateItemRequest {
  name: string;
  description: string;
  category: ItemCategory;
  condition: ItemCondition;
  communityId: string;
  imageUrls?: string[];
}

export interface UpdateItemRequest {
  name?: string;
  description?: string;
  category?: ItemCategory;
  condition?: ItemCondition;
  status?: ItemStatus;
  imageUrls?: string[];
}

export interface ItemSearchRequest {
  query?: string;
  category?: ItemCategory;
  condition?: ItemCondition;
  status?: ItemStatus;
  communityId?: string;
  availableOnly?: boolean;
  page?: number;
  pageSize?: number;
}

export interface ItemDto {
  id: string;
  name: string;
  description: string;
  category: ItemCategory;
  condition: ItemCondition;
  status: ItemStatus;
  imageUrls: string[];
  communityId: string;
  communityName: string;
  ownerId: string;
  ownerName: string;
  ownerProfileImageUrl?: string;
  isAvailable: boolean;
  currentBorrowerId?: string;
  currentBorrowerName?: string;
  createdAt: string;
  updatedAt: string;
}

export enum ItemCategory {
  Tools = 'Tools',
  Electronics = 'Electronics',
  Sports = 'Sports',
  Garden = 'Garden',
  Kitchen = 'Kitchen',
  Books = 'Books',
  Toys = 'Toys',
  Furniture = 'Furniture',
  Clothing = 'Clothing',
  Other = 'Other'
}

export enum ItemCondition {
  New = 'New',
  Excellent = 'Excellent',
  Good = 'Good',
  Fair = 'Fair',
  Poor = 'Poor'
}

export enum ItemStatus {
  Available = 'Available',
  Borrowed = 'Borrowed',
  Maintenance = 'Maintenance',
  Unavailable = 'Unavailable'
}
