# App Municipio – Todos los módulos en .NET MAUI

Esta versión concentra en la aplicación MAUI los tres módulos solicitados. El proyecto Web permanece en la solución, pero no es necesario abrirlo para demostrar las funciones.

## Accesos de demostración

- Administrador: `admin@quito.gob.ec` / `Admin123`
- Ciudadano: `ciudadano@quito.gob.ec` / `Ciudadano123`

La API crea estas cuentas y datos de demostración al iniciar por primera vez en la base `AppMunicipio_Database_v5`.

## Pantallas incluidas en MAUI

### Módulo 1 – Participación Ciudadana

- Agenda de talleres, ferias y campañas.
- Reserva y cancelación de cupos de talleres.
- Gestión CRUD de talleres, ferias y campañas para administrador.
- Consulta de inscritos por taller.
- Inscripciones generales y voluntariado: crear, consultar, editar y cancelar.

### Módulo 2 – Reportes, Seguridad y Bienestar Social

- Reporte ciudadano con GPS y fotografía.
- Gestión de reportes y cambio de estado.
- Alertas comunitarias con GPS e imagen.
- Casos de personas en situación de calle.
- Alertas y seguimiento de situaciones de violencia.
- Panel estadístico dentro de MAUI.

### Módulo 3 – Solicitudes Ciudadanas

- Obras prioritarias para barrios con GPS.
- Reserva de espacios y validación de cruce de horarios.
- Permisos municipales.
- Carga de documentos PDF, Word, JPG o PNG.
- Consulta y gestión de obligaciones e impuestos.
- Consulta del avance mediante el estado y observaciones.

## Cómo iniciar

1. Cierra las copias anteriores del proyecto.
2. Abre `AppMunicipio.sln`.
3. Ejecuta primero `ApiMunicipio` con el perfil `http`.
4. La terminal debe mostrar `Now listening on: http://localhost:5279`.
5. No cierres la API.
6. En otra ventana de Visual Studio establece `MauiMunicipio` como proyecto de inicio.
7. Selecciona `Windows Machine` y ejecuta.

También puedes iniciar la API haciendo doble clic en `INICIAR_API.bat`.

## Compilación ARM64

La computadora usada para las pruebas es ARM64. Si Visual Studio no genera el ejecutable, ejecuta `COMPILAR_MAUI_ARM64.bat` y luego vuelve a pulsar Windows Machine.

## Navegación

En la barra inferior abre **Módulos**. Ahí aparecen todas las funciones organizadas en los tres módulos. Los botones administrativos se habilitan al iniciar sesión con la cuenta administradora.

## GPS, cámara y archivos

- Activa la ubicación de Windows antes de usar GPS.
- Permite el acceso a ubicación y cámara cuando la aplicación lo solicite.
- La carga de archivos admite hasta 10 MB.
- Para probar en Windows, la conexión de la app debe ser `http://localhost:5279`.
