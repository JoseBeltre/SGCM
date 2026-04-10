using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SGCM.Desktop.Models;
using SGCM.Desktop.Services;
using SGCM.Desktop.Utils;

namespace SGCM.Desktop.Forms
{
    public class SpecialtyForm : Form
    {
        private readonly SpecialtyService _service;
        
        private DataGridView _grid = null!;
        private Panel _sidePanel = null!;
        
        private TextBox _txtName = null!;
        private TextBox _txtDescription = null!;
        
        private Button _btnSave = null!;
        private Button _btnCancel = null!;
        private Label _lblFormTitle = null!;

        private int? _editingId = null;

        public SpecialtyForm()
        {
            _service = new SpecialtyService();
            InitializeComponent();
            ThemeManager.ApplyThemeToForm(this);
            this.Load += async (s, e) => await LoadDataAsync();
        }

        private void InitializeComponent()
        {
            this.Text = "Gestión de Especialidades";
            this.Size = new Size(1100, 700);

            var mainContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 750,
                IsSplitterFixed = true,
                BackColor = Color.White
            };

            // PANEL IZQUIERDO
            var leftPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            
            var header = new Label
            {
                Text = "🩺 Especialidades Médicas",
                Font = ThemeManager.GetMainFont(18, FontStyle.Bold),
                ForeColor = ThemeManager.PrimaryColor,
                Dock = DockStyle.Top,
                Height = 60
            };

            var toolbar = new Panel { Dock = DockStyle.Top, Height = 50 };
            
            var btnRefresh = ThemeManager.CreateSecondaryButton("Refrescar");
            btnRefresh.Location = new Point(0, 0);
            btnRefresh.Click += async (s, e) => await LoadDataAsync();

            var btnAdd = ThemeManager.CreatePrimaryButton("+ Nueva Especialidad");
            btnAdd.Location = new Point(btnRefresh.Right + 10, 0);
            btnAdd.Enabled = Session.IsAdmin;
            btnAdd.Click += (s, e) => OpenFormForCreate();

            var btnEdit = ThemeManager.CreateSecondaryButton("Editar");
            btnEdit.Location = new Point(btnAdd.Right + 10, 0);
            btnEdit.Click += (s, e) => OpenFormForEdit();

            var btnDelete = ThemeManager.CreateSecondaryButton("Eliminar");
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

            // PANEL DERECHO
            _sidePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.BackgroundColor,
                Padding = new Padding(20),
                Visible = false
            };

            _lblFormTitle = new Label
            {
                Text = "Nueva Especialidad",
                Font = ThemeManager.GetMainFont(14, FontStyle.Bold),
                ForeColor = ThemeManager.PrimaryColor,
                Dock = DockStyle.Top,
                Height = 40
            };

            int y = 50;
            _txtName = CreateLabeledTextBox("Nombre de la Especialidad:", ref y);
            _txtDescription = CreateLabeledTextBox("Descripción:", ref y);
            _txtDescription.Multiline = true;
            _txtDescription.Height = 100;
            y += 70;

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
                var data = await _service.GetSpecialtiesAsync();
                _grid.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar especialidades: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenFormForCreate()
        {
            _editingId = null;
            _lblFormTitle.Text = "Nueva Especialidad";
            _txtName.Text = "";
            _txtDescription.Text = "";

            _sidePanel.Visible = true;
        }

        private void OpenFormForEdit()
        {
            if (_grid.SelectedRows.Count == 0) return;
            var row = _grid.SelectedRows[0];
            var specialty = (SpecialtyDto)row.DataBoundItem;

            _editingId = specialty.Id;
            _lblFormTitle.Text = "Editar Especialidad";
            _txtName.Text = specialty.Name;
            _txtDescription.Text = specialty.Description;

            _sidePanel.Visible = true;
        }

        private async Task SaveAsync()
        {
            // --- VALIDAR ANTES DE ENVIAR A LA API ---
            bool isValid = Validators.Validate(v =>
            {
                v.Required(_txtName.Text, "Nombre de la Especialidad");
                v.MinLength(_txtName.Text, "Nombre", 3);
                v.Required(_txtDescription.Text, "Descripción");
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
                    var dto = new SpecialtyUpdateDto
                    {
                        Name = _txtName.Text.Trim(),
                        Description = _txtDescription.Text.Trim()
                    };
                    await _service.UpdateAsync(_editingId.Value, dto);
                    MessageBox.Show("✅ Especialidad actualizada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var dto = new SpecialtyCreateDto
                    {
                        Name = _txtName.Text.Trim(),
                        Description = _txtDescription.Text.Trim()
                    };
                    await _service.CreateAsync(dto);
                    MessageBox.Show("✅ Especialidad creada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                _sidePanel.Visible = false;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                var detail = ex.InnerException != null ? $"\nDetalle: {ex.InnerException.Message}" : "";
                MessageBox.Show($"❌ Error al guardar especialidad: {ex.Message}{detail}", "Error del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var specialty = (SpecialtyDto)row.DataBoundItem;

            var result = MessageBox.Show($"¿Eliminar especialidad {specialty.Name}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    await _service.DeleteAsync(specialty.Id);
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
