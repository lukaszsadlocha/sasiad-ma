import React, { useEffect, useState } from 'react';
import { useSearchParams, Link } from 'react-router-dom';
import { CheckCircle, XCircle, Mail, RefreshCw } from 'lucide-react';
import { authService } from '../../services/authService';
import LoadingSpinner from '../common/LoadingSpinner';

const EmailConfirmation: React.FC = () => {
  const [searchParams] = useSearchParams();
  const [status, setStatus] = useState<'loading' | 'success' | 'error' | 'expired'>('loading');
  const [message, setMessage] = useState('');
  const [canResend, setCanResend] = useState(false);
  const [resending, setResending] = useState(false);

  const token = searchParams.get('token');
  const email = searchParams.get('email');

  useEffect(() => {
    const confirmEmail = async () => {
      if (!token) {
        setStatus('error');
        setMessage('Invalid confirmation link. Please check your email for the correct link.');
        return;
      }

      try {
        await authService.confirmEmail(token);
        setStatus('success');
        setMessage('Your email has been confirmed successfully! You can now log in to your account.');
      } catch (error: any) {
        const errorMessage = error?.response?.data?.message || error?.message || 'Failed to confirm email';

        if (errorMessage.includes('expired') || errorMessage.includes('invalid')) {
          setStatus('expired');
          setMessage('This confirmation link has expired. Please request a new one.');
          setCanResend(true);
        } else {
          setStatus('error');
          setMessage(errorMessage);
        }
      }
    };

    confirmEmail();
  }, [token]);

  const handleResendConfirmation = async () => {
    if (!email) {
      setMessage('Email address not available. Please try registering again.');
      return;
    }

    setResending(true);
    try {
      // This would be a new endpoint for resending confirmation emails
      await authService.resendConfirmation?.(email);
      setMessage('A new confirmation email has been sent. Please check your inbox.');
      setCanResend(false);
    } catch (error: any) {
      setMessage('Failed to resend confirmation email. Please try again later.');
    } finally {
      setResending(false);
    }
  };

  const renderIcon = () => {
    switch (status) {
      case 'loading':
        return <LoadingSpinner className="w-12 h-12 text-blue-600" />;
      case 'success':
        return <CheckCircle className="w-12 h-12 text-green-600" />;
      case 'error':
      case 'expired':
        return <XCircle className="w-12 h-12 text-red-600" />;
      default:
        return <Mail className="w-12 h-12 text-gray-400" />;
    }
  };

  const renderTitle = () => {
    switch (status) {
      case 'loading':
        return 'Confirming your email...';
      case 'success':
        return 'Email confirmed!';
      case 'error':
        return 'Confirmation failed';
      case 'expired':
        return 'Link expired';
      default:
        return 'Email confirmation';
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        <div className="text-center">
          <div className="flex justify-center mb-4">
            {renderIcon()}
          </div>

          <h2 className="text-3xl font-extrabold text-gray-900">
            {renderTitle()}
          </h2>

          <p className="mt-4 text-gray-600">
            {message}
          </p>
        </div>

        <div className="space-y-4">
          {status === 'success' && (
            <div className="space-y-3">
              <Link
                to="/login"
                className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
              >
                Go to Login
              </Link>

              <Link
                to="/"
                className="w-full flex justify-center py-2 px-4 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
              >
                Back to Home
              </Link>
            </div>
          )}

          {status === 'expired' && canResend && (
            <div className="space-y-3">
              <button
                onClick={handleResendConfirmation}
                disabled={resending}
                className="w-full flex justify-center items-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {resending ? (
                  <>
                    <RefreshCw className="animate-spin -ml-1 mr-2 h-4 w-4" />
                    Sending...
                  </>
                ) : (
                  'Send new confirmation email'
                )}
              </button>

              <Link
                to="/register"
                className="w-full flex justify-center py-2 px-4 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
              >
                Back to Registration
              </Link>
            </div>
          )}

          {status === 'error' && (
            <div className="space-y-3">
              <Link
                to="/register"
                className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
              >
                Try Registration Again
              </Link>

              <Link
                to="/"
                className="w-full flex justify-center py-2 px-4 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
              >
                Back to Home
              </Link>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default EmailConfirmation;