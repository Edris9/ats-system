export interface User {
  id: string;
  email: string;
  fullName: string;
  role: 'admin' | 'customer';
  companyId: string;
  token: string;
}

export interface Company {
  id: string;
  name: string;
  createdAt: string;
}

export interface Job {
  id: string;
  title: string;
  description?: string;
  location?: string;
  afJobId?: string;
  afJobUrl?: string;
  companyId: string;
  createdAt: string;
}

export interface Candidate {
  id: string;
  fullName: string;
  email?: string;
  phone?: string;
  linkedinUrl?: string;
  status: 'new' | 'interview' | 'offer' | 'rejected';
  notes?: string;
  jobId: string;
  createdAt: string;
}

export interface AfJobSearchResult {
  id: string;
  headline: string;
  description?: string;
  workplaceName?: string;
  municipality?: string;
  url?: string;
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface CreateJobDto {
  title: string;
  description?: string;
  location?: string;
  afJobId?: string;
  afJobUrl?: string;
}

export interface CreateCandidateDto {
  fullName: string;
  email?: string;
  phone?: string;
  linkedinUrl?: string;
  notes?: string;
  jobId: string;
}

export interface CreateUserDto {
  email: string;
  password: string;
  fullName: string;
  role: 'admin' | 'customer';
  companyId?: string;
  companyName?: string;
}