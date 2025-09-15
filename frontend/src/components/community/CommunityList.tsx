import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Plus, Users, MapPin } from 'lucide-react';
import { useApiQuery } from '../../hooks/useApi';
import { communityService } from '../../services/communityService';
import LoadingSpinner from '../common/LoadingSpinner';
import CommunityCard from './CommunityCard';

const CommunityList: React.FC = () => {
  const navigate = useNavigate();

  const {
    data: communities = [],
    isLoading,
    error,
    refetch,
  } = useApiQuery(['user-communities'], communityService.getUserCommunities);

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <LoadingSpinner />
      </div>
    );
  }

  if (error) {
    return (
      <div className="text-center py-12">
        <div className="text-red-600 mb-4">Failed to load communities</div>
        <button
          onClick={() => refetch()}
          className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
        >
          Try Again
        </button>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold text-gray-900">My Communities</h1>
        <button
          onClick={() => navigate('/communities/create')}
          className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
        >
          <Plus className="h-4 w-4 mr-2" />
          Create Community
        </button>
      </div>

      {communities.length === 0 ? (
        <div className="text-center py-12">
          <div className="mx-auto w-24 h-24 bg-gray-100 rounded-full flex items-center justify-center mb-4">
            <Users className="h-12 w-12 text-gray-400" />
          </div>
          <h3 className="text-lg font-medium text-gray-900 mb-2">
            No communities yet
          </h3>
          <p className="text-gray-600 mb-6">
            Join an existing community or create your own to start sharing items with your neighbors.
          </p>
          <div className="space-x-3">
            <button
              onClick={() => navigate('/communities/create')}
              className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-blue-600 hover:bg-blue-700"
            >
              <Plus className="h-4 w-4 mr-2" />
              Create Community
            </button>
            <button
              onClick={() => navigate('/communities/join')}
              className="inline-flex items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md shadow-sm text-gray-700 bg-white hover:bg-gray-50"
            >
              <MapPin className="h-4 w-4 mr-2" />
              Join Community
            </button>
          </div>
        </div>
      ) : (
        <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          {communities.map((community) => (
            <CommunityCard key={community.id} community={community} />
          ))}
        </div>
      )}
    </div>
  );
};

export default CommunityList;