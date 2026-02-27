import Navbar from '../components/Layout/Navbar';
import { useAuth } from '../context/AuthContext';

const DashboardPage = () => {
  const { user } = useAuth();

  return (
    <div className="min-h-screen bg-gray-100">
      <Navbar />
      <div className="p-8">
        <h2 className="text-2xl font-bold text-gray-800 mb-2">
          Välkommen, {user?.fullName}!
        </h2>
        <p className="text-gray-600">Roll: {user?.role}</p>
        <div className="mt-8 grid grid-cols-3 gap-6">
          <div className="bg-white p-6 rounded-lg shadow">
            <h3 className="text-lg font-semibold text-gray-700">Jobb</h3>
            <p className="text-gray-500 mt-1">Hantera dina jobbannonser</p>
          </div>
          <div className="bg-white p-6 rounded-lg shadow">
            <h3 className="text-lg font-semibold text-gray-700">Kandidater</h3>
            <p className="text-gray-500 mt-1">Se alla kandidater i kanban</p>
          </div>
          <div className="bg-white p-6 rounded-lg shadow">
            <h3 className="text-lg font-semibold text-gray-700">Arbetsförmedlingen</h3>
            <p className="text-gray-500 mt-1">Importera jobbannonser</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default DashboardPage;