# API de incidencias de un festival cultural

API ASP.NET Core para gestionar incidencias técnicas durante festivales. Está preparada como código base para realizar una actividad posterior de pruebas unitarias. Este proyecto no contiene tests ni solución de tests.

## Requisitos

- SDK .NET 10 instalado.
- Visual Studio Code.
- Extensión C# Dev Kit.
- Acceso a NuGet para restaurar `Swashbuckle.AspNetCore`.

## Abrir y ejecutar en Visual Studio Code

1. Descomprime la carpeta `IncidenciasFestivalApi`.
2. Abre esa carpeta en VS Code (`File > Open Folder`).
3. Abre `Terminal > New Terminal`.
4. Ejecuta:

   ```powershell
   dotnet --version
   dotnet restore
   dotnet build
   dotnet run --launch-profile http
   ```

5. Abre `http://localhost:5088/swagger` en el navegador.

Si `dotnet --version` no muestra una versión 10.0.x, instala el SDK .NET 10 o cambia el framework con el que trabajas. El proyecto está configurado para `net10.0`.

## Datos iniciales

Se incluyen dos festivales ficticios y varios equipos técnicos de diferentes especialidades. No hay incidencias iniciales: se crean mediante las peticiones `POST`.

## Endpoints

- `GET /api/festivales`
- `GET /api/festivales/{id}/resumen`
- `GET /api/equipos`
- `GET /api/equipos?especialidad=Electricidad`
- `GET /api/incidencias?festivalId=1`
- `GET /api/incidencias/{id}`
- `POST /api/incidencias`
- `POST /api/incidencias/{id}/asignar`
- `POST /api/incidencias/{id}/resolver`

## Ejemplos de peticiones

### Crear incidencia

`POST /api/incidencias`

```json
{
  "festivalId": 1,
  "zona": "Escenario principal",
  "descripcion": "Uno de los focos del lateral no enciende.",
  "especialidadRequerida": "Electricidad",
  "prioridad": "Alta"
}
```

### Asignar un equipo

`POST /api/incidencias/1/asignar`

```json
{
  "equipoId": 1
}
```

### Resolver incidencia

`POST /api/incidencias/1/resolver`

```json
{
  "resolucion": "Se ha sustituido el cable y probado el foco."
}
```

### Consultar resumen

`GET /api/festivales/1/resumen`

## Reglas de negocio incluidas

- Solo se pueden registrar incidencias en festivales activos.
- La zona debe tener entre 2 y 80 caracteres.
- La descripción debe tener entre 10 y 500 caracteres.
- La especialidad y la prioridad deben corresponder a los valores definidos.
- Solo se puede asignar una incidencia abierta.
- El equipo debe existir, estar activo y tener la especialidad adecuada.
- Un equipo no puede tener más de tres incidencias activas asignadas.
- Solo se puede resolver una incidencia asignada.
- La resolución debe tener entre 10 y 500 caracteres.
- El resumen calcula las incidencias abiertas, asignadas, resueltas y críticas pendientes.

## Limitación intencionada

Los datos se almacenan en memoria. Se conservan mientras la API se está ejecutando, pero se pierden al detenerla. No necesita MySQL, EF Core ni servicios externos.
