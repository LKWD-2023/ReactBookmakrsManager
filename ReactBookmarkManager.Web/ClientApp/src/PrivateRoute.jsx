import { Navigate } from 'react-router-dom';
import { useAuthDataContext } from './AuthContext';

const PrivateRoute = ({ children }) => {
  const { user } = useAuthDataContext();

  return user ? children : <Navigate to="/login" replace />;
};

export default PrivateRoute;