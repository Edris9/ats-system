import { useState } from 'react';
import Navbar from '../components/Layout/Navbar';
import { useJobs } from './useJobs';
import { useAfSearch } from '../hooks/useAfSearch';
import type { CreateJobDto } from '../types';

const JobsPage = () => {
  const { jobs, loading, addJob, removeJob } = useJobs();
  const { results, loading: afLoading, search, clear } = useAfSearch();
  const [showForm, setShowForm] = useState(false);
  const [showAfSearch, setShowAfSearch] = useState(false);
  const [afQuery, setAfQuery] = useState('');
  const [form, setForm] = useState<CreateJobDto>({
    title: '',
    description: '',
    location: '',
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await addJob(form);
    setForm({ title: '', description: '', location: '' });
    setShowForm(false);
  };

  const handleImport = async (result: typeof results[0]) => {
    await addJob({
      title: result.headline,
      description: result.description,
      location: result.municipality,
      afJobId: result.id,
      afJobUrl: result.url,
    });
    clear();
    setShowAfSearch(false);
  };

  return (
    <div className="min-h-screen bg-gray-100">
      <Navbar />
      <div className="p-8">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-2xl font-bold text-gray-800">Jobbannonser</h2>
          <div className="flex gap-3">
            <button
              onClick={() => setShowAfSearch(!showAfSearch)}
              className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
            >
              Sök på Arbetsförmedlingen
            </button>
            <button
              onClick={() => setShowForm(!showForm)}
              className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
            >
              + Nytt jobb
            </button>
          </div>
        </div>

        {showAfSearch && (
          <div className="bg-white p-6 rounded-lg shadow mb-6">
            <h3 className="text-lg font-semibold mb-4">Sök jobbannonser</h3>
            <div className="flex gap-3 mb-4">
              <input
                type="text"
                value={afQuery}
                onChange={(e) => setAfQuery(e.target.value)}
                placeholder="T.ex. Backend Developer"
                className="flex-1 border border-gray-300 rounded px-3 py-2 text-gray-900"
              />
              <button
                onClick={() => search(afQuery)}
                disabled={afLoading}
                className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700 disabled:opacity-50"
              >
                {afLoading ? 'Söker...' : 'Sök'}
              </button>
            </div>
            {results.map((r) => (
              <div key={r.id} className="border border-gray-200 rounded p-4 mb-3 flex justify-between items-start">
                <div>
                  <h4 className="font-semibold text-gray-800">{r.headline}</h4>
                  <p className="text-sm text-gray-500">{r.workplaceName} – {r.municipality}</p>
                </div>
                <button
                  onClick={() => handleImport(r)}
                  className="bg-blue-600 text-white px-3 py-1 rounded text-sm hover:bg-blue-700"
                >
                  Importera
                </button>
              </div>
            ))}
          </div>
        )}

        {showForm && (
          <div className="bg-white p-6 rounded-lg shadow mb-6">
            <h3 className="text-lg font-semibold mb-4">Skapa nytt jobb</h3>
            <form onSubmit={handleSubmit} className="space-y-4">
              <input
                type="text"
                placeholder="Titel"
                value={form.title}
                onChange={(e) => setForm({ ...form, title: e.target.value })}
                className="w-full border border-gray-300 rounded px-3 py-2 text-gray-900"
                required
              />
              <textarea
                placeholder="Beskrivning"
                value={form.description}
                onChange={(e) => setForm({ ...form, description: e.target.value })}
                className="w-full border border-gray-300 rounded px-3 py-2 text-gray-900"
                rows={3}
              />
              <input
                type="text"
                placeholder="Plats"
                value={form.location}
                onChange={(e) => setForm({ ...form, location: e.target.value })}
                className="w-full border border-gray-300 rounded px-3 py-2 text-gray-900"
              />
              <div className="flex gap-3">
                <button type="submit" className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
                  Spara
                </button>
                <button type="button" onClick={() => setShowForm(false)} className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400">
                  Avbryt
                </button>
              </div>
            </form>
          </div>
        )}

        {loading ? (
          <p className="text-gray-500">Laddar...</p>
        ) : (
          <div className="grid grid-cols-1 gap-4">
            {jobs.map((job) => (
              <div key={job.id} className="bg-white p-6 rounded-lg shadow flex justify-between items-start">
                <div>
                  <h3 className="text-lg font-semibold text-gray-800">{job.title}</h3>
                  <p className="text-sm text-gray-500">{job.location}</p>
                  <p className="text-sm text-gray-600 mt-2">{job.description}</p>
                  {job.afJobUrl && (
                    <a href={job.afJobUrl} target="_blank" rel="noopener noreferrer" className="text-blue-600 text-sm hover:underline mt-1 block">
                      Visa på Arbetsförmedlingen
                    </a>
                  )}
                </div>
                <button
                  onClick={() => removeJob(job.id)}
                  className="text-red-500 hover:text-red-700 text-sm"
                >
                  Ta bort
                </button>
              </div>
            ))}
            {jobs.length === 0 && (
              <p className="text-gray-500">Inga jobb ännu. Skapa ett eller importera från Arbetsförmedlingen!</p>
            )}
          </div>
        )}
      </div>
    </div>
  );
};

export default JobsPage;