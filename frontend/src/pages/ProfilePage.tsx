import React from 'react';
import { useTranslation } from 'react-i18next';

const ProfilePage: React.FC = () => {
  const { t } = useTranslation();

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-8 text-center">
        <h1 className="text-2xl font-bold text-gray-900 mb-4">{t('profile.title')}</h1>
        <p className="text-gray-600 mb-6">
          {t('profile.description')}
        </p>
        <div className="text-sm text-gray-500">
          🚧 {t('profile.underDevelopment')}
        </div>
      </div>
    </div>
  );
};

export default ProfilePage;
