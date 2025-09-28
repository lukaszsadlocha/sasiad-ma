import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useForm, useFieldArray } from 'react-hook-form';
import { useApiMutation, useApiQuery } from '../../hooks/useApi';
import { itemService } from '../../services/itemService';
import { communityService } from '../../services/communityService';
import { useCommunity } from '../../context/CommunityContext';
import LoadingSpinner from '../common/LoadingSpinner';
import { ArrowLeft, AlertCircle, Plus, X, Upload } from 'lucide-react';
import toast from 'react-hot-toast';
import type { CreateItemRequest } from '../../types/item';

interface CreateItemFormData {
  name: string;
  description: string;
  category: string;
  condition: string;
  communityId: string;
  imageUrls: { url: string }[];
}

const CreateItemForm: React.FC = () => {
  const navigate = useNavigate();
  const { t } = useTranslation();
  const { currentCommunity } = useCommunity();

  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
    reset
  } = useForm<CreateItemFormData>({
    defaultValues: {
      communityId: currentCommunity?.id || '',
      imageUrls: []
    }
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: 'imageUrls'
  });

  const { data: communities = [], isLoading: loadingCommunities } = useApiQuery(
    ['communities'],
    () => communityService.getUserCommunities()
  );

  const createItemMutation = useApiMutation(
    (data: CreateItemRequest) => itemService.create(data),
    {
      onSuccess: () => {
        console.log('Item created successfully');
        toast.success(t('items.create.successMessage'));
        navigate('/items');
        reset();
      },
      onError: (error: any) => {
        console.error('Failed to create item:', error);
        console.error('Error details:', {
          message: error.message,
          code: error.code,
          response: error.response?.data,
          status: error.response?.status
        });

        // Better error messages based on error type
        if (error.response?.status === 401) {
          toast.error('You need to be logged in to create items. Please log in and try again.');
        } else if (error.response?.status === 403) {
          toast.error('You do not have permission to create items in this community.');
        } else if (error.response?.status === 400) {
          toast.error(error.response?.data?.message || 'Invalid item data. Please check your input.');
        } else {
          toast.error(error.message || t('items.create.errorMessage'));
        }
      }
    }
  );

  const onSubmit = (data: CreateItemFormData) => {
    if (!data.communityId) {
      toast.error('Please select a community for your item.');
      return;
    }

    createItemMutation.mutate({
      name: data.name.trim(),
      description: data.description.trim(),
      category: data.category,
      condition: data.condition,
      communityId: data.communityId,
      imageUrls: data.imageUrls.map(img => img.url).filter(url => url.trim() !== '')
    });
  };

  const categories = [
    'Tools',
    'Electronics',
    'Sports',
    'Garden',
    'Kitchen',
    'Books',
    'Toys',
    'Furniture',
    'Clothing',
    'Other'
  ];

  const conditions = [
    'New',
    'Excellent',
    'Good',
    'Fair',
    'Poor'
  ];

  if (loadingCommunities) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="flex justify-center">
          <LoadingSpinner size="lg" />
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="max-w-2xl mx-auto">
        {/* Header */}
        <div className="flex items-center mb-6">
          <button
            onClick={() => navigate('/items')}
            className="mr-4 p-2 text-gray-400 hover:text-gray-600 rounded-full hover:bg-gray-100"
          >
            <ArrowLeft className="h-5 w-5" />
          </button>
          <div>
            <h1 className="text-2xl font-bold text-gray-900">{t('items.create.title')}</h1>
            <p className="text-gray-600">{t('items.create.subtitle')}</p>
          </div>
        </div>

        {/* Form */}
        <div className="bg-white rounded-lg shadow border border-gray-200">
          <form onSubmit={handleSubmit(onSubmit)} className="p-6 space-y-6">
            {/* Item Name */}
            <div>
              <label htmlFor="name" className="block text-sm font-medium text-gray-700 mb-2">
                {t('items.create.nameLabel')}
              </label>
              <input
                type="text"
                id="name"
                {...register('name', {
                  required: t('items.create.nameRequired'),
                  minLength: {
                    value: 2,
                    message: t('items.create.nameMinLength')
                  },
                  maxLength: {
                    value: 100,
                    message: t('items.create.nameMaxLength')
                  }
                })}
                placeholder={t('items.create.namePlaceholder')}
                className={`block w-full rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 ${
                  errors.name
                    ? 'border-red-300 text-red-900 placeholder-red-300'
                    : 'border-gray-300'
                }`}
              />
              {errors.name && (
                <div className="mt-1 flex items-center text-sm text-red-600">
                  <AlertCircle className="h-4 w-4 mr-1" />
                  <span>{errors.name.message}</span>
                </div>
              )}
            </div>

            {/* Description */}
            <div>
              <label htmlFor="description" className="block text-sm font-medium text-gray-700 mb-2">
                {t('items.create.descriptionLabel')}
              </label>
              <textarea
                id="description"
                rows={4}
                {...register('description', {
                  required: t('items.create.descriptionRequired'),
                  minLength: {
                    value: 10,
                    message: t('items.create.descriptionMinLength')
                  },
                  maxLength: {
                    value: 500,
                    message: t('items.create.descriptionMaxLength')
                  }
                })}
                placeholder={t('items.create.descriptionPlaceholder')}
                className={`block w-full rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 ${
                  errors.description
                    ? 'border-red-300 text-red-900 placeholder-red-300'
                    : 'border-gray-300'
                }`}
              />
              {errors.description && (
                <div className="mt-1 flex items-center text-sm text-red-600">
                  <AlertCircle className="h-4 w-4 mr-1" />
                  <span>{errors.description.message}</span>
                </div>
              )}
            </div>

            {/* Category and Condition Row */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              {/* Category */}
              <div>
                <label htmlFor="category" className="block text-sm font-medium text-gray-700 mb-2">
                  {t('items.create.categoryLabel')}
                </label>
                <select
                  id="category"
                  {...register('category', {
                    required: t('items.create.categoryRequired')
                  })}
                  className={`block w-full rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 ${
                    errors.category
                      ? 'border-red-300 text-red-900'
                      : 'border-gray-300'
                  }`}
                >
                  <option value="">{t('items.create.categoryPlaceholder')}</option>
                  {categories.map(category => (
                    <option key={category} value={category}>
                      {t(`items.create.categories.${category.toLowerCase()}`)}
                    </option>
                  ))}
                </select>
                {errors.category && (
                  <div className="mt-1 flex items-center text-sm text-red-600">
                    <AlertCircle className="h-4 w-4 mr-1" />
                    <span>{errors.category.message}</span>
                  </div>
                )}
              </div>

              {/* Condition */}
              <div>
                <label htmlFor="condition" className="block text-sm font-medium text-gray-700 mb-2">
                  {t('items.create.conditionLabel')}
                </label>
                <select
                  id="condition"
                  {...register('condition', {
                    required: t('items.create.conditionRequired')
                  })}
                  className={`block w-full rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 ${
                    errors.condition
                      ? 'border-red-300 text-red-900'
                      : 'border-gray-300'
                  }`}
                >
                  <option value="">{t('items.create.conditionPlaceholder')}</option>
                  {conditions.map(condition => (
                    <option key={condition} value={condition}>
                      {t(`items.create.conditions.${condition.toLowerCase()}`)}
                    </option>
                  ))}
                </select>
                {errors.condition && (
                  <div className="mt-1 flex items-center text-sm text-red-600">
                    <AlertCircle className="h-4 w-4 mr-1" />
                    <span>{errors.condition.message}</span>
                  </div>
                )}
              </div>
            </div>

            {/* Community */}
            <div>
              <label htmlFor="communityId" className="block text-sm font-medium text-gray-700 mb-2">
                {t('items.create.communityLabel')}
              </label>
              <select
                id="communityId"
                {...register('communityId', {
                  required: t('items.create.communityRequired')
                })}
                className={`block w-full rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 ${
                  errors.communityId
                    ? 'border-red-300 text-red-900'
                    : 'border-gray-300'
                }`}
              >
                <option value="">{t('items.create.communityPlaceholder')}</option>
                {communities.map(community => (
                  <option key={community.id} value={community.id}>
                    {community.name}
                  </option>
                ))}
              </select>
              {errors.communityId && (
                <div className="mt-1 flex items-center text-sm text-red-600">
                  <AlertCircle className="h-4 w-4 mr-1" />
                  <span>{errors.communityId.message}</span>
                </div>
              )}
            </div>

            {/* Image URLs */}
            <div>
              <div className="flex items-center justify-between mb-3">
                <label className="block text-sm font-medium text-gray-700">
                  {t('items.create.imagesLabel')}
                </label>
                <button
                  type="button"
                  onClick={() => append({ url: '' })}
                  className="inline-flex items-center px-3 py-1 text-sm font-medium text-blue-600 bg-blue-50 border border-blue-200 rounded-md hover:bg-blue-100 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
                >
                  <Plus className="h-4 w-4 mr-1" />
                  {t('items.create.addImage')}
                </button>
              </div>

              {fields.length === 0 && (
                <p className="text-sm text-gray-500 mb-3">
                  {t('items.create.noImagesAdded')}
                </p>
              )}

              <div className="space-y-3">
                {fields.map((field, index) => (
                  <div key={field.id} className="flex items-center space-x-2">
                    <div className="flex-1">
                      <input
                        type="url"
                        placeholder={t('items.create.imageUrlPlaceholder')}
                        {...register(`imageUrls.${index}.url` as const, {
                          pattern: {
                            value: /^https?:\/\/.+/i,
                            message: t('items.create.invalidImageUrl')
                          }
                        })}
                        className="block w-full rounded-md border-gray-300 shadow-sm focus:ring-blue-500 focus:border-blue-500"
                      />
                      {errors.imageUrls?.[index]?.url && (
                        <div className="mt-1 flex items-center text-sm text-red-600">
                          <AlertCircle className="h-4 w-4 mr-1" />
                          <span>{errors.imageUrls[index]?.url?.message}</span>
                        </div>
                      )}
                    </div>
                    <button
                      type="button"
                      onClick={() => remove(index)}
                      className="p-2 text-red-400 hover:text-red-600 rounded-full hover:bg-red-50"
                    >
                      <X className="h-4 w-4" />
                    </button>
                  </div>
                ))}
              </div>

              {fields.length > 0 && (
                <p className="text-xs text-gray-500 mt-2">
                  {t('items.create.imageUrlHint')}
                </p>
              )}
            </div>

            {/* Form Actions */}
            <div className="flex items-center justify-end space-x-3 pt-4 border-t">
              <button
                type="button"
                onClick={() => navigate('/items')}
                className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md shadow-sm hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
              >
                {t('items.create.cancel')}
              </button>
              <button
                type="submit"
                disabled={createItemMutation.isPending}
                className="px-4 py-2 text-sm font-medium text-white bg-blue-600 border border-transparent rounded-md shadow-sm hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed flex items-center"
              >
                {createItemMutation.isPending && (
                  <LoadingSpinner size="sm" className="mr-2" />
                )}
                {createItemMutation.isPending ? t('items.create.creating') : t('items.create.createButton')}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default CreateItemForm;