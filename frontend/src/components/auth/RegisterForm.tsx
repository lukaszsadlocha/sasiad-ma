import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';
import { useAuth } from '../../hooks/useAuth';
import { RegisterRequest } from '../../types/auth';
import { Eye, EyeOff, Mail, Lock, User } from 'lucide-react';
import LoadingSpinner from '../common/LoadingSpinner';

const RegisterForm: React.FC = () => {
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const { register: registerUser, isLoading } = useAuth();
  const navigate = useNavigate();
  const { t } = useTranslation();

  const {
    register,
    handleSubmit,
    watch,
    formState: { errors }
  } = useForm<RegisterRequest>();

  const password = watch('password');

  const onSubmit = async (data: RegisterRequest) => {
    try {
      await registerUser(data);
      navigate('/dashboard');
    } catch (error) {
      // Error is handled by the context
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        <div>
          <div className="mx-auto h-12 w-12 bg-blue-500 rounded-full flex items-center justify-center">
            <User className="h-6 w-6 text-white" />
          </div>
          <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">
            {t('auth.register.title')}
          </h2>
          <p className="mt-2 text-center text-sm text-gray-600">
            {t('auth.register.subtitle')}{' '}
            <Link
              to="/login"
              className="font-medium text-blue-600 hover:text-blue-500"
            >
              {t('auth.register.signInExisting')}
            </Link>
          </p>
        </div>

        <form className="mt-8 space-y-6" onSubmit={handleSubmit(onSubmit)}>
          <div className="space-y-4">
            <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
              <div>
                <label htmlFor="firstName" className="block text-sm font-medium text-gray-700">
                  {t('auth.register.firstName')}
                </label>
                <div className="mt-1">
                  <input
                    {...register('firstName', {
                      required: t('auth.register.firstNameRequired'),
                      minLength: {
                        value: 2,
                        message: t('auth.register.firstNameMinLength')
                      }
                    })}
                    type="text"
                    autoComplete="given-name"
                    className="appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
                    placeholder={t('auth.register.enterFirstName')}
                  />
                  {errors.firstName && (
                    <p className="mt-1 text-sm text-red-600">{errors.firstName.message}</p>
                  )}
                </div>
              </div>

              <div>
                <label htmlFor="lastName" className="block text-sm font-medium text-gray-700">
                  {t('auth.register.lastName')}
                </label>
                <div className="mt-1">
                  <input
                    {...register('lastName', {
                      required: t('auth.register.lastNameRequired'),
                      minLength: {
                        value: 2,
                        message: t('auth.register.lastNameMinLength')
                      }
                    })}
                    type="text"
                    autoComplete="family-name"
                    className="appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
                    placeholder={t('auth.register.enterLastName')}
                  />
                  {errors.lastName && (
                    <p className="mt-1 text-sm text-red-600">{errors.lastName.message}</p>
                  )}
                </div>
              </div>
            </div>

            <div>
              <label htmlFor="email" className="block text-sm font-medium text-gray-700">
                {t('auth.register.email')}
              </label>
              <div className="mt-1 relative">
                <Mail className="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400" />
                <input
                  {...register('email', {
                    required: t('auth.register.emailRequired'),
                    pattern: {
                      value: /^\S+@\S+$/i,
                      message: t('auth.register.emailInvalid')
                    }
                  })}
                  type="email"
                  autoComplete="email"
                  className="pl-10 appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
                  placeholder={t('auth.register.enterEmail')}
                />
                {errors.email && (
                  <p className="mt-1 text-sm text-red-600">{errors.email.message}</p>
                )}
              </div>
            </div>

            <div>
              <label htmlFor="password" className="block text-sm font-medium text-gray-700">
                {t('auth.register.password')}
              </label>
              <div className="mt-1 relative">
                <Lock className="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400" />
                <input
                  {...register('password', {
                    required: t('auth.register.passwordRequired'),
                    minLength: {
                      value: 8,
                      message: t('auth.register.passwordMinLength')
                    },
                    pattern: {
                      value: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/,
                      message: t('auth.register.passwordStrength')
                    }
                  })}
                  type={showPassword ? 'text' : 'password'}
                  autoComplete="new-password"
                  className="pl-10 pr-10 appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
                  placeholder={t('auth.register.createPassword')}
                />
                <button
                  type="button"
                  className="absolute right-3 top-1/2 transform -translate-y-1/2"
                  onClick={() => setShowPassword(!showPassword)}
                >
                  {showPassword ? (
                    <EyeOff className="h-5 w-5 text-gray-400" />
                  ) : (
                    <Eye className="h-5 w-5 text-gray-400" />
                  )}
                </button>
                {errors.password && (
                  <p className="mt-1 text-sm text-red-600">{errors.password.message}</p>
                )}
              </div>
            </div>

            <div>
              <label htmlFor="confirmPassword" className="block text-sm font-medium text-gray-700">
                {t('auth.register.confirmPassword')}
              </label>
              <div className="mt-1 relative">
                <Lock className="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400" />
                <input
                  {...register('confirmPassword', {
                    required: t('auth.register.confirmPasswordRequired'),
                    validate: (value) => value === password || t('auth.register.passwordMismatch')
                  })}
                  type={showConfirmPassword ? 'text' : 'password'}
                  autoComplete="new-password"
                  className="pl-10 pr-10 appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
                  placeholder={t('auth.register.confirmYourPassword')}
                />
                <button
                  type="button"
                  className="absolute right-3 top-1/2 transform -translate-y-1/2"
                  onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                >
                  {showConfirmPassword ? (
                    <EyeOff className="h-5 w-5 text-gray-400" />
                  ) : (
                    <Eye className="h-5 w-5 text-gray-400" />
                  )}
                </button>
                {errors.confirmPassword && (
                  <p className="mt-1 text-sm text-red-600">{errors.confirmPassword.message}</p>
                )}
              </div>
            </div>
          </div>

          <div>
            <button
              type="submit"
              disabled={isLoading}
              className="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {isLoading ? <LoadingSpinner size="sm" /> : t('auth.register.createAccount')}
            </button>
          </div>

          <div className="text-center text-sm text-gray-600">
            {t('auth.register.termsAgreement')}{' '}
            <Link to="/terms" className="text-blue-600 hover:text-blue-500">
              {t('auth.register.termsOfService')}
            </Link>{' '}
            {t('auth.register.and')}{' '}
            <Link to="/privacy" className="text-blue-600 hover:text-blue-500">
              {t('auth.register.privacyPolicy')}
            </Link>
          </div>
        </form>
      </div>
    </div>
  );
};

export default RegisterForm;
