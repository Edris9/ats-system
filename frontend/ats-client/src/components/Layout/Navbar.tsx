import { useAuth } from '../../context/AuthContext';
import { useNavigate } from 'react-router-dom';

const Navbar = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="bg-blue-600 text-white px-6 py-4 flex justify-between items-center">
      <div className="flex items-center gap-6">
        <h1 className="text-xl font-bold">ATS System</h1>
        <button onClick={() => navigate('/dashboard')} className="hover:underline">Dashboard</button>
        <button onClick={() => navigate('/jobs')} className="hover:underline">Jobb</button>
        <button onClick={() => navigate('/kanban')} className="hover:underline">Kanban</button>
      </div>
      <div className="flex items-center gap-4">
        <span className="text-sm">{user?.fullName} ({user?.role})</span>
        <button onClick={handleLogout} className="bg-white text-blue-600 px-3 py-1 rounded hover:bg-gray-100">
          Logga ut
        </button>
      </div>
    </nav>
  );
};

export default Navbar;