using BasicLinq.Data;
using BasicLinq.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicLinq.Forms;

public partial class MainForm : Form
{
    private readonly AppDbContext _context;

    private int  _pageSize    = 50;
    private int  _currentPage = 1;
    private int  _totalCount  = 0;
    private bool _listo       = false;   // evita queries durante la inicialización

    // Mapa: nombre visible → nombre de propiedad del modelo
    private static readonly Dictionary<string, string> ColumnasPropiedades = new()
    {
        { "Código",            nameof(Cliente.Codigo) },
        { "Alta",              nameof(Cliente.Alta) },
        { "Razón Social",      nameof(Cliente.RazonSocial) },
        { "Nombre Fantasía",   nameof(Cliente.NombreFantasia) },
        { "Contacto",          nameof(Cliente.Contacto) },
        { "Email",             nameof(Cliente.Email) },
        { "Grupo",             nameof(Cliente.Grupo) },
        { "Clasificación",     nameof(Cliente.Clasificacion) },
        { "Domicilio",         nameof(Cliente.Domicilio) },
        { "Ciudad",            nameof(Cliente.Ciudad) },
        { "Código Postal",     nameof(Cliente.CodigoPostal) },
        { "CUIT",              nameof(Cliente.Cuit) },
        { "IVA",               nameof(Cliente.Iva) },
        { "IIBB",              nameof(Cliente.Iibb) },
        { "Ganancias",         nameof(Cliente.Ganancias) },
        { "Cta. Cte.",         nameof(Cliente.IfCtaCte) },
        { "Condición Venta",   nameof(Cliente.CondicionVenta) },
        { "Límite Crédito 1",  nameof(Cliente.LimiteCredito1) },
        { "Límite Crédito 2",  nameof(Cliente.LimiteCredito2) },
        { "Consolidado",       nameof(Cliente.IfConsolidado) },
        { "Vendedor",          nameof(Cliente.Vendedor) },
        { "Transporte",        nameof(Cliente.Transporte) },
        { "Observaciones",     nameof(Cliente.Observaciones) },
        { "Tipo",              nameof(Cliente.Tipo) },
        { "Límite Crédito 3",  nameof(Cliente.LimiteCredito3) },
        { "Límite Crédito 4",  nameof(Cliente.LimiteCredito4) },
        { "No Factura",        nameof(Cliente.IfNoFac) },
        { "Consumidor Final",  nameof(Cliente.IfCf) },
        { "Eliminado",         nameof(Cliente.IfDel) },
        { "Enviar Mail",       nameof(Cliente.IfSendMail) },
    };

    public MainForm(AppDbContext context)
    {
        _context = context;
        InitializeComponent();
        InicializarControles();
    }

    private void InicializarControles()
    {
        var columnas = ColumnasPropiedades.Keys.ToArray();

        cmbColumnaFiltro.Items.AddRange(columnas);
        cmbColumnaFiltro.SelectedIndex = 0;

        cmbColumnaOrden.Items.Add("(ninguno)");
        cmbColumnaOrden.Items.AddRange(columnas);
        cmbColumnaOrden.SelectedIndex = 0;

        cmbPageSize.Items.AddRange([25, 50, 100, 200, 500]);
        cmbPageSize.SelectedItem = _pageSize;

        _listo = true;   // a partir de aquí los eventos ya pueden disparar queries
    }

    // ─── Carga inicial ────────────────────────────────────────────────────────

    private async void MainForm_Load(object sender, EventArgs e)
    {
        await CargarPaginaAsync();
    }

    // ─── Consulta paginada con LINQ server-side ───────────────────────────────

