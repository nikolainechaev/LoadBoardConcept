import React, { useState } from 'react'
import { auth } from '../../firebaseConfig'
import { signInWithEmailAndPassword } from 'firebase/auth'
import './Auth.css' // ✅ Import the CSS file for styles

const Login = () => {
  const [formData, setFormData] = useState({ email: '', password: '' })

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value })
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    try {
      await signInWithEmailAndPassword(auth, formData.email, formData.password)
      alert('🎉 Login Successful!')
    } catch (error) {
      alert('❌ Error: ' + error.message)
    }
  }

  return (
    <div className='auth-container'>
      <div className='auth-box'>
        <h2 className='auth-title'>Login</h2>
        <form onSubmit={handleSubmit}>
          <input
            type='email'
            name='email'
            placeholder='Email'
            onChange={handleChange}
            required
          />
          <input
            type='password'
            name='password'
            placeholder='Password'
            onChange={handleChange}
            required
          />
          <button type='submit'>Login</button>
        </form>
      </div>
    </div>
  )
}

export default Login
