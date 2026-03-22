using BasicLinq.Data;
using BasicLinq.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicLinq.Forms;

public partial class MainForm : Form
{
    private readonly AppDbContext _context;
    private List<Cliente> _todosLosClientes = new();

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
    }

    // ─── Carga inicial ────────────────────────────────────────────────────────

    private async void MainForm_Load(object sender, EventArgs e)
    {
        await CargarClientesAsync();
    }

    private async Task CargarClientesAsync()
    {
        lblEstado.Text = "Cargando...";
        btnBuscar.Enabled = false;
        btnLimpiar.Enabled = false;

        try
        {
            // LINQ: trae todos los clientes ordenados por código
            _todosLosClientes = await _context.Clientes
                .OrderBy(c => c.Codigo)
                .ToListAsync();

            AplicarFiltroYOrden();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al conectar con la base de datos:\n\n{ex.Message}",
                "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblEstado.Text = "Error de conexión";
        }
        finally
        {
            btnBuscar.Enabled = true;
            btnLimpiar.Enabled = true;
        }
    }

    // ─── Filtro y orden con LINQ ──────────────────────────────────────────────

    private void AplicarFiltroYOrden()
    {
        // Partimos de la lista completa en memoria
        IEnumerable<Cliente> query = _todosLosClientes;

        // LINQ: filtrar si hay texto
        string filtroTexto = txtFiltro.Text.Trim();
        if (!string.IsNullOrEmpty(filtroTexto))
        {
            string columnaPropiedad = ColumnasPropiedades[cmbColumnaFiltro.SelectedItem!.ToString()!];
            query = FiltrarPorColumna(query, columnaPropiedad, filtroTexto);
        }

        // LINQ: ordenar si se eligió columna
        if (cmbColumnaOrden.SelectedIndex > 0)
        {
            string columnaPropiedad = ColumnasPropiedades[cmbColumnaOrden.SelectedItem!.ToString()!];
            bool descendente = chkDescendente.Checked;
            query = OrdenarPorColumna(query, columnaPropiedad, descendente);
        }

        var resultado = query.ToList();
        dgvClientes.DataSource = resultado;

        lblEstado.Text = resultado.Count == _todosLosClientes.Count
            ? $"Total: {resultado.Count} clientes"
            : $"Mostrando {resultado.Count} de {_todosLosClientes.Count} clientes";
    }

    /// <summary>
    /// LINQ: filtra la colección comparando el valor de la propiedad
    /// indicada con el texto de búsqueda (contiene, sin distinción de mayúsculas).
    /// </summary>
    private static IEnumerable<Cliente> FiltrarPorColumna(
        IEnumerable<Cliente> clientes, string propiedad, string texto)
    {
        var prop = typeof(Cliente).GetProperty(propiedad)
            ?? throw new ArgumentException($"Propiedad '{propiedad}' no encontrada.");

        texto = texto.ToLowerInvariant();

        // LINQ Where: convierte el valor a string y verifica si contiene el texto
        return clientes.Where(c =>
        {
            var valor = prop.GetValue(c);
            if (valor is null) return false;
            return valor.ToString()!.ToLowerInvariant().Contains(texto);
        });
    }

    /// <summary>
    /// LINQ: ordena la colección por la propiedad indicada usando reflexión.
    /// </summary>
    private static IEnumerable<Cliente> OrdenarPorColumna(
        IEnumerable<Cliente> clientes, string propiedad, bool descendente)
    {
        var prop = typeof(Cliente).GetProperty(propiedad)
            ?? throw new ArgumentException($"Propiedad '{propiedad}' no encontrada.");

        // LINQ OrderBy / OrderByDescending con selector dinámico
        return descendente
            ? clientes.OrderByDescending(c => prop.GetValue(c))
            : clientes.OrderBy(c => prop.GetValue(c));
    }

    // ─── Eventos de controles ─────────────────────────────────────────────────

    private void btnBuscar_Click(object sender, EventArgs e)
        => AplicarFiltroYOrden();

    private void txtFiltro_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
            AplicarFiltroYOrden();
    }

    private void btnLimpiar_Click(object sender, EventArgs e)
    {
        txtFiltro.Clear();
        cmbColumnaFiltro.SelectedIndex = 0;
        cmbColumnaOrden.SelectedIndex = 0;
        chkDescendente.Checked = false;
        AplicarFiltroYOrden();
    }

    private void cmbColumnaOrden_SelectedIndexChanged(object sender, EventArgs e)
        => AplicarFiltroYOrden();

    private void chkDescendente_CheckedChanged(object sender, EventArgs e)
        => AplicarFiltroYOrden();

    // El DataGridView permite ordenar columnas con clic en el encabezado.
    // El evento ColumnHeaderMouseClick usa LINQ sobre la lista actual.
    private void dgvClientes_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        string nombreColumna = dgvClientes.Columns[e.ColumnIndex].DataPropertyName;

        // Alternar dirección si ya está ordenado por esta columna
        bool descendente = false;
        if (cmbColumnaOrden.SelectedIndex > 0)
        {
            string actualPropiedad = ColumnasPropiedades[cmbColumnaOrden.SelectedItem!.ToString()!];
            if (actualPropiedad == nombreColumna)
                descendente = !chkDescendente.Checked;
        }

        // Buscar el nombre visible correspondiente a la propiedad
        var entrada = ColumnasPropiedades.FirstOrDefault(kv => kv.Value == nombreColumna);
        if (entrada.Key != null)
        {
            cmbColumnaOrden.SelectedItem = entrada.Key;
            chkDescendente.Checked = descendente;
            // AplicarFiltroYOrden() se llama desde los eventos de cambio
        }
    }
}
