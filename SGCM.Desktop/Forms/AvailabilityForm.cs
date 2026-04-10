using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SGCM.Desktop.Models;
using SGCM.Desktop.Services;
using SGCM.Desktop.Utils;

namespace SGCM.Desktop.Forms
{
    public class AvailabilityForm : Form
    {
        private readonly AvailabilityService _service;
        
        private DataGridView _grid = null!;
        private Panel _sidePanel = null!;
        
        private TextBox _txtDoctorId = null!;
        private ComboBox _cmbDay = null!;
        private DateTimePicker _dtpStart = null!;
        private DateTimePicker _dtpEnd = null!;
        private CheckBox _chkIsAvailable = null!;
        
        private Button _btnSave = null!;
        private Button _btnCancel = null!;
        private Label _lblFormTitle = null!;

        private int? _editingId = null;

        public AvailabilityForm()
        {
            _service = new AvailabilityService();
            InitializeComponent();
            ThemeManager.ApplyThemeToForm(this);
            this.Load += async (s, e) => await LoadDataAsync();
        }

        private void InitializeComponent()
        {
            this.Text = "Disponibilidad Médica";
            
            var headerPanel = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(20) };
            var lblTitle = ThemeManager.CreateHeaderLabel("Horarios de Disponibilidad");
            headerPanel.Controls.Add(lblTitle);

            var mainContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 600, 
                FixedPanel = FixedPanel.Panel2,
                IsSplitterFixed = true,
                BackColor = ThemeManager.BackgroundColor
            };

            var leftPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            
            var toolbarPanel = new Panel { Dock = DockStyle.Top, Height = 50 };
            var btnRefresh = ThemeManager.CreateSecondaryButton("Refrescar");
            btnRefresh.Location = new Point(0, 0);
            btnRefresh.Click += async (s, e) => await LoadDataAsync();

            var btnAdd = ThemeManager.CreatePrimaryButton("Nuevo Horario");
            btnAdd.Location = new Point(btnRefresh.Right + 10, 0);
            btnAdd.Click += (s, e) => OpenFormForCreate();

            var btnEdit = ThemeManager.CreateSecondaryButton("Editar");
            btnEdit.Location = new Point(btnAdd.Right + 10, 0);
            btnEdit.Click += (s, e) => OpenFormForEdit();

            var btnDelete = ThemeManager.CreateSecondaryButton("Eliminar");
            btnDelete.Location = new Point(btnEdit.Right + 10, 0);
            btnDelete.ForeColor = Color.DarkRed;
            btnDelete.Click += async (s, e) => await DeleteSelectedAsync();

            toolbarPanel.Controls.AddRange(new Control[] { btnRefresh, btnAdd, btnEdit, btnDelete });

            _grid = new DataGridView { Dock = DockStyle.Fill, Margin = new Padding(0, 20, 0, 0) };
            ThemeManager.StyleDataGridView(_grid);
            _grid.SelectionChanged += (s, e) => {
                btnEdit.Enabled = btnDelete.Enabled = _grid.SelectedRows.Count > 0;
            };

            leftPanel.Controls.Add(_grid);
            leftPanel.Controls.Add(toolbarPanel);

            _sidePanel = ThemeManager.CreateCard();
            _sidePanel.Dock = DockStyle.Fill;
            _sidePanel.Visible = false;

            _lblFormTitle = ThemeManager.CreateHeaderLabel("Nuevo Horario");
            _lblFormTitle.Location = new Point(20, 20);

            int yPos = 70;
            _sidePanel.Controls.Add(_lblFormTitle);

            var lblDoc = new Label { Text = "ID Doctor:", Location = new Point(20, yPos), AutoSize = true, Font = ThemeManager.GetMainFont(10, FontStyle.Bold) };
            _txtDoctorId = ThemeManager.CreateTextBox();
            _txtDoctorId.Location = new Point(20, yPos + 20);
            _txtDoctorId.Width = 250;
            _sidePanel.Controls.Add(lblDoc);
            _sidePanel.Controls.Add(_txtDoctorId);
            yPos += 60;

