import React from 'react';

const NotificationsPage: React.FC = () => {
  return (
    <div className="container mx-auto px-4 py-8">
      <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-8 text-center">
        <h1 className="text-2xl font-bold text-gray-900 mb-4">Notifications</h1>
        <p className="text-gray-600 mb-6">
          This page will show your notifications and alerts.
        </p>
        <div className="text-sm text-gray-500">
          🚧 Under development - Notification features coming soon!
        </div>
      </div>
    </div>
  );
};

export default NotificationsPage;
