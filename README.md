# 🏥 Sistema de Gestión de Citas Médicas (SGCM)

Proyecto académico desarrollado para la asignatura **Programación II (TDS-006)** en el Instituto Tecnológico de Las Américas (ITLA).

## 👨‍💻 Autores

- **José Luis Beltre Cordero** - 2025-1241
- **Aslan Guzmán** - 2025-1177

## 📋 Descripción

Sistema web para la gestión integral de citas médicas en clínicas y hospitales, desarrollado con arquitectura en capas utilizando ASP.NET Core y Nuxt.js.

### Características Principales

- ✅ Gestión de citas médicas (crear, consultar, cancelar, reprogramar)
- ✅ Control de disponibilidad de médicos
- ✅ Sistema de notificaciones automáticas (Email/SMS)
- ✅ Gestión de usuarios (Pacientes, Médicos, Administradores)
- ✅ Auditoría y trazabilidad completa
- ✅ Reportes operativos

## 🏗️ Arquitectura

El proyecto implementa una **Arquitectura en Capas (Layered Architecture)** con clara separación de responsabilidades:

```
SGCM/
├── SGCM.Domain          # Entidades y lógica de negocio
├── SGCM.Application     # Casos de uso y servicios
├── SGCM.Persistence     # Acceso a datos y repositorios
├── SGCM.API             # API REST con ASP.NET Core
└── Web/                 # Frontend con Nuxt.js
```

## 🛠️ Tecnologías

### Backend
- **Framework:** ASP.NET Core 8.0
- **ORM:** Entity Framework Core
- **Base de Datos:** SQL Server
- **Autenticación:** JWT
- **Documentación:** Swagger/OpenAPI

### Frontend
- **Framework:** Nuxt.js 3
- **Lenguaje:** TypeScript
- **Estilos:** Tailwind CSS

### Servicios Externos
- **SendGrid** - Envío de correos electrónicos


## 📝 Estado del Proyecto

🚧 **En Desarrollo** - Proyecto académico activo

## 📄 Licencia

Este proyecto es de uso académico para la asignatura Programación II del ITLA.

## 👨‍🏫 Profesor

**Francis Ramírez**  
Programación II – TDS-006
