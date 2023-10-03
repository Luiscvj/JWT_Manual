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
     5)Generamos nuestra Unidad de trabajo para lo cual se tuvo que crear con anterioridad las interfaces y repositorios de cada entidad.El objetivo es             poder   utilizar **lazy loading**.
     6)Asociamos su interfaz a la clase de **UnitOfWork** con el de poder acceder a ella desde cualquier parte del codigo.  
     ![InyeccionUnidadDeTrabajo](https://github.com/Luiscvj/JWT_Manual/assets/130381389/0fc3b4b9-5fb0-44de-8a00-6cd83e23c9ab)
     
-**Creacion Helpers:** Creamos una clase llamada enciptacion para poder usar todos sus metodos al momento de querer encriptar la infromacion para enviarla al servidor y validar el token.De igual forma se creo su interfaz y se inyecto al contenedor de dependencias.
Dentro de esta clase generaremos dos metodos base para **encriptar** y **desencriptar** la informacion.
Aqui tambien tendremos dos llaves  que nos ayudaran al momento de validar el token ,**Login_Key** y **User_Key**.

https://github.com/Luiscvj/JWT_Manual/issues/4#issue-1923438474




---------------------------------------------------------------------------------------------------

## Creacion de los metodos para la generacion del token##
En **Services** tendremos toda la logica detras de la generacion del token , a continuacion se explicaran los metodos.

## RegisterAsync
El método `RegisterAsync` en este fragmento de código es responsable de registrar usuarios en la  aplicación.

### Descripción General

-El método `RegisterAsync` forma parte de la funcionalidad de registro de usuarios en tu proyecto. Permite a los usuarios registrarse en la aplicación. A continuación, se describe cómo funciona este método:

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

```
## GetTokenLogin
# Generación de Token de Inicio de Sesión

El método `getTokenLogin` se utiliza para generar un token de inicio de sesión por parte del cliente. Este token se crea utilizando una combinación de la encriptación AES-256 y una cadena deseada. A continuación, se describe cómo funciona este método:

```csharp
public string getTokenLogin(string email, string password)
{
    // Paso 1: Obtención de la fecha y hora actual en formato UTC.
    string fecha = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

    // Paso 2: Creación del token de inicio de sesión.
    string tokenLogin = _encriptacion.AES256_Encriptar(
        _encriptacion.AES256_LOGIN_Key,
        fecha + '#' + email + '#' + _encriptacion.GetSHA256(password)
    );

    // Paso 3: Retorno del token de inicio de sesión.
    return tokenLogin;
}
````
## LoginByToken

# Autenticación de Usuario por Token

El método `LoginByToken` se utiliza para autenticar a un usuario mediante un token. Este token contiene información cifrada y se valida para permitir el acceso. A continuación, se describe cómo funciona este método:

```csharp
public string LoginByToken(string loginToken)
{
    try
    {
        // Paso 1: Inicialización de variables.
        string tokenUsuario = "";
        string tokenDecoficado = _encriptacion.AES256_Desencriptar(_encriptacion.AES256_LOGIN_Key, loginToken);

        // Paso 2: Extracción de información del token.
        string fecha = tokenDecoficado.Split('#')[0];
        string email = tokenDecoficado.Split('#')[1];
        string password = tokenDecoficado.Split('#')[2];

        // Paso 3: Validación de la fecha.
        DateTime fechaLogin = DateTime.ParseExact(fecha, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
        if (DateTime.UtcNow.Subtract(fechaLogin).TotalSeconds >= 30)
        {
            return "-1";    // -1 = Límite de tiempo excedido
        }

        // Paso 4: Validación del inicio de sesión.
        var verificarUsuario = _unitOfWork.Usuarios.Find(p => p.Email == email && p.Password == password).FirstOrDefault();

        if (verificarUsuario != null)
        {
            // Paso 5: Generación de un nuevo token de usuario.
            tokenUsuario = verificarUsuario.Email + "#" + DateTime.UtcNow.AddHours(18).ToString("yyyyMMddHHmmss");
            tokenUsuario = _encriptacion.AES256_Encriptar(_encriptacion.AES256_USER_Key, tokenUsuario);
            
            // Paso 6: Actualización de la información del usuario.
            verificarUsuario.FechaCreacion = DateTime.UtcNow;
            verificarUsuario.FechaExpiracion = DateTime.UtcNow.AddHours(18);
            _unitOfWork.SaveChanges();
            
            // Paso 7: Retorno del nuevo token de usuario.
            return tokenUsuario;
        }
        else
        {
            return "-2";    // -2 = Usuario o contraseña incorrectos
        }
    }
    catch (Exception)
    {
        return "-3";        // -3 = Error
    }
}
```
## ValidarTokenUsuario
# Validación de Token de Usuario

El método `ValidarTokenUsuario` se utiliza para validar un token de usuario en cada llamada a la aplicación. Este token se descifra y se verifica si ha caducado o si el usuario asociado a él sigue siendo válido. A continuación, se describe cómo funciona este método:

```csharp
public bool ValidarTokenUsuario(string tokenUsuario)
{
    try
    {
        // Paso 1: Corrección del token y descifrado.
        tokenUsuario = _encriptacion.CorregirToken(tokenUsuario);
        string tokenDescodificado = _encriptacion.AES256_Desencriptar(_encriptacion.AES256_USER_Key, tokenUsuario);
        
        // Paso 2: Extracción de información del token.
        string emailUsuario = tokenDescodificado.Split('#')[0];
        string fecha = tokenDescodificado.Split('#')[1]; 
        
        // Paso 3: Búsqueda del usuario en la base de datos.
        var verificarEmail = _unitOfWork.Usuarios.Find(p => p.Email == emailUsuario).FirstOrDefault();
        
        // Paso 4: Validación de la fecha de caducidad.
        DateTime fechaCaducidad = DateTime.ParseExact(fecha, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
        if (DateTime.UtcNow > fechaCaducidad || DateTime.UtcNow > verificarEmail.FechaExpiracion)
            return false;
        else
            return true;
    }
    catch (Exception)
    {
        return false;
    }
}
```

-**Creacion del controlador:**
     -En el controlador vamos a realizar todas las solicitudes al servidor para poder generar el token .Cada solicitud se explica a continuacion:

## AddUser:

Me permite agregar un usario a la base de datos:

```csharp
public async Task<ActionResult> AddUser(RegisterDto registerDto)
{
    // Mapear los datos del DTO a un objeto de Usuario
    Usuario user = _mapper.Map<Usuario>(registerDto);

    // Llamar al método RegisterAsync del servicio UserService para registrar al usuario
    _userService.RegisterAsync(user);
    
    // Devolver una respuesta HTTP OK con un mensaje de éxito
    return Ok("Usuario creado con éxito");
}
```

## GetTokenLogin
```csharp
public ActionResult GetTokenLogin([FromForm] string email, [FromForm] string password)
{
    // Este método se utiliza para generar un token de autenticación para un usuario
    // basado en su dirección de correo electrónico (email) y contraseña (password).

    // Se llama al método getTokenLogin del servicio _userService y se le pasan
    // los parámetros email y password como datos del formulario.

    // Luego, el token generado se devuelve como parte de la respuesta HTTP OK.

    return Ok(_userService.getTokenLogin(email, password));
}
```

## LoginByToken 

```csharp
public ActionResult LoginByToken([FromForm] string loginToken)
{
    // Este método se utiliza para realizar un proceso de inicio de sesión
    // utilizando un token de autenticación (loginToken) proporcionado por el cliente.

    // Se llama al método LoginByToken del servicio _userService y se le pasa
    // el token de inicio de sesión como un dato del formulario.

    // Luego, se realiza una comprobación del valor devuelto por el servicio y se
    // devuelve una respuesta HTTP adecuada en función del resultado.

    string token = _userService.LoginByToken(loginToken);

    switch (token)
    {
        case "-1": return BadRequest("Límite de tiempo excedido");
        case "-2": return BadRequest("Usuario o clave incorrectos");
        case "-3": return BadRequest("No se pudo hacer el login, revise los datos enviados");
        default: return Ok(token);
    }
}

```




