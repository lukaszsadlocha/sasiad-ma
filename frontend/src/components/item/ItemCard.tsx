import React from 'react';
import { useNavigate } from 'react-router-dom';
import { User, Clock, Tag } from 'lucide-react';
import type { Item } from '../../types/item';
import { formatters } from '../../utils/formatters';

interface ItemCardProps {
  item: Item;
  showActions?: boolean;
}

const ItemCard: React.FC<ItemCardProps> = ({ item, showActions = true }) => {
  const navigate = useNavigate();

  const handleViewItem = () => {
    navigate(`/items/${item.id}`);
  };

  const getStatusColor = (status: string) => {
    switch (status?.toLowerCase()) {
      case 'available':
        return 'bg-green-100 text-green-800';
      case 'borrowed':
        return 'bg-yellow-100 text-yellow-800';
      case 'unavailable':
        return 'bg-red-100 text-red-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  return (
    <div
      className="bg-white rounded-lg border border-gray-200 hover:shadow-md transition-shadow cursor-pointer overflow-hidden"
      onClick={handleViewItem}
    >
      {/* Item Image */}
      <div className="aspect-w-16 aspect-h-9">
        {item.images && item.images.length > 0 ? (
          <img
            src={item.images[0]}
            alt={item.name}
            className="w-full h-48 object-cover"
          />
        ) : (
          <div className="w-full h-48 bg-gray-100 flex items-center justify-center">
            <Tag className="h-12 w-12 text-gray-400" />
          </div>
        )}
      </div>

      {/* Item Info */}
      <div className="p-4">
        <div className="flex items-start justify-between mb-2">
          <h3 className="text-lg font-semibold text-gray-900 truncate flex-1 mr-2">
            {item.name}
          </h3>
          <span
            className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${getStatusColor(
              item.status
            )}`}
          >
            {item.status}
          </span>
        </div>

        <p className="text-gray-600 text-sm mb-3 line-clamp-2">
          {item.description || 'No description available'}
        </p>

        {/* Item Details */}
        <div className="space-y-2 mb-4">
          {item.category && (
            <div className="flex items-center text-sm text-gray-500">
              <Tag className="h-4 w-4 mr-2" />
              <span>{item.category}</span>
            </div>
          )}

          {item.condition && (
            <div className="flex items-center text-sm text-gray-500">
              <span className="w-4 h-4 mr-2 flex items-center justify-center">
                {item.condition === 'New' && '🆕'}
                {item.condition === 'Good' && '✅'}
                {item.condition === 'Fair' && '⚠️'}
                {item.condition === 'Poor' && '🔴'}
              </span>
              <span>Condition: {item.condition}</span>
            </div>
          )}
        </div>

        {/* Owner Info */}
        <div className="flex items-center justify-between text-sm">
          <div className="flex items-center text-gray-500">
            <User className="h-4 w-4 mr-2" />
            <span>{item.ownerName || 'Unknown'}</span>
          </div>

          {item.createdAt && (
            <div className="flex items-center text-gray-400">
              <Clock className="h-4 w-4 mr-1" />
              <span>{formatters.timeAgo(item.createdAt)}</span>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ItemCard;