import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Edit, MapPin, Calendar, Star, Package, Users } from 'lucide-react';
import { useAuth } from '../../hooks/useAuth';
import { formatters } from '../../utils/formatters';
import ReputationDisplay from './ReputationDisplay';

interface UserProfileProps {
  userId?: string;
  showEdit?: boolean;
}

const UserProfile: React.FC<UserProfileProps> = ({
  userId,
  showEdit = true
}) => {
  const { user } = useAuth();
  const navigate = useNavigate();

  // For now, we'll use the current user. In a full implementation,
  // you'd fetch user data based on userId
  const profileUser = user;

  if (!profileUser) {
    return (
      <div className="text-center py-12">
        <p className="text-gray-600">User not found</p>
      </div>
    );
  }

  const isOwnProfile = !userId || userId === profileUser.id;

  return (
    <div className="max-w-4xl mx-auto">
      <div className="bg-white shadow rounded-lg overflow-hidden">
        {/* Cover/Header Section */}
        <div className="h-32 bg-gradient-to-r from-blue-400 to-blue-600"></div>

        {/* Profile Section */}
        <div className="px-6 py-6">
          <div className="sm:flex sm:items-center sm:justify-between">
            <div className="sm:flex sm:space-x-5">
              <div className="flex-shrink-0">
                <div className="mx-auto h-20 w-20 rounded-full overflow-hidden -mt-16 border-4 border-white bg-white">
                  {profileUser.profileImageUrl ? (
                    <img
                      className="h-full w-full object-cover"
                      src={profileUser.profileImageUrl}
                      alt={`${profileUser.firstName} ${profileUser.lastName}`}
                    />
                  ) : (
                    <div className="h-full w-full bg-gray-300 flex items-center justify-center">
                      <span className="text-xl font-medium text-gray-700">
                        {profileUser.firstName?.[0]}{profileUser.lastName?.[0]}
                      </span>
                    </div>
                  )}
                </div>
              </div>
              <div className="mt-4 text-center sm:mt-0 sm:pt-1 sm:text-left">
                <p className="text-xl font-bold text-gray-900 sm:text-2xl">
                  {profileUser.firstName} {profileUser.lastName}
                </p>
                {profileUser.bio && (
                  <p className="text-sm font-medium text-gray-600 mt-1">
                    {profileUser.bio}
                  </p>
                )}
              </div>
            </div>
            <div className="mt-5 flex justify-center sm:mt-0">
              {isOwnProfile && showEdit && (
                <button
                  onClick={() => navigate('/profile/edit')}
                  className="flex justify-center items-center px-4 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50"
                >
                  <Edit className="h-4 w-4 mr-2" />
                  Edit Profile
                </button>
              )}
            </div>
          </div>
        </div>

        {/* Profile Details */}
        <div className="border-t border-gray-200 px-6 py-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {/* Basic Info */}
            <div className="space-y-4">
              <h3 className="text-lg font-medium text-gray-900">Basic Information</h3>

              {profileUser.email && (
                <div className="flex items-center text-sm text-gray-600">
                  <span className="font-medium w-24">Email:</span>
                  <span>{profileUser.email}</span>
                </div>
              )}

              {profileUser.phone && (
                <div className="flex items-center text-sm text-gray-600">
                  <span className="font-medium w-24">Phone:</span>
                  <span>{formatters.phone(profileUser.phone)}</span>
                </div>
              )}

              {profileUser.location && (
                <div className="flex items-center text-sm text-gray-600">
                  <MapPin className="h-4 w-4 mr-2" />
                  <span>{profileUser.location}</span>
                </div>
              )}

              <div className="flex items-center text-sm text-gray-600">
                <Calendar className="h-4 w-4 mr-2" />
                <span>Joined {formatters.date(profileUser.createdAt)}</span>
              </div>
            </div>

            {/* Stats & Reputation */}
            <div className="space-y-4">
              <h3 className="text-lg font-medium text-gray-900">Community Stats</h3>

              <ReputationDisplay
                score={profileUser.reputation || 0}
                totalTransactions={profileUser.totalTransactions || 0}
              />

              <div className="grid grid-cols-2 gap-4">
                <div className="text-center p-4 bg-gray-50 rounded-lg">
                  <Package className="h-8 w-8 mx-auto text-blue-600 mb-2" />
                  <div className="text-2xl font-bold text-gray-900">
                    {profileUser.itemsShared || 0}
                  </div>
                  <div className="text-sm text-gray-600">Items Shared</div>
                </div>

                <div className="text-center p-4 bg-gray-50 rounded-lg">
                  <Users className="h-8 w-8 mx-auto text-green-600 mb-2" />
                  <div className="text-2xl font-bold text-gray-900">
                    {profileUser.communitiesJoined || 0}
                  </div>
                  <div className="text-sm text-gray-600">Communities</div>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Recent Activity - Placeholder */}
        <div className="border-t border-gray-200 px-6 py-6">
          <h3 className="text-lg font-medium text-gray-900 mb-4">Recent Activity</h3>
          <div className="text-sm text-gray-600">
            No recent activity to display.
          </div>
        </div>
      </div>
    </div>
  );
};

export default UserProfile;