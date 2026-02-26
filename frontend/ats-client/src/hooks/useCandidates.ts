import { useState, useEffect } from 'react';
import { getCandidates, createCandidate, updateCandidateStatus } from '../api/candidates';
import type { Candidate, CreateCandidateDto } from '../types';

export const useCandidates = () => {
  const [candidates, setCandidates] = useState<Candidate[]>([]);
  const [loading, setLoading] = useState(false);

  const fetchCandidates = async () => {
    setLoading(true);
    try {
      const data = await getCandidates();
      setCandidates(data);
    } finally {
      setLoading(false);
    }
  };

  const addCandidate = async (dto: CreateCandidateDto) => {
    const candidate = await createCandidate(dto);
    setCandidates((prev) => [candidate, ...prev]);
    return candidate;
  };

  const updateStatus = async (id: string, status: string) => {
    const updated = await updateCandidateStatus(id, status);
    setCandidates((prev) =>
      prev.map((c) => (c.id === id ? updated : c))
    );
  };

  useEffect(() => {
    fetchCandidates();
  }, []);

  return { candidates, loading, fetchCandidates, addCandidate, updateStatus };
};