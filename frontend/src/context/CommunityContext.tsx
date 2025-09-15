import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { useApiQuery } from '../hooks/useApi';
import { communityService } from '../services/communityService';
import type { Community } from '../types/community';

interface CommunityContextType {
  currentCommunity: Community | null;
  userCommunities: Community[];
  setCurrentCommunity: (community: Community | null) => void;
  refreshCommunities: () => void;
  isLoading: boolean;
  error: string | null;
}

const CommunityContext = createContext<CommunityContextType | undefined>(undefined);

interface CommunityProviderProps {
  children: ReactNode;
}

export const CommunityProvider: React.FC<CommunityProviderProps> = ({ children }) => {
  const [currentCommunity, setCurrentCommunity] = useState<Community | null>(null);

  const {
    data: userCommunities = [],
    isLoading,
    error,
    refetch: refreshCommunities,
  } = useApiQuery(
    ['user-communities'],
    communityService.getUserCommunities
  );

  // Set default community if none selected and user has communities
  useEffect(() => {
    if (!currentCommunity && userCommunities.length > 0) {
      const savedCommunityId = localStorage.getItem('currentCommunityId');
      const savedCommunity = savedCommunityId
        ? userCommunities.find(c => c.id === savedCommunityId)
        : userCommunities[0];

      if (savedCommunity) {
        setCurrentCommunity(savedCommunity);
      }
    }
  }, [userCommunities, currentCommunity]);

  // Save current community to localStorage
  useEffect(() => {
    if (currentCommunity) {
      localStorage.setItem('currentCommunityId', currentCommunity.id);
    } else {
      localStorage.removeItem('currentCommunityId');
    }
  }, [currentCommunity]);

  const value: CommunityContextType = {
    currentCommunity,
    userCommunities,
    setCurrentCommunity,
    refreshCommunities,
    isLoading,
    error: error instanceof Error ? error.message : null,
  };

  return (
    <CommunityContext.Provider value={value}>
      {children}
    </CommunityContext.Provider>
  );
};

export const useCommunity = () => {
  const context = useContext(CommunityContext);
  if (context === undefined) {
    throw new Error('useCommunity must be used within a CommunityProvider');
  }
  return context;
};