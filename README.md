# Pruebas unitarias --- API de incidencias de un festival

Proyecto en C# orientado a comprobar el comportamiento de la lógica de
una API REST para la gestión de incidencias de un festival cultural.

## Objetivo

Practicar la creación y ejecución de pruebas unitarias, verificando
tanto los casos correctos como los errores de validación y las reglas de
negocio.

## Tecnologías utilizadas

-   **C#**
-   **.NET 10**
-   **xUnit** para definir y ejecutar las pruebas automatizadas
-   **Git y GitHub** para el control de versiones y la publicación del
    proyecto

## ¿Qué se prueba?

Las pruebas cubren distintos comportamientos del servicio de
incidencias:

-   Consulta de festivales e incidencias.
-   Comprobación de errores cuando se consulta un festival que no
    existe.
-   Creación de incidencias en festivales activos.
-   Rechazo de operaciones no permitidas, como crear incidencias en
    festivales inactivos.
-   Validación de los datos, como la longitud de la zona y de la
    descripción.
-   Validación de especialidades técnicas.
-   Asignación de incidencias a equipos técnicos.
-   Resolución de incidencias y validación del texto de resolución.
-   Consulta del resumen del festival y recuento de incidencias por
    estado.

## Estructura del repositorio

``` text
EjercicioEvaluable-TestsUnitarios-IncidenciasFestival/
├── FestivalIncidencias.Tests/   # Proyecto de pruebas
├── IncidenciasFestivalApi/      # Proyecto de la API
├── .gitignore
├── Memoria-Pruebas-Unitarias-IncidenciasFestival.pdf
└── README.md
```

## Ejecutar las pruebas

Necesitas tener instalado el SDK de .NET compatible con el proyecto.

Desde la carpeta raíz del repositorio, ejecuta:

``` bash
dotnet test
```

También puedes abrir la solución en Visual Studio y ejecutar las pruebas
desde **Test Explorer**.

## Resultado

En la ejecución documentada del proyecto, las **11 pruebas unitarias**
finalizaron correctamente. Las pruebas permiten comprobar el
comportamiento esperado del servicio y detectar errores en los casos de
validación y reglas de negocio.

## Documentación

En el repositorio se incluye
`Memoria-Pruebas-Unitarias-IncidenciasFestival.pdf`, con explicaciones
del proceso, ejemplos y evidencias de ejecución.

## Autor

**Armando Merino**
