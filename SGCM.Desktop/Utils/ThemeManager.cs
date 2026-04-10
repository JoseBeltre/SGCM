using System.Drawing;
using System.Windows.Forms;

namespace SGCM.Desktop.Utils
{
    public static class ThemeManager
    {
        // Paleta de colores principal
        public static Color PrimaryColor = ColorTranslator.FromHtml("#3e93c1");
        public static Color PrimaryHoverColor = ColorTranslator.FromHtml("#32769a");
        public static Color PrimaryLightColor = ColorTranslator.FromHtml("#ecf4f9");

        // Paleta Neutral
        public static Color TextPrimaryColor = ColorTranslator.FromHtml("#191b18");
        public static Color TextSecondaryColor = ColorTranslator.FromHtml("#4c5049");
        public static Color BorderColor = ColorTranslator.FromHtml("#cbcfc9");
        public static Color BackgroundColor = ColorTranslator.FromHtml("#f2f3f2");
        public static Color CardColor = Color.White;

        // Estados y acentos
        public static Color SuccessColor = ColorTranslator.FromHtml("#87a45b");
        public static Color AccentColor = ColorTranslator.FromHtml("#f5a30a");

        public static Font GetMainFont(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font("Segoe UI", size, style);
        }

        public static void ApplyThemeToForm(Form form)
        {
            form.BackColor = BackgroundColor;
            form.ForeColor = TextPrimaryColor;
            form.Font = GetMainFont(10);
            
            // Si el formulario debe ser pantalla completa
            form.WindowState = FormWindowState.Maximized;
            // Opcional: para esconder bordes si quisieran un look ultra moderno, pero mantener botones de sistema es util
            // form.FormBorderStyle = FormBorderStyle.None;
        }

        public static Button CreatePrimaryButton(string text, int width = 120, int height = 40)
        {
            var btn = new Button
            {
                Text = text,
                Width = width,
                Height = height,
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = GetMainFont(10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            
            // Efecto Hover simple
            btn.MouseEnter += (s, e) => btn.BackColor = PrimaryHoverColor;
            btn.MouseLeave += (s, e) => btn.BackColor = PrimaryColor;
            
            return btn;
        }

        public static Button CreateSecondaryButton(string text, int width = 120, int height = 40)
        {
            var btn = new Button
            {
                Text = text,
                Width = width,
                Height = height,
                BackColor = CardColor,
                ForeColor = TextPrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Font = GetMainFont(10, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = BorderColor;
            btn.FlatAppearance.BorderSize = 1;

            btn.MouseEnter += (s, e) => btn.BackColor = BackgroundColor;
            btn.MouseLeave += (s, e) => btn.BackColor = CardColor;

            return btn;
        }

        public static Panel CreateCard()
        {
            return new Panel
            {
                BackColor = CardColor,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(20)
            };
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = CardColor;
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = BorderColor;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryLightColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextSecondaryColor;
            dgv.ColumnHeadersDefaultCellStyle.Font = GetMainFont(10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
            dgv.ColumnHeadersHeight = 40;

            dgv.DefaultCellStyle.BackColor = CardColor;
            dgv.DefaultCellStyle.ForeColor = TextPrimaryColor;
            dgv.DefaultCellStyle.SelectionBackColor = PrimaryLightColor;
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimaryColor;
            dgv.DefaultCellStyle.Font = GetMainFont(10);
            dgv.DefaultCellStyle.Padding = new Padding(10, 5, 10, 5);
            dgv.RowTemplate.Height = 40;
        }

        public static Label CreateHeaderLabel(string text)
        {
            return new Label
            {
                Text = text,
                ForeColor = TextPrimaryColor,
                Font = GetMainFont(18, FontStyle.Bold),
                AutoSize = true
            };
        }
        
        public static TextBox CreateTextBox()
        {
            return new TextBox
            {
                Font = GetMainFont(11),
                ForeColor = TextPrimaryColor,
                BackColor = CardColor,
                BorderStyle = BorderStyle.FixedSingle,
            };
        }
    }
}
