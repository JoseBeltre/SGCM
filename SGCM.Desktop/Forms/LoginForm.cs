using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SGCM.Desktop.Models;
using SGCM.Desktop.Services;
using SGCM.Desktop.Utils;

namespace SGCM.Desktop.Forms
{
    public class LoginForm : Form
    {
        private readonly AuthService _authService;
        
        private TextBox _txtUsername = null!;
        private TextBox _txtPassword = null!;
        private Button _btnLogin = null!;
        private Label _lblStatus = null!;

        public LoginForm()
        {
            _authService = new AuthService();
            InitializeComponent();
            ThemeManager.ApplyThemeToForm(this);
            CenterToScreen();
            
            // Re-aplicar tamaño para Login (no maximizado por defecto para mejor UX de acceso)
            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        private void InitializeComponent()
        {
            this.Text = "SGCM - Inicio de Sesión";
            
            var lblLogo = new Label
            {
                Text = "SGCM",
                Font = ThemeManager.GetMainFont(28, FontStyle.Bold),
                ForeColor = ThemeManager.PrimaryColor,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 100
            };

            var container = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40)
            };

            var lblUser = new Label { Text = "Usuario:", Dock = DockStyle.Top, Height = 30, Font = ThemeManager.GetMainFont(10, FontStyle.Bold) };
            _txtUsername = ThemeManager.CreateTextBox();
            _txtUsername.Dock = DockStyle.Top;
            _txtUsername.Margin = new Padding(0, 0, 0, 20);

            var lblPass = new Label { Text = "Contraseña:", Dock = DockStyle.Top, Height = 30, Font = ThemeManager.GetMainFont(10, FontStyle.Bold), Margin = new Padding(0, 10, 0, 0) };
            _txtPassword = ThemeManager.CreateTextBox();
            _txtPassword.Dock = DockStyle.Top;
            _txtPassword.PasswordChar = '*';

            _lblStatus = new Label
            {
                Text = "",
                ForeColor = Color.DarkRed,
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = ThemeManager.GetMainFont(9)
            };

            _btnLogin = ThemeManager.CreatePrimaryButton("Iniciar Sesión");
            _btnLogin.Dock = DockStyle.Top;
            _btnLogin.Height = 50;
            _btnLogin.Click += async (s, e) => await HandleLoginAsync();

            container.Controls.Add(_btnLogin);
            container.Controls.Add(_lblStatus);
            container.Controls.Add(_txtPassword);
            container.Controls.Add(lblPass);
            container.Controls.Add(_txtUsername);
            container.Controls.Add(lblUser);

            this.Controls.Add(container);
            this.Controls.Add(lblLogo);
            
            this.AcceptButton = _btnLogin;
        }

        private async Task HandleLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(_txtUsername.Text) || string.IsNullOrWhiteSpace(_txtPassword.Text))
            {
                _lblStatus.Text = "Por favor ingrese credenciales.";
                return;
            }

            try
            {
                _btnLogin.Enabled = false;
                _lblStatus.Text = "Autenticando...";
                _lblStatus.ForeColor = ThemeManager.TextSecondaryColor;

                var result = await _authService.LoginAsync(_txtUsername.Text, _txtPassword.Text);

                if (result != null)
                {
                    // Bloqueo de Pacientes (Requerimiento de Negocio)
                    var roles = result.GetEffectiveRoles();
                    if (roles.Contains("Paciente") || roles.Contains("Patient"))
                    {
                        MessageBox.Show("Acceso Denegado: Esta aplicación es exclusiva para personal administrativo y médico.", 
                            "Restricción de Acceso", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        _btnLogin.Enabled = true;
                        _lblStatus.Text = "No tiene permisos para entrar.";
                        return;
                    }

                    // Poblar sesión con soporte de múltiples roles
                    Session.Token = result.Token;
                    Session.Roles = result.GetEffectiveRoles();
                    Session.UserId = result.Id;
                    Session.FullName = result.FullName;
                    Session.Email = result.Email;

                    // Abrir MainForm
                    var mainForm = new MainForm();
                    mainForm.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                _lblStatus.Text = "Error: " + ex.Message;
                _lblStatus.ForeColor = Color.DarkRed;
            }
            finally
            {
                _btnLogin.Enabled = true;
            }
        }
    }
}
