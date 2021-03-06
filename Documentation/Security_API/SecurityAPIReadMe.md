# Introducción

El cuidado y almacenamiento de los datos sensibles de usuario, así como el registro, la comprobación y el contraste de los mismos quedan dentro de la responsabilidad de esta API de seguridad. 

# Responsabilidades

1. Creación de tablas necesarias para el funcionamiento.

2. Generación de claves de encriptación con las que asegurar el intercambio seguro de información.

3. Registro de nuevos usuarios.

4. Almacenamiento encriptado de nuevos usuarios con claves pública y privada propias y únicas para cada usuario. La clave privada del usuario será, a su vez, encriptada empleando una pareja de claves pública y privada propias de la aplicación que variará cada 100 usos.

5. Cambio regular de la encriptación usada por la API. Con ella será necesario actualizar la base de datos para que las claves privadas de usuario pasen a estar encriptadas con las nuevas claves.

# Tecnologías empleadas

    - SDK de .Net Core 3.1
    - ASP .Net Core 3.1
    - .Net Core 3.1
    - PostgreSQL 11.8
    - JSON
    - NewtonSoft
    - Draw.io
    - RSA
    - Docker

## Documentación Oficial

- **.Net Core**: https://docs.microsoft.com/es-es/dotnet/core/
- **PostgreSql**: https://www.postgresql.org/docs/
- **JSON**: https://developer.mozilla.org/es/docs/Learn/JavaScript/Objects/JSON
- **Newtonsoft**: https://www.newtonsoft.com/json/help/html/Introduction.htm
- **Draw.io**: http://draw.io
- **RSA**: https://es.wikipedia.org/wiki/RSA
- **Docker**: https://www.docker.com



# Puesta en Marcha

1. Instalar las cuatro primeras dependencias citadas en el apartado anterior.

2. Configurar base de datos Postgresql; generación de usuario con permisos para la creación de tablas, inserción y consulta y creación de base de datos.

3. Introducir datos de conexión en el archivo que se encuentra en siguiendo la ruta /Seurity_API/DAOs/Connection/connectionProperties.json . En su interior es necesario completar los atributos de conexión; servidor, puerto, usuario, contraseña y base de datos.

4. Ejecutar el comando dotnet build desde la consola de comandos. Ref: https://docs.microsoft.com/es-es/dotnet/core/tools/dotnet-build

5. Ejecutar el comando dotnet run desde la terminal del sistema operativo. Ref: https://docs.microsoft.com/es-es/dotnet/core/tools/dotnet-run

6. Es posible sustituir los dos pasos anteriores al abrir el proyecto desde un IDE con opción de arrancado automático.

7. Una vez inicializada, estará disponible desde https://{$endpoint:default:localhost:5003}/Security/.

8. Un servicio adicional que encripta el texto solicitado estará disponible desde https://{$endpoint:default:localhost:5003}/EncryptationService/.

# Modificar url

Dentro de la carpeta situada en ./APIs/Security_API/Properties se encuentra el archivo launchProperties.json . En su interior se puede modificar la dirección y el puerto de acceso.

# Url para pruebas

Actualmente puede probarse enviando la petición a la url https://security-api.azurewebsites.net/ . El servicio es gratuito por lo que pueden producirse fallos debido a la falta de disponibilidad del servidor. Se recomienda repetir cada petición varias veces. En ocasiones la base de datos puede encontrarse saturada, por lo que será necesario esperar unos minutos antes de que vuelva a estar disponible.

# Métodos admitidos en la petición

- new : Si éste parámetro se establece a verdadero añadirá un nuevo usuario. En caso contrario verificará si los datos recibidos en la petición son correctos.

# Cómo se usa

## Dar de alta nuevo usuario

    1. Se realiza una petición get a la url https://{$endpoint:default:localhost:5003}/Security/ que devolverá la clave pública con la que encriptar la información esperada.

    2. Con la clave pública generada en el paso anterior se encriptan el email de usuario y su contraseña y se establece el parámetro new a true. Puede verse una petición de entrada en el apartado siguiente.

    3. Se realiza una petición POST a la url https://{$endpoint:default:localhost:5003}/Security/ . 

    4. La clave pública que es devuelta como respuesta es única para cada usuario y deberá ser almacenada por éste para futuras autentificaciones del mismo.

## Servicio Extra

Para poder probar la aplicación sin necesidad de desarrollar un conector para la misma ésta tiene incluido un servicio extra que permite encriptar cualquier texto mediante una clave pública. Para ello será necesario enviar una petición POST como la que sigue a la url https://{$endpoint:default:localhost:5003}/EncryptationService/ .

