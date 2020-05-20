# Covid
Proyecto Fin de FP

En construcción: PUEDE HABER ERRORES\n
Under construction: CAN BE ERRORS\n

Licencia CC BY-SA 


# Puesta en Marcha

Para iniciar la aplicación en modo test no es necesaria ninguna configuración,
basta con dirigir las consultas a https://localhost:5001/api/covidTest modificando
la url dependiendo del ordenador que la hospede y su configuración personal. 

Para iniciar la aplicación completa, es necesario, como requisito previo, tener
instalado el servidor Postgresql con un usuario y una base de datos activa.

-  Modificar el archivo connectionProperties.json que se encuentra en la ruta
DAOs/Connection/connectionProperties.json introduciendo la configuración de la
base de datos propia.

- Iniciar la aplicación y esperar entre media hora y una hora y media, dependiendo
del ordenador empleado y la velocidad de la base de datos propia. 
Tras la espera, comprobar que ha generado correctamente todas las tablas y que se han 
insertado datos en ellas.

¡Advertencia!

El programa está configurado para detectar el sitema operativo que lo ejecuta
y variar las rutas en función del mismo. Sin embargo, esta funcionalidad no ha
sido probada aún en Windows, por lo que, si tras arrancar la aplicación, pasados
unos minutos, no se ha generado ninguna tabla en su base de datos, lo mejor será
comprobar que las rutas estén adecuadamente configuradas. Debería verse alguna 
excepción en la consola. 

Esta es una lista de las clases que dependen de una ruta a archivo:

- CoVid/DAOs/Connection/ConnectionPostgreSql.cs
- CoVid/DAOs/CreateTableOperations/PostgreSqlCreateTable.cs
- CoVid/Processes/InitialCreateTables/createTables_Unix_Paths.json
- CoVid\Processes\InitialCreateTables\createTables_Unix_Paths.json
- CoVid/DAOs/InsertTableOperations/PostgreSqlInsert.cs
- CoVid/Processes/InitialDataInsertion/insertData_Unix_Paths.json
- CoVid\Processes\InitialDataInsertion\insertData_Windows_Paths.json

Por ahora únicamente genera las tablas e inserta una parte de los datos,
aquella que proviene del centro de datos de la Unión Europea.
A medida que se vayan incorporando más funcionalidades se irán
añadiendo en este archivo.