    private async Task CargarPaginaAsync()
    {
        SetCargando(true);

        try
        {
            // IQueryable base — la consulta vive en el servidor hasta el ToListAsync
            IQueryable<Cliente> query = _context.Clientes;

            // LINQ Where: filtro aplicado en la BD
            string filtroTexto = txtFiltro.Text.Trim();
            if (!string.IsNullOrEmpty(filtroTexto))
            {
                string prop = ColumnasPropiedades[cmbColumnaFiltro.SelectedItem!.ToString()!];
                query = AplicarFiltroQuery(query, prop, filtroTexto);
            }

            // LINQ Count: una sola consulta COUNT(*) al servidor
            _totalCount = await query.CountAsync();

            // LINQ OrderBy: orden en el servidor
            string sortProp = cmbColumnaOrden.SelectedIndex > 0
                ? ColumnasPropiedades[cmbColumnaOrden.SelectedItem!.ToString()!]
                : nameof(Cliente.Codigo);

            query = AplicarOrdenQuery(query, sortProp, chkDescendente.Checked);

            // LINQ Skip/Take: paginación en el servidor — solo llegan _pageSize filas
            var pagina = await query
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToListAsync();

            dgvClientes.DataSource = pagina;
            ActualizarInfoPaginacion();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al consultar la base de datos:\n\n{ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblEstado.Text = "Error de conexión";
        }
        finally
        {
            SetCargando(false);
        }
    }

    // ─── Filtro server-side (IQueryable → WHERE en SQL) ──────────────────────

    /// <summary>
    /// Agrega una cláusula WHERE al IQueryable según el tipo de la columna.
    /// Todo se traduce a SQL por EF Core — no se carga nada en memoria.
    /// </summary>
    private static IQueryable<Cliente> AplicarFiltroQuery(
        IQueryable<Cliente> q, string propiedad, string texto)
    {
        // Columnas de texto: LIKE %texto%
        if (EsColumnaTexto(propiedad))
            return AplicarFiltroTexto(q, propiedad, texto);

        // Columnas numéricas enteras
        if (int.TryParse(texto, out int valorInt))
            return AplicarFiltroInt(q, propiedad, valorInt);

        // Columnas decimales
        if (decimal.TryParse(texto, out decimal valorDec))
            return AplicarFiltroDecimal(q, propiedad, valorDec);

        // Si el texto no matchea el tipo, no filtrar (evita excepción)
        return q;
    }

    private static bool EsColumnaTexto(string propiedad) =>
        propiedad is nameof(Cliente.RazonSocial)
                  or nameof(Cliente.NombreFantasia)
                  or nameof(Cliente.Contacto)
                  or nameof(Cliente.Email)
                  or nameof(Cliente.Grupo)
                  or nameof(Cliente.Clasificacion)
                  or nameof(Cliente.Domicilio)
                  or nameof(Cliente.Ciudad)
                  or nameof(Cliente.CodigoPostal)
                  or nameof(Cliente.Cuit)
                  or nameof(Cliente.Iva)
                  or nameof(Cliente.Iibb)
                  or nameof(Cliente.Ganancias)
                  or nameof(Cliente.Consolidado)
                  or nameof(Cliente.Transporte)
                  or nameof(Cliente.Observaciones)
                  or nameof(Cliente.Tipo);

    private static IQueryable<Cliente> AplicarFiltroTexto(
        IQueryable<Cliente> q, string propiedad, string texto) =>
        propiedad switch
        {
            nameof(Cliente.RazonSocial)    => q.Where(c => c.RazonSocial    != null && c.RazonSocial.Contains(texto)),
            nameof(Cliente.NombreFantasia) => q.Where(c => c.NombreFantasia != null && c.NombreFantasia.Contains(texto)),
            nameof(Cliente.Contacto)       => q.Where(c => c.Contacto       != null && c.Contacto.Contains(texto)),
            nameof(Cliente.Email)          => q.Where(c => c.Email          != null && c.Email.Contains(texto)),
            nameof(Cliente.Grupo)          => q.Where(c => c.Grupo          != null && c.Grupo.Contains(texto)),
            nameof(Cliente.Clasificacion)  => q.Where(c => c.Clasificacion  != null && c.Clasificacion.Contains(texto)),
            nameof(Cliente.Domicilio)      => q.Where(c => c.Domicilio      != null && c.Domicilio.Contains(texto)),
            nameof(Cliente.Ciudad)         => q.Where(c => c.Ciudad         != null && c.Ciudad.Contains(texto)),
            nameof(Cliente.CodigoPostal)   => q.Where(c => c.CodigoPostal   != null && c.CodigoPostal.Contains(texto)),
            nameof(Cliente.Cuit)           => q.Where(c => c.Cuit           != null && c.Cuit.Contains(texto)),
            nameof(Cliente.Iva)            => q.Where(c => c.Iva            != null && c.Iva.Contains(texto)),
            nameof(Cliente.Iibb)           => q.Where(c => c.Iibb           != null && c.Iibb.Contains(texto)),
            nameof(Cliente.Ganancias)      => q.Where(c => c.Ganancias      != null && c.Ganancias.Contains(texto)),
            nameof(Cliente.Consolidado)    => q.Where(c => c.Consolidado    != null && c.Consolidado.Contains(texto)),
            nameof(Cliente.Transporte)     => q.Where(c => c.Transporte     != null && c.Transporte.Contains(texto)),
            nameof(Cliente.Observaciones)  => q.Where(c => c.Observaciones  != null && c.Observaciones.Contains(texto)),
            nameof(Cliente.Tipo)           => q.Where(c => c.Tipo           != null && c.Tipo.Contains(texto)),
            _                              => q
        };

    private static IQueryable<Cliente> AplicarFiltroInt(
        IQueryable<Cliente> q, string propiedad, int valor) =>
        propiedad switch
        {
            nameof(Cliente.Codigo)         => q.Where(c => c.Codigo == valor),
            nameof(Cliente.CondicionVenta) => q.Where(c => c.CondicionVenta == valor),
            nameof(Cliente.Vendedor)       => q.Where(c => c.Vendedor == valor),
            nameof(Cliente.Mark)           => q.Where(c => c.Mark == valor),
            nameof(Cliente.DomNumero)      => q.Where(c => c.DomNumero == valor),
            nameof(Cliente.Eor)            => q.Where(c => c.Eor == valor),
            _                              => q
        };

    private static IQueryable<Cliente> AplicarFiltroDecimal(
        IQueryable<Cliente> q, string propiedad, decimal valor) =>
        propiedad switch
        {
            nameof(Cliente.LimiteCredito1) => q.Where(c => c.LimiteCredito1 == valor),
            nameof(Cliente.LimiteCredito2) => q.Where(c => c.LimiteCredito2 == valor),
            nameof(Cliente.LimiteCredito3) => q.Where(c => c.LimiteCredito3 == valor),
            nameof(Cliente.LimiteCredito4) => q.Where(c => c.LimiteCredito4 == valor),
            _                              => q
        };

    // ─── Orden server-side (IQueryable → ORDER BY en SQL) ────────────────────

    private static IQueryable<Cliente> AplicarOrdenQuery(
        IQueryable<Cliente> q, string propiedad, bool descendente) =>
        propiedad switch
        {
            nameof(Cliente.Codigo)         => descendente ? q.OrderByDescending(c => c.Codigo)         : q.OrderBy(c => c.Codigo),
            nameof(Cliente.Alta)           => descendente ? q.OrderByDescending(c => c.Alta)           : q.OrderBy(c => c.Alta),
            nameof(Cliente.RazonSocial)    => descendente ? q.OrderByDescending(c => c.RazonSocial)    : q.OrderBy(c => c.RazonSocial),
            nameof(Cliente.NombreFantasia) => descendente ? q.OrderByDescending(c => c.NombreFantasia) : q.OrderBy(c => c.NombreFantasia),
            nameof(Cliente.Contacto)       => descendente ? q.OrderByDescending(c => c.Contacto)       : q.OrderBy(c => c.Contacto),
            nameof(Cliente.Email)          => descendente ? q.OrderByDescending(c => c.Email)          : q.OrderBy(c => c.Email),
            nameof(Cliente.Ciudad)         => descendente ? q.OrderByDescending(c => c.Ciudad)         : q.OrderBy(c => c.Ciudad),
            nameof(Cliente.Cuit)           => descendente ? q.OrderByDescending(c => c.Cuit)           : q.OrderBy(c => c.Cuit),
            nameof(Cliente.CondicionVenta) => descendente ? q.OrderByDescending(c => c.CondicionVenta) : q.OrderBy(c => c.CondicionVenta),
            nameof(Cliente.LimiteCredito1) => descendente ? q.OrderByDescending(c => c.LimiteCredito1) : q.OrderBy(c => c.LimiteCredito1),
            nameof(Cliente.Vendedor)       => descendente ? q.OrderByDescending(c => c.Vendedor)       : q.OrderBy(c => c.Vendedor),
            nameof(Cliente.Tipo)           => descendente ? q.OrderByDescending(c => c.Tipo)           : q.OrderBy(c => c.Tipo),
            _                              => q.OrderBy(c => c.Codigo)
        };

    // ─── Paginación ───────────────────────────────────────────────────────────

    private int TotalPaginas => Math.Max(1, (int)Math.Ceiling(_totalCount / (double)_pageSize));

    private void ActualizarInfoPaginacion()
    {
        lblPagina.Text = $"Página {_currentPage} de {TotalPaginas}";
        lblEstado.Text = $"Total: {_totalCount} registros";

        btnPrimera.Enabled  = _currentPage > 1;
        btnAnterior.Enabled = _currentPage > 1;
        btnSiguiente.Enabled = _currentPage < TotalPaginas;
        btnUltima.Enabled   = _currentPage < TotalPaginas;
    }

    private async void btnPrimera_Click(object? sender, EventArgs e)
    {
        _currentPage = 1;
        await CargarPaginaAsync();
    }

    private async void btnAnterior_Click(object? sender, EventArgs e)
    {
        if (_currentPage > 1) { _currentPage--; await CargarPaginaAsync(); }
    }

    private async void btnSiguiente_Click(object? sender, EventArgs e)
    {
        if (_currentPage < TotalPaginas) { _currentPage++; await CargarPaginaAsync(); }
    }

    private async void btnUltima_Click(object? sender, EventArgs e)
    {
        _currentPage = TotalPaginas;
        await CargarPaginaAsync();
    }

    private async void cmbPageSize_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (!_listo) return;
        if (cmbPageSize.SelectedItem is int size)
        {
            _pageSize    = size;
            _currentPage = 1;
            await CargarPaginaAsync();
        }
    }