### Petición de entrada

<pre>
<code>
{
    "key": "<RSAKeyValue><Modulus>4dK0GrJTtF/1N9EEDIhZPtwpPV7nOhZPV8QjT8f3BUZ1TIwyy0YmxqYurWwvwv1xip5x5wFJ5DUyYRtB1zjiB47GOjxIlsrGIjRJYQm9Sf/7hbJdb6/8sPZN6hNUcPleFc/kCVpch2OwxcV2fimrNB1NSKvAWXj+bnPdQ4wjK4kVFWIuUZBeReDVHreoR4/sQhpMI9CUp6hY8J6vqOyp//CJV6ay41PVvfBPqSl1i0j1Si1BKZ6jDfcKuM+CasMGLb45meNgr4Np/zAjfk6j7/KpscGtbimt5tv86ezrerSW6T7bmsQY34Ybtte84lhxXHhWp/9w5d2NvLue5PWiRL3moA63gJT+xLbtjkBx7+juAb/t0X1JtNRrTAznMhpqSlwWp76VbytthyeQxNWr+NNrGJQE64L2RpMzZYdioYtJ8ZDBdUVtFzHGcEVvBcja/fN/fd6Dm2th8urZ30h7No7+3ObPQjXTqG/3sq1lUmDYu6DJvJbP00+IJrVcvwbH</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>",

    "text":"ejemplo@ejemplo.com"
}
</code>
</pre>

### Ejemplo de respuesta

<pre>
<code>
or+dtsOSdKzXyZJdBNqHGRQQvFa4HXC4AvjsvOHEXqwbvtEkwv5Unx6etYqcCxIH5RCVjNuNO8E3z0A4cQBrXx9yzF428kTqBGxoRXx7OQYd42yFksaOoO4uV22+ozquhI671WoWK74OWC6eQYOhHaWYgvWFSRm29VUbdY2JGWNht9XlRt3prJ3sotsr2OF5BfvAvKsZBTclogchyv8KOYAnx93AXciAzOx6u8rOzrVXRTo0LuvYOAan7wS5RFtbOBBbzK2rx7JI30W7k5QE4IsMSUxBhcK0tyvK5bsb+yYcDYjQPfyLHf1T941LvaDqG4MUgE8QePQdU7tTOfwVpdPAAwK5HNJ3XGMBS0oFAk5PeKu0hYm7cU77tSiSXLMwbbgaPV+df3z1/+JResx2Sr6O1qezHF6GXow+awgKykxgNw3gj4TzMKsZPFaQ0kmiKbQIs4C9IZ0KlRaPdylaK/llvMzq07REycqvrcDRa7a3aCYGE0Fwy4sRo+NX2jle
</code>
</pre>

## Autentificar usuario

    1. Con la clave pública única para cada usuario se encripta la información relativa a su email y contraseña. El elemento new es establecido como false.

    2. Se añade la clave pública como valor del elemento public_key de la petición de entrada. Hay un ejemplo en el apartado siguiente.

    3. Se realiza una petición POST a la url https://{$endpoint:default:localhost:5003}/Security/ .

    4. La respuesta devolverá true o false dependiendo de si el usuario está registrado y la contraseña es correcta.


# Petición de entrada

