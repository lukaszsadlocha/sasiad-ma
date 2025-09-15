import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { Upload, Users, MapPin, FileText } from 'lucide-react';
import toast from 'react-hot-toast';
import { useApiMutation } from '../../hooks/useApi';
import { communityService } from '../../services/communityService';
import type { CreateCommunityRequest } from '../../types/community';
import LoadingSpinner from '../common/LoadingSpinner';

interface CommunityFormData {
  name: string;
  description: string;
  location: string;
  isPrivate: boolean;
}

const CreateCommunityForm: React.FC = () => {
  const navigate = useNavigate();
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [imagePreview, setImagePreview] = useState<string | null>(null);

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<CommunityFormData>();

  const createCommunityMutation = useApiMutation(
    communityService.create,
    {
      onSuccess: (community) => {
        toast.success('Community created successfully!');
        navigate(`/communities/${community.id}`);
      },
      onError: (error: any) => {
        const message = error?.response?.data?.message || 'Failed to create community';
        toast.error(message);
      },
    }
  );

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      if (file.size > 5 * 1024 * 1024) { // 5MB limit
        toast.error('Image size must be less than 5MB');
        return;
      }

      setImageFile(file);
      const reader = new FileReader();
      reader.onload = () => {
        setImagePreview(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  };

  const onSubmit = async (data: CommunityFormData) => {
    try {
      const communityData: CreateCommunityRequest = {
        name: data.name,
        description: data.description,
        location: data.location,
        isPrivate: data.isPrivate,
      };

      await createCommunityMutation.mutateAsync(communityData);
    } catch (error) {
      // Error handled by mutation
    }
  };

  return (
    <div className="max-w-2xl mx-auto">
      <div className="bg-white shadow rounded-lg">
        <div className="px-6 py-4 border-b border-gray-200">
          <h2 className="text-lg font-medium text-gray-900">Create New Community</h2>
          <p className="mt-1 text-sm text-gray-600">
            Start a new community to share items with your neighbors
          </p>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="p-6 space-y-6">
          {/* Community Image */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Community Image (Optional)
            </label>
            <div className="flex items-center space-x-4">
              <div className="w-24 h-24 bg-gray-100 rounded-lg flex items-center justify-center overflow-hidden">
                {imagePreview ? (
                  <img
                    src={imagePreview}
                    alt="Preview"
                    className="w-full h-full object-cover"
                  />
                ) : (
                  <Users className="h-8 w-8 text-gray-400" />
                )}
              </div>
              <div>
                <label
                  htmlFor="image-upload"
                  className="cursor-pointer inline-flex items-center px-3 py-2 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 bg-white hover:bg-gray-50"
                >
                  <Upload className="h-4 w-4 mr-2" />
                  Upload Image
                </label>
                <input
                  id="image-upload"
                  type="file"
                  accept="image/*"
                  onChange={handleImageChange}
                  className="hidden"
                />
                <p className="mt-1 text-xs text-gray-500">
                  PNG, JPG up to 5MB
                </p>
              </div>
            </div>
          </div>

          {/* Community Name */}
          <div>
            <label htmlFor="name" className="block text-sm font-medium text-gray-700">
              Community Name *
            </label>
            <div className="mt-1">
              <input
                {...register('name', {
                  required: 'Community name is required',
                  minLength: {
                    value: 3,
                    message: 'Name must be at least 3 characters',
                  },
                  maxLength: {
                    value: 50,
                    message: 'Name must be less than 50 characters',
                  },
                })}
                type="text"
                className="block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
                placeholder="Enter community name"
              />
              {errors.name && (
                <p className="mt-1 text-sm text-red-600">{errors.name.message}</p>
              )}
            </div>
          </div>

          {/* Description */}
          <div>
            <label htmlFor="description" className="block text-sm font-medium text-gray-700">
              Description *
            </label>
            <div className="mt-1">
              <textarea
                {...register('description', {
                  required: 'Description is required',
                  minLength: {
                    value: 10,
                    message: 'Description must be at least 10 characters',
                  },
                  maxLength: {
                    value: 500,
                    message: 'Description must be less than 500 characters',
                  },
                })}
                rows={4}
                className="block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
                placeholder="Describe your community, its purpose, and what makes it special"
              />
              {errors.description && (
                <p className="mt-1 text-sm text-red-600">{errors.description.message}</p>
              )}
            </div>
          </div>

          {/* Location */}
          <div>
            <label htmlFor="location" className="block text-sm font-medium text-gray-700">
              Location *
            </label>
            <div className="mt-1 relative">
              <MapPin className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
              <input
                {...register('location', {
                  required: 'Location is required',
                  minLength: {
                    value: 5,
                    message: 'Location must be at least 5 characters',
                  },
                })}
                type="text"
                className="block w-full pl-10 rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
                placeholder="e.g. Downtown Austin, Oak Street Neighborhood"
              />
              {errors.location && (
                <p className="mt-1 text-sm text-red-600">{errors.location.message}</p>
              )}
            </div>
          </div>

          {/* Privacy Setting */}
          <div>
            <div className="flex items-start">
              <div className="flex items-center h-5">
                <input
                  {...register('isPrivate')}
                  type="checkbox"
                  className="focus:ring-blue-500 h-4 w-4 text-blue-600 border-gray-300 rounded"
                />
              </div>
              <div className="ml-3 text-sm">
                <label htmlFor="isPrivate" className="font-medium text-gray-700">
                  Private Community
                </label>
                <p className="text-gray-500">
                  Only invited members can join this community. Otherwise, anyone with the invitation link can join.
                </p>
              </div>
            </div>
          </div>

          {/* Form Actions */}
          <div className="flex justify-end space-x-3 pt-6 border-t border-gray-200">
            <button
              type="button"
              onClick={() => navigate('/communities')}
              className="px-4 py-2 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={createCommunityMutation.isPending}
              className="inline-flex items-center px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {createCommunityMutation.isPending ? (
                <>
                  <LoadingSpinner className="w-4 h-4 mr-2" />
                  Creating...
                </>
              ) : (
                'Create Community'
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateCommunityForm;