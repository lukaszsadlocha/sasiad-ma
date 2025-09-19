import React from 'react';
import JoinCommunityForm from '../components/community/JoinCommunityForm';

const JoinCommunityPage: React.FC = () => {
  return (
    <div className="min-h-screen bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
      <JoinCommunityForm />
    </div>
  );
};

export default JoinCommunityPage;