# BasicLinq

Aplicación de escritorio en **Windows Forms / .NET 8** que demuestra el uso de **LINQ con Entity Framework Core** para realizar consultas paginadas, filtradas y ordenadas del lado del servidor sobre una base de datos SQL Server.

---

## Características

- **Paginación server-side** — usa `Skip` / `Take` sobre `IQueryable`, transfiriendo solo los registros de la página activa.
- **Filtrado dinámico** — soporta columnas de texto (`Contains` → `LIKE`), enteros y decimales con coincidencia exacta.
- **Ordenamiento server-side** — `OrderBy` / `OrderByDescending` resueltos en la base de datos.
- **Página automática** — calcula las filas visibles según el alto del `DataGridView` y se recalcula al redimensionar la ventana.
- **Tamaños de página manuales** — 25, 50, 100, 200 y 500 registros.
- **Conversores de valor EF Core** — mapea `bool? ↔ tinyint/int` preservando `NULL` para compatibilidad con esquemas legacy.
- **Operaciones asíncronas** — `async/await` en todas las consultas para mantener la UI receptiva.

---

## Tecnologías

| Tecnología | Versión |
|---|---|
| .NET | 8.0 |
| Windows Forms | .NET 8 (net8.0-windows) |
| Entity Framework Core | 8.0.11 |
| EF Core SQL Server | 8.0.11 |
| Microsoft.Extensions.Configuration | 8.0.0 |

---

## Estructura del proyecto

```
BasicLinq/
├── Program.cs                  # Punto de entrada, configuración de DI y DbContext
├── appsettings.json            # Cadena de conexión (no versionado)
├── appsettings.example.json    # Plantilla de configuración
├── Data/
│   └── AppDbContext.cs         # DbContext con conversores de valor personalizados
├── Models/
│   └── Cliente.cs              # Entidad mapeada a la tabla "clientes"
└── Forms/
    ├── MainForm.cs             # Lógica de UI: paginación, filtrado y ordenamiento
    └── MainForm.Designer.cs    # Código generado por el diseñador WinForms
```

---

## Configuración

La cadena de conexión se lee desde `appsettings.json`, que está excluido del control de versiones.

1. Copia la plantilla:

   ```bash
   cp appsettings.example.json appsettings.json
   ```

2. Edita `appsettings.json` con tus credenciales:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=TU_SERVIDOR;Database=TU_BD;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

3. Asegúrate de que SQL Server esté accesible desde tu máquina.

---

## Primeros pasos

### Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- SQL Server (Express o superior)
- Visual Studio 2022+ o cualquier IDE compatible con .NET 8

### Compilar y ejecutar

```bash
git clone https://github.com/tu-usuario/BasicLinq.git
cd BasicLinq

# Restaurar dependencias
dotnet restore

# Ejecutar
dotnet run
```

---

## Cómo funciona

### Consulta paginada

```csharp
// Construir la consulta base (IQueryable — sin ejecutar aún)
IQueryable<Cliente> query = _context.Clientes.AsNoTracking();

// Aplicar filtro (se traduce a WHERE en SQL)
query = query.Where(c => c.RazonSocial != null && c.RazonSocial.Contains(texto));

// Contar el total (COUNT ejecutado en el servidor)
_totalCount = await query.CountAsync();

// Paginar (SKIP/TAKE ejecutados en el servidor)
var pagina = await query
    .Skip((_currentPage - 1) * _pageSize)
    .Take(_pageSize)
    .ToListAsync();
```

### Página automática

```csharp
private int CalcularFilasPorPagina()
{
    int altaDisponible = dgvClientes.ClientSize.Height - dgvClientes.ColumnHeadersHeight;
    int altoFila = dgvClientes.RowTemplate.Height;
    return Math.Max(1, altaDisponible / altoFila);
}
```

### Conversores de valor EF Core

El `AppDbContext` registra conversores que manejan columnas legacy `tinyint` / `int` como `bool?` en C#, preservando los valores `NULL` de la base de datos:

```csharp
// bool? <-> tinyint  (NULL → null, 0 → false, != 0 → true)
// bool? <-> int      (NULL → null, 0 → false, != 0 → true)
```

---

## Interfaz de usuario

| Zona | Controles |
|---|---|
| **Barra superior** | Campo de texto de filtro, selector de columna, botones Buscar / Limpiar |
| **Ordenamiento** | Combo de columna, checkbox Descendente |
| **Grilla** | Solo lectura, colores alternos por fila |
| **Barra inferior** | Botones Primera / Anterior / Siguiente / Última, contador de página, selector de tamaño |

---

## Licencia

Distribuido bajo los términos del archivo [LICENSE](LICENSE).
