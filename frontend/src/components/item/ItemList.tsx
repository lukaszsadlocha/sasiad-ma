import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { Plus, Search, Filter } from 'lucide-react';
import { useApiQuery } from '../../hooks/useApi';
import { itemService } from '../../services/itemService';
import { useCommunity } from '../../context/CommunityContext';
import LoadingSpinner from '../common/LoadingSpinner';
import ItemCard from './ItemCard';
import ItemSearch from './ItemSearch';

const ItemList: React.FC = () => {
  const navigate = useNavigate();
  const { currentCommunity } = useCommunity();
  const { t } = useTranslation();
  const [showSearch, setShowSearch] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('');

  const {
    data: items = [],
    isLoading,
    error,
    refetch,
  } = useApiQuery(
    ['items', currentCommunity?.id],
    () => itemService.getAll(currentCommunity?.id),
    { enabled: true }
  );

  const filteredItems = items.filter(item => {
    const matchesSearch = !searchQuery ||
      item.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
      item.description?.toLowerCase().includes(searchQuery.toLowerCase());

    const matchesCategory = !selectedCategory || item.category === selectedCategory;

    return matchesSearch && matchesCategory;
  });

  const categories = Array.from(new Set(items.map(item => item.category).filter(Boolean)));

  // Removed the community requirement check - items can be viewed across all communities

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
        <div className="text-red-600 mb-4">{t('items.loadError')}</div>
        <button
          onClick={() => refetch()}
          className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
        >
          {t('common.tryAgain')}
        </button>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">
            {currentCommunity ? `Items in ${currentCommunity.name}` : 'All Items'}
          </h1>
          <p className="text-gray-600">
            {filteredItems.length} item{filteredItems.length !== 1 ? 's' : ''} available
          </p>
        </div>

        <div className="flex items-center space-x-3">
          <button
            onClick={() => setShowSearch(!showSearch)}
            className="inline-flex items-center px-3 py-2 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 bg-white hover:bg-gray-50"
          >
            <Search className="h-4 w-4 mr-2" />
{t('items.search.placeholder')}
          </button>

          <button
            onClick={() => navigate('/items/create')}
            className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-blue-600 hover:bg-blue-700"
          >
            <Plus className="h-4 w-4 mr-2" />
{t('items.addButton')}
          </button>
        </div>
      </div>

      {/* Search and Filters */}
      {showSearch && (
        <div className="bg-gray-50 rounded-lg p-4 space-y-4">
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <input
                type="text"
                placeholder={t('items.search.placeholder')}
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className="block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
              />
            </div>

            <div>
              <select
                value={selectedCategory}
                onChange={(e) => setSelectedCategory(e.target.value)}
                className="block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
              >
                <option value="">{t('items.search.allCategories')}</option>
                {categories.map(category => (
                  <option key={category} value={category}>
                    {category}
                  </option>
                ))}
              </select>
            </div>
          </div>

          {(searchQuery || selectedCategory) && (
            <div className="flex items-center justify-between">
              <span className="text-sm text-gray-600">
                {filteredItems.length} result{filteredItems.length !== 1 ? 's' : ''} found
              </span>
              <button
                onClick={() => {
                  setSearchQuery('');
                  setSelectedCategory('');
                }}
                className="text-sm text-blue-600 hover:text-blue-700"
              >
{t('items.search.clearAll')}
              </button>
            </div>
          )}
        </div>
      )}

      {/* Items Grid */}
      {filteredItems.length === 0 ? (
        <div className="text-center py-12">
          <div className="mx-auto w-24 h-24 bg-gray-100 rounded-full flex items-center justify-center mb-4">
            {searchQuery || selectedCategory ? (
              <Search className="h-12 w-12 text-gray-400" />
            ) : (
              <Plus className="h-12 w-12 text-gray-400" />
            )}
          </div>
          <h3 className="text-lg font-medium text-gray-900 mb-2">
            {searchQuery || selectedCategory ? t('items.noItemsTitle') : t('items.myTitle')}
          </h3>
          <p className="text-gray-600 mb-6">
            {searchQuery || selectedCategory
              ? 'Try adjusting your search terms or filters'
              : 'Be the first to add an item to share with your community'
            }
          </p>
          {!searchQuery && !selectedCategory && (
            <button
              onClick={() => navigate('/items/create')}
              className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-blue-600 hover:bg-blue-700"
            >
              <Plus className="h-4 w-4 mr-2" />
{t('items.addButton')}
            </button>
          )}
        </div>
      ) : (
        <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
          {filteredItems.map((item) => (
            <ItemCard key={item.id} item={item} />
          ))}
        </div>
      )}
    </div>
  );
};

export default ItemList;