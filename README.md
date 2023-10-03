# JWT_Manual


## JSON Web Tokens (JWT)

JSON Web Tokens (JWT) es un estándar abierto (RFC 7519) que define un método compacto y seguro para representar información entre dos partes como un objeto JSON. Es ampliamente utilizado para la autenticación y la transmisión segura de información entre sistemas.

### Características principales de JWT:

- **Compacto y autónomo:** Un JWT se representa en una cadena de caracteres compacta y autocontenida, lo que lo hace fácil de transportar y transmitir.

- **Seguridad:** JWTs pueden estar firmados digitalmente y/o cifrados, lo que garantiza la integridad y la confidencialidad de los datos transmitidos.

- **Transporte ligero:** Dado que son representados como cadenas de caracteres, los JWTs son ideales para ser transmitidos a través de URL, encabezados HTTP o como parte de datos de formularios.

### Estructura de un JWT

Un JWT consta de tres partes separadas por puntos (`.`):

1. **Header (Encabezado):** Contiene metadatos sobre el tipo de token y el algoritmo de firma utilizado.

2. **Payload (Carga útil):** Contiene los datos que queremos transmitir, como reclamaciones (claims) específicas, como el ID de usuario o roles.

3. **Signature (Firma):** Se utiliza para verificar que el mensaje no ha sido alterado durante la transmisión y que proviene de una fuente confiable. La firma se calcula utilizando la información del encabezado, la carga útil y una clave secreta.

### Uso común de JWT

Los JWTs son ampliamente utilizados en aplicaciones web y servicios API para implementar:

- **Autenticación:** Los JWTs se emiten después de que un usuario se autentica con éxito y se utilizan para identificar al usuario en solicitudes posteriores.

- **Autorización:** Los JWTs pueden incluir información de roles y permisos, lo que permite a los servidores autorizar a los usuarios para realizar ciertas acciones.

- **Sesiones seguras:** Almacenar información de sesión en un JWT permite a las aplicaciones web mantener sesiones de usuario sin necesidad de almacenar información en el servidor.

- **Protección contra CSRF:** Los JWTs pueden ayudar a proteger contra ataques de falsificación de solicitudes entre sitios (CSRF) al requerir que las solicitudes contengan un token válido.


### Implementacion en .NET

## Requisitos.
- **Version de .NET:** Mayor o igual a .NET 5.0
- **Implementacion de librerias:**
     Persistencia: Microsoft.EntityFrameworkCore , Version >= 7
                   Pomelo.EntityFrameworkCore.MySql ,Version >= 7
     Dominio:  Microsoft.EntityFrameworkCore , Version >= 7
     API : Microsoft.EntityFrameworkCore , Version >= 7
           AutoMapper.Extensions.Microsoft.DependencyInjection , Version >= 12
           Microsoft.EntityFrameworkCore.Design , Version >= 7.

- **Familiarizacion con el manejo de capas en proyectos API**
 ![Captura de pantalla 2023-10-03 013056](https://github.com/Luiscvj/JWT_Manual/assets/130381389/b028f085-e328-460a-931e-95e0e56b2d1e)



### Procedimiento
