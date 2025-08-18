<h1>KanbanBack</h1>

<p>Backend para la aplicación Kanban, desarrollado en <strong>.NET 8</strong> con <strong>Entity Framework Core</strong>.</p>

<h2>Tecnologías usadas</h2>
<ul>
  <li>.NET 8.0 (net8.0)</li>
  <li>C#</li>
  <li>Entity Framework Core 8.0.18 (SQL Server)</li>
  <li>Swagger / Swashbuckle 6.6.2</li>
  <li>SQL Server LocalDB (<code>(localdb)\\matim</code>)</li>
  <li>CORS habilitado (<code>AllowAnyOrigin, AllowAnyMethod, AllowAnyHeader</code>)</li>
</ul>
<h2>Instalación de dependencias</h2>
<p>Antes de ejecutar el proyecto, instalar las siguientes dependencias mediante <code>dotnet add package</code> o NuGet:</p>
<ul>
  <li>Entity Framework Core SQL Server:
    <pre><code>dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.18</code></pre>
  </li>
  <li>EF Core Tools:
    <pre><code>dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.18</code></pre>
  </li>
</ul>
<p>Estas librerías ya están listadas en el archivo <code>.csproj</code>, pero ejecutar los comandos asegura que estén instaladas en la máquina del usuario.</p>

<h2>Requisitos</h2>
<ul>
  <li><a href="https://dotnet.microsoft.com/en-us/download/dotnet/8.0">.NET 8 SDK</a></li>
  <li>SQL Server LocalDB o SQL Server Express</li>
  <li>Visual Studio 2022+ o VS Code</li>
</ul>

<h2>Instalación y configuración</h2>
<ol>
  <li>Clonar el repositorio:
    <pre><code>git clone &lt;https://github.com/MatiasMichaux98/backKanban.git&gt;
cd KanbanBack</code></pre>
  </li>
  <li>Restaurar dependencias:
    <pre><code>dotnet restore</code></pre>
  </li>
  <li>Configurar la base de datos en <code>appsettings.json</code>:
    <pre><code>{
  "ConnectionStrings": {
    "conexionSQL": "Data Source=(localdb)\\&lt;TU_INSTANCIA&gt;;Initial Catalog=&lt;TU_BASE_DE_DATOS&gt;;Integrated Security=True;TrustServerCertificate=True;"
  }
}</code></pre>
  </li>
  <li>Aplicar migraciones de Entity Framework (si es necesario):
    <pre><code>dotnet ef database update</code></pre>
  </li>
</ol>

<h2>CORS</h2>
<p>Política abierta configurada como <code>"CustomPolicy"</code>: permite cualquier origen, método y cabecera.</p>

<h2>Swagger / API Testing</h2>
<p>Al ejecutar el proyecto, Swagger UI estará disponible en modo desarrollo:</p>
<pre><code>https://localhost:&lt;puerto&gt;/swagger</code></pre>
<p>Desde ahí podés probar todos los endpoints de tu API.</p>

<h2>Ejecución</h2>
<pre><code>dotnet run</code></pre>
<p>El backend escuchará en el puerto configurado por defecto de tu proyecto. Los endpoints se pueden consumir desde cualquier cliente (Frontend, Postman, etc.).</p>
