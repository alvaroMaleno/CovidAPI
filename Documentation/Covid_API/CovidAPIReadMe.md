# Introducción

Siguiendo con la tendencia actual, especialmente dentro del marco de trabajo de .NET, de fraccionar las aplicaciones en diversos microservicios capaces de funcionar de manera independiente y de ser gestionados por una persona que será la responsable de uno o varios de los mismos, el conjunto total de esta aplicación ha sido dividido en tres aplicaciones. Dos de ellas tienen una funcionalidad independiente de las demás y atienden a la responsabilidad procedural de la aplicación, y una tercera que se emplea como marco de interacción con el usuario. 

La interacción con el usuario así como la coordinación de los diferentes procesos y funcionalidades que presenta esta aplicación le corresponde a esta API. 

# Responabilidades

1. Recepción de nuevas peticiones de registro de usuario y transmisión de las mismas a la API de Seguridad Security_API.

2. Entrega de clave pública única de usuario tras cada nuevo registro.

3. Transmisión de peticiones de autentificación a la API de seguridad y generación de token en caso de que el usuario haya ingresado correctamente sus datos de ingreso al sistema.

4. Autentificación token mediante de usuarios que realizan nuevas peticiones de información.

5. Transmisión de las peticiones de consulta llevadas a cabo por los usuarios hacia la API de gestión de datos DataAccess_API y de su respuesta en sentido inverso.

6. Generación y refresco de una memoria caché que permita el servicio de los datos acerca de la evolución de la pandemia de la totalidad de países disponibles, filtrados por rango de fechas, empleando para ello el menor tiempo posible.

# Tecnologías empleadas

    - SDK de .Net Core 3.1
    - ASP .Net Core 3.1
    - .Net Core 3.1
    - JSON
    - JWT
    - NewtonSoft
    - Draw.io

## Documentación Oficial

- **.Net Core**: https://docs.microsoft.com/es-es/dotnet/core/

- **JSON**: https://developer.mozilla.org/es/docs/Learn/JavaScript/Objects/JSON
- **Newtonsoft**: https://www.newtonsoft.com/json/help/html/Introduction.htm
- **Draw.io**: http://draw.io




# Puesta en Marcha

1. Instalar las cuatro primeras dependencias citadas en el apartado anterior.

2. Poner en marcha las dos APIs componentes: DataAccess_API y Security_API.

3. Inicializar.

4. Una vez inicializada, estará disponible desde:
    -  https://localhost:5001/api/covid para consultas.

    - https://localhost:5001/api/authorize para autentificaciones.

    - https://localhost:5001/api/user para nuevos usuarios.

5. Ver manual completo de usuario para conocer el proceso en profundidad.

# Modificar url

Dentro de la carpeta situada en Covid_API/Properties se encuentra el archivo launchProperties.json . En su interior se puede modificar la dirección y el puerto de acceso.


# Nuevo Usuario

![Nuevo Usuario](./Process/GeneralNewUser.png)

## Petición ejemplo

- Tras obtener la clave pública con la que encriptar la información realizando una petición get a https://localhost:5001/api/user: 
<pre>
    <code>
{
    "email": "xP9BAdai/dJDWZqAKaWjUWiSvK4U1I76xxkYcU2q+dFf2jtY8yN3MUXqGxCdmmyAPtw2lVnWQTPLlp8EbTunvRfgarWv3ZN5ztzWqH/jauEntwfiJNi5WLowkWsk2nfC/M6+7Pc5cJjcP7xOFUmKDojWaMNo2mZns/WeCjzUCbl3VrXZoWC5tguP+nG+/FuZu/1JapVcRapXA8Y6Fv3BhDk8MFg2ieSGShBbloantcXleOqxsLCVxr09elHjvvbCz6keGWBYdmmUVViSZvBgs8nzIwrUCa9prcyCp/MVLPCKd4/J0yj8rOFSP8b89wyo0oGwAObrZfy3Dj89Gh+E4IFnEF8A52GkSAM7qY6LISKkDyvN/vwI/S+N7EYPQx92s9eqB0uHYY9hA3Ut+pXx8ByaM5rcPlF284HvnApsHoymdIAry60BdD711L2ejv1cp/GnPoG3gpaeup8zmo60qPMTGXoYnLrUW9vweyiHqCJ01G5cFYxti+KMhIxfml9V",
    "pass": "WqMti7Du5JldRjaP0rDTFA/GmzYrneQg6QUrmLoMtvB6/3U/Ego3qW/IIcKQ7CEmRwyTuirElwd1WIGqNojZ7bgcyZVy8mQciMEmSSF6l8IrYny4OXWSfcHpG6G+2FP/zQZrglckTc2Hx56dJc2IY6yJJDGduMakesBHqP/UDkpVZGBUwfOx13NTIXRTQgmCGb+Se3tyK7aI+0eIxDIyFZ+SCK8TJZXykSaVbPltoJ4R/9z4/LG8VhYIkmIb5FQfu9lgSwJd+NUca+7vr8eSuobsbG4rx0YJs0CF78TRFU8ZshNbMXgVfl2EiTQfCR3Gk5UBKKhdi551rPe2cL8JwX2jtJMc6ApD7Rh/xV/rWyPbplSRikI7gO4Y2zbSkUu6rsc1UHEc/MGHT65wUerzDtvocQaRii2LGrLNOFB1Li1YJLi3eD9jImqgNaCjJHtK6jntF3VVn2CwwU6ROSkEkm8wChuiARjxq3yAPLU+wS+riLoinNf7TlbqahTpsanF",
    "new": true
}
    </code>
