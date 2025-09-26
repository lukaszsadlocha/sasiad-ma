import React from 'react';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useAuth } from '../../hooks/useAuth';
import { Home, Users, Package, Bell, User, LogOut, LayoutDashboard } from 'lucide-react';
import LanguageSelector from './LanguageSelector';

const Header: React.FC = () => {
  const { user, isAuthenticated, logout } = useAuth();
  const { t } = useTranslation();

  const handleLogout = () => {
    logout();
  };

  return (
    <header className="bg-white shadow-sm border-b border-gray-200">
      <div className="container mx-auto px-4">
        <div className="flex items-center justify-between h-16">
          {/* Logo */}
          <Link to="/" className="flex items-center space-x-2">
            <div className="w-8 h-8 bg-blue-500 rounded-lg flex items-center justify-center">
              <Home className="h-5 w-5 text-white" />
            </div>
            <span className="text-xl font-bold text-gray-900">{t('app.title')}</span>
          </Link>

          {/* Navigation */}
          {isAuthenticated ? (
            <nav className="flex items-center space-x-6">
              <Link
                to="/dashboard"
                className="flex items-center space-x-1 text-gray-700 hover:text-blue-600 transition-colors"
              >
                <LayoutDashboard className="h-4 w-4" />
                <span>{t('header.dashboard')}</span>
              </Link>
              <Link 
                to="/communities" 
                className="flex items-center space-x-1 text-gray-700 hover:text-blue-600 transition-colors"
              >
                <Users className="h-4 w-4" />
                <span>{t('header.communities')}</span>
              </Link>
              <Link 
                to="/items" 
                className="flex items-center space-x-1 text-gray-700 hover:text-blue-600 transition-colors"
              >
                <Package className="h-4 w-4" />
                <span>{t('header.items')}</span>
              </Link>
              <Link 
                to="/notifications" 
                className="flex items-center space-x-1 text-gray-700 hover:text-blue-600 transition-colors"
              >
                <Bell className="h-4 w-4" />
                <span>{t('header.notifications')}</span>
              </Link>
              
              {/* Language Selector */}
              <LanguageSelector />

              {/* User Menu */}
              <div className="flex items-center space-x-3">
                <Link 
                  to="/profile" 
                  className="flex items-center space-x-2 text-gray-700 hover:text-blue-600 transition-colors"
                >
                  {user?.profileImageUrl ? (
                    <img 
                      src={user.profileImageUrl} 
                      alt={user.firstName}
                      className="w-8 h-8 rounded-full object-cover"
                    />
                  ) : (
                    <User className="h-8 w-8 text-gray-400 bg-gray-100 rounded-full p-1" />
                  )}
                  <span className="hidden md:block">{user?.firstName}</span>
                </Link>
                
                <button
                  onClick={handleLogout}
                  className="flex items-center space-x-1 text-gray-700 hover:text-red-600 transition-colors"
                  title="Logout"
                >
                  <LogOut className="h-4 w-4" />
                  <span className="hidden md:block">{t('header.logout')}</span>
                </button>
              </div>
            </nav>
          ) : (
            <nav className="flex items-center space-x-4">
              <LanguageSelector />
              <Link 
                to="/login" 
                className="text-gray-700 hover:text-blue-600 transition-colors"
              >
                {t('header.login')}
              </Link>
              <Link 
                to="/register" 
                className="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600 transition-colors"
              >
                {t('header.register')}
              </Link>
            </nav>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;
