import React from 'react';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { Home, ArrowLeft } from 'lucide-react';

const NotFoundPage: React.FC = () => {
  const { t } = useTranslation();

  return (
    <div className="min-h-screen bg-gray-50 flex items-center justify-center px-4">
      <div className="max-w-md w-full text-center">
        <div className="mb-8">
          <div className="mx-auto h-24 w-24 bg-gray-100 rounded-full flex items-center justify-center mb-6">
            <span className="text-4xl">😵</span>
          </div>
          <h1 className="text-6xl font-bold text-gray-900 mb-4">404</h1>
          <h2 className="text-2xl font-semibold text-gray-700 mb-2">
            {t('errors.pageNotFound')}
          </h2>
          <p className="text-gray-600 mb-8">
            {t('errors.sorryMessage')}
          </p>
        </div>

        <div className="space-y-4">
          <Link
            to="/"
            className="inline-flex items-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 transition-colors"
          >
            <Home className="mr-2 h-5 w-5" />
{t('errors.goHome')}
          </Link>
          
          <div>
            <button
              onClick={() => window.history.back()}
              className="inline-flex items-center px-6 py-3 border border-gray-300 text-base font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 transition-colors"
            >
              <ArrowLeft className="mr-2 h-5 w-5" />
{t('errors.goBack')}
            </button>
          </div>
        </div>

        <div className="mt-8 text-sm text-gray-500">
          {t('errors.mistakeMessage')}{' '}
          <Link to="/contact" className="text-blue-600 hover:text-blue-500">
            {t('errors.contactUs')}
          </Link>
        </div>
      </div>
    </div>
  );
};

export default NotFoundPage;