</pre>

## Respuesta de ejemplo

    <RSAKeyValue><Modulus>nlFoRNsY4J2Bfvg/5e8NHocLCLkLbrWte/JnBCnbT2hn1Zh3s/mOHv6SCh1UmaXZ9b5Ey0/hKibOU1xwSb6m8l1VSAdaz63tU0ayfrg1mFLwi2vW8MIDpR6yJLO+HHUpyRW7UTJ/WFNmPLckRUTxdekl3XAwqrZ+fMcpNqavD8rKG62x3gUetngrZXSeC5O732d4IoTQb4inTPDoCT+QW2rg1CLlhic+WRPyp/T97CAKeCLnuzLfUKVx574/WQ0BGFxPn4oOdfMmm5EbsJpzcqMge0u6YARasSzjbC2MlErP9VcrhTAlQdyidiSxNuyJxInIyVt15XMDO/D/h7WgfkXh4F6aunRsseXSMRiLSoVn/45/nr5+dxC+V7Eb16ZeL3MYOg2BvetsNMyLEfVGhVU+zhZE76G1yQTkGfGV2gQca/wjJLphCvKE9SewW1GhHFuwrBN6e7SzXV8GSZhCE0VNgpcbe/IoW2LX414Q4xaNFRwyrV6FtXWbbSVkNniF</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>

# Autentificar Usuario

![Autentificar Usuario](./Process/Authenticate.png)

## Petición de ejemplo

- POST a https://localhost:5001/api/authorize:

<pre>
    <code>
{
    "email": "xP9BAdai/dJDWZqAKaWjUWiSvK4U1I76xxkYcU2q+dFf2jtY8yN3MUXqGxCdmmyAPtw2lVnWQTPLlp8EbTunvRfgarWv3ZN5ztzWqH/jauEntwfiJNi5WLowkWsk2nfC/M6+7Pc5cJjcP7xOFUmKDojWaMNo2mZns/WeCjzUCbl3VrXZoWC5tguP+nG+/FuZu/1JapVcRapXA8Y6Fv3BhDk8MFg2ieSGShBbloantcXleOqxsLCVxr09elHjvvbCz6keGWBYdmmUVViSZvBgs8nzIwrUCa9prcyCp/MVLPCKd4/J0yj8rOFSP8b89wyo0oGwAObrZfy3Dj89Gh+E4IFnEF8A52GkSAM7qY6LISKkDyvN/vwI/S+N7EYPQx92s9eqB0uHYY9hA3Ut+pXx8ByaM5rcPlF284HvnApsHoymdIAry60BdD711L2ejv1cp/GnPoG3gpaeup8zmo60qPMTGXoYnLrUW9vweyiHqCJ01G5cFYxti+KMhIxfml9V",
    "pass": "WqMti7Du5JldRjaP0rDTFA/GmzYrneQg6QUrmLoMtvB6/3U/Ego3qW/IIcKQ7CEmRwyTuirElwd1WIGqNojZ7bgcyZVy8mQciMEmSSF6l8IrYny4OXWSfcHpG6G+2FP/zQZrglckTc2Hx56dJc2IY6yJJDGduMakesBHqP/UDkpVZGBUwfOx13NTIXRTQgmCGb+Se3tyK7aI+0eIxDIyFZ+SCK8TJZXykSaVbPltoJ4R/9z4/LG8VhYIkmIb5FQfu9lgSwJd+NUca+7vr8eSuobsbG4rx0YJs0CF78TRFU8ZshNbMXgVfl2EiTQfCR3Gk5UBKKhdi551rPe2cL8JwX2jtJMc6ApD7Rh/xV/rWyPbplSRikI7gO4Y2zbSkUu6rsc1UHEc/MGHT65wUerzDtvocQaRii2LGrLNOFB1Li1YJLi3eD9jImqgNaCjJHtK6jntF3VVn2CwwU6ROSkEkm8wChuiARjxq3yAPLU+wS+riLoinNf7TlbqahTpsanF",
    
    "new": false,

    "public_key" : "<RSAKeyValue><Modulus>nlFoRNsY4J2Bfvg/5e8NHocLCLkLbrWte/JnBCnbT2hn1Zh3s/mOHv6SCh1UmaXZ9b5Ey0/hKibOU1xwSb6m8l1VSAdaz63tU0ayfrg1mFLwi2vW8MIDpR6yJLO+HHUpyRW7UTJ/WFNmPLckRUTxdekl3XAwqrZ+fMcpNqavD8rKG62x3gUetngrZXSeC5O732d4IoTQb4inTPDoCT+QW2rg1CLlhic+WRPyp/T97CAKeCLnuzLfUKVx574/WQ0BGFxPn4oOdfMmm5EbsJpzcqMge0u6YARasSzjbC2MlErP9VcrhTAlQdyidiSxNuyJxInIyVt15XMDO/D/h7WgfkXh4F6aunRsseXSMRiLSoVn/45/nr5+dxC+V7Eb16ZeL3MYOg2BvetsNMyLEfVGhVU+zhZE76G1yQTkGfGV2gQca/wjJLphCvKE9SewW1GhHFuwrBN6e7SzXV8GSZhCE0VNgpcbe/IoW2LX414Q4xaNFRwyrV6FtXWbbSVkNniF</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
}
    </code>
