import React from 'react';
import { X, Bell, Users, Package, MessageCircle, AlertTriangle } from 'lucide-react';
import { useApiMutation } from '../../hooks/useApi';
import { notificationService } from '../../services/notificationService';
import type { Notification } from '../../types/notification';
import { formatters } from '../../utils/formatters';

interface NotificationCardProps {
  notification: Notification;
  onMarkAsRead?: () => void;
  onDelete?: () => void;
}

const NotificationCard: React.FC<NotificationCardProps> = ({
  notification,
  onMarkAsRead,
  onDelete,
}) => {
  const markAsReadMutation = useApiMutation(
    notificationService.markAsRead,
    {
      onSuccess: () => {
        onMarkAsRead?.();
      },
    }
  );

  const deleteMutation = useApiMutation(
    notificationService.delete,
    {
      onSuccess: () => {
        onDelete?.();
      },
    }
  );

  const handleMarkAsRead = (e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    if (!notification.isRead) {
      markAsReadMutation.mutate(notification.id);
    }
  };

  const handleDelete = (e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    deleteMutation.mutate(notification.id);
  };

  const getNotificationIcon = () => {
    switch (notification.type) {
      case 'BORROW_REQUEST':
        return <Package className="h-5 w-5" />;
      case 'BORROW_APPROVED':
        return <Package className="h-5 w-5 text-green-600" />;
      case 'BORROW_REJECTED':
        return <Package className="h-5 w-5 text-red-600" />;
      case 'ITEM_RETURNED':
        return <Package className="h-5 w-5 text-blue-600" />;
      case 'COMMUNITY_INVITE':
        return <Users className="h-5 w-5 text-purple-600" />;
      case 'MESSAGE':
        return <MessageCircle className="h-5 w-5 text-blue-600" />;
      case 'REMINDER':
        return <AlertTriangle className="h-5 w-5 text-yellow-600" />;
      default:
        return <Bell className="h-5 w-5" />;
    }
  };

  const getNotificationColor = () => {
    if (notification.isRead) {
      return 'bg-white';
    }
    switch (notification.priority) {
      case 'HIGH':
        return 'bg-red-50 border-l-4 border-l-red-400';
      case 'MEDIUM':
        return 'bg-yellow-50 border-l-4 border-l-yellow-400';
      default:
        return 'bg-blue-50 border-l-4 border-l-blue-400';
    }
  };

  return (
    <div
      className={`rounded-lg border border-gray-200 p-4 hover:shadow-sm transition-shadow ${getNotificationColor()}`}
      onClick={handleMarkAsRead}
    >
      <div className="flex items-start space-x-3">
        {/* Icon */}
        <div className="flex-shrink-0 mt-0.5">
          {getNotificationIcon()}
        </div>

        {/* Content */}
        <div className="flex-1 min-w-0">
          <div className="flex items-start justify-between">
            <div className="flex-1">
              <p className={`text-sm ${notification.isRead ? 'text-gray-700' : 'text-gray-900 font-medium'}`}>
                {notification.title}
              </p>
              {notification.message && (
                <p className="text-sm text-gray-600 mt-1">
                  {notification.message}
                </p>
              )}
              <p className="text-xs text-gray-400 mt-2">
                {formatters.timeAgo(notification.createdAt)}
              </p>
            </div>

            {/* Actions */}
            <div className="flex items-center space-x-1 ml-2">
              {!notification.isRead && (
                <button
                  onClick={handleMarkAsRead}
                  disabled={markAsReadMutation.isPending}
                  className="p-1 text-gray-400 hover:text-blue-600 rounded"
                  title="Mark as read"
                >
                  <div className="w-2 h-2 bg-blue-600 rounded-full"></div>
                </button>
              )}

              <button
                onClick={handleDelete}
                disabled={deleteMutation.isPending}
                className="p-1 text-gray-400 hover:text-red-600 rounded"
                title="Delete notification"
              >
                <X className="h-4 w-4" />
              </button>
            </div>
          </div>

          {/* Action Button */}
          {notification.actionUrl && (
            <div className="mt-3">
              <a
                href={notification.actionUrl}
                className="inline-flex items-center px-3 py-1 border border-gray-300 shadow-sm text-xs font-medium rounded text-gray-700 bg-white hover:bg-gray-50"
                onClick={(e) => e.stopPropagation()}
              >
                {notification.actionText || 'View'}
              </a>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default NotificationCard;