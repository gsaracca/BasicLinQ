namespace BasicLinq.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    // ── Toolbar superior ──────────────────────────────────────────────────────
    private Panel      pnlToolbar;
    private Label      lblFiltro;
    private ComboBox   cmbColumnaFiltro;
    private TextBox    txtFiltro;
    private Button     btnBuscar;
    private Button     btnLimpiar;
    private Label      lblOrden;
    private ComboBox   cmbColumnaOrden;
    private CheckBox   chkDescendente;
    private Label      lblEstado;

    // ── Grid ──────────────────────────────────────────────────────────────────
    private DataGridView dgvClientes;

    // ── Barra de paginación inferior ──────────────────────────────────────────
    private Panel    pnlPaginacion;
    private Button   btnPrimera;
    private Button   btnAnterior;
    private Label    lblPagina;
    private Button   btnSiguiente;
    private Button   btnUltima;
    private Label    lblFilasPorPagina;
    private ComboBox cmbPageSize;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlToolbar        = new Panel();
        lblFiltro         = new Label();
        cmbColumnaFiltro  = new ComboBox();
        txtFiltro         = new TextBox();
        btnBuscar         = new Button();
        btnLimpiar        = new Button();
        lblOrden          = new Label();
        cmbColumnaOrden   = new ComboBox();
        chkDescendente    = new CheckBox();
        lblEstado         = new Label();
        dgvClientes       = new DataGridView();
        pnlPaginacion     = new Panel();
        btnPrimera        = new Button();
        btnAnterior       = new Button();
        lblPagina         = new Label();
        btnSiguiente      = new Button();
        btnUltima         = new Button();
        lblFilasPorPagina = new Label();
        cmbPageSize       = new ComboBox();

        pnlToolbar.SuspendLayout();
        pnlPaginacion.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
        SuspendLayout();

        // ── pnlToolbar ────────────────────────────────────────────────────────
        pnlToolbar.BackColor = Color.FromArgb(240, 240, 245);
        pnlToolbar.Controls.Add(lblFiltro);
        pnlToolbar.Controls.Add(cmbColumnaFiltro);
        pnlToolbar.Controls.Add(txtFiltro);
        pnlToolbar.Controls.Add(btnBuscar);
        pnlToolbar.Controls.Add(btnLimpiar);
        pnlToolbar.Controls.Add(lblOrden);
        pnlToolbar.Controls.Add(cmbColumnaOrden);
        pnlToolbar.Controls.Add(chkDescendente);
        pnlToolbar.Controls.Add(lblEstado);
        pnlToolbar.Dock    = DockStyle.Top;
        pnlToolbar.Height  = 90;
        pnlToolbar.Padding = new Padding(8);

        // Fila 1 — Filtro
        lblFiltro.AutoSize = true;
        lblFiltro.Font     = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFiltro.Location = new Point(10, 14);
        lblFiltro.Text     = "Filtrar por:";

        cmbColumnaFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbColumnaFiltro.Font          = new Font("Segoe UI", 9F);
        cmbColumnaFiltro.Location      = new Point(90, 11);
        cmbColumnaFiltro.Size          = new Size(180, 23);

        txtFiltro.Font            = new Font("Segoe UI", 9F);
        txtFiltro.Location        = new Point(278, 11);
        txtFiltro.Size            = new Size(220, 23);
        txtFiltro.PlaceholderText = "Ingrese texto a buscar...";
        txtFiltro.KeyDown        += txtFiltro_KeyDown;

        btnBuscar.Font     = new Font("Segoe UI", 9F);
        btnBuscar.Location = new Point(506, 10);
        btnBuscar.Size     = new Size(80, 25);
        btnBuscar.Text     = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click   += btnBuscar_Click;

        btnLimpiar.Font     = new Font("Segoe UI", 9F);
        btnLimpiar.Location = new Point(594, 10);
        btnLimpiar.Size     = new Size(80, 25);
        btnLimpiar.Text     = "Limpiar";
        btnLimpiar.UseVisualStyleBackColor = true;
        btnLimpiar.Click   += btnLimpiar_Click;

        // Fila 2 — Orden
        lblOrden.AutoSize = true;
        lblOrden.Font     = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblOrden.Location = new Point(10, 50);
        lblOrden.Text     = "Ordenar por:";

        cmbColumnaOrden.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbColumnaOrden.Font          = new Font("Segoe UI", 9F);
        cmbColumnaOrden.Location      = new Point(90, 47);
        cmbColumnaOrden.Size          = new Size(180, 23);
        cmbColumnaOrden.SelectedIndexChanged += cmbColumnaOrden_SelectedIndexChanged;

        chkDescendente.AutoSize = true;
        chkDescendente.Font     = new Font("Segoe UI", 9F);
        chkDescendente.Location = new Point(278, 49);
        chkDescendente.Text     = "Descendente";
        chkDescendente.CheckedChanged += chkDescendente_CheckedChanged;

        lblEstado.AutoSize  = false;
        lblEstado.Font      = new Font("Segoe UI", 9F, FontStyle.Italic);
        lblEstado.ForeColor = Color.FromArgb(80, 80, 120);
        lblEstado.Location  = new Point(420, 50);
        lblEstado.Size      = new Size(380, 20);
        lblEstado.Text      = "Listo";
        lblEstado.TextAlign = ContentAlignment.MiddleLeft;

        // ── dgvClientes ───────────────────────────────────────────────────────
        dgvClientes.AllowUserToAddRows    = false;
        dgvClientes.AllowUserToDeleteRows = false;
        dgvClientes.AutoSizeColumnsMode   = DataGridViewAutoSizeColumnsMode.AllCells;
        dgvClientes.BackgroundColor       = Color.White;
        dgvClientes.BorderStyle           = BorderStyle.None;
        dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvClientes.Dock        = DockStyle.Fill;
        dgvClientes.Font        = new Font("Segoe UI", 9F);
        dgvClientes.ReadOnly    = true;
        dgvClientes.RowHeadersVisible = false;
        dgvClientes.SelectionMode     = DataGridViewSelectionMode.FullRowSelect;
        dgvClientes.MultiSelect       = false;
        dgvClientes.ColumnHeaderMouseClick += dgvClientes_ColumnHeaderMouseClick;

        dgvClientes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
        dgvClientes.EnableHeadersVisualStyles = false;
        dgvClientes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 90, 150);
        dgvClientes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvClientes.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 9F, FontStyle.Bold);

        // ── pnlPaginacion ─────────────────────────────────────────────────────
        pnlPaginacion.BackColor = Color.FromArgb(235, 235, 242);
        pnlPaginacion.Controls.Add(btnPrimera);
        pnlPaginacion.Controls.Add(btnAnterior);
        pnlPaginacion.Controls.Add(lblPagina);
        pnlPaginacion.Controls.Add(btnSiguiente);
        pnlPaginacion.Controls.Add(btnUltima);
        pnlPaginacion.Controls.Add(lblFilasPorPagina);
        pnlPaginacion.Controls.Add(cmbPageSize);
        pnlPaginacion.Dock   = DockStyle.Bottom;
        pnlPaginacion.Height = 38;

        btnPrimera.Font     = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnPrimera.Location = new Point(10, 7);
        btnPrimera.Size     = new Size(36, 24);
        btnPrimera.Text     = "|<";
        btnPrimera.UseVisualStyleBackColor = true;
        btnPrimera.Click   += btnPrimera_Click;

        btnAnterior.Font     = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnAnterior.Location = new Point(50, 7);
        btnAnterior.Size     = new Size(36, 24);
        btnAnterior.Text     = "<";
        btnAnterior.UseVisualStyleBackColor = true;
        btnAnterior.Click   += btnAnterior_Click;

        lblPagina.AutoSize  = false;
        lblPagina.Font      = new Font("Segoe UI", 9F);
        lblPagina.Location  = new Point(92, 10);
        lblPagina.Size      = new Size(160, 18);
        lblPagina.Text      = "Página 1 de 1";
        lblPagina.TextAlign = ContentAlignment.MiddleCenter;

        btnSiguiente.Font     = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSiguiente.Location = new Point(257, 7);
        btnSiguiente.Size     = new Size(36, 24);
        btnSiguiente.Text     = ">";
        btnSiguiente.UseVisualStyleBackColor = true;
        btnSiguiente.Click   += btnSiguiente_Click;

        btnUltima.Font     = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnUltima.Location = new Point(297, 7);
        btnUltima.Size     = new Size(36, 24);
        btnUltima.Text     = ">|";
        btnUltima.UseVisualStyleBackColor = true;
        btnUltima.Click   += btnUltima_Click;

        lblFilasPorPagina.AutoSize = true;
        lblFilasPorPagina.Font     = new Font("Segoe UI", 9F);
        lblFilasPorPagina.Location = new Point(350, 11);
        lblFilasPorPagina.Text     = "Filas por página:";

        cmbPageSize.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPageSize.Font          = new Font("Segoe UI", 9F);
        cmbPageSize.Location      = new Point(460, 8);
        cmbPageSize.Size          = new Size(65, 23);
        cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;

        // ── MainForm ──────────────────────────────────────────────────────────
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(1200, 650);
        Controls.Add(dgvClientes);
        Controls.Add(pnlPaginacion);
        Controls.Add(pnlToolbar);
        Font          = new Font("Segoe UI", 9F);
        MinimumSize   = new Size(900, 450);
        StartPosition = FormStartPosition.CenterScreen;
        Text          = "Browse de Clientes — LINQ Demo";
        Load         += MainForm_Load;

        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        pnlPaginacion.ResumeLayout(false);
        pnlPaginacion.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
        ResumeLayout(false);
    }
}