    // ─── Eventos de filtro/orden ──────────────────────────────────────────────

    private async void btnBuscar_Click(object? sender, EventArgs e)
    {
        _currentPage = 1;
        await CargarPaginaAsync();
    }

    private async void txtFiltro_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            _currentPage = 1;
            await CargarPaginaAsync();
        }
    }

    private async void btnLimpiar_Click(object? sender, EventArgs e)
    {
        txtFiltro.Clear();
        cmbColumnaFiltro.SelectedIndex = 0;
        cmbColumnaOrden.SelectedIndex  = 0;
        chkDescendente.Checked = false;
        _currentPage = 1;
        await CargarPaginaAsync();
    }

    private async void cmbColumnaOrden_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (!_listo) return;
        _currentPage = 1;
        await CargarPaginaAsync();
    }

    private async void chkDescendente_CheckedChanged(object? sender, EventArgs e)
    {
        if (!_listo) return;
        _currentPage = 1;
        await CargarPaginaAsync();
    }

    // Clic en encabezado del grid
    private async void dgvClientes_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        string nombrePropiedad = dgvClientes.Columns[e.ColumnIndex].DataPropertyName;
        var entrada = ColumnasPropiedades.FirstOrDefault(kv => kv.Value == nombrePropiedad);
        if (entrada.Key == null) return;

        bool descendente = cmbColumnaOrden.SelectedItem?.ToString() == entrada.Key
            ? !chkDescendente.Checked
            : false;

        // Suspender eventos temporalmente para evitar doble recarga
        cmbColumnaOrden.SelectedIndexChanged -= cmbColumnaOrden_SelectedIndexChanged;
        chkDescendente.CheckedChanged        -= chkDescendente_CheckedChanged;

        cmbColumnaOrden.SelectedItem = entrada.Key;
        chkDescendente.Checked = descendente;

        cmbColumnaOrden.SelectedIndexChanged += cmbColumnaOrden_SelectedIndexChanged;
        chkDescendente.CheckedChanged        += chkDescendente_CheckedChanged;

        _currentPage = 1;
        await CargarPaginaAsync();
    }

    // ─── UI helper ───────────────────────────────────────────────────────────

    private void SetCargando(bool cargando)
    {
        Cursor = cargando ? Cursors.WaitCursor : Cursors.Default;
        btnBuscar.Enabled    = !cargando;
        btnLimpiar.Enabled   = !cargando;
        btnPrimera.Enabled   = !cargando;
        btnAnterior.Enabled  = !cargando;
        btnSiguiente.Enabled = !cargando;
        btnUltima.Enabled    = !cargando;
        if (cargando) lblEstado.Text = "Consultando...";
    }
}
