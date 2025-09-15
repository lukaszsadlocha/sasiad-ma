import React from 'react';
import { useNotifications } from '../../hooks/useNotifications';
import { useApiMutation } from '../../hooks/useApi';
import { notificationService } from '../../services/notificationService';
import { CheckCheck, Trash2 } from 'lucide-react';
import LoadingSpinner from '../common/LoadingSpinner';
import NotificationCard from './NotificationCard';

const NotificationList: React.FC = () => {
  const { notifications, unreadCount, isLoading, error, markAsRead, removeNotification } = useNotifications();

  const markAllAsReadMutation = useApiMutation(
    notificationService.markAllAsRead,
    {
      onSuccess: () => {
        // The context will handle updating the state
      },
    }
  );

  const deleteAllMutation = useApiMutation(
    notificationService.deleteAll,
    {
      onSuccess: () => {
        // The context will handle updating the state
      },
    }
  );

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <LoadingSpinner />
      </div>
    );
  }

  if (error) {
    return (
      <div className="text-center py-12">
        <div className="text-red-600 mb-4">Failed to load notifications</div>
      </div>
    );
  }

  const handleMarkAllAsRead = () => {
    markAllAsReadMutation.mutate();
  };

  const handleDeleteAll = () => {
    if (window.confirm('Are you sure you want to delete all notifications? This action cannot be undone.')) {
      deleteAllMutation.mutate();
    }
  };

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Notifications</h1>
          <p className="text-gray-600">
            {notifications.length} notification{notifications.length !== 1 ? 's' : ''}
            {unreadCount > 0 && (
              <span className="ml-2 inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
                {unreadCount} unread
              </span>
            )}
          </p>
        </div>

        {notifications.length > 0 && (
          <div className="flex items-center space-x-3">
            {unreadCount > 0 && (
              <button
                onClick={handleMarkAllAsRead}
                disabled={markAllAsReadMutation.isPending}
                className="inline-flex items-center px-3 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50"
              >
                <CheckCheck className="h-4 w-4 mr-2" />
                {markAllAsReadMutation.isPending ? 'Marking...' : 'Mark all as read'}
              </button>
            )}

            <button
              onClick={handleDeleteAll}
              disabled={deleteAllMutation.isPending}
              className="inline-flex items-center px-3 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 disabled:opacity-50"
            >
              <Trash2 className="h-4 w-4 mr-2" />
              {deleteAllMutation.isPending ? 'Deleting...' : 'Delete all'}
            </button>
          </div>
        )}
      </div>

      {/* Notifications */}
      {notifications.length === 0 ? (
        <div className="text-center py-12">
          <div className="mx-auto w-24 h-24 bg-gray-100 rounded-full flex items-center justify-center mb-4">
            <CheckCheck className="h-12 w-12 text-gray-400" />
          </div>
          <h3 className="text-lg font-medium text-gray-900 mb-2">
            All caught up!
          </h3>
          <p className="text-gray-600">
            You don't have any notifications right now.
          </p>
        </div>
      ) : (
        <div className="space-y-4">
          {notifications.map((notification) => (
            <NotificationCard
              key={notification.id}
              notification={notification}
              onMarkAsRead={() => markAsRead(notification.id)}
              onDelete={() => removeNotification(notification.id)}
            />
          ))}
        </div>
      )}
    </div>
  );
};

export default NotificationList;