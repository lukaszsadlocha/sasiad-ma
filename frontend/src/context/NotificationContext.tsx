import React, { createContext, useContext, useState, useEffect, ReactNode, useRef } from 'react';
import { useApiQuery } from '../hooks/useApi';
import { notificationService } from '../services/notificationService';
import type { Notification } from '../types/notification';

interface NotificationContextType {
  notifications: Notification[];
  unreadCount: number;
  isLoading: boolean;
  error: string | null;
  refreshNotifications: () => void;
  markAsRead: (id: string) => void;
  markAllAsRead: () => void;
  addNotification: (notification: Notification) => void;
  removeNotification: (id: string) => void;
  settings: Record<string, boolean>;
  updateSettings: (settings: Record<string, boolean>) => void;
}

const NotificationContext = createContext<NotificationContextType | undefined>(undefined);

interface NotificationProviderProps {
  children: ReactNode;
}

export const NotificationProvider: React.FC<NotificationProviderProps> = ({ children }) => {
  const [localNotifications, setLocalNotifications] = useState<Notification[]>([]);
  const [settings, setSettings] = useState<Record<string, boolean>>({});
  const originalSettingsRef = useRef<Record<string, boolean>>({});
  const lastFetchedSettingsRef = useRef<Record<string, boolean>>({});

  const {
    data: fetchedNotifications = [],
    isLoading,
    error,
    refetch: refreshNotifications,
  } = useApiQuery(
    ['notifications'],
    notificationService.getAll,
    { refetchOnWindowFocus: true }
  );

  const { data: fetchedSettings = {} } = useApiQuery(
    ['notification-settings'],
    notificationService.getSettings
  );

  // Use fetched notifications directly, only use local state for optimistic updates
  const notifications = localNotifications.length > 0 ? localNotifications : (fetchedNotifications || []);

  useEffect(() => {
    if (Object.keys(fetchedSettings).length > 0) {
      // Only update if settings have actually changed from last fetch
      const settingsChanged = Object.keys(fetchedSettings).some(key =>
        lastFetchedSettingsRef.current[key] !== fetchedSettings[key]
      ) || Object.keys(lastFetchedSettingsRef.current).length !== Object.keys(fetchedSettings).length;

      if (settingsChanged) {
        setSettings(fetchedSettings);
        originalSettingsRef.current = fetchedSettings;
        lastFetchedSettingsRef.current = fetchedSettings;
      }
    }
  }, [fetchedSettings]);

  const unreadCount = Array.isArray(notifications) ? notifications.filter(n => !n.isRead).length : 0;

  const markAsRead = (id: string) => {
    setLocalNotifications(prev => {
      const current = prev.length > 0 ? prev : (fetchedNotifications || []);
      return current.map(notification =>
        notification.id === id
          ? { ...notification, isRead: true }
          : notification
      );
    });
  };

  const markAllAsRead = () => {
    setLocalNotifications(prev => {
      const current = prev.length > 0 ? prev : (fetchedNotifications || []);
      return current.map(notification => ({ ...notification, isRead: true }));
    });
  };

  const addNotification = (notification: Notification) => {
    setLocalNotifications(prev => {
      const current = prev.length > 0 ? prev : (fetchedNotifications || []);
      return [notification, ...current];
    });
  };

  const removeNotification = (id: string) => {
    setLocalNotifications(prev => {
      const current = prev.length > 0 ? prev : (fetchedNotifications || []);
      return current.filter(n => n.id !== id);
    });
  };

  const updateSettings = async (newSettings: Record<string, boolean>) => {
    const previousSettings = settings;
    setSettings(newSettings);
    try {
      await notificationService.updateSettings(newSettings);
      originalSettingsRef.current = newSettings;
    } catch (error) {
      console.error('Failed to update notification settings:', error);
      // Revert on error
      setSettings(previousSettings);
    }
  };

  // Reset local notifications when refetching to sync with server
  const refreshNotificationsAndSync = () => {
    setLocalNotifications([]);
    refreshNotifications();
  };

  // Set up WebSocket or SSE for real-time notifications
  useEffect(() => {
    // This would connect to a WebSocket or SSE endpoint for real-time notifications
    // For now, we'll poll every 30 seconds
    const interval = setInterval(() => {
      refreshNotificationsAndSync();
    }, 30000);

    return () => clearInterval(interval);
  }, [refreshNotifications]);

  // Request browser notification permission
  useEffect(() => {
    if ('Notification' in window && Notification.permission === 'default') {
      Notification.requestPermission();
    }
  }, []);

  const value: NotificationContextType = {
    notifications,
    unreadCount,
    isLoading,
    error: error instanceof Error ? error.message : null,
    refreshNotifications: refreshNotificationsAndSync,
    markAsRead,
    markAllAsRead,
    addNotification,
    removeNotification,
    settings,
    updateSettings,
  };

  return (
    <NotificationContext.Provider value={value}>
      {children}
    </NotificationContext.Provider>
  );
};

export const useNotificationContext = () => {
  const context = useContext(NotificationContext);
  if (context === undefined) {
    throw new Error('useNotificationContext must be used within a NotificationProvider');
  }
  return context;
};