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



### Procedimiento :
-**Configuracion basica:**
     1)Configuramos nuestra clase de contexto.
     2) Creamos nuestra cadena de conexion en appsettings.json
     3)Asociamos nuestra clase de contexto con nuestra cadena de conexion  en nuestro contenedor de dependencias para poder acceder a la base de datos.
     4) Creamos nuestra clase **Usuario** , la cual nos va permitir registrar, validar y usar el token generado.    
     ![usuario](https://github.com/Luiscvj/JWT_Manual/assets/130381389/93385dd5-dbb3-41d4-9531-4f2a5627c83e)   
     5)Generamos nuestra Unidad de trabajo para lo cual se tuvo que crear con anterioridad las interfaces y repositorios de cada entidad.El objetivo es poder      utilizar **lazy loading**.
     6)Asociamos su interfaz a la clase de **UnitOfWork** con el de poder acceder a ella desde cualquier parte del codigo.
     ![InyeccionUnidadDeTrabajo](https://github.com/Luiscvj/JWT_Manual/assets/130381389/0fc3b4b9-5fb0-44de-8a00-6cd83e23c9ab)
     
-**Creacion Helpers:** Creamos una clase llamada enciptacion para poder usar todos sus metodos al momento de querer encriptar la infromacion para enviarla al servidor y validar el token.De igual forma se creo su interfaz y se inyecto al contenedor de dependencias.
Dentro de esta clase generaremos dos metodos base para **encriptar** y **desencriptar** la informacion.
Aqui tambien tendremos dos llaves  que nos ayudaran al momento de validar el token ,**Login_Key** y **User_Key**.

https://github.com/Luiscvj/JWT_Manual/issues/4#issue-1923438474




---------------------------------------------------------------------------------------------------

## Creacion de los metodos para la generacion del token##
En **Services** tendremos toda la logica detras de la generacion del token , a continuacion se explicaran los metodos.


El método `RegisterAsync` en este fragmento de código es responsable de registrar usuarios en ña  aplicación.

### Descripción General

El método `RegisterAsync` forma parte de la funcionalidad de registro de usuarios en tu proyecto. Permite a los usuarios registrarse en la aplicación. A continuación, se describe cómo funciona este método:

```csharp
public async Task<object> RegisterAsync(Usuario user)
{
    // Paso 1: Creación de un objeto Usuario con los datos proporcionados.
    var usuario = new Usuario
    {
        Email = user.Email,
        Password = _encriptacion.GetSHA256(user.Password),
    };

    Console.WriteLine(usuario);

    // Paso 2: Comprobación de si el usuario ya existe en la base de datos.
    var usuarioExiste = _unitOfWork.Usuarios
                                .Find(u => u.Email == user.Email)
                                .FirstOrDefault();

    // Paso 3: Manejo de la lógica de registro.
    if (usuarioExiste == null)
    {
        try
        {
            // Paso 3.1: Agregar el nuevo usuario a la base de datos.
            _unitOfWork.Usuarios.Add(usuario);
            
            // Paso 3.2: Guardar los cambios en la base de datos.
            await _unitOfWork.SaveChanges();

            // Paso 3.3: Retornar un mensaje de éxito.
            return new
            {
                mensaje = "Se ha registrado exitosamente",
                status = true
            };
        }
        catch (Exception ex)
        {
            // Paso 3.4: Manejo de excepciones en caso de error.
            var message = ex.Message;
            return new
            {
                mensaje = message,
                status = false
            };
        }
    }
    else
    {
        // Paso 4: Retornar un mensaje de error si el usuario ya está registrado.
        return new
        {
            mensaje = "Ya se encuentra registrado.",
            status = false
        };
    }
}

