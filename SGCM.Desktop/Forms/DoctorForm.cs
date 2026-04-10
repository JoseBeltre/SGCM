using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SGCM.Desktop.Models;
using SGCM.Desktop.Services;
using SGCM.Desktop.Utils;

namespace SGCM.Desktop.Forms
{
    public class DoctorForm : Form
    {
        private readonly DoctorService _doctorService;
        private readonly SpecialtyService _specialtyService;
        private List<DoctorDto> _doctors = new();
        private List<SpecialtyDto> _specialties = new();
        
        private DataGridView _grid = null!;
        private Label _lblStatus = null!;
        private Panel _sidePanel = null!;
        
        // Fields
        private TextBox _txtFullName = null!;
        private TextBox _txtEmail = null!;
        private TextBox _txtPhone = null!;
        private ComboBox _cmbSpecialty = null!;
        private TextBox _txtNationalId = null!;
        private TextBox _txtLicense = null!;
        private TextBox _txtOffice = null!;
        private CheckBox _chkIsActive = null!;
        
        private Button _btnSave = null!;
        private Label _lblFormTitle = null!;
        private int? _editingId = null;

        public DoctorForm()
        {
            _doctorService = new DoctorService();
            _specialtyService = new SpecialtyService();
            InitializeComponent();
            ThemeManager.ApplyThemeToForm(this);
            this.Load += async (s, e) => await LoadAllDataAsync();
        }

        private void InitializeComponent()
        {
            this.Text = "Gestión de Médicos";
            this.Size = new Size(1100, 700);

            var mainContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 700,
                IsSplitterFixed = true,
                BackColor = Color.White
            };

            // LEFT PANEL: Grid and ToolBar
            var leftPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            
            var header = new Label
            {
                Text = "🩺 Directorio de Médicos",
                Font = ThemeManager.GetMainFont(18, FontStyle.Bold),
                ForeColor = ThemeManager.PrimaryColor,
                Dock = DockStyle.Top,
                Height = 60
            };

            var toolbar = new Panel { Dock = DockStyle.Top, Height = 50 };
            
            var btnAdd = ThemeManager.CreatePrimaryButton("+ Nuevo Médico");
            btnAdd.Location = new Point(0, 0);
            btnAdd.Enabled = Session.IsAdmin;
            btnAdd.Click += (s, e) => OpenFormForCreate();

            var btnEdit = ThemeManager.CreateSecondaryButton("Editar");
            btnEdit.Location = new Point(btnAdd.Right + 10, 0);
            btnEdit.Click += (s, e) => OpenFormForEdit();

            var btnDelete = ThemeManager.CreateSecondaryButton("Eliminar");
            btnDelete.Location = new Point(btnEdit.Right + 10, 0);
            btnDelete.ForeColor = Color.Crimson;
            btnDelete.Click += async (s, e) => await DeleteSelectedAsync();

