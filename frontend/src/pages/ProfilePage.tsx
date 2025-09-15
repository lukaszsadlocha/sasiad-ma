import React from 'react';

const ProfilePage: React.FC = () => {
  return (
    <div className="container mx-auto px-4 py-8">
      <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-8 text-center">
        <h1 className="text-2xl font-bold text-gray-900 mb-4">Profile</h1>
        <p className="text-gray-600 mb-6">
          This page will show your profile information and allow you to edit it.
        </p>
        <div className="text-sm text-gray-500">
          🚧 Under development - Profile management features coming soon!
        </div>
      </div>
    </div>
  );
};

export default ProfilePage;
