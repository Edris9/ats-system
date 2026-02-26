import { useState, useEffect } from 'react';
import { getJobs, createJob, deleteJob } from '../api/jobs';
import type { Job, CreateJobDto } from '../types';

export const useJobs = () => {
  const [jobs, setJobs] = useState<Job[]>([]);
  const [loading, setLoading] = useState(false);

  const fetchJobs = async () => {
    setLoading(true);
    try {
      const data = await getJobs();
      setJobs(data);
    } finally {
      setLoading(false);
    }
  };

  const addJob = async (dto: CreateJobDto) => {
    const job = await createJob(dto);
    setJobs((prev) => [job, ...prev]);
    return job;
  };

  const removeJob = async (id: string) => {
    await deleteJob(id);
    setJobs((prev) => prev.filter((j) => j.id !== id));
  };

  useEffect(() => {
    fetchJobs();
  }, []);

  return { jobs, loading, fetchJobs, addJob, removeJob };
};