            var lblDay = new Label { Text = "Día de la semana:", Location = new Point(20, yPos), AutoSize = true, Font = ThemeManager.GetMainFont(10, FontStyle.Bold) };
            _cmbDay = new ComboBox { Location = new Point(20, yPos + 20), Width = 250, Font = ThemeManager.GetMainFont(11), DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbDay.DataSource = Enum.GetValues(typeof(DayOfWeek));
            _sidePanel.Controls.Add(lblDay);
            _sidePanel.Controls.Add(_cmbDay);
            yPos += 60;

            var lblStart = new Label { Text = "Hora Inicio:", Location = new Point(20, yPos), AutoSize = true, Font = ThemeManager.GetMainFont(10, FontStyle.Bold) };
            _dtpStart = new DateTimePicker { Location = new Point(20, yPos + 20), Width = 250, Font = ThemeManager.GetMainFont(11), Format = DateTimePickerFormat.Time, ShowUpDown = true };
            _sidePanel.Controls.Add(lblStart);
            _sidePanel.Controls.Add(_dtpStart);
            yPos += 60;

            var lblEnd = new Label { Text = "Hora Fin:", Location = new Point(20, yPos), AutoSize = true, Font = ThemeManager.GetMainFont(10, FontStyle.Bold) };
            _dtpEnd = new DateTimePicker { Location = new Point(20, yPos + 20), Width = 250, Font = ThemeManager.GetMainFont(11), Format = DateTimePickerFormat.Time, ShowUpDown = true };
            _sidePanel.Controls.Add(lblEnd);
            _sidePanel.Controls.Add(_dtpEnd);
            yPos += 60;

            _chkIsAvailable = new CheckBox { Text = "Disponible", Location = new Point(20, yPos), AutoSize = true, Font = ThemeManager.GetMainFont(10, FontStyle.Bold), Checked = true };
            _sidePanel.Controls.Add(_chkIsAvailable);
            yPos += 40;

            _btnSave = ThemeManager.CreatePrimaryButton("Guardar");
            _btnSave.Location = new Point(20, yPos + 20);
            _btnSave.Click += async (s, e) => await SaveAsync();

            _btnCancel = ThemeManager.CreateSecondaryButton("Cancelar");
            _btnCancel.Location = new Point(_btnSave.Right + 10, yPos + 20);
            _btnCancel.Click += (s, e) => _sidePanel.Visible = false;

            _sidePanel.Controls.Add(_btnSave);
            _sidePanel.Controls.Add(_btnCancel);

            mainContainer.Panel1.Controls.Add(leftPanel);
            mainContainer.Panel2.Controls.Add(_sidePanel);
            mainContainer.Panel2.Padding = new Padding(20);

            this.Controls.Add(mainContainer);
            this.Controls.Add(headerPanel);

            btnEdit.Enabled = btnDelete.Enabled = false;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var data = await _service.GetAvailabilitiesAsync();
                _grid.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar disponibilidades: {ex.Message}", "Error - 404/API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenFormForCreate()
        {
            _editingId = null;
            _lblFormTitle.Text = "Nuevo Horario";
            _txtDoctorId.Text = Session.IsDoctor ? Session.UserId.ToString() : "";
            _txtDoctorId.Enabled = !Session.IsDoctor;
            
            _cmbDay.SelectedIndex = 0;
            _dtpStart.Value = DateTime.Today.AddHours(8); 
            _dtpEnd.Value = DateTime.Today.AddHours(17); 
            _chkIsAvailable.Checked = true;
            
            _sidePanel.Visible = true;
        }

        private void OpenFormForEdit()
        {
            if (_grid.SelectedRows.Count == 0) return;
            var row = _grid.SelectedRows[0];
            var av = (AvailabilityDto)row.DataBoundItem;

            _editingId = av.Id;
            _lblFormTitle.Text = "Editar Horario";
            _txtDoctorId.Text = av.DoctorId.ToString();
            _txtDoctorId.Enabled = false;
            
            _cmbDay.SelectedItem = av.DayOfWeek;
            _dtpStart.Value = DateTime.Today.Add(av.StartTime);
            _dtpEnd.Value = DateTime.Today.Add(av.EndTime);
            _chkIsAvailable.Checked = av.IsAvailable;

            _sidePanel.Visible = true;
        }

        private async Task SaveAsync()
        {
            // --- VALIDAR ANTES DE ENVIAR A LA API ---
            bool isValid = Validators.Validate(v =>
            {
                if (!_editingId.HasValue)
                    v.PositiveInteger(_txtDoctorId.Text, "ID Doctor");

                v.SelectionRequired(_cmbDay.SelectedItem, "Día de la Semana");
                v.TimeRange(_dtpStart.Value.TimeOfDay, _dtpEnd.Value.TimeOfDay, "Horario");
                v.Custom(
                    _dtpEnd.Value.TimeOfDay.TotalHours >= 1,
                    "La hora de fin debe ser al menos 1 hora después del inicio."
                );
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
                    var dto = new AvailabilityUpdateDto
                    {
                        DayOfWeek = (DayOfWeek)_cmbDay.SelectedItem!,
                        StartTime = _dtpStart.Value.TimeOfDay,
                        EndTime = _dtpEnd.Value.TimeOfDay,
                        IsAvailable = _chkIsAvailable.Checked
                    };
                    await _service.UpdateAsync(_editingId.Value, dto);
                    MessageBox.Show("✅ Disponibilidad actualizada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var dto = new AvailabilityCreateDto
                    {
                        DoctorId = int.Parse(_txtDoctorId.Text),
                        DayOfWeek = (DayOfWeek)_cmbDay.SelectedItem!,
                        StartTime = _dtpStart.Value.TimeOfDay,
                        EndTime = _dtpEnd.Value.TimeOfDay,
                        IsAvailable = _chkIsAvailable.Checked
                    };
                    await _service.CreateAsync(dto);
                    MessageBox.Show("✅ Disponibilidad creada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                _sidePanel.Visible = false;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var av = (AvailabilityDto)row.DataBoundItem;

            var result = MessageBox.Show($"¿Eliminar horario del doctor {av.DoctorName}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    await _service.DeleteAsync(av.Id);
                    await LoadDataAsync();
                    MessageBox.Show("Registro eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