</pre>

## Respuesta de ejemplo

<pre>
    <code>

eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI8UlNBS2V5VmFsdWU-PE1vZHVsdXM-bmxGb1JOc1k0SjJCZnZnLzVlOE5Ib2NMQ0xrTGJyV3RlL0puQkNuYlQyaG4xWmgzcy9tT0h2NlNDaDFVbWFYWjliNUV5MC9oS2liT1UxeHdTYjZtOGwxVlNBZGF6NjN0VTBheWZyZzFtRkx3aTJ2VzhNSURwUjZ5SkxPK0hIVXB5Ulc3VVRKL1dGTm1QTGNrUlVUeGRla2wzWEF3cXJaK2ZNY3BOcWF2RDhyS0c2MngzZ1VldG5nclpYU2VDNU83MzJkNElvVFFiNGluVFBEb0NUK1FXMnJnMUNMbGhpYytXUlB5cC9UOTdDQUtlQ0xudXpMZlVLVng1NzQvV1EwQkdGeFBuNG9PZGZNbW01RWJzSnB6Y3FNZ2UwdTZZQVJhc1N6amJDMk1sRXJQOVZjcmhUQWxRZHlpZGlTeE51eUp4SW5JeVZ0MTVYTURPL0QvaDdXZ2ZrWGg0RjZhdW5Sc3NlWFNNUmlMU29Wbi80NS9ucjUrZHhDK1Y3RWIxNlplTDNNWU9nMkJ2ZXRzTk15TEVmVkdoVlUremhaRTc2RzF5UVRrR2ZHVjJnUWNhL3dqSkxwaEN2S0U5U2V3VzFHaEhGdXdyQk42ZTdTelhWOEdTWmhDRTBWTmdwY2JlL0lvVzJMWDQxNFE0eGFORlJ3eXJWNkZ0WFdiYlNWa05uaUY8L01vZHVsdXM-PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjwvUlNBS2V5VmFsdWU-IiwianRpIjoiYzcwOGJhNGMtMzdmMy00ODIyLWFlY2ItNGVkODM2N2FjOGZmIiwibmJmIjoxNTk2MTMwOTQzLCJleHAiOjE1OTYyMTczNDMsImlzcyI6ImNvdmlkVG9rZW5Jc3N1ZXIiLCJhdWQiOiJjb3ZpZFRva2VuQXVkaWVuY2UifQ.2R01pAlinhRYsAIqsVtl9np6BegdzHHIpaBuCBirkLY

    </code>
</pre>

# Petición de Información

![Petición Información](./Process/OneOrMoreCountries.png)

## Petición de ejemplo

- A https://localhost:5001/api/covid con token como autentificación de cabecera:

<pre>
    <code>
    {
    "covid_data":
    {
        "countries":
        [
            
            "ES",
            "AD",
            "AE",
            "AU",
            "NO",
            "PA",
            "SS",
            "TG",
            "TL"

            
            
        ],
        "dates":
        {
            
            "startDate": "31/12/2019",
            "endDate": "28/07/2020",
            "separator": "/"
        
        },

        "dataType": "any"
    }
    
}

    </code>
</pre>

## Respuesta de ejemplo

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

# Petición de Información para todos los países disponibles

![Petición Información](./Process/CachéProcess.png)

## Petición de ejemplo

- A https://localhost:5001/api/covid con token como autentificación de cabecera:

<pre>
    <code>
    {
    "covid_data":
    {
        "countries":
        [
            "*"
        ],
        "dates":
        {
            
            "startDate": "31/12/2019",
            "endDate": "28/07/2020",
            "separator": "/"
        
        },

        "dataType": "any"
    }
    
}

    </code>
</pre>

## Respuesta de ejemplo

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
