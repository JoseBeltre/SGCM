using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SGCM.Desktop.Services;
using SGCM.Desktop.Utils;

namespace SGCM.Desktop.Forms
{
    public class ReportForm : Form
    {
        private readonly ReportService _service;
        
        private Panel _statsContainer = null!;

        public ReportForm()
        {
            _service = new ReportService();
            InitializeComponent();
            ThemeManager.ApplyThemeToForm(this);
            this.Load += async (s, e) => await LoadDataAsync();
        }

        private void InitializeComponent()
        {
            this.Text = "Reportes y Estadísticas";
            
            var headerPanel = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(20) };
            var lblTitle = ThemeManager.CreateHeaderLabel("Panel de Reportes");
            headerPanel.Controls.Add(lblTitle);

            _statsContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40),
                AutoScroll = true,
                BackColor = ThemeManager.BackgroundColor
            };

            var toolbar = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(40, 0, 40, 0) };
            var btnRefresh = ThemeManager.CreateSecondaryButton("Refrescar Estadísticas");
            btnRefresh.Location = new Point(40, 10);
            btnRefresh.Width = 200;
            btnRefresh.Click += async (s, e) => await LoadDataAsync();
            toolbar.Controls.Add(btnRefresh);

            this.Controls.Add(_statsContainer);
            this.Controls.Add(toolbar);
            this.Controls.Add(headerPanel);
        }

        private Panel CreateStatCard(string title, string value, Color color)
        {
            var card = ThemeManager.CreateCard();
            card.Size = new Size(250, 150);
            card.Margin = new Padding(15);
            
            var lblTitle = new Label
            {
                Text = title,
                ForeColor = ThemeManager.TextSecondaryColor,
                Font = ThemeManager.GetMainFont(12, FontStyle.Bold),
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 40
            };

            var lblValue = new Label
            {
                Text = value,
                ForeColor = color,
                Font = ThemeManager.GetMainFont(24, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);
            
            return card;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var stats = await _service.GetAppointmentStatsAsync();
                
                _statsContainer.Controls.Clear();
                
                _statsContainer.Controls.Add(CreateStatCard("Total Citas", stats.TotalAppointments.ToString(), ThemeManager.PrimaryColor));
                _statsContainer.Controls.Add(CreateStatCard("Confirmadas", stats.ConfirmedAppointments.ToString(), ThemeManager.SuccessColor));
                _statsContainer.Controls.Add(CreateStatCard("Completadas", stats.CompletedAppointments.ToString(), Color.ForestGreen));
                _statsContainer.Controls.Add(CreateStatCard("Canceladas", stats.CancelledAppointments.ToString(), Color.Crimson));
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar reportes: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
