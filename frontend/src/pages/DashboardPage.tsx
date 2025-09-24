import React from 'react';
import { useAuth } from '../hooks/useAuth';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { Users, Package, Bell, Plus, TrendingUp } from 'lucide-react';
import { useApiQuery } from '../hooks/useApi';
import { dashboardService } from '../services/dashboardService';
import LoadingSpinner from '../components/common/LoadingSpinner';

const DashboardPage: React.FC = () => {
  const { user } = useAuth();
  const { t } = useTranslation();

  const {
    data: dashboardStats,
    isLoading: statsLoading,
    error: statsError
  } = useApiQuery(
    ['dashboard-stats'],
    dashboardService.getStats,
    { refetchOnWindowFocus: true }
  );

  const stats = [
    {
      name: t('dashboard.stats.communities'),
      value: dashboardStats?.communitiesCount?.toString() || '0',
      icon: Users,
      color: 'bg-blue-500',
      href: '/communities'
    },
    {
      name: t('dashboard.stats.yourItems'),
      value: dashboardStats?.itemsCount?.toString() || '0',
      icon: Package,
      color: 'bg-green-500',
      href: '/items'
    },
    {
      name: t('dashboard.stats.activeBorrows'),
      value: dashboardStats?.activeBorrowsCount?.toString() || '0',
      icon: TrendingUp,
      color: 'bg-purple-500',
      href: '/items'
    },
    {
      name: t('dashboard.stats.notifications'),
      value: dashboardStats?.unreadNotificationsCount?.toString() || '0',
      icon: Bell,
      color: 'bg-yellow-500',
      href: '/notifications'
    }
  ];

  const quickActions = [
    {
      name: t('dashboard.quickActions.addNewItem.title'),
      description: t('dashboard.quickActions.addNewItem.description'),
      icon: Package,
      href: '/items/new',
      color: 'bg-blue-500'
    },
    {
      name: t('dashboard.quickActions.joinCommunity.title'),
      description: t('dashboard.quickActions.joinCommunity.description'),
      icon: Users,
      href: '/communities/join',
      color: 'bg-green-500'
    },
    {
      name: t('dashboard.quickActions.browseItems.title'),
      description: t('dashboard.quickActions.browseItems.description'),
      icon: TrendingUp,
      href: '/items',
      color: 'bg-purple-500'
    }
  ];

  if (statsLoading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  if (statsError) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <h2 className="text-xl font-semibold text-gray-900 mb-2">
            {t('dashboard.errors.loadingTitle')}
          </h2>
          <p className="text-gray-600">
            {t('dashboard.errors.loadingMessage')}
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="container mx-auto px-4 py-8">
        {/* Welcome Header */}
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900">
            {t('dashboard.welcome', { name: user?.firstName })}
          </h1>
          <p className="text-gray-600 mt-2">
            {t('dashboard.subtitle')}
          </p>
        </div>

        {/* Stats Overview */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
          {stats.map((stat) => (
            <Link
              key={stat.name}
              to={stat.href}
              className="bg-white p-6 rounded-lg shadow-sm hover:shadow-md transition-shadow border border-gray-200"
            >
              <div className="flex items-center">
                <div className={`p-3 rounded-md ${stat.color}`}>
                  <stat.icon className="h-6 w-6 text-white" />
                </div>
                <div className="ml-4">
                  <p className="text-sm font-medium text-gray-600">{stat.name}</p>
                  <p className="text-2xl font-bold text-gray-900">{stat.value}</p>
                </div>
              </div>
            </Link>
          ))}
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Quick Actions */}
          <div className="lg:col-span-2">
            <div className="bg-white rounded-lg shadow-sm border border-gray-200">
              <div className="px-6 py-4 border-b border-gray-200">
                <h2 className="text-lg font-semibold text-gray-900">{t('dashboard.quickActions.title')}</h2>
              </div>
              <div className="p-6">
                <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                  {quickActions.map((action) => (
                    <Link
                      key={action.name}
                      to={action.href}
                      className="p-4 border border-gray-200 rounded-lg hover:border-blue-300 hover:shadow-sm transition-all"
                    >
                      <div className={`w-10 h-10 rounded-lg ${action.color} flex items-center justify-center mb-3`}>
                        <action.icon className="h-5 w-5 text-white" />
                      </div>
                      <h3 className="font-semibold text-gray-900 mb-1">{action.name}</h3>
                      <p className="text-sm text-gray-600">{action.description}</p>
                    </Link>
                  ))}
                </div>
              </div>
            </div>
          </div>

          {/* Recent Activity */}
          <div className="lg:col-span-1">
            <div className="bg-white rounded-lg shadow-sm border border-gray-200">
              <div className="px-6 py-4 border-b border-gray-200">
                <h2 className="text-lg font-semibold text-gray-900">{t('dashboard.recentActivity.title')}</h2>
              </div>
              <div className="p-6">
                <div className="space-y-4">
                  <div className="flex items-start space-x-3">
                    <div className="w-8 h-8 bg-green-100 rounded-full flex items-center justify-center">
                      <Package className="h-4 w-4 text-green-600" />
                    </div>
                    <div>
                      <p className="text-sm text-gray-900">
                        {t('dashboard.recentActivity.examples.returned', { item: 'Power Drill', user: 'John' })}
                      </p>
                      <p className="text-xs text-gray-500">{t('dashboard.recentActivity.timeAgo.hoursAgo', { count: 2 })}</p>
                    </div>
                  </div>

                  <div className="flex items-start space-x-3">
                    <div className="w-8 h-8 bg-blue-100 rounded-full flex items-center justify-center">
                      <Users className="h-4 w-4 text-blue-600" />
                    </div>
                    <div>
                      <p className="text-sm text-gray-900">
                        {t('dashboard.recentActivity.examples.joined', { user: 'Sarah', community: 'Oak Street Community' })}
                      </p>
                      <p className="text-xs text-gray-500">{t('dashboard.recentActivity.timeAgo.dayAgo')}</p>
                    </div>
                  </div>

                  <div className="flex items-start space-x-3">
                    <div className="w-8 h-8 bg-purple-100 rounded-full flex items-center justify-center">
                      <Bell className="h-4 w-4 text-purple-600" />
                    </div>
                    <div>
                      <p className="text-sm text-gray-900">
                        {t('dashboard.recentActivity.examples.newRequest', { item: 'Ladder' })}
                      </p>
                      <p className="text-xs text-gray-500">{t('dashboard.recentActivity.timeAgo.daysAgo', { count: 2 })}</p>
                    </div>
                  </div>
                </div>

                <div className="mt-4 pt-4 border-t border-gray-200">
                  <Link
                    to="/notifications"
                    className="text-sm text-blue-600 hover:text-blue-500 font-medium"
                  >
                    {t('dashboard.recentActivity.viewAll')}
                  </Link>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default DashboardPage;
