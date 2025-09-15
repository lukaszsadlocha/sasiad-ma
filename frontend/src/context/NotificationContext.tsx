import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
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
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [settings, setSettings] = useState<Record<string, boolean>>({});

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

  useEffect(() => {
    setNotifications(fetchedNotifications);
  }, [fetchedNotifications]);

  useEffect(() => {
    setSettings(fetchedSettings);
  }, [fetchedSettings]);

  const unreadCount = notifications.filter(n => !n.isRead).length;

  const markAsRead = (id: string) => {
    setNotifications(prev =>
      prev.map(notification =>
        notification.id === id
          ? { ...notification, isRead: true }
          : notification
      )
    );
  };

  const markAllAsRead = () => {
    setNotifications(prev =>
      prev.map(notification => ({ ...notification, isRead: true }))
    );
  };

  const addNotification = (notification: Notification) => {
    setNotifications(prev => [notification, ...prev]);
  };

  const removeNotification = (id: string) => {
    setNotifications(prev => prev.filter(n => n.id !== id));
  };

  const updateSettings = async (newSettings: Record<string, boolean>) => {
    setSettings(newSettings);
    try {
      await notificationService.updateSettings(newSettings);
    } catch (error) {
      console.error('Failed to update notification settings:', error);
      // Revert on error
      setSettings(fetchedSettings);
    }
  };

  // Set up WebSocket or SSE for real-time notifications
  useEffect(() => {
    // This would connect to a WebSocket or SSE endpoint for real-time notifications
    // For now, we'll poll every 30 seconds
    const interval = setInterval(() => {
      refreshNotifications();
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
    refreshNotifications,
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