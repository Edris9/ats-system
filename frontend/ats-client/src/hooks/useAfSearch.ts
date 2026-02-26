import { useState } from 'react';
import { searchJobs } from '../api/arbetsformedlingen';
import type { AfJobSearchResult } from '../types';

export const useAfSearch = () => {
  const [results, setResults] = useState<AfJobSearchResult[]>([]);
  const [loading, setLoading] = useState(false);

  const search = async (query: string) => {
    setLoading(true);
    try {
      const data = await searchJobs(query);
      setResults(data);
    } finally {
      setLoading(false);
    }
  };

  const clear = () => setResults([]);

  return { results, loading, search, clear };
};