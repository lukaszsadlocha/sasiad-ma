import React from 'react';
import { Routes, Route } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { Toaster } from 'react-hot-toast';
import { useAuth } from './hooks/useAuth';
import Header from './components/common/Header';
import Footer from './components/common/Footer';
import ProtectedRoute from './components/common/ProtectedRoute';
import LoadingSpinner from './components/common/LoadingSpinner';
import ErrorBoundary from './components/common/ErrorBoundary';
import { CommunityProvider } from './context/CommunityContext';
import { NotificationProvider } from './context/NotificationContext';

// Pages
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import DashboardPage from './pages/DashboardPage';
import CommunityPage from './pages/CommunityPage';
import ItemsPage from './pages/ItemsPage';
import ProfilePage from './pages/ProfilePage';
import NotificationsPage from './pages/NotificationsPage';
import JoinCommunityPage from './pages/JoinCommunityPage';
import NotFoundPage from './pages/NotFoundPage';

// Components for direct routing
import EmailConfirmation from './components/auth/EmailConfirmation';
import CommunityList from './components/community/CommunityList';
import CreateCommunityForm from './components/community/CreateCommunityForm';
import ItemList from './components/item/ItemList';
import UserProfile from './components/user/UserProfile';

// Create a query client
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      refetchOnWindowFocus: false,
    },
  },
});

const AppContent: React.FC = () => {
  const { isLoading, isAuthenticated } = useAuth();

  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <Header />
      <main className="flex-1 container mx-auto px-4 py-8">
        <Routes>
          {/* Root route - shows dashboard for authenticated users, homepage for others */}
          <Route path="/" element={
            isAuthenticated ? (
              <ProtectedRoute>
                <DashboardPage />
              </ProtectedRoute>
            ) : (
              <HomePage />
            )
          } />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/auth/confirm-email" element={<EmailConfirmation />} />

          {/* Protected routes */}
          <Route path="/dashboard" element={
            <ProtectedRoute>
              <DashboardPage />
            </ProtectedRoute>
          } />

          {/* Community routes */}
          <Route path="/communities" element={
            <ProtectedRoute>
              <CommunityList />
            </ProtectedRoute>
          } />
          <Route path="/communities/create" element={
            <ProtectedRoute>
              <CreateCommunityForm />
            </ProtectedRoute>
          } />
          <Route path="/communities/join" element={
            <ProtectedRoute>
              <JoinCommunityPage />
            </ProtectedRoute>
          } />
          <Route path="/communities/:id" element={
            <ProtectedRoute>
              <CommunityPage />
            </ProtectedRoute>
          } />

          {/* Item routes */}
          <Route path="/items" element={
            <ProtectedRoute>
              <ItemList />
            </ProtectedRoute>
          } />
          <Route path="/items/:id" element={
            <ProtectedRoute>
              <ItemsPage />
            </ProtectedRoute>
          } />

          {/* User routes */}
          <Route path="/profile" element={
            <ProtectedRoute>
              <UserProfile />
            </ProtectedRoute>
          } />
          <Route path="/profile/:userId" element={
            <ProtectedRoute>
              <ProfilePage />
            </ProtectedRoute>
          } />

          {/* Notification routes */}
          <Route path="/notifications" element={
            <ProtectedRoute>
              <NotificationsPage />
            </ProtectedRoute>
          } />

          {/* 404 route */}
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
      </main>
      <Footer />
      <Toaster position="top-right" />
    </div>
  );
};

const App: React.FC = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <ErrorBoundary>
        <CommunityProvider>
          <NotificationProvider>
            <AppContent />
          </NotificationProvider>
        </CommunityProvider>
      </ErrorBoundary>
    </QueryClientProvider>
  );
};

export default App;
