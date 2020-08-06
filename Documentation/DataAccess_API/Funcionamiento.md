# Introducción

Siguiendo con la tendencia actual, especialmente dentro del marco de trabajo de .NET, de fraccionar las aplicaciones en diversos microservicios capaces de funcionar de manera independiente y de ser gestionados por una persona que será la responsable de uno o varios de los mismos, el conjunto total de esta aplicación ha sido dividido en tres aplicaciones. Dos de ellas tienen una funcionalidad independiente de las demás y atienden a la responsabilidad procedural de la aplicación, y una tercera que se emplea como marco de interacción con el usuario. 

La creación, acceso, respaldo y actualización de los datos ha quedado dentro de la responsabilidad particular de esta API. Así, todo lo que implique interacción con proveedores externos de datos y con base de datos propia es tratado dentro de este programa. 

# Responabilidades

1. Creación de tablas necesarias para el funcionamiento.

2. Obtención de datos desde servidores externos.

3. Transformación entre modelo de datos externo-interno.

4. Mantenimiento y actualización permanente de los datos.

5. Servicio, desde base de datos hacia otros microservicios de la aplicación, de la información solicitada en el formato de datos interno.

# Tecnologías empleadas

    - SDK de .Net Core 3.1
    - ASP .Net Core 3.1
    - .Net Core 3.1
    - PostgreSQL 11.8
    - JSON

# Puesta en Marcha

1. Instalar todas dependencias citadas en el apartado anterior excepto la última.

2. Configurar base de datos Postgresql; generación de usuario con permisos para la creación de tablas, inserción y consulta y creación de base de datos.

3. Introducir datos de conexión en el archivo que se encuentra en siguiendo la ruta /DataAccess_API/DAOs/Connection/connectionProperties.json . En su interior es necesario completar los atributos de conexión; servidor, puerto, usuario, contraseña y base de datos.

4. Ejecutar el comando dotnet build desde la consola de comandos. Ref: https://docs.microsoft.com/es-es/dotnet/core/tools/dotnet-build

5. Ejecutar el comando dotnet run desde la terminal del sistema operativo. Ref: https://docs.microsoft.com/es-es/dotnet/core/tools/dotnet-run

6. Es posible sustituir los dos pasos anteriores al abrir el proyecto desde un IDE con opción de arrancado automático.

7. Una vez inicializada, estará disponible desde https://localhost:5005/CovidDataBase .

# Modificar url

Dentro de la carpeta situada en DataAccess_API/Properties se encuentra el archivo launchProperties.json . En su interior se puede modificar la dirección y el puerto de acceso.

# Métodos admitidos en la petición

- **GetGeoZoneData**: Se emplea para obtener información sobre la evolución histórica de la pandemia de Covid - 19 relativa a uno o varios países dentro de un rango de fechas.

- **GetAllGeoZoneData**: Se emplea para obtener información sobre la evolución histórica de la pandemia de Covid - 19 relativa todos los países disponibles dentro de un rango de fechas.

- **GetAllGeoZoneDataForAllDates**: Se emplea para obtener información sobre la evolución histórica de la pandemia de Covid - 19 relativa todos los países disponibles para cada una de las fechas disponibles.

- **GetAllCountries**: Se emplea para obtener una lista completa de todos los países disponibles para consulta.

- **GetAllDates**: Se emplea para obtener una lista completa de todas las fechas disponibles para consulta.


# Petición de entrada

<pre>
    <code>
{
    "method": "GetGeoZoneData",
    "covid_data": {
        "countries": [
            "ES",
            "AN"
        ],
        "dates": {
            "startDate": "31/12/2019",
            "endDate": "01/01/2020",
            "separator": "/"
        }
    }
}
    </code>
</pre>
# Ejemplo de respuesta
<pre>
    <code>
[
    {
        "father": null,
        "sonList": null,
        "geoID": "ES",
        "code": "ESP",
        "name": "Spain",
        "population": 46723749,
        "dataList": [
            {
                "id": 2,
                "cases": 0,
                "deaths": 0,
                "cured": 0,
                "date": {
                    "id": 2,
                    "date": "01/01/2020",
                    "dateSeparator": "/",
                    "dateFormat": "dd/MM/yyyy"
                }
            },
            {
                "id": 1,
                "cases": 0,
                "deaths": 0,
                "cured": 0,
                "date": {
                    "id": 1,
                    "date": "31/12/2019",
                    "dateSeparator": "/",
                    "dateFormat": "dd/MM/yyyy"
                }
            }
        ]
    }
]
    </code>
</pre>