            toolbar.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete });

            _grid = new DataGridView();
            ThemeManager.StyleDataGridView(_grid);
            _grid.Dock = DockStyle.Fill;
            _grid.SelectionChanged += (s, e) => {
                bool hasSelection = _grid.SelectedRows.Count > 0;
                btnEdit.Enabled = hasSelection && Session.IsAdmin;
                btnDelete.Enabled = hasSelection && Session.IsAdmin;
            };

            var footer = new Panel { Dock = DockStyle.Bottom, Height = 40 };
            _lblStatus = new Label { Dock = DockStyle.Left, Text = "Cargando...", AutoSize = true, Padding = new Padding(10) };
            footer.Controls.Add(_lblStatus);

            leftPanel.Controls.Add(_grid);
            leftPanel.Controls.Add(toolbar);
            leftPanel.Controls.Add(header);
            leftPanel.Controls.Add(footer);

            // RIGHT PANEL: Side Form
            _sidePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.BackgroundColor,
                Padding = new Padding(20),
                Visible = false
            };

            _lblFormTitle = new Label
            {
                Text = "Detalles del Médico",
                Font = ThemeManager.GetMainFont(14, FontStyle.Bold),
                ForeColor = ThemeManager.PrimaryColor,
                Dock = DockStyle.Top,
                Height = 40
            };

            int y = 50;
            _txtFullName = CreateLabeledTextBox("Nombre Completo:", ref y);
            _txtEmail = CreateLabeledTextBox("Email:", ref y);
            _txtPhone = CreateLabeledTextBox("Teléfono:", ref y);
            
            AddLabel("Especialidad:", ref y);
            _cmbSpecialty = new ComboBox { Location = new Point(20, y), Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
            _sidePanel.Controls.Add(_cmbSpecialty);
            y += 40;

            _txtNationalId = CreateLabeledTextBox("Cédula:", ref y);
            _txtLicense = CreateLabeledTextBox("Exequatur:", ref y);
            _txtOffice = CreateLabeledTextBox("Consultorio:", ref y);

            _chkIsActive = new CheckBox { Text = "Activo", Location = new Point(20, y), Checked = true };
            _sidePanel.Controls.Add(_chkIsActive);
            y += 30;

            _btnSave = ThemeManager.CreatePrimaryButton("💾 Guardar");
            _btnSave.Location = new Point(20, y);
            _btnSave.Click += async (s, e) => await SaveAsync();

            var btnCancel = ThemeManager.CreateSecondaryButton("Cancelar");
            btnCancel.Location = new Point(_btnSave.Right + 10, y);
            btnCancel.Click += (s, e) => _sidePanel.Visible = false;

            _sidePanel.Controls.Add(_lblFormTitle);
            _sidePanel.Controls.Add(_btnSave);
            _sidePanel.Controls.Add(btnCancel);

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

        private async Task LoadAllDataAsync()
        {
            try {
                _specialties = await _specialtyService.GetSpecialtiesAsync();
                _cmbSpecialty.DataSource = _specialties;
                _cmbSpecialty.DisplayMember = "Name";
                _cmbSpecialty.ValueMember = "Id";
                
                await LoadDataAsync();
            } catch { }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                _lblStatus.Text = "Actualizando directorio...";
                var userService = new UserService();
                var users = await userService.GetUsersAsync();
                _doctors = await _doctorService.GetDoctorsAsync();

                foreach (var d in _doctors)
                {
                    var u = users.FirstOrDefault(x => x.Id == d.UserId);
                    if (u != null)
                    {
                        d.FullName = u.FullName;
                        d.Email = u.Email;
                        d.Phone = u.Phone;
                    }
                }

                _grid.DataSource = null;
                _grid.DataSource = _doctors.Select(d => new {
                    d.Id,
                    Nombre = d.FullName,
                    Especialidad = _specialties.FirstOrDefault(s => s.Id == d.SpecialtyId)?.Name ?? "N/A",
                    Cedula = d.NationalId,
                    Exequatur = d.LicenseNumber,
                    Email = d.Email,
                    Estado = d.IsActive ? "Activo" : "Inactivo"
                }).ToList();
                _lblStatus.Text = $"Registros: {_doctors.Count}";
            }
            catch (Exception ex)
            {
                _lblStatus.Text = "Error al cargar datos.";
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenFormForCreate()
        {
            _editingId = null;
            _lblFormTitle.Text = "Nuevo Médico";
            _txtFullName.Text = _txtEmail.Text = _txtPhone.Text = _txtNationalId.Text = _txtLicense.Text = _txtOffice.Text = "";
            _sidePanel.Visible = true;
        }

        private void OpenFormForEdit()
        {
            if (_grid.SelectedRows.Count == 0) return;
            var doc = _doctors.First(d => d.Id == (int)_grid.SelectedRows[0].Cells["Id"].Value);
            _editingId = doc.Id;
            _lblFormTitle.Text = "Editar Médico";
            _txtFullName.Text = doc.FullName;
            _txtEmail.Text = doc.Email;
            _txtPhone.Text = doc.Phone;
            _txtNationalId.Text = doc.NationalId;
            _txtLicense.Text = doc.LicenseNumber;
            _txtOffice.Text = doc.AssignedOffice;
            _cmbSpecialty.SelectedValue = doc.SpecialtyId;
            _chkIsActive.Checked = doc.IsActive;
            _sidePanel.Visible = true;
        }

        private async Task SaveAsync()
        {
            try
            {
                _btnSave.Enabled = false;
                var userService = new UserService();
                if (_editingId == null)
                {
                    var userDto = new UserCreateDto {
                        FullName = _txtFullName.Text.Trim(),
                        Email = _txtEmail.Text.Trim(),
                        Phone = _txtPhone.Text.Trim(),
                        PasswordHash = "Doctor123!",
                        UserType = SGCM.Domain.Enums.UserType.Medico
                    };
                    var user = await userService.CreateAsync(userDto);

                    await _doctorService.CreateAsync(new AddDoctorDto {
                        UserId = user.Id,
                        SpecialtyId = (int)_cmbSpecialty.SelectedValue, 
                        NationalId = _txtNationalId.Text.Trim(),
                        LicenseNumber = _txtLicense.Text.Trim(), 
                        AssignedOffice = _txtOffice.Text.Trim(),
                        HireDate = DateTime.Now
                    });
                }
                else
                {
                    var doc = _doctors.First(d => d.Id == _editingId.Value);
                    await userService.UpdateAsync(doc.UserId, new UserUpdateDto {
                        FullName = _txtFullName.Text.Trim(),
                        Email = _txtEmail.Text.Trim(),
                        Phone = _txtPhone.Text.Trim()
                    });

                    await _doctorService.UpdateAsync(_editingId.Value, new UpdateDoctorDto {
                        SpecialtyId = (int)_cmbSpecialty.SelectedValue, 
                        AssignedOffice = _txtOffice.Text.Trim(),
                        IsActive = _chkIsActive.Checked
                    });
                }
                _sidePanel.Visible = false;
                await LoadDataAsync();
                MessageBox.Show("Guardado con éxito.");
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            finally { _btnSave.Enabled = true; }
        }

        private async Task DeleteSelectedAsync()
        {
            if (_grid.SelectedRows.Count == 0) return;
            if (MessageBox.Show("¿Borrar médico?", "Confirma", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try {
                    await _doctorService.DeleteAsync((int)_grid.SelectedRows[0].Cells["Id"].Value);
                    await LoadDataAsync();
                } catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
    }
}
