using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SGCM.Desktop.Models;
using SGCM.Desktop.Services;
using SGCM.Desktop.Utils;

namespace SGCM.Desktop.Forms
{
    public class UserForm : Form
    {
        private readonly UserService _service;
        
        private DataGridView _grid = null!;
        private Panel _sidePanel = null!;
        
        private TextBox _txtFullName = null!;
        private TextBox _txtEmail = null!;
        private TextBox _txtPhone = null!;
        private TextBox _txtPassword = null!;
        private ComboBox _cmbRole = null!;
        private CheckBox _chkIsActive = null!;
        
        private Button _btnSave = null!;
        private Button _btnCancel = null!;
        private Label _lblFormTitle = null!;

        private int? _editingId = null;

        public UserForm()
        {
            _service = new UserService();
            InitializeComponent();
            ThemeManager.ApplyThemeToForm(this);
            this.Load += async (s, e) => await LoadDataAsync();
        }

        private void InitializeComponent()
        {
            this.Text = "Gestión de Usuarios";
            this.Size = new Size(1100, 700);

            var mainContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 750,
                IsSplitterFixed = true,
                BackColor = Color.White
            };

            // PANEL IZQUIERDO: Grid y Toolbar
            var leftPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            
            var header = new Label
            {
                Text = "👤 Gestión de Usuarios",
                Font = ThemeManager.GetMainFont(18, FontStyle.Bold),
                ForeColor = ThemeManager.PrimaryColor,
                Dock = DockStyle.Top,
                Height = 60
            };

            var toolbar = new Panel { Dock = DockStyle.Top, Height = 50 };
            
            var btnRefresh = ThemeManager.CreateSecondaryButton("Refrescar");
            btnRefresh.Location = new Point(0, 0);
            btnRefresh.Click += async (s, e) => await LoadDataAsync();

            var btnAdd = ThemeManager.CreatePrimaryButton("+ Nuevo Usuario");
            btnAdd.Location = new Point(btnRefresh.Right + 10, 0);
            btnAdd.Enabled = Session.IsAdmin;
            btnAdd.Click += (s, e) => OpenFormForCreate();

            var btnEdit = ThemeManager.CreateSecondaryButton("Editar");
            btnEdit.Location = new Point(btnAdd.Right + 10, 0);
            btnEdit.Click += (s, e) => OpenFormForEdit();

            var btnDelete = ThemeManager.CreateSecondaryButton("Desactivar");
            btnDelete.Location = new Point(btnEdit.Right + 10, 0);
            btnDelete.ForeColor = Color.Crimson;
            btnDelete.Click += async (s, e) => await DeleteSelectedAsync();

            toolbar.Controls.AddRange(new Control[] { btnRefresh, btnAdd, btnEdit, btnDelete });

            _grid = new DataGridView();
            ThemeManager.StyleDataGridView(_grid);
            _grid.Dock = DockStyle.Fill;
            _grid.SelectionChanged += (s, e) => {
                bool hasSelection = _grid.SelectedRows.Count > 0;
                btnEdit.Enabled = hasSelection && Session.IsAdmin;
                btnDelete.Enabled = hasSelection && Session.IsAdmin;
            };

            leftPanel.Controls.Add(_grid);
            leftPanel.Controls.Add(toolbar);
            leftPanel.Controls.Add(header);

            // PANEL DERECHO: Formulario Lateral
            _sidePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.BackgroundColor,
                Padding = new Padding(20),
                Visible = false
            };

            _lblFormTitle = new Label
            {
                Text = "Nuevo Usuario",
                Font = ThemeManager.GetMainFont(14, FontStyle.Bold),
                ForeColor = ThemeManager.PrimaryColor,
                Dock = DockStyle.Top,
                Height = 40
            };

            int y = 50;
            _txtFullName = CreateLabeledTextBox("Nombre Completo:", ref y);
            _txtEmail = CreateLabeledTextBox("Email:", ref y);
            _txtPhone = CreateLabeledTextBox("Teléfono:", ref y);
            _txtPassword = CreateLabeledTextBox("Contraseña:", ref y);
            _txtPassword.PasswordChar = '*';

            AddLabel("Rol del Sistema:", ref y);
            _cmbRole = new ComboBox { Location = new Point(20, y), Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbRole.Items.AddRange(new[] { "Administrador", "Medico", "Paciente" });
            _sidePanel.Controls.Add(_cmbRole);
            y += 40;

            _chkIsActive = new CheckBox { Text = "Usuario Activo", Location = new Point(20, y), Checked = true };
            _sidePanel.Controls.Add(_chkIsActive);
            y += 30;

            _btnSave = ThemeManager.CreatePrimaryButton("💾 Guardar");
            _btnSave.Location = new Point(20, y);
            _btnSave.Click += async (s, e) => await SaveAsync();

            _btnCancel = ThemeManager.CreateSecondaryButton("Cancelar");
            _btnCancel.Location = new Point(_btnSave.Right + 10, y);
            _btnCancel.Click += (s, e) => _sidePanel.Visible = false;

            _sidePanel.Controls.Add(_lblFormTitle);
            _sidePanel.Controls.Add(_btnSave);
            _sidePanel.Controls.Add(_btnCancel);

            mainContainer.Panel1.Controls.Add(leftPanel);
            mainContainer.Panel2.Controls.Add(_sidePanel);
            this.Controls.Add(mainContainer);

            btnEdit.Enabled = btnDelete.Enabled = false;
        }

