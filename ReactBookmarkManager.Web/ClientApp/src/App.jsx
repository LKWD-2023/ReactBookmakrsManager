import React from 'react';
import { Route, Routes } from 'react-router-dom';
import Layout from './Components/Layout';
import Home from './Pages/Home';
import Signup from './Pages/Signup';
import Login from './Pages/Login';
import MyBookmarks from './Pages/MyBookmarks';
import AddBookmark from './Pages/AddBookmark';
import AuthContextComponent from './AuthContext';
import PrivateRoute from './PrivateRoute';
import Logout from './Pages/Logout';

const App = () => {
    return (
        <AuthContextComponent>
            <Layout>
                <Routes>
                    <Route exact path='/' element={<Home />} />
                    <Route exact path='/signup' element={<Signup />} />
                    <Route exact path='/login' element={<Login />} />
                    <Route exact path='/logout' element={<Logout />} />
                    <Route exact path='/my-bookmarks' element={
                        <PrivateRoute>
                            <MyBookmarks />
                        </PrivateRoute>} />
                    <Route exact path='/add-bookmark' element={
                        <PrivateRoute>
                            <AddBookmark />
                        </PrivateRoute>} />
                </Routes>
            </Layout>
        </AuthContextComponent>
    );
}

export default App;