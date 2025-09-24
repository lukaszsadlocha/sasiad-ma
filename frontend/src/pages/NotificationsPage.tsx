import React from 'react';
import { useTranslation } from 'react-i18next';

const NotificationsPage: React.FC = () => {
  const { t } = useTranslation();

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-8 text-center">
        <h1 className="text-2xl font-bold text-gray-900 mb-4">{t('notifications.title')}</h1>
        <p className="text-gray-600 mb-6">
          {t('notifications.description')}
        </p>
        <div className="text-sm text-gray-500">
          🚧 {t('notifications.underDevelopment')}
        </div>
      </div>
    </div>
  );
};

export default NotificationsPage;
