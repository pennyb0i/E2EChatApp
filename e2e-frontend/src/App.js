import './App.css';
import { Routes, Route } from 'react-router-dom';
import Home from './pages/home/home'
import Login from './pages/login/login'
import Register from './pages/register/register'
import Friends from "./pages/friends/friends";
import Chat from "./pages/chat/chat";

const App = () => {
  return (
      <>
        <Routes>
            <Route path="/" element={<Home/>} />
          <Route path="/home" element={<Home/>} />
          <Route path="/login" element={<Login/>} />
          <Route path="/register" element={<Register/>} />
            <Route path="/friends" element={<Friends/>} />
            <Route path="/chat" element={<Chat/>} />
        </Routes>
      </>
  );
};

export default App;
