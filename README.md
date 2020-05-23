# Covid
Proyecto Fin de FP

En construcción: PUEDE HABER ERRORES

Under construction: CAN BE ERRORS

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
Tras la espera, comprobar que han generado correctamente todas las tablas y que se han 
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

#Funcionalidades activas

	Ya es posible realizar consultas aunque no existe un control de errores
completo. Una vez el programa ha generado la base de datos, será posible
acceder a los datos de la misma realizando una llamada POST a https://localhost:5001/api/covid .

La contraseña pedida por defecto para realizar pruebas es "secret". La estruc-
tura de la petición será la siguiente:


{
    "user": 
    
        {
            "id": "mail@example.net",
            "pass": "secret"
        }
    ,

    "covid_data":
    {
        "countries":
        [
            
            "ao",
            "sp",
            "du"
            
        ],
        "dates":
        {
            
            "startDate": "31/12/2019",
            "endDate": "22/05/2020"
        
        },

        "dataType": "methodName",
        "statisticalMethod" : "anyAvailable"
    }
    
}

Se pueden incluir tantos países como se desee dentro de la lista "countries". Si 
se quieren solicitar los datos relativos a la pandemia de todos los paises que hay
almacenados en la base de datos basta con inlcuir * dentro de la lista de países solicitados.

Hay, además, dos operaciones de consulta más: 

	- Es posible obtener una lista completa de las fechas permitidas estableciendo
	el elemento "dataType" como "getdates".

	- Para obtener la lista de paises disponibles basta con establecer el elemento
	"dataType" como "getcountries".

#Próxima funcionalidad a desarrollar

	Lo próximo a desarrollar será el sistema de autentificación de usuarios. Para
garantizar la seguridad se ha establecido el siguiente procedimiento:

	1. El registro se realizará mediante el empleo de encriptación en Base64 únicamente
	para el primer envío de información.

	2. Una vez recibida la información por vez primera el programa se encargará de
	generar unas claves pública y privada RSA mediante y, enviar en la respuesta
	la clave pública única para el nuevo usuario generado.

	3. Con esa clave pública se encriptará el nombre de usuario y su contraseña almacenando
	en la base de datos relativa a los usuarios nombre de usuario encriptado, contraseña encriptada 
	y clave pública.

	4. Para garantizar la seguridad de las claves privadas se desarrollará el siguiente
	procedimiento:

		1. Encriptar la clave privada generada mediante algoritmo RSA nuevamente.
		
		2. Almacenar en una base de datos dispuesta para ello las claves pública y privada
		generadas encriptadas mediante SHA 256 junto con la fecha y hora de su generación.

		3. Crear un proceso independiente y automático que se encargue de comprobar si
		han pasado más de 12 horas desde la creación de la clave de desencriptación de claves 
		privada y pública y, en caso afirmativo, recuperarlas, generar unas claves maestras nuevas, 
		recorrer toda la base de datos de claves públicas y privadas almacenadas desencriptándolas
		y sustituyéndolas por una encriptación con las nuevas claves maestras, eliminar las claves
		internas antiguas y almacenar las generadas recientemente.

		Para asegurar que el proceso no altere la base de datos irremediablemente por un 
		error se recomienda generar unas tablas de inserción y unas permanentes, de modo
		que únicamente sean modificadas las permanentes en caso de inserción exitosa realizando
		un volcado de las mismas tras haber efectuado todas las operaciones.

	Se trata de un proceso algo complejo y vulnerable únicamente durante el momento de
recibir el primer alta del usuario, ya que posteriormente, la obtención de las claves
privadas con las que desencriptar la información sensible se complicará y además correrá
contra reloj pues, un supuesto atacante, dispondría de 12 horas para eliminar la seguridad
del servidor en el que se hospeda la base de datos, romper la seguridad de la propia base
de datos, obtener la clave privada maestra con la que desencriptar todas las claves privadas
asignadas, cada una de ellas a un usuario y, emplear cada una de esas claves privadas para
desencriptar la información del usuario. Sin olvidar, que se debe de desencriptar la clave
maestra SHA256 mediante.

	Como ventaja, puede modificarse la regularidad en la variación de claves pasando de 12
horas a 20 minutos, por ejemplo, o, llegado el caso, incluso a uno o dos minutos.

# Emplear una base de datos distinta

Para emplear una base de datos diferente a la escogida, será necesario generar
ciertas clases específicas para la misma:

- Crear una clase que implemente la interfaz IDataBaseConnector.
- Crear una clase que implemente la interfaz ICreate.
- Crear una clase que implemente la interfaz IDataBaseInsert.
- Crear una clase que implemente la interfaz IDatabaseAccess.

Una vez generadas las clases precedentes, crear una implementación
de la clase abstracta CovidDAO. Dicha clase estará compuesta por
cada uno de los objetos que implementan las anteriores interfaces
y los empleará para construir sus métodos. 

Por último, ir a la clase Program.cs y sustituir en la construcción
de la clase InitialDataInsertions el parámetro correspondiente a la
inyección de la clase CovidDAO.

Las pruebas indican una velocidad de acceso a datos elevada - en torno
a respuestas dadas entre 17 y 80 ms en transacciones que incluyen los
datos relativos a varios países y, de entre 800 y 1600 ms en obtener
el conjunto de datos de la base de datos, en un ordenador sencillo con
un procesador AMD de gama media - no obstante,
un almacén de datos en memoria RAM sería aún más veloz.



