import './assets/styles/App.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from 'react-query';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import Header from './layouts/Header.tsx';
import Footer from './layouts/Footer.tsx';
import RouteGuard from './components/RouteGuard.tsx';
import Dashboard from './pages/Dashboard.tsx';
import NotFound from './pages/NotFound.tsx';
import SignIn from './pages/account/SignIn.tsx';
import Details from './pages/account/Details.tsx';
import UsersList from './pages/users/usersList.tsx';

function App() {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <ToastContainer autoClose={2000} />
        <div className="flex flex-col h-screen w-screen justify-between sm:px-6 lg:px-8 mx-auto max-h-screen">
          <Header />
          <main className="mb-auto flex-grow">
            <Routes>
              <Route path="/sign-in" element={<SignIn />} />
              <Route path="*" element={<NotFound />} />
              <Route
                path="/"
                element={<RouteGuard component={<Dashboard />} />}
              />
              <Route
                path="/account"
                element={<RouteGuard component={<Details />} />}
              />
              <Route
                path="/users"
                element={<RouteGuard component={<UsersList />} />}
              />
            </Routes>
          </main>
          <Footer />
        </div>
      </BrowserRouter>
    </QueryClientProvider>
  );
}

export default App;