<pre>
    <code>
{
    "email": "PIwpNi5y3S3g37JXpBfWUF9mhKo2mQHWOmy79HFnLb6BvGSy8S1ayXQaKctaQsBXvDBYJzUizhBQKcHQJGyx+X6rQrzTBlvTAFIlOPznAlPqL3xGayFdJ1mBsNg2clpXFhZs5D9iuKWhJcej+aYbDPYR3hyEmrRchfXALrHxKGsIhlhyWOQpIMqjx9DeOydmXzNiuGJMn1mZahOQkEmZcBuv3Rra0r2aY2nWH2KxiFZoOSu+ZJiRh++QqfjVG5IIrGX9r5V7asB9BWF8U862tqP4UUhYzuvEEM13WaqHMB8H6a3cKqlzoUL+jIfnf5vYI7Mdr+MwiboXsK4PGD4Qd61NMQWPv9ZVeMtopjpUzDWDBx6n1cF2vv0BngMoU1o8yOG/2m1Oug8SqfqsiDasY0h9vWp+bmsH3zrFjDcmg3X8sIA+o45PhnaVBjUqmlVid04j5t+QbXUru8B57ZBGkoU94dWXzeTSEZ6Qrr1aPNFfEfpiwhL9OVIe5aF1k0cg",
    "pass": "iLqLoAOSx2oecCirygpc9UcCrJCdWTlnHc2+piYLxY/YnfmU5hTOrpai9A/8aoFR2WUbaBv+cUpJmLKfLy6/UBnuuusLtm+BMKP1dkZwxLaxFe1Dyj7NGd/yCYiKM/8Wp4GYe6zShc5s0WBOuT6Hn7dJtrRyU8igRNfWENJkdig67KiUvIvnSAzb6CWbwu4l13B66zStAsJU47FM4yWbUcFQmQ8ffS5tBtN62qQdHFzX+AAefFk+ObEJMHyc1ajeSHzCJSkwOvyF0rWH1WbkfSF9eXjuzcRTs95mKHOfFJPrMnF/+pTjzyNHD9tZJQkCKydXYLU8Z/qytJ3USev73KvBO/9L9he1tA8Re3gABqkQ7lKB2UZKOgHh5UNT9IsBgVHxe2GM0rb124DiOGY8IyeMD2L/1UDVvm86l+rAkPX3hWtWd5ePqEG1mRY0Hiuf3nv9h98gTd+reYg1EGy4S9qjKVCZVAur/e7af9pdgNWGxa/jVoc2gv+5RsvnL6uk",
    
    "new": false,

    "public_key" : "<RSAKeyValue><Modulus>nlFoRNsY4J2Bfvg/5e8NHocLCLkLbrWte/JnBCnbT2hn1Zh3s/mOHv6SCh1UmaXZ9b5Ey0/hKibOU1xwSb6m8l1VSAdaz63tU0ayfrg1mFLwi2vW8MIDpR6yJLO+HHUpyRW7UTJ/WFNmPLckRUTxdekl3XAwqrZ+fMcpNqavD8rKG62x3gUetngrZXSeC5O732d4IoTQb4inTPDoCT+QW2rg1CLlhic+WRPyp/T97CAKeCLnuzLfUKVx574/WQ0BGFxPn4oOdfMmm5EbsJpzcqMge0u6YARasSzjbC2MlErP9VcrhTAlQdyidiSxNuyJxInIyVt15XMDO/D/h7WgfkXh4F6aunRsseXSMRiLSoVn/45/nr5+dxC+V7Eb16ZeL3MYOg2BvetsNMyLEfVGhVU+zhZE76G1yQTkGfGV2gQca/wjJLphCvKE9SewW1GhHFuwrBN6e7SzXV8GSZhCE0VNgpcbe/IoW2LX414Q4xaNFRwyrV6FtXWbbSVkNniF</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
}
    </code>
</pre>
# Ejemplos de respuesta
<pre>
    <code>
<RSAKeyValue><Modulus>nlFoRNsY4J2Bfvg/5e8NHocLCLkLbrWte/JnBCnbT2hn1Zh3s/mOHv6SCh1UmaXZ9b5Ey0/hKibOU1xwSb6m8l1VSAdaz63tU0ayfrg1mFLwi2vW8MIDpR6yJLO+HHUpyRW7UTJ/WFNmPLckRUTxdekl3XAwqrZ+fMcpNqavD8rKG62x3gUetngrZXSeC5O732d4IoTQb4inTPDoCT+QW2rg1CLlhic+WRPyp/T97CAKeCLnuzLfUKVx574/WQ0BGFxPn4oOdfMmm5EbsJpzcqMge0u6YARasSzjbC2MlErP9VcrhTAlQdyidiSxNuyJxInIyVt15XMDO/D/h7WgfkXh4F6aunRsseXSMRiLSoVn/45/nr5+dxC+V7Eb16ZeL3MYOg2BvetsNMyLEfVGhVU+zhZE76G1yQTkGfGV2gQca/wjJLphCvKE9SewW1GhHFuwrBN6e7SzXV8GSZhCE0VNgpcbe/IoW2LX414Q4xaNFRwyrV6FtXWbbSVkNniF</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>
    </code>
</pre>

<pre>
    <code>
true
    </code>
</pre>

# Manual Simple de Usuario

- [MarkDown](./SimpleUserGuide/ManualSimpleUsuario_Security_API.md)

- [PDF](./SimpleUserGuide/ManualSimpleUsuario_Security_API.pdf)

- [DOC](./SimpleUserGuide/ManualSimpleUsuario_Security_API.doc)

- [DOCX](./SimpleUserGuide/ManualSimpleUsuario_Security_API.docx)
