export interface LoginCredentials {
  email: string;
  password?: string;
}

export interface RegisterPatient {
  fullName: string;
  nationalId: string;
  dateOfBirth: string;
  phone?: string;
  email: string;
  password?: string;
}

export interface AuthSession {
  id: number;
  profileId: number;
  fullName: string;
  email: string;
  userType: string;
}
