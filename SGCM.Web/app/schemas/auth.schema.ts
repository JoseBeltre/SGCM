import { z } from 'zod'

export const loginSchema = z.object({
  email: z.string().min(1, 'El correo es obligatorio').email('Debes ingresar un correo válido'),
  password: z.string().min(1, 'La contraseña es obligatoria')
})

export const registerSchema = z.object({
  fullName: z.string().min(3, 'Debes proveer un nombre completo válido'),
  nationalId: z
    .string()
    .min(5, 'Tu cédula debe tener al menos 5 caracteres')
    .max(11, 'La cédula no puede tener más de 11 caracteres')
    .regex(/^[0-9]+$/, 'La cédula no debe contener guiones ni letras'),
  dateOfBirth: z.string().min(1, 'La fecha de nacimiento es obligatoria'),
  phone: z.string().optional(),
  email: z.string().email('Debes ingresar un correo válido'),
  password: z.string().min(6, 'La contraseña debe tener al menos 6 caracteres')
})
