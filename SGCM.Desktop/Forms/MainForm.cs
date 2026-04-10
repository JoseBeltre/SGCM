using System;
using System.Drawing;
using System.Windows.Forms;
using SGCM.Desktop.Models;
using SGCM.Desktop.Utils;

namespace SGCM.Desktop.Forms
{
    /// <summary>
    /// Formulario principal de la aplicación. Construye el menú dinámicamente
    /// según los roles del usuario autenticado (Admin, Doctor, o ambos).
    /// </summary>
    public class MainForm : Form
    {
        private Panel _sidebar = null!;
        private Panel _contentArea = null!;
        private Label _lblDashboardTitle = null!;
        private Label _lblUserInfo = null!;
        private Button? _activeNavButton = null;

        public MainForm()
        {
            InitializeComponent();
            ThemeManager.ApplyThemeToForm(this);
            this.Text = $"SGCM — {Session.DisplayRole}";

            // Abrir módulo por defecto según rol principal
            if (Session.IsDoctor)
                OpenModule(new AppointmentForm(), "📅 Mi Agenda");
            else if (Session.IsAdmin)
                OpenModule(new UserForm(), "👥 Usuarios del Sistema");
            else
                OpenModule(new AppointmentForm(), "📅 Citas Médicas");

            this.FormClosing += (s, e) => System.Windows.Forms.Application.Exit();
        }

        private void InitializeComponent()
        {
            // ===== SIDEBAR =====
            _sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 260,
                BackColor = Color.White,
                Padding = new Padding(0)
            };

            // Logo / Cabecera del Sidebar
            var logoPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 90,
                BackColor = ThemeManager.PrimaryColor
            };

            var lblLogo = new Label
            {
                Text = "⚕ SGCM",
                Font = ThemeManager.GetMainFont(18, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 55
            };

            var lblTagline = new Label
            {
                Text = "Sistema de Gestión de Citas",
                Font = ThemeManager.GetMainFont(7.5f, FontStyle.Italic),
                ForeColor = Color.FromArgb(200, 255, 255, 255),
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 22
            };

            logoPanel.Controls.Add(lblTagline);
            logoPanel.Controls.Add(lblLogo);
            _sidebar.Controls.Add(logoPanel);

            // Información del usuario
            var userInfoPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = ThemeManager.PrimaryLightColor,
                Padding = new Padding(15, 8, 15, 8)
            };

            var lblName = new Label
            {
                Text = string.IsNullOrWhiteSpace(Session.FullName) ? Session.Email : Session.FullName,
                Font = ThemeManager.GetMainFont(10.5f, FontStyle.Bold),
                ForeColor = ThemeManager.TextPrimaryColor,
                Dock = DockStyle.Top,
                Height = 26,
                TextAlign = ContentAlignment.BottomLeft
            };

            _lblUserInfo = new Label
            {
                Text = BuildRoleBadgeText(),
                Font = ThemeManager.GetMainFont(8.5f, FontStyle.Regular),
                ForeColor = ThemeManager.PrimaryColor,
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.TopLeft
            };

            userInfoPanel.Controls.Add(_lblUserInfo);
            userInfoPanel.Controls.Add(lblName);
            _sidebar.Controls.Add(userInfoPanel);

