import React from 'react'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import Register from './pages/auth/Register.js'
import Login from './pages/auth/Login.js'

function App() {
  return (
    <Router>
      <Routes>
        <Route path='/register' element={<Register />} />
        <Route path='/login' element={<Login />} />
        {/* Catch-All Route */}
        <Route path='*' element={<Login />} />
      </Routes>
    </Router>
  )
}

export default App
