import React, { useState, useEffect } from 'react';
import { Search, Filter, X } from 'lucide-react';
import { useDebounce } from '../../hooks/useDebounce';
import type { ItemSearchRequest } from '../../types/item';

interface ItemSearchProps {
  onSearch: (params: ItemSearchRequest) => void;
  onClear: () => void;
  availableCategories?: string[];
  className?: string;
}

const ItemSearch: React.FC<ItemSearchProps> = ({
  onSearch,
  onClear,
  availableCategories = [],
  className = '',
}) => {
  const [searchTerm, setSearchTerm] = useState('');
  const [category, setCategory] = useState('');
  const [condition, setCondition] = useState('');
  const [status, setStatus] = useState('');
  const [showFilters, setShowFilters] = useState(false);

  const debouncedSearchTerm = useDebounce(searchTerm, 300);

  // Trigger search when debounced search term or filters change
  useEffect(() => {
    const searchParams: ItemSearchRequest = {
      query: debouncedSearchTerm || undefined,
      category: category || undefined,
      condition: condition || undefined,
      status: status || undefined,
    };

    // Only trigger search if at least one parameter is provided
    if (Object.values(searchParams).some(value => value !== undefined)) {
      onSearch(searchParams);
    } else if (!debouncedSearchTerm && !category && !condition && !status) {
      onClear();
    }
  }, [debouncedSearchTerm, category, condition, status, onSearch, onClear]);

  const handleClearAll = () => {
    setSearchTerm('');
    setCategory('');
    setCondition('');
    setStatus('');
    onClear();
  };

  const hasActiveFilters = searchTerm || category || condition || status;

  return (
    <div className={`bg-white rounded-lg border border-gray-200 ${className}`}>
      {/* Search Bar */}
      <div className="p-4 border-b border-gray-200">
        <div className="relative">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
          <input
            type="text"
            placeholder="Search items..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="block w-full pl-10 pr-4 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
          />
        </div>

        <div className="flex items-center justify-between mt-3">
          <button
            onClick={() => setShowFilters(!showFilters)}
            className="inline-flex items-center text-sm text-gray-600 hover:text-gray-900"
          >
            <Filter className="h-4 w-4 mr-1" />
            Filters
            {hasActiveFilters && (
              <span className="ml-1 inline-flex items-center justify-center px-2 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
                Active
              </span>
            )}
          </button>

          {hasActiveFilters && (
            <button
              onClick={handleClearAll}
              className="inline-flex items-center text-sm text-gray-600 hover:text-gray-900"
            >
              <X className="h-4 w-4 mr-1" />
              Clear All
            </button>
          )}
        </div>
      </div>

      {/* Filters */}
      {showFilters && (
        <div className="p-4 bg-gray-50 border-t border-gray-200">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            {/* Category Filter */}
            <div>
              <label htmlFor="category" className="block text-sm font-medium text-gray-700 mb-1">
                Category
              </label>
              <select
                id="category"
                value={category}
                onChange={(e) => setCategory(e.target.value)}
                className="block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
              >
                <option value="">All Categories</option>
                {availableCategories.map(cat => (
                  <option key={cat} value={cat}>
                    {cat}
                  </option>
                ))}
              </select>
            </div>

            {/* Condition Filter */}
            <div>
              <label htmlFor="condition" className="block text-sm font-medium text-gray-700 mb-1">
                Condition
              </label>
              <select
                id="condition"
                value={condition}
                onChange={(e) => setCondition(e.target.value)}
                className="block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
              >
                <option value="">Any Condition</option>
                <option value="New">New</option>
                <option value="Good">Good</option>
                <option value="Fair">Fair</option>
                <option value="Poor">Poor</option>
              </select>
            </div>

            {/* Status Filter */}
            <div>
              <label htmlFor="status" className="block text-sm font-medium text-gray-700 mb-1">
                Availability
              </label>
              <select
                id="status"
                value={status}
                onChange={(e) => setStatus(e.target.value)}
                className="block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
              >
                <option value="">Any Status</option>
                <option value="Available">Available</option>
                <option value="Borrowed">Borrowed</option>
                <option value="Unavailable">Unavailable</option>
              </select>
            </div>
          </div>

          {/* Applied Filters Summary */}
          {hasActiveFilters && (
            <div className="mt-4 pt-4 border-t border-gray-200">
              <div className="flex flex-wrap gap-2">
                {searchTerm && (
                  <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
                    Search: "{searchTerm}"
                    <button
                      onClick={() => setSearchTerm('')}
                      className="ml-1 inline-flex items-center justify-center w-4 h-4 rounded-full hover:bg-blue-200"
                    >
                      <X className="h-3 w-3" />
                    </button>
                  </span>
                )}
                {category && (
                  <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
                    Category: {category}
                    <button
                      onClick={() => setCategory('')}
                      className="ml-1 inline-flex items-center justify-center w-4 h-4 rounded-full hover:bg-green-200"
                    >
                      <X className="h-3 w-3" />
                    </button>
                  </span>
                )}
                {condition && (
                  <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-yellow-100 text-yellow-800">
                    Condition: {condition}
                    <button
                      onClick={() => setCondition('')}
                      className="ml-1 inline-flex items-center justify-center w-4 h-4 rounded-full hover:bg-yellow-200"
                    >
                      <X className="h-3 w-3" />
                    </button>
                  </span>
                )}
                {status && (
                  <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-purple-100 text-purple-800">
                    Status: {status}
                    <button
                      onClick={() => setStatus('')}
                      className="ml-1 inline-flex items-center justify-center w-4 h-4 rounded-full hover:bg-purple-200"
                    >
                      <X className="h-3 w-3" />
                    </button>
                  </span>
                )}
              </div>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default ItemSearch;