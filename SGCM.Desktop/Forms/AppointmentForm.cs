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
    public class AppointmentForm : Form
    {
        private readonly AppointmentService _appointmentService;
        private readonly DoctorService _doctorService;
        private readonly PatientService _patientService;
        private readonly UserService _userService;

        private List<AppointmentDto> _appointments = new();
        private List<DoctorDto> _doctors = new();
        private List<PatientDto> _patients = new();

        private DataGridView _grid = null!;
        private Label _lblStatus = null!;
        private Panel _sidePanel = null!;

        // Form fields
        private ComboBox _cmbDoctor = null!;
        private ComboBox _cmbPatient = null!;
        private DateTimePicker _dtpDate = null!;
        private TextBox _txtReason = null!;

        private Button _btnSave = null!;
        private Label _lblFormTitle = null!;
        private int? _editingId = null;

        public AppointmentForm()
        {
            _appointmentService = new AppointmentService();
            _doctorService = new DoctorService();
            _patientService = new PatientService();
            _userService = new UserService();

            InitializeComponent();
            ThemeManager.ApplyThemeToForm(this);
            this.Load += async (s, e) => await LoadAllDataAsync();
        }

        private void InitializeComponent()
        {
            this.Text = "Gestión de Citas";
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
                Text = "📅 Citas Médicas",
                Font = ThemeManager.GetMainFont(18, FontStyle.Bold),
                ForeColor = ThemeManager.PrimaryColor,
                Dock = DockStyle.Top,
                Height = 60
            };

            var toolbar = new Panel { Dock = DockStyle.Top, Height = 50 };

            var btnAdd = ThemeManager.CreatePrimaryButton("+ Nueva Cita");
            btnAdd.Location = new Point(0, 0);
            btnAdd.Click += (s, e) => OpenFormForCreate();

            var btnEdit = ThemeManager.CreateSecondaryButton("Editar");
            btnEdit.Location = new Point(btnAdd.Right + 10, 0);
            btnEdit.Click += (s, e) => OpenFormForEdit();

            var btnDelete = ThemeManager.CreateSecondaryButton("Eliminar");
            btnDelete.Location = new Point(btnEdit.Right + 10, 0);
            btnDelete.ForeColor = Color.Crimson;
            btnDelete.Click += async (s, e) => await DeleteSelectedAsync();

            var btnConfirm = ThemeManager.CreateSecondaryButton("✓ Confirmar");
            btnConfirm.Location = new Point(btnDelete.Right + 10, 0);
            btnConfirm.Click += async (s, e) => await ChangeStatusAsync("confirm");

            var btnComplete = ThemeManager.CreateSecondaryButton("✓ Completar");
            btnComplete.Location = new Point(btnConfirm.Right + 10, 0);
            btnComplete.Click += async (s, e) => await ChangeStatusAsync("complete");

            var btnCancel = ThemeManager.CreateSecondaryButton("✗ Cancelar Cita");
            btnCancel.Location = new Point(btnComplete.Right + 10, 0);
            btnCancel.ForeColor = Color.OrangeRed;
            btnCancel.Click += async (s, e) => await CancelAppointmentAsync();

            toolbar.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, btnConfirm, btnComplete, btnCancel });

            _grid = new DataGridView();
            ThemeManager.StyleDataGridView(_grid);
            _grid.Dock = DockStyle.Fill;
            _grid.SelectionChanged += (s, e) =>
            {
                bool hasSelection = _grid.SelectedRows.Count > 0;
                string currentStatus = "";
                if (hasSelection)
                {
                    var statusCell = _grid.SelectedRows[0].Cells["Estado"];
                    currentStatus = statusCell?.Value?.ToString() ?? "";
                }

                btnEdit.Enabled = hasSelection && (currentStatus == "Solicitada" || currentStatus == "Confirmada");
                btnDelete.Enabled = hasSelection && Session.IsAdmin;
                // Solo se puede confirmar una cita con estado "Solicitada"
                btnConfirm.Enabled = hasSelection && currentStatus == "Solicitada";
                // Solo se puede completar una cita con estado "Confirmada"
                btnComplete.Enabled = hasSelection && currentStatus == "Confirmada";
                // Solo se puede cancelar si no está ya completada o cancelada
                btnCancel.Enabled = hasSelection && currentStatus != "Completada" && currentStatus != "Cancelada";
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
                Text = "Nueva Cita",
                Font = ThemeManager.GetMainFont(14, FontStyle.Bold),
                ForeColor = ThemeManager.PrimaryColor,
                Dock = DockStyle.Top,
                Height = 40
            };

            int y = 50;

            AddLabel("Médico:", ref y);
            _cmbDoctor = new ComboBox { Location = new Point(20, y), Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
            _sidePanel.Controls.Add(_cmbDoctor);
            y += 40;

            AddLabel("Paciente:", ref y);
            _cmbPatient = new ComboBox { Location = new Point(20, y), Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
            _sidePanel.Controls.Add(_cmbPatient);
            y += 40;

            AddLabel("Fecha y Hora:", ref y);
            _dtpDate = new DateTimePicker
            {
                Location = new Point(20, y),
                Width = 260,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy  HH:mm",
                Value = DateTime.Now.AddHours(1)
            };
            _sidePanel.Controls.Add(_dtpDate);
            y += 40;

            _txtReason = CreateLabeledTextBox("Motivo de Consulta:", ref y);

            _btnSave = ThemeManager.CreatePrimaryButton("💾 Guardar");
            _btnSave.Location = new Point(20, y);
            _btnSave.Click += async (s, e) => await SaveAsync();

            var btnFormCancel = ThemeManager.CreateSecondaryButton("Cancelar");
            btnFormCancel.Location = new Point(_btnSave.Right + 10, y);
            btnFormCancel.Click += (s, e) => _sidePanel.Visible = false;

            _sidePanel.Controls.Add(_lblFormTitle);
            _sidePanel.Controls.Add(_btnSave);
            _sidePanel.Controls.Add(btnFormCancel);

            mainContainer.Panel1.Controls.Add(leftPanel);
            mainContainer.Panel2.Controls.Add(_sidePanel);
            this.Controls.Add(mainContainer);

            btnEdit.Enabled = btnDelete.Enabled = btnConfirm.Enabled = btnComplete.Enabled = btnCancel.Enabled = false;
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

        // ===== CARGA DE DATOS (mismo patrón que DoctorForm) =====

        private async Task LoadAllDataAsync()
        {
            try
            {
                // Cargar usuarios para resolver nombres
                var users = await _userService.GetUsersAsync();

                // Cargar doctores y enriquecer con nombres
                _doctors = await _doctorService.GetDoctorsAsync();
                foreach (var d in _doctors)
                {
                    var u = users.FirstOrDefault(x => x.Id == d.UserId);
                    if (u != null) { d.FullName = u.FullName; d.Email = u.Email; d.Phone = u.Phone; }
                }

                _cmbDoctor.DataSource = _doctors;
                _cmbDoctor.DisplayMember = "DisplayName";
                _cmbDoctor.ValueMember = "Id";

                // Cargar pacientes y enriquecer con nombres
                _patients = await _patientService.GetPatientsAsync();
                foreach (var p in _patients)
                {
                    var u = users.FirstOrDefault(x => x.Id == p.UserId);
                    if (u != null) { p.FullName = u.FullName; p.Email = u.Email; p.Phone = u.Phone; }
                }

                _cmbPatient.DataSource = _patients;
                _cmbPatient.DisplayMember = "DisplayName";
                _cmbPatient.ValueMember = "Id";

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar catálogos: " + ex.Message);
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                _lblStatus.Text = "Actualizando citas...";
                _appointments = await _appointmentService.GetAppointmentsAsync();

                _grid.DataSource = null;
                _grid.DataSource = _appointments.Select(a => new
                {
                    a.Id,
                    Paciente = _patients.FirstOrDefault(p => p.Id == a.PatientId)?.DisplayName ?? $"#{a.PatientId}",
                    Médico = _doctors.FirstOrDefault(d => d.Id == a.DoctorId)?.DisplayName ?? $"#{a.DoctorId}",
                    Fecha = a.AppointmentDate.ToString("dd/MM/yyyy HH:mm"),
                    Duración = $"{a.DurationMinutes} min",
                    Estado = a.Status,
                    Motivo = a.ConsultationReason ?? ""
                }).ToList();

                _lblStatus.Text = $"Registros: {_appointments.Count}";
            }
            catch (Exception ex)
            {
                _lblStatus.Text = "Error al cargar citas.";
                MessageBox.Show(ex.Message);
            }
        }

        // ===== CREAR / EDITAR (mismo patrón que DoctorForm) =====

        private void OpenFormForCreate()
        {
            _editingId = null;
            _lblFormTitle.Text = "Nueva Cita";
            _txtReason.Text = "";
            _dtpDate.Value = DateTime.Now.AddHours(1);
            _cmbDoctor.Enabled = true;
            _cmbPatient.Enabled = true;

            // Si es Doctor, pre-seleccionar y bloquear
            if (Session.IsDoctor)
            {
                var myDoc = _doctors.FirstOrDefault(d => d.UserId == Session.UserId);
                if (myDoc != null) _cmbDoctor.SelectedValue = myDoc.Id;
                _cmbDoctor.Enabled = false;
            }

            _sidePanel.Visible = true;
        }

        private void OpenFormForEdit()
        {
            if (_grid.SelectedRows.Count == 0) return;
            var appt = _appointments.First(a => a.Id == (int)_grid.SelectedRows[0].Cells["Id"].Value);

            _editingId = appt.Id;
            _lblFormTitle.Text = "Editar Cita";
            _cmbDoctor.SelectedValue = appt.DoctorId;
            _cmbDoctor.Enabled = false;
            _cmbPatient.SelectedValue = appt.PatientId;
            _cmbPatient.Enabled = false;
            _dtpDate.Value = appt.AppointmentDate;
            _txtReason.Text = appt.ConsultationReason ?? "";
            _sidePanel.Visible = true;
        }

        // ===== GUARDAR (mismo patrón que DoctorForm.SaveAsync) =====

        private async Task SaveAsync()
        {
            try
            {
                _btnSave.Enabled = false;

                if (_editingId == null)
                {
                    // CREAR - exactamente como Doctor: construir DTO y llamar al service
                    await _appointmentService.CreateAsync(new AppointmentCreateDto
                    {
                        PatientId = (int)_cmbPatient.SelectedValue,
                        DoctorId = (int)_cmbDoctor.SelectedValue,
                        AppointmentDate = _dtpDate.Value,
                        ConsultationReason = _txtReason.Text.Trim()
                    });
                }
                else
                {
                    // EDITAR - actualizar datos de la cita
                    await _appointmentService.UpdateAsync(_editingId.Value, new AppointmentUpdateDto
                    {
                        AppointmentDate = _dtpDate.Value,
                        ConsultationReason = _txtReason.Text.Trim()
                    });
                }

                _sidePanel.Visible = false;
                await LoadDataAsync();
                MessageBox.Show("Guardado con éxito.");
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            finally { _btnSave.Enabled = true; }
        }

        // ===== ELIMINAR (mismo patrón que DoctorForm) =====

        private async Task DeleteSelectedAsync()
        {
            if (_grid.SelectedRows.Count == 0) return;
            if (MessageBox.Show("¿Eliminar esta cita?", "Confirma", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    await _appointmentService.DeleteAsync((int)_grid.SelectedRows[0].Cells["Id"].Value);
                    await LoadDataAsync();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        // ===== CAMBIAR ESTADO (confirm / complete) =====

        private async Task ChangeStatusAsync(string action)
        {
            if (_grid.SelectedRows.Count == 0) return;
            var id = (int)_grid.SelectedRows[0].Cells["Id"].Value;
            var statusCell = _grid.SelectedRows[0].Cells["Estado"];
            var currentStatus = statusCell?.Value?.ToString() ?? "";

            // Validar transiciones de estado válidas en el cliente antes de llamar al API
            if (action == "confirm" && currentStatus != "Solicitada")
            {
                MessageBox.Show($"Solo se pueden confirmar citas con estado 'Solicitada'.\nEstado actual: '{currentStatus}'.",
                    "Transición no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (action == "complete" && currentStatus != "Confirmada")
            {
                MessageBox.Show($"Solo se pueden completar citas con estado 'Confirmada'.\nEstado actual: '{currentStatus}'.",
                    "Transición no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var actionName = action == "confirm" ? "confirmar" : "completar";
            var confirm = MessageBox.Show($"¿Desea {actionName} esta cita?", "Confirmar acción",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                if (action == "confirm") await _appointmentService.ConfirmAsync(id);
                else if (action == "complete") await _appointmentService.CompleteAsync(id);
                await LoadDataAsync();
                MessageBox.Show($"Cita {(action == "confirm" ? "confirmada" : "completada")} con éxito.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al {actionName} la cita:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== CANCELAR CITA =====

        private async Task CancelAppointmentAsync()
        {
            if (_grid.SelectedRows.Count == 0) return;
            var id = (int)_grid.SelectedRows[0].Cells["Id"].Value;
            var statusCell = _grid.SelectedRows[0].Cells["Estado"];
            var currentStatus = statusCell?.Value?.ToString() ?? "";

            if (currentStatus == "Completada" || currentStatus == "Cancelada")
            {
                MessageBox.Show($"No se puede cancelar una cita con estado '{currentStatus}'.",
                    "Transición no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Solicitar motivo de cancelación
            var reasonForm = new Form
            {
                Text = "Motivo de Cancelación",
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblReason = new Label { Text = "Indique el motivo de cancelación:", Location = new Point(20, 20), AutoSize = true };
            var txtReason = new TextBox { Location = new Point(20, 50), Width = 340, Multiline = true, Height = 60 };
            var btnOk = new Button { Text = "Cancelar Cita", Location = new Point(20, 120), DialogResult = DialogResult.OK };
            var btnBack = new Button { Text = "Volver", Location = new Point(140, 120), DialogResult = DialogResult.Cancel };

            reasonForm.Controls.AddRange(new Control[] { lblReason, txtReason, btnOk, btnBack });
            reasonForm.AcceptButton = btnOk;
            reasonForm.CancelButton = btnBack;

            if (reasonForm.ShowDialog() != DialogResult.OK) return;

            var reason = txtReason.Text.Trim();
            if (string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("Debe indicar un motivo de cancelación.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                await _appointmentService.CancelAsync(id, reason);
                await LoadDataAsync();
                MessageBox.Show("Cita cancelada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cancelar la cita:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
