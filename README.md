# Sistema de Gestión y Control de Acceso de Empleados

Desarrollé este backend en **ASP.NET Core Web API** para gestionar y controlar el ingreso y salida de los empleados en las instalaciones de la organización, reemplazando el registro manual en planillas y hojas de cálculo por un sistema centralizado y trazable.

Construí la solución aplicando **Clean Architecture**, **Domain-Driven Design (DDD)** y **CQRS**, utilizando **Entity Framework Core** con **SQL Server** para la persistencia de datos.

---

## Arquitectura que implementé

Organicé la solución en cuatro proyectos independientes respetando la regla de dependencias hacia el dominio:

- **`SGCAE.Domain`**: Definí las entidades (`Empleado`, `Credencial`, `PuntoAcceso`, `RegistroAcceso`), los objetos de valor (`DocumentoIdentidad`, `CorreoElectronico`, `CodigoCredencial`), enumeraciones, excepciones de dominio e interfaces de repositorios.
- **`SGCAE.Application`**: Implementé los casos de uso separando explícitamente las operaciones de escritura (`Commands`) y las de lectura (`Queries`) mediante el patrón **CQRS**.
- **`SGCAE.Infrastructure`**: Configuré el contexto de datos `SgcaeDbContext`, el mapeo Fluent API, las migraciones y los repositorios sobre **SQL Server**.
- **`SGCAE.Api`**: Expuse los controladores REST documentados con OpenAPI/Swagger y un middleware global para el manejo de reglas de negocio.

---

## Requerimientos funcionales cubiertos

- **RF-01 (Gestión de empleados)**: Registro, consulta, actualización y desactivación del vínculo laboral (`/api/empleados`).
- **RF-02 (Gestión de credenciales)**: Asignación de credenciales a empleados activos, consulta de estado, bloqueo y revocación (`/api/credenciales`).
- **RF-03 (Registro de entradas y salidas)**: Validación de punto de acceso, estado de credencial y estado laboral del empleado, registrando tanto los accesos autorizados como los rechazados (`POST /api/accesos`).
- **RF-04 (Consulta del historial de accesos)**: Consulta paginada con filtros por empleado, rango de fechas, punto de acceso y resultado de validación (`GET /api/accesos/historial`).
- **RF-06 (Administración de puntos de acceso)**: Registro y consulta de puntos físicos identificando cuáles están habilitados (`/api/puntos-acceso`).

---

## Cómo ejecutar el proyecto

1. Inicio mi contenedor de **SQL Server** con Docker:
   ```bash
   docker compose up -d
   ```
2. Ejecuto la API desde la raíz de la solución:
   ```bash
   dotnet run --project src/SGCAE.Api
   ```
   *(Al iniciar, la aplicación aplica automáticamente las migraciones de Entity Framework Core y crea la base de datos `SgcaeDb` con todas sus tablas si aún no existen).*
3. Abro la documentación interactiva en `http://localhost:5092/swagger` o ejecuto las peticiones de prueba que dejé en `src/SGCAE.Api/SGCAE.Api.http`.
