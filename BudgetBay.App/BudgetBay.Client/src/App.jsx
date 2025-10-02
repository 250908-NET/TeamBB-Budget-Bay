import { Routes, Route } from 'react-router-dom';

import HomePage from './pages/HomePage/HomePage'
import LoginPage from './pages/LoginPage/LoginPage';

function App() {

  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/hello" element={<h1>Hello World</h1>} />
    </Routes>
  )
}

export default App
