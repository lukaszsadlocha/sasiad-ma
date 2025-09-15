import { useContext, useCallback } from 'react';
import { useApiQuery, useApiMutation } from './useApi';
import { notificationService } from '../services/notificationService';
import { NotificationContext } from '../context/NotificationContext';
import type { Notification, CreateNotificationRequest } from '../types/notification';

export const useNotifications = () => {
  const context = useContext(NotificationContext);

  if (!context) {
    throw new Error('useNotifications must be used within a NotificationProvider');
  }

  return context;
};

export const useNotificationsList = () => {
  return useApiQuery(
    ['notifications'],
    notificationService.getAll,
    { refetchOnWindowFocus: true }
  );
};

export const useUnreadNotifications = () => {
  return useApiQuery(
    ['notifications', 'unread'],
    notificationService.getUnread,
    { refetchOnWindowFocus: true }
  );
};

export const useMarkNotificationAsRead = () => {
  return useApiMutation(
    notificationService.markAsRead,
    {
      invalidateQueries: ['notifications'],
      onSuccess: () => {
        // Optionally show toast notification
      }
    }
  );
};

export const useMarkAllNotificationsAsRead = () => {
  return useApiMutation(
    notificationService.markAllAsRead,
    {
      invalidateQueries: ['notifications'],
      onSuccess: () => {
        // Optionally show toast notification
      }
    }
  );
};

export const useDeleteNotification = () => {
  return useApiMutation(
    notificationService.delete,
    {
      invalidateQueries: ['notifications'],
      onSuccess: () => {
        // Optionally show toast notification
      }
    }
  );
};

export const useNotificationPermission = () => {
  const requestPermission = useCallback(async () => {
    if (!('Notification' in window)) {
      return 'not-supported';
    }

    if (Notification.permission === 'granted') {
      return 'granted';
    }

    if (Notification.permission !== 'denied') {
      const permission = await Notification.requestPermission();
      return permission;
    }

    return Notification.permission;
  }, []);

  const showNotification = useCallback((title: string, options?: NotificationOptions) => {
    if (Notification.permission === 'granted') {
      new Notification(title, {
        icon: '/favicon.ico',
        ...options,
      });
    }
  }, []);

  return {
    permission: Notification.permission,
    requestPermission,
    showNotification,
  };
};