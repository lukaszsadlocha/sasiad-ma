import React from 'react';
import { useAuth } from '../hooks/useAuth';
import { Link } from 'react-router-dom';
import { Users, Package, Bell, Plus, TrendingUp } from 'lucide-react';

const DashboardPage: React.FC = () => {
  const { user } = useAuth();

  const stats = [
    {
      name: 'Communities',
      value: '2',
      icon: Users,
      color: 'bg-blue-500',
      href: '/communities'
    },
    {
      name: 'Your Items',
      value: '5',
      icon: Package,
      color: 'bg-green-500',
      href: '/items'
    },
    {
      name: 'Active Borrows',
      value: '1',
      icon: TrendingUp,
      color: 'bg-purple-500',
      href: '/items'
    },
    {
      name: 'Notifications',
      value: '3',
      icon: Bell,
      color: 'bg-yellow-500',
      href: '/notifications'
    }
  ];

  const quickActions = [
    {
      name: 'Add New Item',
      description: 'Share an item with your community',
      icon: Package,
      href: '/items/new',
      color: 'bg-blue-500'
    },
    {
      name: 'Join Community',
      description: 'Join a new neighborhood group',
      icon: Users,
      href: '/communities/join',
      color: 'bg-green-500'
    },
    {
      name: 'Browse Items',
      description: 'Find items to borrow',
      icon: TrendingUp,
      href: '/items',
      color: 'bg-purple-500'
    }
  ];

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="container mx-auto px-4 py-8">
        {/* Welcome Header */}
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900">
            Welcome back, {user?.firstName}! 👋
          </h1>
          <p className="text-gray-600 mt-2">
            Here's what's happening in your communities
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
                <h2 className="text-lg font-semibold text-gray-900">Quick Actions</h2>
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
                <h2 className="text-lg font-semibold text-gray-900">Recent Activity</h2>
              </div>
              <div className="p-6">
                <div className="space-y-4">
                  <div className="flex items-start space-x-3">
                    <div className="w-8 h-8 bg-green-100 rounded-full flex items-center justify-center">
                      <Package className="h-4 w-4 text-green-600" />
                    </div>
                    <div>
                      <p className="text-sm text-gray-900">
                        Your <strong>Power Drill</strong> was returned by John
                      </p>
                      <p className="text-xs text-gray-500">2 hours ago</p>
                    </div>
                  </div>

                  <div className="flex items-start space-x-3">
                    <div className="w-8 h-8 bg-blue-100 rounded-full flex items-center justify-center">
                      <Users className="h-4 w-4 text-blue-600" />
                    </div>
                    <div>
                      <p className="text-sm text-gray-900">
                        Sarah joined <strong>Oak Street Community</strong>
                      </p>
                      <p className="text-xs text-gray-500">1 day ago</p>
                    </div>
                  </div>

                  <div className="flex items-start space-x-3">
                    <div className="w-8 h-8 bg-purple-100 rounded-full flex items-center justify-center">
                      <Bell className="h-4 w-4 text-purple-600" />
                    </div>
                    <div>
                      <p className="text-sm text-gray-900">
                        New borrow request for your <strong>Ladder</strong>
                      </p>
                      <p className="text-xs text-gray-500">2 days ago</p>
                    </div>
                  </div>
                </div>

                <div className="mt-4 pt-4 border-t border-gray-200">
                  <Link
                    to="/notifications"
                    className="text-sm text-blue-600 hover:text-blue-500 font-medium"
                  >
                    View all activity →
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
