import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { Users, MapPin, Settings } from 'lucide-react';
import type { Community } from '../../types/community';
import { formatters } from '../../utils/formatters';

interface CommunityCardProps {
  community: Community;
  showActions?: boolean;
}

const CommunityCard: React.FC<CommunityCardProps> = ({
  community,
  showActions = true
}) => {
  const navigate = useNavigate();
  const { t } = useTranslation();

  const handleViewCommunity = () => {
    navigate(`/communities/${community.id}`);
  };

  const handleManageCommunity = (e: React.MouseEvent) => {
    e.stopPropagation();
    navigate(`/communities/${community.id}/manage`);
  };

  return (
    <div
      className="bg-white rounded-lg border border-gray-200 hover:shadow-md transition-shadow cursor-pointer"
      onClick={handleViewCommunity}
    >
      {/* Community Image */}
      <div className="aspect-w-16 aspect-h-9">
        {community.imageUrl && community.imageUrl.trim() ? (
          <img
            src={community.imageUrl}
            alt={community.name}
            className="w-full h-48 object-cover rounded-t-lg"
            onError={(e) => {
              console.error('Image load error for community:', community.name);
              // Hide the broken image and show fallback
              (e.target as HTMLImageElement).style.display = 'none';
              const fallback = (e.target as HTMLImageElement).nextElementSibling;
              if (fallback) {
                (fallback as HTMLElement).style.display = 'flex';
              }
            }}
          />
        ) : null}
        <div
          className={`w-full h-48 bg-gradient-to-br from-blue-400 to-blue-600 rounded-t-lg flex items-center justify-center ${
            community.imageUrl && community.imageUrl.trim() ? 'hidden' : 'flex'
          }`}
        >
          <Users className="h-16 w-16 text-white opacity-80" />
        </div>
      </div>

      {/* Community Info */}
      <div className="p-6">
        <div className="flex items-start justify-between mb-3">
          <h3 className="text-lg font-semibold text-gray-900 truncate">
            {community.name}
          </h3>
          {showActions && (
            <button
              onClick={handleManageCommunity}
              className="p-1 text-gray-400 hover:text-gray-600 rounded"
              title="Manage community"
            >
              <Settings className="h-4 w-4" />
            </button>
          )}
        </div>

        <p className="text-gray-600 text-sm mb-4 line-clamp-2">
          {community.description || t('community.card.noDescription')}
        </p>

        <div className="space-y-2">
          <div className="flex items-center text-sm text-gray-500">
            <Users className="h-4 w-4 mr-2" />
            <span>
              {community.activeMembersCount || 0} {(community.activeMembersCount || 0) === 1 ? t('community.card.member') : t('community.card.members')}
            </span>
          </div>

          {community.address && (
            <div className="flex items-center text-sm text-gray-500">
              <MapPin className="h-4 w-4 mr-2" />
              <span className="truncate">{community.address}</span>
            </div>
          )}
        </div>

        <div className="mt-4 flex items-center justify-between text-xs text-gray-400">
          <span>
            {t('community.card.created')} {formatters.timeAgo(community.createdAt)}
          </span>
          <span>
            {community.isPublic ? t('community.card.public') : t('community.card.private')}
          </span>
        </div>
      </div>
    </div>
  );
};

export default CommunityCard;