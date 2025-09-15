import { useState, useCallback } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { api } from '../services/api';

interface UseApiOptions {
  onSuccess?: (data: any) => void;
  onError?: (error: any) => void;
}

export const useApi = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const queryClient = useQueryClient();

  const execute = useCallback(async (apiCall: () => Promise<any>, options?: UseApiOptions) => {
    setLoading(true);
    setError(null);

    try {
      const result = await apiCall();
      options?.onSuccess?.(result);
      return result;
    } catch (err: any) {
      const errorMessage = err.response?.data?.message || err.message || 'An error occurred';
      setError(errorMessage);
      options?.onError?.(err);
      throw err;
    } finally {
      setLoading(false);
    }
  }, []);

  return { execute, loading, error, queryClient };
};

export const useApiQuery = <T>(
  key: string[],
  apiCall: () => Promise<T>,
  options?: {
    enabled?: boolean;
    refetchOnWindowFocus?: boolean;
    retry?: number;
  }
) => {
  return useQuery({
    queryKey: key,
    queryFn: apiCall,
    enabled: options?.enabled ?? true,
    refetchOnWindowFocus: options?.refetchOnWindowFocus ?? false,
    retry: options?.retry ?? 1,
  });
};

export const useApiMutation = <T, V>(
  apiCall: (variables: V) => Promise<T>,
  options?: UseApiOptions & {
    invalidateQueries?: string[];
  }
) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: apiCall,
    onSuccess: (data, variables) => {
      options?.onSuccess?.(data);
      if (options?.invalidateQueries) {
        options.invalidateQueries.forEach(queryKey => {
          queryClient.invalidateQueries({ queryKey: [queryKey] });
        });
      }
    },
    onError: (error) => {
      options?.onError?.(error);
    },
  });
};