            // ===== SEPARADOR =====
            var sep = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = ThemeManager.BorderColor };
            _sidebar.Controls.Add(sep);

            // ===== MENÚ DE NAVEGACIÓN =====
            // Se construye dinámicamente según los roles del usuario.
            // Módulos disponibles para TODOS los usuarios con acceso:
            BuildNavigationMenu();

            // ===== BOTÓN CERRAR SESIÓN =====
            var btnLogout = new Button
            {
                Text = "⏻  Cerrar Sesión",
                Dock = DockStyle.Bottom,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Font = ThemeManager.GetMainFont(10, FontStyle.Bold),
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 245, 245);
            btnLogout.Click += (s, e) =>
            {
                var confirm = MessageBox.Show(
                    "¿Está seguro que desea cerrar sesión?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    Session.Logout();
                    var login = new LoginForm();
                    login.Show();
                    this.Hide();
                }
            };
            _sidebar.Controls.Add(btnLogout);

            // Separador antes del logout
            var sepBottom = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = ThemeManager.BorderColor };
            _sidebar.Controls.Add(sepBottom);

            // ===== CABECERA PRINCIPAL =====
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.White,
                Padding = new Padding(30, 15, 30, 0)
            };

            // Línea inferior del header
            var headerBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = ThemeManager.BorderColor };
            header.Controls.Add(headerBorder);

            _lblDashboardTitle = new Label
            {
                Text = "Dashboard",
                Font = ThemeManager.GetMainFont(16, FontStyle.Bold),
                ForeColor = ThemeManager.TextPrimaryColor,
                Dock = DockStyle.Left,
                AutoSize = false,
                Width = 400,
                TextAlign = ContentAlignment.MiddleLeft
            };
            header.Controls.Add(_lblDashboardTitle);

            // ===== ÁREA DE CONTENIDO =====
            _contentArea = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.BackgroundColor
            };

            this.Controls.Add(_contentArea);
            this.Controls.Add(header);
            this.Controls.Add(_sidebar);
        }

        /// <summary>
        /// Construye el texto de badge de roles para el panel de usuario.
        /// </summary>
        private static string BuildRoleBadgeText()
        {
            if (Session.IsAdmin && Session.IsDoctor) return "🔑 Admin  •  🩺 Doctor";
            if (Session.IsAdmin)                     return "🔑 Administrador";
            if (Session.IsDoctor)                    return "🩺 Médico";
            if (Session.IsReceptionist)              return "📋 Recepcionista";
            return Session.DisplayRole;
        }

        /// <summary>
        /// Construye el menú de navegación dinámicamente.
        /// - Módulos de Doctor visibles a: Doctor, y también Admin con doble rol.
        /// - Módulos de Admin visibles a: Admin (y Admin+Doctor).
        /// - Módulos compartidos visibles a todos.
        /// </summary>
        private void BuildNavigationMenu()
        {
            var navContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            _sidebar.Controls.Add(navContainer);

            // --- SECCIÓN: MÓDULO DOCTOR ---
            if (Session.IsDoctor || Session.IsAdmin)
            {
                AddSectionLabel(navContainer, "AGENDA");
                AddNavButton(navContainer, "📅 Mis Citas",
                    (s, e) => OpenModule(new AppointmentForm(), "📅 Gestión de Citas"));
                
                // NOTA: AvailabilityForm y otros módulos médicos irían aquí
            }

            // --- SECCIÓN: ADMINISTRACIÓN ---
            if (Session.IsAdmin)
            {
                AddSectionLabel(navContainer, "SISTEMA");
                AddNavButton(navContainer, "👥 Usuarios",
                    (s, e) => OpenModule(new UserForm(), "👥 Usuarios del Sistema"));
                
                AddNavButton(navContainer, "🔍 Auditoría",
                    (s, e) => OpenModule(new AuditForm(), "🔍 Logs de Auditoría"));
            }

            // --- SECCIÓN: PERSONAL Y PACIENTES ---
            AddSectionLabel(navContainer, "DIRECTORIO");
            
            AddNavButton(navContainer, "🩺 Médicos", 
                (s, e) => OpenModule(new DoctorForm(), "🩺 Directorio Médico"));
            AddNavButton(navContainer, "👤 Pacientes", 
                (s, e) => OpenModule(new PatientForm(), "👤 Gestión de Pacientes"));

            // --- SECCIÓN: MÓDULOS COMPARTIDOS ---
            AddSectionLabel(navContainer, "CATÁLOGOS");
            AddNavButton(navContainer, "🏥 Especialidades",
                (s, e) => OpenModule(new SpecialtyForm(), "🏥 Especialidades Médicas"));

            AddSectionLabel(navContainer, "ANÁLISIS");
            AddNavButton(navContainer, "📊 Reportes",
                (s, e) => OpenModule(new ReportForm(), "📊 Estadísticas y Reportes"));
        }

        private void AddSectionLabel(Panel container, string text)
        {
            var lbl = new Label
            {
                Text = text,
                Font = ThemeManager.GetMainFont(7.5f, FontStyle.Bold),
                ForeColor = ThemeManager.TextSecondaryColor,
                Dock = DockStyle.Top,
                Height = 35,
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(20, 0, 0, 5)
            };
            container.Controls.Add(lbl);
            lbl.BringToFront(); // Cambiado de SendToBack para apilar correctamente hacia arriba
        }

        private void AddNavButton(Panel container, string text, EventHandler clickEvent)
        {
            var btn = new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 48,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = ThemeManager.GetMainFont(10.5f),
                ForeColor = ThemeManager.TextSecondaryColor,
                Cursor = Cursors.Hand,
                Padding = new Padding(20, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ThemeManager.PrimaryLightColor;

            btn.Click += (s, e) =>
            {
                // Resaltar el botón activo
                if (_activeNavButton != null)
                {
                    _activeNavButton.BackColor = Color.Transparent;
                    _activeNavButton.ForeColor = ThemeManager.TextSecondaryColor;
                    _activeNavButton.Font = ThemeManager.GetMainFont(10.5f);
                }
                btn.BackColor = ThemeManager.PrimaryLightColor;
                btn.ForeColor = ThemeManager.PrimaryColor;
                btn.Font = ThemeManager.GetMainFont(10.5f, FontStyle.Bold);
                _activeNavButton = btn;

                clickEvent(s, e);
            };

            _sidebar.Controls.Add(btn);
            btn.BringToFront(); // Cambiado de SendToBack
        }

        private void OpenModule(Form form, string title)
        {
            _lblDashboardTitle.Text = title;

            _contentArea.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            _contentArea.Controls.Add(form);
            form.Show();
        }
    }
}
