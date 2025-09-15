export interface Notification {
  id: string;
  userId: string;
  type: NotificationType;
  title: string;
  message: string;
  data?: Record<string, any>;
  isRead: boolean;
  createdAt: string;
}

export interface CreateNotificationRequest {
  userId: string;
  type: NotificationType;
  title: string;
  message: string;
  data?: Record<string, any>;
}

export interface NotificationDto {
  id: string;
  type: NotificationType;
  title: string;
  message: string;
  data?: Record<string, any>;
  isRead: boolean;
  createdAt: string;
  relativeTime: string;
}

export enum NotificationType {
  BorrowRequestReceived = 'BorrowRequestReceived',
  BorrowRequestApproved = 'BorrowRequestApproved',
  BorrowRequestDeclined = 'BorrowRequestDeclined',
  ItemBorrowed = 'ItemBorrowed',
  ItemReturned = 'ItemReturned',
  ItemOverdue = 'ItemOverdue',
  NewItemAdded = 'NewItemAdded',
  CommunityInvite = 'CommunityInvite',
  CommunityJoined = 'CommunityJoined',
  System = 'System'
}

export interface NotificationSettings {
  emailNotifications: boolean;
  borrowRequestNotifications: boolean;
  itemNotifications: boolean;
  communityNotifications: boolean;
  overdueNotifications: boolean;
  systemNotifications: boolean;
}
