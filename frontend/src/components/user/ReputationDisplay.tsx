import React from 'react';
import { Star, Award, TrendingUp } from 'lucide-react';
import { formatters } from '../../utils/formatters';

interface ReputationDisplayProps {
  score: number;
  totalTransactions?: number;
  showDetails?: boolean;
  className?: string;
}

const ReputationDisplay: React.FC<ReputationDisplayProps> = ({
  score,
  totalTransactions = 0,
  showDetails = true,
  className = '',
}) => {
  const getReputationLevel = (score: number) => {
    if (score >= 4.8) return { level: 'Excellent', color: 'text-green-600', bgColor: 'bg-green-100' };
    if (score >= 4.0) return { level: 'Great', color: 'text-blue-600', bgColor: 'bg-blue-100' };
    if (score >= 3.0) return { level: 'Good', color: 'text-yellow-600', bgColor: 'bg-yellow-100' };
    if (score >= 2.0) return { level: 'Fair', color: 'text-orange-600', bgColor: 'bg-orange-100' };
    if (score >= 1.0) return { level: 'Poor', color: 'text-red-600', bgColor: 'bg-red-100' };
    return { level: 'New', color: 'text-gray-600', bgColor: 'bg-gray-100' };
  };

  const reputation = getReputationLevel(score);

  const renderStars = (score: number) => {
    const fullStars = Math.floor(score);
    const hasHalfStar = score % 1 >= 0.5;
    const emptyStars = 5 - fullStars - (hasHalfStar ? 1 : 0);

    return (
      <div className="flex items-center">
        {/* Full stars */}
        {Array.from({ length: fullStars }, (_, i) => (
          <Star
            key={`full-${i}`}
            className="h-4 w-4 text-yellow-400 fill-current"
          />
        ))}

        {/* Half star */}
        {hasHalfStar && (
          <div className="relative">
            <Star className="h-4 w-4 text-gray-300" />
            <div className="absolute inset-0 overflow-hidden w-1/2">
              <Star className="h-4 w-4 text-yellow-400 fill-current" />
            </div>
          </div>
        )}

        {/* Empty stars */}
        {Array.from({ length: emptyStars }, (_, i) => (
          <Star
            key={`empty-${i}`}
            className="h-4 w-4 text-gray-300"
          />
        ))}
      </div>
    );
  };

  if (!showDetails) {
    return (
      <div className={`flex items-center space-x-2 ${className}`}>
        {renderStars(score)}
        <span className="text-sm font-medium text-gray-900">
          {score.toFixed(1)}
        </span>
        {totalTransactions > 0 && (
          <span className="text-sm text-gray-500">
            ({totalTransactions} transaction{totalTransactions !== 1 ? 's' : ''})
          </span>
        )}
      </div>
    );
  }

  return (
    <div className={`space-y-3 ${className}`}>
      {/* Reputation Score */}
      <div className="flex items-center justify-between">
        <div className="flex items-center space-x-3">
          <div className="flex items-center space-x-2">
            {renderStars(score)}
            <span className="text-lg font-semibold text-gray-900">
              {score.toFixed(1)}
            </span>
          </div>

          <span
            className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${reputation.bgColor} ${reputation.color}`}
          >
            <Award className="h-3 w-3 mr-1" />
            {reputation.level}
          </span>
        </div>

        {totalTransactions > 0 && (
          <div className="text-right">
            <div className="text-sm font-medium text-gray-900">
              {totalTransactions}
            </div>
            <div className="text-xs text-gray-500">
              transaction{totalTransactions !== 1 ? 's' : ''}
            </div>
          </div>
        )}
      </div>

      {/* Reputation Progress */}
      {score > 0 && (
        <div className="space-y-2">
          <div className="flex justify-between text-sm">
            <span className="text-gray-600">Reputation Progress</span>
            <span className="font-medium">{Math.round((score / 5) * 100)}%</span>
          </div>
          <div className="w-full bg-gray-200 rounded-full h-2">
            <div
              className="bg-blue-600 h-2 rounded-full transition-all duration-300"
              style={{ width: `${(score / 5) * 100}%` }}
            ></div>
          </div>
        </div>
      )}

      {/* Reputation Explanation */}
      {totalTransactions === 0 && (
        <div className="flex items-center space-x-2 text-sm text-gray-500">
          <TrendingUp className="h-4 w-4" />
          <span>Complete your first transaction to build reputation</span>
        </div>
      )}

      {score === 0 && totalTransactions > 0 && (
        <div className="text-sm text-gray-500">
          Your reputation will improve as you complete more transactions
        </div>
      )}
    </div>
  );
};

export default ReputationDisplay;