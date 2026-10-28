# Pruebas unitarias — API de incidencias de un festival

## Descripción

Proyecto de una API para gestionar incidencias de un festival. Incluye pruebas unitarias para comprobar el comportamiento de las operaciones principales y sus validaciones.

## Tecnologías utilizadas

- **C#**
- **.NET 10**
- **xUnit** para las pruebas unitarias
- **Git y GitHub** para el control de versiones

## Resumen de las pruebas

En la última ejecución documentada, **las 11 pruebas unitarias pasaron correctamente**.

| Área comprobada | Qué se verifica |
|---|---|
| Consulta de festivales e incidencias | Que se puedan consultar los datos correspondientes. |
| Festival inexistente | Que la API gestione correctamente la búsqueda de un festival que no existe. |
| Creación de incidencias | Que se permita crear una incidencia para un festival activo. |
| Festival inactivo | Que no se permita crear una incidencia para un festival inactivo. |
| Validación de datos | Que se validen la zona y la longitud de la descripción. |
| Especialidad técnica | Que se valide el valor de la especialidad técnica mediante el tipo enumerado correspondiente. |
| Asignación de equipo | Que una incidencia pueda asignarse a un equipo. |
| Resolución de incidencias | Que se pueda resolver una incidencia y se validen los datos necesarios. |
| Resumen del festival | Que se calculen los recuentos de incidencias del resumen. |

> **Resultado:** 11 pruebas superadas en la última ejecución documentada. Ejecuta `dotnet test` para comprobar el resultado en tu entorno actual.

## Estructura del repositorio

```text
EjercicioEvaluable-TestsUnitarios-IncidenciasFestival/
├── FestivalIncidencias.Tests/
├── IncidenciasFestivalApi/
├── Memoria-Pruebas-Unitarias-IncidenciasFestival.pdf
├── README.md
└── .gitignore
```

*La estructura puede variar si se añaden archivos o carpetas al proyecto.*

## Cómo ejecutar las pruebas

Abre una terminal en la carpeta que contiene la solución y ejecuta:

```bash
dotnet test
```

El comando compila los proyectos necesarios y ejecuta las pruebas configuradas.

## Documentación

En la raíz del repositorio se incluye el documento:

`Memoria-Pruebas-Unitarias-IncidenciasFestival.pdf`

Este PDF contiene la memoria del proyecto y complementa la información de este README.

## Autor

Armando Merino
