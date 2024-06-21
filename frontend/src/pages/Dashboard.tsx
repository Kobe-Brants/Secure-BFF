import { useNavigate } from 'react-router-dom';
import { UserIcon, UsersIcon } from '@heroicons/react/24/outline';

export default function Dashboard() {
  const navigate = useNavigate();

  const paths = [
    {
      name: 'Account',
      path: 'account',
      icon: <UserIcon className="w-12 text-white" />,
      isAdminOnly: false,
    },
    {
      name: 'Users',
      path: 'users',
      icon: <UsersIcon className="w-12 text-white" />,
      isAdminOnly: false,
    },
  ];

  const handleNavigation = (page: string) => {
    navigate(`/${page}`);
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4">
        {paths.map(path => {
          return (
            <div key={path.name} className="rounded-lg p-4 text-center">
              <button
                className="flex items-center justify-center bg-green rounded-lg w-24 h-24 mx-auto mb-4 hover:scale-105 transition"
                onClick={() => handleNavigation(path.path)}
              >
                {path.icon}
              </button>
              <h3 className="text-md font-semibold mb-2">{path.name}</h3>
            </div>
          );
        })}
      </div>
    </div>
  );
}
