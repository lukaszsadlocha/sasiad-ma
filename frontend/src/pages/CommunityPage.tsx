import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useApiQuery, useApiMutation } from '../hooks/useApi';
import { communityService } from '../services/communityService';
import { useAuth } from '../hooks/useAuth';
import { formatters } from '../utils/formatters';
import LoadingSpinner from '../components/common/LoadingSpinner';
import {
  Users,
  MapPin,
  Settings,
  UserPlus,
  LogOut,
  Crown,
  Calendar,
  Copy,
  Shield,
  Globe,
  Lock
} from 'lucide-react';
import toast from 'react-hot-toast';
import type { Community, CommunityMember } from '../types/community';

const CommunityPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAuth();
  const { t } = useTranslation();
  const [showInvitationCode, setShowInvitationCode] = useState(false);

  const {
    data: community,
    isLoading: communityLoading,
    error: communityError,
    refetch: refetchCommunity
  } = useApiQuery(
    ['community', id],
    () => communityService.getById(id!),
    { enabled: !!id }
  );

  const leaveCommunityMutation = useApiMutation(
    () => communityService.leave(id!),
    {
      onSuccess: () => {
        toast.success(t('community.leftSuccess'));
        navigate('/communities');
      },
      onError: (error: any) => {
        toast.error(error?.response?.data?.message || t('community.leftError'));
      },
    }
  );

  const generateInvitationMutation = useApiMutation(
    () => communityService.generateInvitationCode(id!),
    {
      onSuccess: (data) => {
        refetchCommunity();
        toast.success(t('community.newCodeGenerated'));
      },
      onError: (error: any) => {
        toast.error(error?.response?.data?.message || t('community.generateCodeError'));
      },
    }
  );

  if (!id) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center">
          <h1 className="text-2xl font-bold text-gray-900 mb-4">{t('community.notFound')}</h1>
          <p className="text-gray-600">{t('community.notFoundMessage')}</p>
        </div>
      </div>
    );
  }

  if (communityLoading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="flex justify-center">
          <LoadingSpinner size="lg" />
        </div>
      </div>
    );
  }

  if (communityError || !community) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center">
          <h1 className="text-2xl font-bold text-gray-900 mb-4">{t('community.errorLoading')}</h1>
          <p className="text-gray-600">{t('community.loadError')}</p>
          <button
            onClick={() => navigate('/communities')}
            className="mt-4 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
          >
            {t('community.backToCommunities')}
          </button>
        </div>
      </div>
    );
  }

  const isUserMember = community.members?.some(member => member.userId === user?.id);
  const userMembership = community.members?.find(member => member.userId === user?.id);
  const isAdmin = userMembership?.isAdmin || false;

  const handleCopyInvitationCode = async () => {
    try {
      await navigator.clipboard.writeText(community.invitationCode);
      toast.success(t('community.codeCopied'));
    } catch (error) {
      toast.error(t('community.copyCodeError'));
    }
  };

  const handleLeaveCommunity = () => {
    if (window.confirm(t('community.leaveConfirm'))) {
      leaveCommunityMutation.mutate();
    }
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="max-w-4xl mx-auto space-y-6">
        {/* Community Header */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden">
          {/* Cover Image */}
          <div className="h-48 bg-gradient-to-r from-blue-500 to-blue-600 relative">
            {community.imageUrl ? (
              <img
                src={community.imageUrl}
                alt={community.name}
                className="w-full h-full object-cover"
              />
            ) : (
              <div className="flex items-center justify-center h-full">
                <Users className="h-16 w-16 text-white opacity-80" />
              </div>
            )}

            {/* Action buttons overlay */}
            <div className="absolute top-4 right-4 flex space-x-2">
              {isAdmin && (
                <button
                  onClick={() => navigate(`/communities/${id}/manage`)}
                  className="p-2 bg-white bg-opacity-90 rounded-full hover:bg-opacity-100 transition-all"
                  title="Manage community"
                >
                  <Settings className="h-5 w-5 text-gray-700" />
                </button>
              )}
            </div>
          </div>

          {/* Community Info */}
          <div className="p-6">
            <div className="flex justify-between items-start mb-4">
              <div>
                <h1 className="text-2xl font-bold text-gray-900 mb-2">{community.name}</h1>
                <p className="text-gray-600 mb-4">{community.description}</p>
              </div>

              {isUserMember && (
                <div className="flex space-x-2">
                  <button
                    onClick={handleLeaveCommunity}
                    disabled={leaveCommunityMutation.isPending}
                    className="flex items-center px-3 py-2 text-red-600 border border-red-300 rounded-md hover:bg-red-50 disabled:opacity-50"
                  >
                    <LogOut className="h-4 w-4 mr-1" />
{t('community.leave')}
                  </button>
                </div>
              )}
            </div>

            {/* Community Stats */}
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-6">
              <div className="text-center p-3 bg-gray-50 rounded-lg">
                <div className="flex items-center justify-center mb-1">
                  <Users className="h-5 w-5 text-gray-600" />
                </div>
                <div className="text-lg font-semibold text-gray-900">{community.activeMembersCount}</div>
                <div className="text-xs text-gray-600">{t('community.members')}</div>
              </div>

              <div className="text-center p-3 bg-gray-50 rounded-lg">
                <div className="flex items-center justify-center mb-1">
                  {community.isPublic ? (
                    <Globe className="h-5 w-5 text-green-600" />
                  ) : (
                    <Lock className="h-5 w-5 text-gray-600" />
                  )}
                </div>
                <div className="text-lg font-semibold text-gray-900">
                  {community.isPublic ? t('community.card.public') : t('community.card.private')}
                </div>
                <div className="text-xs text-gray-600">{t('community.visibility')}</div>
              </div>

              <div className="text-center p-3 bg-gray-50 rounded-lg">
                <div className="flex items-center justify-center mb-1">
                  <UserPlus className="h-5 w-5 text-gray-600" />
                </div>
                <div className="text-lg font-semibold text-gray-900">{community.maxMembers}</div>
                <div className="text-xs text-gray-600">{t('community.maxMembers')}</div>
              </div>

              <div className="text-center p-3 bg-gray-50 rounded-lg">
                <div className="flex items-center justify-center mb-1">
                  <Calendar className="h-5 w-5 text-gray-600" />
                </div>
                <div className="text-lg font-semibold text-gray-900">
                  {formatters.timeAgo(community.createdAt)}
                </div>
                <div className="text-xs text-gray-600">{t('community.created')}</div>
              </div>
            </div>

            {/* Location */}
            {community.address && (
              <div className="flex items-center text-gray-600 mb-4">
                <MapPin className="h-4 w-4 mr-2" />
                <span>{community.address}</span>
                {community.city && <span>, {community.city}</span>}
                {community.postalCode && <span> {community.postalCode}</span>}
              </div>
            )}

            {/* Invitation Code (for members) */}
            {isUserMember && (
              <div className="bg-blue-50 border border-blue-200 rounded-lg p-4">
                <div className="flex items-center justify-between mb-2">
                  <h3 className="font-medium text-blue-900">{t('community.invitationCode')}</h3>
                  {isAdmin && (
                    <button
                      onClick={() => generateInvitationMutation.mutate()}
                      disabled={generateInvitationMutation.isPending}
                      className="text-blue-600 text-sm hover:text-blue-800 disabled:opacity-50"
                    >
{t('community.generateNew')}
                    </button>
                  )}
                </div>
                <div className="flex items-center space-x-2">
                  <code className="flex-1 bg-white px-3 py-2 rounded border text-sm font-mono">
                    {showInvitationCode ? community.invitationCode : '••••••••••'}
                  </code>
                  <button
                    onClick={() => setShowInvitationCode(!showInvitationCode)}
                    className="px-3 py-2 text-blue-600 border border-blue-300 rounded hover:bg-blue-50"
                  >
                    {showInvitationCode ? t('community.hide') : t('community.show')}
                  </button>
                  {showInvitationCode && (
                    <button
                      onClick={handleCopyInvitationCode}
                      className="p-2 text-blue-600 hover:bg-blue-50 rounded"
                      title={t('community.copyCode')}
                    >
                      <Copy className="h-4 w-4" />
                    </button>
                  )}
                </div>
                <p className="text-xs text-blue-700 mt-1">
{t('community.shareCodeText')}
                </p>
              </div>
            )}
          </div>
        </div>

        {/* Members List */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200">
          <div className="p-6 border-b border-gray-200">
            <h2 className="text-lg font-semibold text-gray-900">{t('community.members')} ({community.activeMembersCount})</h2>
          </div>

          <div className="p-6">
            {community.members && community.members.length > 0 ? (
              <div className="grid gap-4">
                {community.members
                  .filter(member => member.isActive)
                  .map((member) => (
                    <div key={member.id} className="flex items-center justify-between p-4 border border-gray-200 rounded-lg hover:bg-gray-50">
                      <div className="flex items-center space-x-3">
                        <div className="w-10 h-10 bg-gray-200 rounded-full flex items-center justify-center">
                          {member.userProfileImageUrl ? (
                            <img
                              src={member.userProfileImageUrl}
                              alt={`${member.userFirstName} ${member.userLastName}`}
                              className="w-10 h-10 rounded-full object-cover"
                            />
                          ) : (
                            <span className="text-gray-600 font-medium">
                              {member.userFirstName.charAt(0)}{member.userLastName.charAt(0)}
                            </span>
                          )}
                        </div>
                        <div>
                          <div className="flex items-center space-x-2">
                            <p className="font-medium text-gray-900">
                              {member.userFirstName} {member.userLastName}
                            </p>
                            {member.isAdmin && (
                              <Crown className="h-4 w-4 text-yellow-500" title={t('community.admin')} />
                            )}
                          </div>
                          <p className="text-sm text-gray-600">{member.userEmail}</p>
                        </div>
                      </div>

                      <div className="text-right">
                        <p className="text-sm text-gray-600">
                          {t('community.joined')} {formatters.timeAgo(member.joinedAt)}
                        </p>
                      </div>
                    </div>
                  ))}
              </div>
            ) : (
              <div className="text-center py-8">
                <Users className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                <p className="text-gray-600">{t('community.noMembersFound')}</p>
              </div>
            )}
          </div>
        </div>

        {/* Action Buttons */}
        <div className="flex justify-center space-x-4">
          <button
            onClick={() => navigate('/communities')}
            className="px-6 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
          >
            {t('community.backToCommunities')}
          </button>

          {!isUserMember && (
            <button
              onClick={() => navigate('/communities/join', { state: { invitationCode: '' } })}
              className="px-6 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
            >
{t('community.joinCommunity')}
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default CommunityPage;
