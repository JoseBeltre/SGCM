using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SGCM.Desktop.Services;
using SGCM.Desktop.Utils;

namespace SGCM.Desktop.Forms
{
    public class AuditForm : Form
    {
        private readonly AuditService _service;
        private DataGridView _grid = null!;

        public AuditForm()
        {
            _service = new AuditService();
            InitializeComponent();
            ThemeManager.ApplyThemeToForm(this);
            this.Load += async (s, e) => await LoadDataAsync();
        }

        private void InitializeComponent()
        {
            this.Text = "Registro de Auditoría";
            
            var headerPanel = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(20) };
            var lblTitle = ThemeManager.CreateHeaderLabel("Logs de Auditoría");
            headerPanel.Controls.Add(lblTitle);

            var mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            
            var toolbarPanel = new Panel { Dock = DockStyle.Top, Height = 50 };
            var btnRefresh = ThemeManager.CreateSecondaryButton("Actualizar Logs");
            btnRefresh.Location = new Point(0, 0);
            btnRefresh.Click += async (s, e) => await LoadDataAsync();
            toolbarPanel.Controls.Add(btnRefresh);

            _grid = new DataGridView { Dock = DockStyle.Fill, Margin = new Padding(0, 20, 0, 0) };
            ThemeManager.StyleDataGridView(_grid);

            mainPanel.Controls.Add(_grid);
            mainPanel.Controls.Add(toolbarPanel);

            this.Controls.Add(mainPanel);
            this.Controls.Add(headerPanel);
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var data = await _service.GetAuditLogsAsync();
                _grid.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar auditoría: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
