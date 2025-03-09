import React, { useState } from 'react'
import { auth } from '../../firebaseConfig'
import { createUserWithEmailAndPassword, updateProfile } from 'firebase/auth'
import './Auth.css' // ✅ Import the CSS file for styles

const Register = () => {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
    role: '',
  })

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value })
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    try {
      console.log('📌 Starting Registration:', formData)

      if (!formData.role) {
        alert('Please select a role (Broker or Carrier)')
        return
      }

      // ✅ 1️⃣ Create Firebase Authentication User First
      const userCredential = await createUserWithEmailAndPassword(
        auth,
        formData.email,
        formData.password
      )

      console.log('✅ Firebase Auth User Created:', userCredential.user)

      // ✅ 2️⃣ Update Firebase User Profile
      await updateProfile(userCredential.user, { displayName: formData.name })

      console.log('✅ Profile Updated. Requesting Firebase Custom Token...')

      // ✅ 3️⃣ Request a Firebase Custom Token from the backend
      const tokenResponse = await fetch(
        'https://localhost:7284/api/auth/request-token',
        {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ email: formData.email }),
        }
      )

      const tokenData = await tokenResponse.json()
      if (!tokenResponse.ok) {
        alert(`❌ Error: ${tokenData.message}`)
        return
      }

      console.log('✅ Firebase Custom Token Received:', tokenData.token)

      // ✅ 4️⃣ Store user details in Firestore
      const firestoreResponse = await fetch(
        'https://localhost:7284/api/auth/store-user',
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${tokenData.token}`, // Use Firebase Custom Token
          },
          body: JSON.stringify({
            name: formData.name,
            email: formData.email,
            password: formData.password,
            role: formData.role,
          }),
        }
      )

      const firestoreData = await firestoreResponse.json()
      if (!firestoreResponse.ok) {
        alert(`❌ Error: ${firestoreData.message}`)
        return
      }

      console.log('✅ User Stored in Firestore:', firestoreData)
      alert('🎉 User Registered Successfully!')
    } catch (error) {
      console.error('❌ Registration Error:', error.message)
      alert('Error: ' + error.message)
    }
  }

  return (
    <div className='auth-container'>
      <div className='auth-box'>
        <h2 className='auth-title'>Register</h2>
        <form onSubmit={handleSubmit}>
          <input
            type='text'
            name='name'
            placeholder='Name'
            onChange={handleChange}
            required
          />
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
          <select name='role' onChange={handleChange} required>
            <option value=''>Select Role</option>
            <option value='broker'>Broker</option>
            <option value='carrier'>Carrier</option>
          </select>
          <button type='submit'>Register</button>
        </form>
      </div>
    </div>
  )
}

export default Register
