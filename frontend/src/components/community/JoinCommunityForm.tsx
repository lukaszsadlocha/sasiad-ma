import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { toast } from 'react-hot-toast';
import { communityService } from '../../services/communityService';
import { JoinCommunityRequest } from '../../types/community';
import { Users, ArrowLeft, Key } from 'lucide-react';
import LoadingSpinner from '../common/LoadingSpinner';

const JoinCommunityForm: React.FC = () => {
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors }
  } = useForm<JoinCommunityRequest>();

  const onSubmit = async (data: JoinCommunityRequest) => {
    try {
      setIsLoading(true);
      await communityService.join(data);
      toast.success('Successfully joined community!');
      navigate('/communities');
    } catch (error: any) {
      toast.error(error.message || 'Failed to join community');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="max-w-md mx-auto space-y-6">
      {/* Header */}
      <div className="text-center">
        <div className="mx-auto h-12 w-12 bg-blue-500 rounded-full flex items-center justify-center mb-4">
          <Users className="h-6 w-6 text-white" />
        </div>
        <h1 className="text-2xl font-bold text-gray-900">Join Community</h1>
        <p className="mt-2 text-gray-600">
          Enter an invitation code to join an existing community
        </p>
      </div>

      {/* Form */}
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <div>
          <label htmlFor="invitationCode" className="block text-sm font-medium text-gray-700">
            Invitation Code
          </label>
          <div className="mt-1 relative">
            <Key className="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400" />
            <input
              {...register('invitationCode', {
                required: 'Invitation code is required',
                minLength: {
                  value: 3,
                  message: 'Invitation code must be at least 3 characters'
                }
              })}
              type="text"
              placeholder="Enter invitation code"
              className="pl-10 appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
            />
            {errors.invitationCode && (
              <p className="mt-1 text-sm text-red-600">{errors.invitationCode.message}</p>
            )}
          </div>
        </div>

        <div className="flex space-x-3">
          <button
            type="button"
            onClick={() => navigate('/communities')}
            className="flex-1 inline-flex justify-center items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
          >
            <ArrowLeft className="h-4 w-4 mr-2" />
            Back
          </button>
          <button
            type="submit"
            disabled={isLoading}
            className="flex-1 inline-flex justify-center items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {isLoading ? (
              <LoadingSpinner size="sm" />
            ) : (
              <>
                <Users className="h-4 w-4 mr-2" />
                Join Community
              </>
            )}
          </button>
        </div>
      </form>

      {/* Help Text */}
      <div className="text-center text-sm text-gray-500">
        <p>Don't have an invitation code?</p>
        <button
          onClick={() => navigate('/communities/create')}
          className="text-blue-600 hover:text-blue-500 font-medium"
        >
          Create your own community
        </button>
      </div>
    </div>
  );
};

export default JoinCommunityForm;