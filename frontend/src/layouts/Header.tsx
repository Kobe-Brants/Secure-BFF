import { Fragment } from 'react';
import { Disclosure, Menu, Transition } from '@headlessui/react';
import avatar from '../assets/images/header/avatar.png';
import { useNavigate } from 'react-router-dom';
import { useAuthContext } from '../services/contexts/AuthContext.tsx';
import { toast } from 'react-toastify';

function classNames(...classes: string[]) {
  return classes.filter(Boolean).join(' ');
}

export default function Header() {
  const navigate = useNavigate();
  const { user, logout } = useAuthContext();

  const greetUser = () => {
    const hrs = new Date().getHours();

    if (hrs < 12) {
      return 'Good Morning,';
    } else if (hrs >= 12 && hrs <= 17) {
      return 'Good Afternoon,';
    } else if (hrs >= 17 && hrs <= 24) {
      return 'Good Evening,';
    }
  };
  const handleLogout = () => {
    logout().then(() => {
      navigate('/login');
      toast.success('Signed out successfully');
    });
  };

  const handleGoHome = () => {
    navigate('/');
  };

  return (
    <Disclosure as="nav" className="bg-gray-700">
      <>
        <div className="relative flex h-16 items-center justify-between">
          <div className="flex flex-1 items-center justify-center sm:items-stretch sm:justify-start">
            <div className="flex flex-shrink-0 items-center">
              <img
                className="block h-8 w-auto lg:hidden rounded cursor-pointer"
                src={'app_logo.png'}
                alt="logo"
                onClick={handleGoHome}
              />
              <img
                className="hidden h-8 w-auto lg:block rounded cursor-pointer"
                src={'app_logo.png'}
                alt="logo"
                onClick={handleGoHome}
              />
              <div className="px-4 text-xl font-semibold whitespace-nowrap">
                Frontend
              </div>
            </div>
            <div className="hidden sm:ml-6 sm:block">
              <div className="flex space-x-4">
                {user && (
                  <a className="rounded-md px-3 py-2 text-sm font-bold">
                    <span className="text-yellow">{greetUser()}</span>{' '}
                    {user?.displayName || user?.email}
                  </a>
                )}
              </div>
            </div>
          </div>
          <div className="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0">
            <div className="px-4 text-sm font-semibold whitespace-nowrap hidden lg:block">
              {new Date().toLocaleDateString()}
            </div>
            {/* Profile dropdown */}
            {user && (
              <Menu as="div" className="relative ml-3">
                <div>
                  <Menu.Button className="flex rounded-full bg-gray-800 text-sm focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800">
                    <span className="sr-only">Open user menu</span>
                    <img
                      className="h-8 w-8 rounded-full"
                      src={avatar}
                      alt="avatar"
                    />
                  </Menu.Button>
                </div>
                <Transition
                  as={Fragment}
                  enter="transition ease-out duration-100"
                  enterFrom="transform opacity-0 scale-95"
                  enterTo="transform opacity-100 scale-100"
                  leave="transition ease-in duration-75"
                  leaveFrom="transform opacity-100 scale-100"
                  leaveTo="transform opacity-0 scale-95"
                >
                  <Menu.Items className="absolute right-0 z-10 mt-2 w-48 origin-top-right rounded-md bg-white py-1 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
                    <Menu.Item>
                      <a
                        className={
                          'block px-4 py-2 text-sm text-gray-700 bg-white'
                        }
                      >
                        {user.email}
                      </a>
                    </Menu.Item>
                    <Menu.Item>
                      {({ active }) => (
                        <a
                          href=""
                          onClick={handleLogout}
                          className={classNames(
                            active ? 'bg-yellow' : '',
                            'block px-4 py-2 text-sm text-gray-700'
                          )}
                        >
                          Sign out
                        </a>
                      )}
                    </Menu.Item>
                  </Menu.Items>
                </Transition>
              </Menu>
            )}
          </div>
        </div>

        <Disclosure.Panel className="sm:hidden">
          <div className="space-y-1 px-2 pb-3 pt-2">
            <Disclosure.Button
              as="a"
              className="block rounded-md px-3 py-2 text-base font-medium"
            >
              <span className="text-yellow">{greetUser()}</span>{' '}
              {user?.displayName}{' '}
            </Disclosure.Button>
          </div>
        </Disclosure.Panel>
      </>
    </Disclosure>
  );
}
