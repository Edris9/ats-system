import { useState } from 'react';
import Navbar from '../components/Layout/Navbar';
import { useCandidates } from '../hooks/useCandidates';
import { useJobs } from './useJobs';
import { KANBAN_COLUMNS } from '../constants/kanbanColumns';
import type { Candidate, CreateCandidateDto } from '../types';

const KanbanPage = () => {
  const { candidates, loading, addCandidate, updateStatus } = useCandidates();
  const { jobs } = useJobs();
  const [filterJob, setFilterJob] = useState('');
  const [filterName, setFilterName] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [form, setForm] = useState<CreateCandidateDto>({
    fullName: '',
    email: '',
    phone: '',
    linkedinUrl: '',
    notes: '',
    jobId: '',
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await addCandidate(form);
    setForm({ fullName: '', email: '', phone: '', linkedinUrl: '', notes: '', jobId: '' });
    setShowForm(false);
  };

  const filteredCandidates = candidates.filter((c) => {
    const matchJob = filterJob ? c.jobId === filterJob : true;
    const matchName = filterName ? c.fullName.toLowerCase().includes(filterName.toLowerCase()) : true;
    return matchJob && matchName;
  });

  const getCandidatesByStatus = (status: string) =>
    filteredCandidates.filter((c) => c.status === status);

  const handleDragStart = (e: React.DragEvent, candidateId: string) => {
    e.dataTransfer.setData('candidateId', candidateId);
  };

  const handleDrop = async (e: React.DragEvent, status: string) => {
    e.preventDefault();
    const candidateId = e.dataTransfer.getData('candidateId');
    await updateStatus(candidateId, status);
  };

  const handleDragOver = (e: React.DragEvent) => {
    e.preventDefault();
  };

  return (
    <div className="min-h-screen bg-gray-100">
      <Navbar />
      <div className="p-8">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-2xl font-bold text-gray-800">Kanban</h2>
          <button
            onClick={() => setShowForm(!showForm)}
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
          >
            + Lägg till kandidat
          </button>
        </div>

        {/* Filter */}
        <div className="flex gap-4 mb-6">
          <select
            value={filterJob}
            onChange={(e) => setFilterJob(e.target.value)}
            className="border border-gray-300 rounded px-3 py-2 text-gray-900 bg-white"
          >
            <option value="">Alla jobb</option>
            {jobs.map((job) => (
              <option key={job.id} value={job.id}>{job.title}</option>
            ))}
          </select>
          <input
            type="text"
            placeholder="Filtrera på namn..."
            value={filterName}
            onChange={(e) => setFilterName(e.target.value)}
            className="border border-gray-300 rounded px-3 py-2 text-gray-900"
          />
        </div>

        {/* Lägg till kandidat form */}
        {showForm && (
          <div className="bg-white p-6 rounded-lg shadow mb-6">
            <h3 className="text-lg font-semibold mb-4">Lägg till kandidat</h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <select
                value={form.jobId}
                onChange={(e) => setForm({ ...form, jobId: e.target.value })}
                className="w-full border border-gray-300 rounded px-3 py-2 text-gray-900"
                required
              >
                <option value="">Välj jobb</option>
                {jobs.map((job) => (
                  <option key={job.id} value={job.id}>{job.title}</option>
                ))}
              </select>
              <input
                type="text"
                placeholder="Namn *"
                value={form.fullName}
                onChange={(e) => setForm({ ...form, fullName: e.target.value })}
                className="w-full border border-gray-300 rounded px-3 py-2 text-gray-900"
                required
              />
              <input
                type="email"
                placeholder="Email"
                value={form.email}
                onChange={(e) => setForm({ ...form, email: e.target.value })}
                className="w-full border border-gray-300 rounded px-3 py-2 text-gray-900"
              />
              <input
                type="text"
                placeholder="Telefon"
                value={form.phone}
                onChange={(e) => setForm({ ...form, phone: e.target.value })}
                className="w-full border border-gray-300 rounded px-3 py-2 text-gray-900"
              />
              <input
                type="url"
                placeholder="LinkedIn URL"
                value={form.linkedinUrl}
                onChange={(e) => setForm({ ...form, linkedinUrl: e.target.value })}
                className="w-full border border-gray-300 rounded px-3 py-2 text-gray-900"
              />
              <textarea
                placeholder="Anteckningar"
                value={form.notes}
                onChange={(e) => setForm({ ...form, notes: e.target.value })}
                className="w-full border border-gray-300 rounded px-3 py-2 text-gray-900"
                rows={2}
              />
              <div className="flex gap-3">
                <button type="submit" className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
                  Spara
                </button>
                <button type="button" onClick={() => setShowForm(false)} className="bg-gray-300 text-gray-700 px-4 py-2 rounded">
                  Avbryt
                </button>
              </div>
            </form>
          </div>
        )}

        {/* Kanban board */}
        {loading ? (
          <p className="text-gray-500">Laddar...</p>
        ) : (
          <div className="grid grid-cols-4 gap-4">
            {KANBAN_COLUMNS.map((col) => (
              <div
                key={col.id}
                onDrop={(e) => handleDrop(e, col.id)}
                onDragOver={handleDragOver}
                className="bg-gray-200 rounded-lg p-4 min-h-96"
              >
                <h3 className="font-semibold text-gray-700 mb-3">
                  {col.label} ({getCandidatesByStatus(col.id).length})
                </h3>
                <div className="space-y-3">
                  {getCandidatesByStatus(col.id).map((candidate) => (
                    <CandidateCard key={candidate.id} candidate={candidate} onDragStart={handleDragStart} jobs={jobs} />
                  ))}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

const CandidateCard = ({
  candidate,
  onDragStart,
  jobs,
}: {
  candidate: Candidate;
  onDragStart: (e: React.DragEvent, id: string) => void;
  jobs: { id: string; title: string }[];
}) => {
  const job = jobs.find((j) => j.id === candidate.jobId);

  return (
    <div
      draggable
      onDragStart={(e) => onDragStart(e, candidate.id)}
      className="bg-white rounded-lg p-4 shadow cursor-grab active:cursor-grabbing"
    >
      <h4 className="font-semibold text-gray-800">{candidate.fullName}</h4>
      <p className="text-xs text-blue-600 mt-1">{job?.title}</p>
      {candidate.email && <p className="text-xs text-gray-500 mt-1">{candidate.email}</p>}
      {candidate.linkedinUrl && (
        <a
          href={candidate.linkedinUrl}
          target="_blank"
          rel="noopener noreferrer"
          className="text-xs text-blue-500 hover:underline mt-1 block"
        >
          LinkedIn
        </a>
      )}
      {candidate.notes && <p className="text-xs text-gray-400 mt-2 italic">{candidate.notes}</p>}
    </div>
  );
};

export default KanbanPage;