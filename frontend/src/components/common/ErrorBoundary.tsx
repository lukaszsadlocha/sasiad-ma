import React, { Component, ErrorInfo, ReactNode } from 'react';
import { AlertCircle, RefreshCw } from 'lucide-react';
import { useTranslation } from 'react-i18next';

interface Props {
  children: ReactNode;
  fallback?: ReactNode;
}

interface State {
  hasError: boolean;
  error?: Error;
  errorInfo?: ErrorInfo;
}

class ErrorBoundary extends Component<Props, State> {
  constructor(props: Props) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError(error: Error): State {
    return { hasError: true, error };
  }

  componentDidCatch(error: Error, errorInfo: ErrorInfo) {
    console.error('ErrorBoundary caught an error:', error, errorInfo);
    this.setState({ error, errorInfo });
  }

  handleReset = () => {
    this.setState({ hasError: false, error: undefined, errorInfo: undefined });
  };

  render() {
    if (this.state.hasError) {
      if (this.props.fallback) {
        return this.props.fallback;
      }

      return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50">
          <div className="max-w-md w-full bg-white rounded-lg shadow-lg p-6">
            <div className="flex items-center justify-center w-12 h-12 mx-auto bg-red-100 rounded-full">
              <AlertCircle className="w-6 h-6 text-red-600" />
            </div>

            <div className="mt-4 text-center">
              <ErrorBoundaryContent error={this.state.error} errorInfo={this.state.errorInfo} onReset={this.handleReset} />
            </div>

          </div>
        </div>
      );
    }

    return this.props.children;
  }
}

interface ErrorBoundaryContentProps {
  error?: Error;
  errorInfo?: ErrorInfo;
  onReset: () => void;
}

const ErrorBoundaryContent: React.FC<ErrorBoundaryContentProps> = ({ error, errorInfo, onReset }) => {
  const { t } = useTranslation();

  return (
    <>
      <h1 className="text-lg font-semibold text-gray-900">
        {t('errorBoundary.title')}
      </h1>
      <p className="mt-2 text-sm text-gray-600">
        {t('errorBoundary.message')}
      </p>

      {process.env.NODE_ENV === 'development' && error && (
        <div className="mt-4 p-3 bg-gray-100 rounded text-xs">
          <details>
            <summary className="cursor-pointer font-medium text-gray-700">
              {t('errorBoundary.errorDetails')}
            </summary>
            <div className="mt-2 whitespace-pre-wrap text-red-600">
              {error.toString()}
              {errorInfo?.componentStack}
            </div>
          </details>
        </div>
      )}

      <div className="mt-6 flex gap-3">
        <button
          onClick={() => window.location.reload()}
          className="flex-1 inline-flex justify-center items-center px-4 py-2 border border-gray-300 rounded-md shadow-sm bg-white text-sm font-medium text-gray-700 hover:bg-gray-50"
        >
          <RefreshCw className="w-4 h-4 mr-2" />
          {t('errorBoundary.refreshPage')}
        </button>
        <button
          onClick={onReset}
          className="flex-1 inline-flex justify-center items-center px-4 py-2 border border-transparent rounded-md shadow-sm bg-blue-600 text-sm font-medium text-white hover:bg-blue-700"
        >
          {t('errorBoundary.tryAgain')}
        </button>
      </div>
    </>
  );
};

export default ErrorBoundary;