# Covid

Licencia CC BY-SA 

# Introducción

Siguiendo con la tendencia actual, especialmente dentro del marco de trabajo de .NET, de fraccionar las aplicaciones en diversos microservicios capaces de funcionar de manera independiente y de ser gestionados por una persona que será la responsable de uno o varios de los mismos, el conjunto total de esta aplicación ha sido dividido en tres aplicaciones. Dos de ellas tienen una funcionalidad independiente de las demás y atienden a la responsabilidad procedural de la aplicación, y una tercera que se emplea como marco de interacción con el usuario.

# Idea General

Tras la estructuración de la aplicación en múltiples microservicios subyace la idea de la rehutilización del software. De lo que se trata es de generar componentes independientes entre sí que, por encargarse de una única responsabilidad dentro de un sistema mayor sean capaces de servir como bloques de construcción para aplicaciones de mayor envergadura.

![Flujo General](./Diagrams/DiseñoGeneral.png)

## Acceso a Datos

Así, el acceso a datos es gestionado por un API con capacidad para generar sus propias tablas y modelos de datos con la ayuda, únicamente de una conexión a base de datos establecida por el programador desde un archivo de configuración. Es posible que ni el modelo de datos ni los procesos particulares que requiere sean empleados por otras aplicaciones jamás, sin embargo, éste componente puede emplearse como esqueleto para generar aplicaciones diferentes cuya responsabilidad constista en gestionar el acceso a los datos almacenados en una base de datos.

Además, si se plantease la necesidad de migrar el almacenaje de los datos hacia un proveedor de base de datos distinto los cambios a realizar dentro de la aplicación se encontrarían completamente aislados y, junto con un diseño abstracto de componentes, resultarían muy sencillos de llevar a buen término.

## Gestión de la seguridad

Pero por otro lado se encuntra la gestión de información sensible y los procesos de autentificación de usuario. Son comunes a miles de aplicaciones y páginas web y bastaría el empleo de un único microservicio para todas ellas. Éste podría ser el mismo para un conjunto o funcionar por duplicado para cada una de las aplicaiones o páginas web que lo requiriesen pero, no sería necesario programarlo más de una vez. 

Igualmente debe de ser capaz de ponerse en marcha por sí mismo con la menor necesidad de manipulación por parte del programador por lo que todos los procesos de creación de tablas, generación de contraseñas y claves propias se han automatizado y simplemente resulta imprescindible añadir una conexión a base de datos dentro del archivo de configuración correspondiente.

Se recomienda siempre el uso de una base de datos específica para almacenar la información sensible. Siguiendo el diseño del bloque anterior la forma en la que está programada la aplicación facilita la sustitución de unas clases por otras como medio de cambiar de proveedor de base de datos.

## El conjunto

Finalmente es necesario un servicio que se encargue de interactuar con el usuario sin  que éste tenga que acceder directamente a cada uno de sus componentes. Además, este servicio, o microservicio,  debería de poner en común todas las funcionalidades de la aplicación siendo fácilmente reemplazable por otro u otros. Es también el encargado de tokenizar el acceso a la misma, de manera que únicamente sea necesaria una autentificación cada cierto tiempo, limitando así el acceso a la información sensible que queda doblemente oculta dentro de este sistema multicomponental.

# Objetivos

- Adaptabilidad
- Rehutilización
- Sofware OpenSource
- Seguridad en el tratamiento de la información sensible.

# Tecnologías

- SDK de .Net Core 3.1
- ASP .Net Core 3.1
- .Net Core 3.1
- PostgreSQL 11.8
- Docker
- JSON
- NewtonSoft
- JWT
- Draw.io
- Swagger OpenAPI

# ReadMe particular para cada componente

- [Covid_API](./Documentation/Covid_API/CovidAPIReadMe.md)
- [DataAccess_API](./Documentation/DataAccess_API/DataAccessAPIReadMe.md)
- [Security_API](./Documentation/Security_API/SecurityAPIReadMe.md)

# Manual simple de usuario

## DataAccess_API

- [MarkDown](./Documentation/DataAccess_API/SimpleUserGuide/ManualSimpleUsuario_DataAccess_API.md)

- [PDF](./Documentation/DataAccess_API/SimpleUserGuide/ManualSimpleUsuario_DataAccess_API.pdf)

- [DOC](./Documentation/DataAccess_API/SimpleUserGuide/ManualSimpleUsuario_DataAccess_API.doc)

- [DOCX](./Documentation/DataAccess_API/SimpleUserGuide/ManualSimpleUsuario_DataAccess_API.docx)

## Security_API

- [MarkDown](./Documentation/Security_API/SimpleUserGuide/ManualSimpleUsuario_Security_API.md)

- [PDF](./Documentation/Security_API/SimpleUserGuide/ManualSimpleUsuario_Security_API.pdf)

- [DOC](./Documentation/Security_API/SimpleUserGuide/ManualSimpleUsuario_Security_API.doc)

- [DOCX](./Documentation/Security_API/SimpleUserGuide/ManualSimpleUsuario_Security_API.docx)

## Covid_API

- [MarkDown](./Documentation/Covid_API/SimpleUserGuide/ManualSimpleUsuario_Covid_API.md)

- [PDF](./Documentation/Covid_API/SimpleUserGuide/ManualSimpleUsuario_Covid_API.pdf)

- [DOC](./Documentation/Covid_API/SimpleUserGuide/ManualSimpleUsuario_Covid_API.doc)

- [DOCX](./Documentation/Covid_API/SimpleUserGuide/ManualSimpleUsuario_Covid_API.docx)

# Documentación de conjunto completa

- [MarkDown](./Documentation/Documentacion.md)

- [DOCX](./Documentation/Documentacion.docx)

- [PDF](./Documentation/Documentacion.pdf)

# Manual de uso

- [DOCX](./Documentation/Manual de uso.docx)

- [PDF](./Documentation/Manual de uso.pdf)

- [Poryecto Postman](./Documentation/CovidAPI.postman_collection.json)
