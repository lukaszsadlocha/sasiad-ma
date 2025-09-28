import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useApiQuery, useApiMutation } from '../hooks/useApi';
import { itemService } from '../services/itemService';
import { useAuth } from '../hooks/useAuth';
import { formatters } from '../utils/formatters';
import LoadingSpinner from '../components/common/LoadingSpinner';
import {
  Tag,
  User,
  Clock,
  MapPin,
  MessageCircle,
  Share2,
  Edit,
  Trash2,
  Heart,
  Star,
  Camera,
  ChevronLeft,
  ChevronRight
} from 'lucide-react';
import toast from 'react-hot-toast';
import type { Item } from '../types/item';

const ItemsPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAuth();
  const { t } = useTranslation();
  const [currentImageIndex, setCurrentImageIndex] = useState(0);
  const [showFullDescription, setShowFullDescription] = useState(false);

  const {
    data: item,
    isLoading: itemLoading,
    error: itemError,
    refetch: refetchItem
  } = useApiQuery(
    ['item', id],
    () => itemService.getById(id!),
    { enabled: !!id }
  );

  if (!id) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center">
          <h1 className="text-2xl font-bold text-gray-900 mb-4">{t('items.notFound')}</h1>
          <p className="text-gray-600">{t('items.notFoundMessage')}</p>
        </div>
      </div>
    );
  }

  if (itemLoading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="flex justify-center">
          <LoadingSpinner size="lg" />
        </div>
      </div>
    );
  }

  if (itemError || !item) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center">
          <h1 className="text-2xl font-bold text-gray-900 mb-4">{t('items.errorLoading')}</h1>
          <p className="text-gray-600">{t('items.loadError')}</p>
          <button
            onClick={() => navigate('/items')}
            className="mt-4 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
          >
{t('items.backToItems')}
          </button>
        </div>
      </div>
    );
  }

  const isOwner = item.ownerId === user?.id;
  const hasImages = item.imageUrls && item.imageUrls.length > 0;
  const isAvailable = item.status === 'Available';

  const getStatusColor = (status: string) => {
    switch (status?.toLowerCase()) {
      case 'available':
        return 'bg-green-100 text-green-800 border-green-200';
      case 'borrowed':
        return 'bg-yellow-100 text-yellow-800 border-yellow-200';
      case 'maintenance':
        return 'bg-orange-100 text-orange-800 border-orange-200';
      case 'unavailable':
        return 'bg-red-100 text-red-800 border-red-200';
      default:
        return 'bg-gray-100 text-gray-800 border-gray-200';
    }
  };

  const getConditionIcon = (condition: string) => {
    switch (condition?.toLowerCase()) {
      case 'new':
        return '🆕';
      case 'excellent':
        return '⭐';
      case 'good':
        return '✅';
      case 'fair':
        return '⚠️';
      case 'poor':
        return '🔴';
      default:
        return '❓';
    }
  };

  const nextImage = () => {
    if (hasImages) {
      setCurrentImageIndex((prev) =>
        prev === item.imageUrls.length - 1 ? 0 : prev + 1
      );
    }
  };

  const prevImage = () => {
    if (hasImages) {
      setCurrentImageIndex((prev) =>
        prev === 0 ? item.imageUrls.length - 1 : prev - 1
      );
    }
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="max-w-6xl mx-auto">
        {/* Breadcrumb */}
        <nav className="flex items-center space-x-2 text-sm text-gray-600 mb-6">
          <button
            onClick={() => navigate('/items')}
            className="hover:text-blue-600"
          >
{t('items.title')}
          </button>
          <span>/</span>
          <span className="text-gray-900">{item.name}</span>
        </nav>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
          {/* Images Section */}
          <div className="space-y-4">
            <div className="relative bg-gray-100 rounded-lg overflow-hidden">
              {hasImages ? (
                <>
                  <img
                    src={item.imageUrls[currentImageIndex]}
                    alt={`${item.name} - Image ${currentImageIndex + 1}`}
                    className="w-full h-96 object-cover"
                  />

                  {item.imageUrls.length > 1 && (
                    <>
                      <button
                        onClick={prevImage}
                        className="absolute left-2 top-1/2 transform -translate-y-1/2 bg-black bg-opacity-50 text-white p-2 rounded-full hover:bg-opacity-75"
                      >
                        <ChevronLeft className="h-5 w-5" />
                      </button>
                      <button
                        onClick={nextImage}
                        className="absolute right-2 top-1/2 transform -translate-y-1/2 bg-black bg-opacity-50 text-white p-2 rounded-full hover:bg-opacity-75"
                      >
                        <ChevronRight className="h-5 w-5" />
                      </button>

                      <div className="absolute bottom-4 left-1/2 transform -translate-x-1/2 flex space-x-2">
                        {item.imageUrls.map((_, index) => (
                          <button
                            key={index}
                            onClick={() => setCurrentImageIndex(index)}
                            className={`w-2 h-2 rounded-full ${
                              index === currentImageIndex ? 'bg-white' : 'bg-white bg-opacity-50'
                            }`}
                          />
                        ))}
                      </div>
                    </>
                  )}
                </>
              ) : (
                <div className="w-full h-96 flex items-center justify-center">
                  <Camera className="h-24 w-24 text-gray-400" />
                </div>
              )}
            </div>

            {/* Thumbnail images */}
            {hasImages && item.imageUrls.length > 1 && (
              <div className="grid grid-cols-4 gap-2">
                {item.imageUrls.map((imageUrl, index) => (
                  <button
                    key={index}
                    onClick={() => setCurrentImageIndex(index)}
                    className={`relative aspect-square rounded-lg overflow-hidden border-2 ${
                      index === currentImageIndex ? 'border-blue-500' : 'border-gray-200'
                    }`}
                  >
                    <img
                      src={imageUrl}
                      alt={`${item.name} thumbnail ${index + 1}`}
                      className="w-full h-full object-cover"
                    />
                  </button>
                ))}
              </div>
            )}
          </div>

          {/* Item Details */}
          <div className="space-y-6">
            {/* Header */}
            <div>
              <div className="flex items-start justify-between mb-2">
                <h1 className="text-3xl font-bold text-gray-900">{item.name}</h1>
                {isOwner && (
                  <div className="flex space-x-2">
                    <button
                      onClick={() => navigate(`/items/${item.id}/edit`)}
                      className="p-2 text-gray-400 hover:text-blue-600 rounded-full hover:bg-gray-100"
                      title="Edit item"
                    >
                      <Edit className="h-5 w-5" />
                    </button>
                  </div>
                )}
              </div>

              <div className="flex items-center space-x-4 mb-4">
                <span
                  className={`inline-flex items-center px-3 py-1 rounded-full text-sm font-medium border ${getStatusColor(
                    item.status
                  )}`}
                >
                  {item.status}
                </span>

                <div className="flex items-center text-sm text-gray-600">
                  <span className="mr-2">{getConditionIcon(item.condition)}</span>
                  <span>{t('items.condition')} {item.condition}</span>
                </div>
              </div>
            </div>

            {/* Description */}
            <div>
              <h2 className="text-lg font-semibold text-gray-900 mb-2">{t('items.description')}</h2>
              <div className="text-gray-700">
                {item.description ? (
                  <div>
                    <p className={showFullDescription ? '' : 'line-clamp-3'}>
                      {item.description}
                    </p>
                    {item.description.length > 150 && (
                      <button
                        onClick={() => setShowFullDescription(!showFullDescription)}
                        className="text-blue-600 text-sm hover:text-blue-800 mt-1"
                      >
                        {showFullDescription ? t('items.showLess') : t('items.showMore')}
                      </button>
                    )}
                  </div>
                ) : (
                  <p className="text-gray-500 italic">{t('items.noDescription')}</p>
                )}
              </div>
            </div>

            {/* Item Info */}
            <div className="grid grid-cols-2 gap-4">
              <div>
                <h3 className="text-sm font-medium text-gray-700 mb-2">{t('items.category')}</h3>
                <div className="flex items-center text-gray-900">
                  <Tag className="h-4 w-4 mr-2" />
                  <span>{item.category}</span>
                </div>
              </div>

              <div>
                <h3 className="text-sm font-medium text-gray-700 mb-2">{t('items.community')}</h3>
                <div className="flex items-center text-gray-900">
                  <MapPin className="h-4 w-4 mr-2" />
                  <span>{item.communityName}</span>
                </div>
              </div>
            </div>

            {/* Owner Info */}
            <div className="bg-gray-50 rounded-lg p-4">
              <h3 className="text-sm font-medium text-gray-700 mb-3">{t('items.itemOwner')}</h3>
              <div className="flex items-center space-x-3">
                <div className="w-10 h-10 bg-gray-200 rounded-full flex items-center justify-center">
                  {item.ownerProfileImageUrl ? (
                    <img
                      src={item.ownerProfileImageUrl}
                      alt={item.ownerName}
                      className="w-10 h-10 rounded-full object-cover"
                    />
                  ) : (
                    <span className="text-gray-600 font-medium">
                      {item.ownerName?.charAt(0)?.toUpperCase() || 'U'}
                    </span>
                  )}
                </div>
                <div>
                  <p className="font-medium text-gray-900">{item.ownerName}</p>
                  <p className="text-sm text-gray-600">
                    {t('items.memberSince')} {formatters.timeAgo(item.createdAt)}
                  </p>
                </div>
              </div>
            </div>

            {/* Actions */}
            <div className="space-y-3">
              {!isOwner && isAvailable && (
                <button
                  onClick={() => navigate(`/items/${item.id}/request`)}
                  className="w-full bg-blue-600 text-white py-3 px-4 rounded-lg font-medium hover:bg-blue-700 transition-colors"
                >
{t('items.requestToBorrow')}
                </button>
              )}

              <div className="grid grid-cols-2 gap-3">
                <button
                  onClick={() => {
                    // Share functionality
                    navigator.share?.({
                      title: item.name,
                      text: `Check out this item: ${item.name}`,
                      url: window.location.href
                    }).catch(() => {
                      // Fallback: copy to clipboard
                      navigator.clipboard.writeText(window.location.href);
                      toast.success('Link copied to clipboard');
                    });
                  }}
                  className="flex items-center justify-center px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50"
                >
                  <Share2 className="h-4 w-4 mr-2" />
{t('items.share')}
                </button>

                {!isOwner && (
                  <button
                    onClick={() => navigate(`/messages/new?user=${item.ownerId}`)}
                    className="flex items-center justify-center px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50"
                  >
                    <MessageCircle className="h-4 w-4 mr-2" />
{t('items.message')}
                  </button>
                )}
              </div>
            </div>

            {/* Meta Info */}
            <div className="pt-4 border-t border-gray-200 text-sm text-gray-500">
              <div className="flex justify-between">
                <span>{t('items.added')} {formatters.timeAgo(item.createdAt)}</span>
                <span>{t('items.updated')} {formatters.timeAgo(item.updatedAt)}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ItemsPage;
