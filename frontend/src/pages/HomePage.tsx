import React from 'react';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useAuth } from '../hooks/useAuth';
import { ArrowRight, Users, Package, Heart } from 'lucide-react';

const HomePage: React.FC = () => {
  const { isAuthenticated } = useAuth();
  const { t } = useTranslation();

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
      {/* Hero Section */}
      <div className="relative overflow-hidden">
        <div className="container mx-auto px-4 py-16 sm:py-24">
          <div className="text-center">
            <h1 className="text-4xl sm:text-6xl font-extrabold text-gray-900 mb-6">
              {t('homepage.hero.title')}
              <span className="text-blue-600"> {t('homepage.hero.titleHighlight')}</span>
            </h1>
            <p className="text-xl text-gray-600 max-w-3xl mx-auto mb-8">
              {t('homepage.hero.subtitle')}
            </p>
            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              {isAuthenticated ? (
                <Link
                  to="/dashboard"
                  className="inline-flex items-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 transition-colors"
                >
                  {t('homepage.hero.goToDashboard')}
                  <ArrowRight className="ml-2 h-5 w-5" />
                </Link>
              ) : (
                <>
                  <Link
                    to="/register"
                    className="inline-flex items-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 transition-colors"
                  >
                    {t('homepage.hero.getStarted')}
                    <ArrowRight className="ml-2 h-5 w-5" />
                  </Link>
                  <Link
                    to="/login"
                    className="inline-flex items-center px-6 py-3 border border-blue-600 text-base font-medium rounded-md text-blue-600 bg-white hover:bg-blue-50 transition-colors"
                  >
                    {t('homepage.hero.signIn')}
                  </Link>
                </>
              )}
            </div>
          </div>
        </div>
      </div>

      {/* Features Section */}
      <div className="py-16 bg-white">
        <div className="container mx-auto px-4">
          <div className="text-center mb-12">
            <h2 className="text-3xl font-extrabold text-gray-900 mb-4">
              {t('homepage.howItWorks.title')}
            </h2>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              {t('homepage.howItWorks.subtitle')}
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            <div className="text-center">
              <div className="mx-auto h-16 w-16 bg-blue-100 rounded-full flex items-center justify-center mb-4">
                <Users className="h-8 w-8 text-blue-600" />
              </div>
              <h3 className="text-xl font-semibold text-gray-900 mb-2">
                {t('homepage.howItWorks.joinCommunity.title')}
              </h3>
              <p className="text-gray-600">
                {t('homepage.howItWorks.joinCommunity.description')}
              </p>
            </div>

            <div className="text-center">
              <div className="mx-auto h-16 w-16 bg-green-100 rounded-full flex items-center justify-center mb-4">
                <Package className="h-8 w-8 text-green-600" />
              </div>
              <h3 className="text-xl font-semibold text-gray-900 mb-2">
                {t('homepage.howItWorks.shareItems.title')}
              </h3>
              <p className="text-gray-600">
                {t('homepage.howItWorks.shareItems.description')}
              </p>
            </div>

            <div className="text-center">
              <div className="mx-auto h-16 w-16 bg-purple-100 rounded-full flex items-center justify-center mb-4">
                <Heart className="h-8 w-8 text-purple-600" />
              </div>
              <h3 className="text-xl font-semibold text-gray-900 mb-2">
                {t('homepage.howItWorks.buildTrust.title')}
              </h3>
              <p className="text-gray-600">
                {t('homepage.howItWorks.buildTrust.description')}
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Benefits Section */}
      <div className="py-16 bg-gray-50">
        <div className="container mx-auto px-4">
          <div className="text-center mb-12">
            <h2 className="text-3xl font-extrabold text-gray-900 mb-4">
              {t('homepage.benefits.title')}
            </h2>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              {t('homepage.benefits.subtitle')}
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            <div className="bg-white p-6 rounded-lg shadow-sm">
              <h3 className="text-lg font-semibold text-gray-900 mb-2">
                {t('homepage.benefits.saveMoney.title')}
              </h3>
              <p className="text-gray-600">
                {t('homepage.benefits.saveMoney.description')}
              </p>
            </div>

            <div className="bg-white p-6 rounded-lg shadow-sm">
              <h3 className="text-lg font-semibold text-gray-900 mb-2">
                {t('homepage.benefits.reduceWaste.title')}
              </h3>
              <p className="text-gray-600">
                {t('homepage.benefits.reduceWaste.description')}
              </p>
            </div>

            <div className="bg-white p-6 rounded-lg shadow-sm">
              <h3 className="text-lg font-semibold text-gray-900 mb-2">
                {t('homepage.benefits.meetNeighbors.title')}
              </h3>
              <p className="text-gray-600">
                {t('homepage.benefits.meetNeighbors.description')}
              </p>
            </div>

            <div className="bg-white p-6 rounded-lg shadow-sm">
              <h3 className="text-lg font-semibold text-gray-900 mb-2">
                {t('homepage.benefits.freeToUse.title')}
              </h3>
              <p className="text-gray-600">
                {t('homepage.benefits.freeToUse.description')}
              </p>
            </div>

            <div className="bg-white p-6 rounded-lg shadow-sm">
              <h3 className="text-lg font-semibold text-gray-900 mb-2">
                {t('homepage.benefits.easyCommunication.title')}
              </h3>
              <p className="text-gray-600">
                {t('homepage.benefits.easyCommunication.description')}
              </p>
            </div>

            <div className="bg-white p-6 rounded-lg shadow-sm">
              <h3 className="text-lg font-semibold text-gray-900 mb-2">
                {t('homepage.benefits.trustSafety.title')}
              </h3>
              <p className="text-gray-600">
                {t('homepage.benefits.trustSafety.description')}
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* CTA Section */}
      {!isAuthenticated && (
        <div className="py-16 bg-blue-600">
          <div className="container mx-auto px-4 text-center">
            <h2 className="text-3xl font-extrabold text-white mb-4">
              {t('homepage.cta.title')}
            </h2>
            <p className="text-xl text-blue-100 mb-8 max-w-2xl mx-auto">
              {t('homepage.cta.subtitle')}
            </p>
            <Link
              to="/register"
              className="inline-flex items-center px-8 py-3 border border-transparent text-base font-medium rounded-md text-blue-600 bg-white hover:bg-blue-50 transition-colors"
            >
              {t('homepage.cta.createAccount')}
              <ArrowRight className="ml-2 h-5 w-5" />
            </Link>
          </div>
        </div>
      )}
    </div>
  );
};

export default HomePage;