        private TextBox CreateLabeledTextBox(string label, ref int y)
        {
            AddLabel(label, ref y);
            var txt = ThemeManager.CreateTextBox();
            txt.Location = new Point(20, y);
            txt.Width = 260;
            _sidePanel.Controls.Add(txt);
            y += 40;
            return txt;
        }

        private void AddLabel(string text, ref int y)
        {
            var lbl = new Label { Text = text, Location = new Point(20, y), AutoSize = true, Font = ThemeManager.GetMainFont(9, FontStyle.Bold) };
            _sidePanel.Controls.Add(lbl);
            y += 20;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var data = await _service.GetUsersAsync();
                _grid.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenFormForCreate()
        {
            _editingId = null;
            _lblFormTitle.Text = "Nuevo Usuario";
            _txtFullName.Text = "";
            _txtEmail.Text = "";
            _txtPhone.Text = "";
            _txtPassword.Text = "";
            _txtPassword.Enabled = true;
            _cmbRole.SelectedIndex = 0;
            _chkIsActive.Checked = true;
            _chkIsActive.Enabled = false;

            _sidePanel.Visible = true;
        }

        private void OpenFormForEdit()
        {
            if (_grid.SelectedRows.Count == 0) return;
            var row = _grid.SelectedRows[0];
            var user = (UserDto)row.DataBoundItem;

            _editingId = user.Id;
            _lblFormTitle.Text = "Editar Usuario";
            _txtFullName.Text = user.FullName;
            _txtEmail.Text = user.Email;
            _txtPhone.Text = user.Phone;
            _txtPassword.Text = "";
            _txtPassword.Enabled = false; 
            _cmbRole.SelectedItem = user.UserType.ToString();
            _chkIsActive.Checked = user.IsActive;
            _chkIsActive.Enabled = true;

            _sidePanel.Visible = true;
        }

        private async Task SaveAsync()
        {
            // --- VALIDAR ANTES DE ENVIAR A LA API ---
            bool isValid = Validators.Validate(v =>
            {
                v.Required(_txtFullName.Text, "Nombre Completo");
                v.Required(_txtEmail.Text, "Email");
                v.Email(_txtEmail.Text, "Email");
                v.SelectionRequired(_cmbRole.SelectedItem, "Roles");

                if (!_editingId.HasValue)
                {
                    // Modo Crear
                    v.Required(_txtPassword.Text, "Contraseña");
                    v.MinLength(_txtPassword.Text, "Contraseña", 6);
                }
            }, out string errorMsg);

            if (!isValid)
            {
                MessageBox.Show($"Por favor corrija los siguientes errores:\n\n{errorMsg}",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _btnSave.Enabled = false;
                if (_editingId.HasValue)
                {
                    var dto = new UserUpdateDto
                    {
                        FullName = _txtFullName.Text.Trim(),
                        Email = _txtEmail.Text.Trim(),
                        Phone = _txtPhone.Text.Trim(),
                    };
                    await _service.UpdateAsync(_editingId.Value, dto);
                    MessageBox.Show("✅ Usuario actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var dto = new UserCreateDto
                    {
                        FullName = _txtFullName.Text.Trim(),
                        Email = _txtEmail.Text.Trim(),
                        Phone = _txtPhone.Text.Trim(),
                        PasswordHash = _txtPassword.Text,
                        UserType = Enum.Parse<SGCM.Domain.Enums.UserType>(_cmbRole.SelectedItem?.ToString() ?? "Paciente")
                    };
                    await _service.CreateAsync(dto);
                    MessageBox.Show("✅ Usuario creado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                _sidePanel.Visible = false;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                var detail = ex.InnerException != null ? $"\nDetalle: {ex.InnerException.Message}" : "";
                MessageBox.Show($"❌ Error al guardar usuario: {ex.Message}{detail}", "Error del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnSave.Enabled = true;
            }
        }

        private async Task DeleteSelectedAsync()
        {
            if (_grid.SelectedRows.Count == 0) return;
            
            var row = _grid.SelectedRows[0];
            var user = (UserDto)row.DataBoundItem;

            var result = MessageBox.Show($"¿Desea desactivar al usuario {user.Email}?\n\nEsta acción impedirá su inicio de sesión pero mantendrá su historial.", "Confirmar Desactivación", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    await _service.DeleteAsync(user.Id);
                    await LoadDataAsync();
                    MessageBox.Show("Usuario desactivado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
