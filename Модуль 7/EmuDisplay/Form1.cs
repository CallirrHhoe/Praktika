using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
namespace EmuDisplay
{
    // Цветовые схемы графического интерфейса
    public enum LcdColorTheme
    {
        BlueWhite,
        GreenBlack,
        YellowBlack
    }
    // Контейнер данных для логирования транзакций командной шины
    public class LcdCommandEventArgs : EventArgs
    {
        public string CommandName { get; }
        public string Details { get; }
        public DateTime Timestamp { get; }
        public LcdCommandEventArgs(string cmd, string details)
        {
            CommandName = cmd;
            Details = details;
            Timestamp = DateTime.Now;
        }
    }
    // Вычислительное ядро симулятора контроллера 
    public class Lcd2004Core
    {
        public const int Columns = 20;
        public const int Rows = 4;
        private char[,] ddram = new char[Rows, Columns]; 
        private byte[][] cgram = new byte[8][]; // Память пользовательских знакогенераторов
        private int cursorX = 0;
        private int cursorY = 0;
        private bool displayOn = true;
        private bool cursorOn = true;
        private bool blinkOn = true;
        private bool shiftIncrement = true;
        private bool displayShift = false;
        private int displayOffset = 0; // Кольцевой сдвиг адресации DDRAM
        public event EventHandler<LcdCommandEventArgs> CommandExecuted;
        public event EventHandler DisplayUpdated;
        public int CursorX => cursorX;
        public int CursorY => cursorY;
        public bool DisplayOn => displayOn;
        public bool CursorOn => cursorOn;
        public bool BlinkOn => blinkOn;
        public Lcd2004Core()
        {
            for (int i = 0; i < 8; i++) cgram[i] = new byte[8];
            Clear();
        }
        // Генерация событий обновления состояния контроллера
        private void OnCommand(string name, string details)
        {
            CommandExecuted?.Invoke(this, new LcdCommandEventArgs(name, details));
            DisplayUpdated?.Invoke(this, EventArgs.Empty);
        }
        // Расчет циклического индекса ячейки с учетом текущего смещения экрана
        public char GetCharAt(int r, int c)
        {
            int actualCol = (c + displayOffset) % Columns;
            if (actualCol < 0) actualCol += Columns;
            return ddram[r, actualCol];
        }
        public byte[] GetCustomChar(int index)
        {
            if (index >= 0 && index < 8) return cgram[index];
            return new byte[8];
        }
        // Очистка ОЗУ дисплея
        public void Clear()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    ddram[r, c] = ' ';
            cursorX = 0;
            cursorY = 0;
            displayOffset = 0;
            OnCommand("Clear Display", "DDRAM заполнена пробелами, курсор в (0,0)");
        }
        // Возврат курсора в начальную позицию
        public void ReturnHome()
        {
            cursorX = 0;
            cursorY = 0;
            displayOffset = 0;
            OnCommand("Return Home", "Курсор возвращен в (0,0), сдвиг сброшен");
        }
        // Сдвига при вводе
        public void EntryModeSet(bool increment, bool shift)
        {
            shiftIncrement = increment;
            displayShift = shift;
            OnCommand("Entry Mode Set", $"Автосдвиг курсора вправо: {increment}, Сдвиг экрана: {shift}");
        }
        // Управление видимостью растра, курсора и его мерцания
        public void DisplayOnOffControl(bool display, bool cursor, bool blink)
        {
            displayOn = display;
            cursorOn = cursor;
            blinkOn = blink;
            OnCommand("Display On/Off", $"Экран: {display}, Курсор: {cursor}, Мигание: {blink}");
        }
        // Аппаратный сдвиг каретки или содержимого экрана
        public void CursorOrDisplayShift(bool shiftScreen, bool shiftRight)
        {
            if (shiftScreen)
            {
                if (shiftRight) displayOffset = (displayOffset - 1 + Columns) % Columns;
                else displayOffset = (displayOffset + 1) % Columns;
                OnCommand("Shift Display", "Экран сдвинут " + (shiftRight ? "вправо" : "влево"));
            }
            else
            {
                if (shiftRight) MoveCursorRight();
                else MoveCursorLeft();
                OnCommand("Shift Cursor", "Курсор сдвинут " + (shiftRight ? "вправо" : "влево"));
            }
        }
        // Прямое позиционирование адреса курсора
        public void SetCursor(int row, int col)
        {
            if (row >= 0 && row < Rows) cursorY = row;
            if (col >= 0 && col < Columns) cursorX = col;
            OnCommand("Set Cursor Pos", $"Координаты установлены в Строка: {cursorY}, Столбец: {cursorX}");
        }
        // Запись матрицы 5x8 в таблицу символов CGRAM
        public void StoreCustomChar(int index, byte[] pattern)
        {
            if (index >= 0 && index < 8 && pattern != null && pattern.Length == 8)
            {
                for (int i = 0; i < 8; i++) cgram[index][i] = (byte)(pattern[i] & 0x1F);
                OnCommand("CGRAM Write", $"Создан кастомный символ в ячейке {index}");
            }
        }
        // Посимвольный вывод в DDRAM с обработкой автосдвигов
        public void WriteChar(char c)
        {
            if (!displayOn) return;
            ddram[cursorY, cursorX] = c;
            if (displayShift)
            {
                if (shiftIncrement) displayOffset = (displayOffset + 1) % Columns;
                else displayOffset = (displayOffset - 1 + Columns) % Columns;
            }
            if (shiftIncrement) MoveCursorRight();
            else MoveCursorLeft();
            DisplayUpdated?.Invoke(this, EventArgs.Empty);
        }
        // Потоковый вывод строк и обработка управляющих символов переноса
        public void WriteString(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            foreach (char c in text)
            {
                if (c == '\n')
                {
                    cursorX = 0;
                    cursorY = (cursorY + 1) % Rows;
                }
                else if (c == '\r')
                {
                    cursorX = 0;
                }
                else
                {
                    WriteChar(c);
                }
            }
            OnCommand("Write String", $"Выведена строка длиной {text.Length} симв.");
        }
        private void MoveCursorRight()
        {
            cursorX++;
            if (cursorX >= Columns)
            {
                cursorX = 0;
                cursorY++;
                if (cursorY >= Rows) cursorY = 0;
            }
        }
        private void MoveCursorLeft()
        {
            cursorX--;
            if (cursorX < 0)
            {
                cursorX = Columns - 1;
                cursorY--;
                if (cursorY < 0) cursorY = Rows - 1;
            }
        }
    }
    //UI-компонент отрисовки жидкокристаллической матрицы дисплея 
    public class LcdVisualPanel : Panel
    {
        private Lcd2004Core core;
        private LcdColorTheme theme = LcdColorTheme.BlueWhite;
        private bool blinkState = true;
        private System.Windows.Forms.Timer blinkTimer;
        private Color colorBg = Color.FromArgb(0, 0, 139);
        private Color colorFg = Color.FromArgb(255, 255, 255);
        private Color colorPixelOff = Color.FromArgb(0, 0, 100);
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public LcdColorTheme Theme
        {
            get => theme;
            set
            {
                theme = value;
                ApplyTheme();
                Invalidate();
            }
        }
        public LcdVisualPanel(Lcd2004Core core)
        {
            this.core = core;
            this.DoubleBuffered = true; // Защита от мерцания при перерисовке
            this.core.DisplayUpdated += (s, e) => Invalidate();
            ApplyTheme();
            blinkTimer = new System.Windows.Forms.Timer();
            blinkTimer.Interval = 400; // Период мигания аппаратного курсора
            blinkTimer.Tick += (s, e) => { blinkState = !blinkState; Invalidate(); };
            blinkTimer.Start();
        }
        // Конфигурация палитры активных/неактивных пикселей экрана
        private void ApplyTheme()
        {
            switch (theme)
            {
                case LcdColorTheme.BlueWhite:
                    colorBg = Color.FromArgb(15, 30, 170);
                    colorFg = Color.FromArgb(240, 250, 255);
                    colorPixelOff = Color.FromArgb(25, 45, 190);
                    break;
                case LcdColorTheme.GreenBlack:
                    colorBg = Color.FromArgb(30, 140, 30);
                    colorFg = Color.FromArgb(5, 20, 5);
                    colorPixelOff = Color.FromArgb(35, 125, 35);
                    break;
                case LcdColorTheme.YellowBlack:
                    colorBg = Color.FromArgb(180, 170, 10);
                    colorFg = Color.FromArgb(10, 10, 0);
                    colorPixelOff = Color.FromArgb(165, 155, 10);
                    break;
            }
        }
        // Рендеринг рамки, сетки символов и курсора
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(40, 40, 40));
            int borderWidth = 12;
            using (Brush borderBrush = new SolidBrush(Color.FromArgb(20, 20, 20)))
            {
                g.FillRectangle(borderBrush, 0, 0, Width, Height);
            }
            int displayW = Width - (borderWidth * 2);
            int displayH = Height - (borderWidth * 2);
            using (Brush bgBrush = new SolidBrush(colorBg))
            {
                g.FillRectangle(bgBrush, borderWidth, borderWidth, displayW, displayH);
            }
            if (!core.DisplayOn) return;
            float cellW = (float)displayW / Lcd2004Core.Columns;
            float cellH = (float)displayH / Lcd2004Core.Rows;
            float paddingX = cellW * 0.08f;
            float paddingY = cellH * 0.08f;
            using (Brush fgBrush = new SolidBrush(colorFg))
            using (Brush offBrush = new SolidBrush(colorPixelOff))
            {
                for (int r = 0; r < Lcd2004Core.Rows; r++)
                {
                    for (int c = 0; c < Lcd2004Core.Columns; c++)
                    {
                        float x = borderWidth + (c * cellW) + paddingX;
                        float y = borderWidth + (r * cellH) + paddingY;
                        float w = cellW - (paddingX * 2);
                        float h = cellH - (paddingY * 2);
                        char ch = core.GetCharAt(r, c);
                        bool isCursor = (r == core.CursorY && c == core.CursorX);
                        // Выбор шрифтового 
                        if (ch >= 0 && ch < 8) DrawCustomCharacter(g, core.GetCustomChar(ch), x, y, w, h, fgBrush, offBrush);
                        else DrawStandardCharacter(g, ch, x, y, w, h, fgBrush);
                        // Логика визуализации инверсного или подчеркивающего курсора
                        if (isCursor)
                        {
                            if (core.BlinkOn && blinkState)
                            {
                                using (Brush cursorBrush = new SolidBrush(Color.FromArgb(140, colorFg.R, colorFg.G, colorFg.B)))
                                    g.FillRectangle(cursorBrush, x, y, w, h);
                            }
                            else if (core.CursorOn)
                            {
                                using (Pen cursorPen = new Pen(colorFg, 2))
                                    g.DrawLine(cursorPen, x, y + h, x + w, y + h);
                            }
                        }
                    }
                }
            }
        }
        // Вывод стандартных глифов через моноширинный системный шрифт
        private void DrawStandardCharacter(Graphics g, char c, float x, float y, float w, float h, Brush brush)
        {
            if (c == ' ') return;
            using (Font f = new Font("Consolas", h * 0.85f, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                string s = c.ToString();
                SizeF size = g.MeasureString(s, f);
                float tx = x + (w - size.Width) / 2f;
                float ty = y + (h - size.Height) / 2f;
                g.DrawString(s, f, brush, tx, ty);
            }
        }
        // Попиксельный рендеринг матрицы 5x8
        private void DrawCustomCharacter(Graphics g, byte[] pattern, float x, float y, float w, float h, Brush fg, Brush bg)
        {
            float pW = w / 5f;
            float pH = h / 8f;
            for (int i = 0; i < 8; i++)
            {
                byte rowByte = pattern[i];
                for (int j = 0; j < 5; j++)
                {
                    bool bit = (rowByte & (1 << (4 - j))) != 0;
                    float px = x + (j * pW);
                    float py = y + (i * pH);
                    g.FillRectangle(bit ? fg : bg, px, py, pW - 0.5f, pH - 0.5f);
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) blinkTimer?.Dispose();
            base.Dispose(disposing);
        }
    }
    // Главная графическая форма приложения 
    public partial class EmuDisplay : Form
    {
        private Lcd2004Core lcd;
        private LcdVisualPanel viewPanel;
        private GroupBox grpTextControls;
        private TextBox txtInput;
        private Button btnWriteText;
        private Button btnClear;
        private Button btnHome;
        private GroupBox grpHardwareControls;
        private CheckBox chkDispOn;
        private CheckBox chkCursorOn;
        private CheckBox chkBlinkOn;
        private NumericUpDown numRow;
        private NumericUpDown numCol;
        private Button btnSetCursor;
        private Label lblRow;
        private Label lblCol;
        private GroupBox grpCgramControls;
        private CheckBox[,] cgramPixels = new CheckBox[8, 5]; // Интерактивная матрица бит CGRAM
        private ComboBox cmbCgramIndex;
        private Button btnSaveChar;
        private Label lblCgram;
        private Button btnInsertCustom;
        private GroupBox grpMacroControls;
        private Button btnMacro1;
        private Button btnMacro2;
        private Button btnMacro3;
        private ComboBox cmbTheme;
        private Label lblTheme;
        private GroupBox grpLogPanel;
        private ComboBox cmbLogs;
        private Label lblLogCount;
        private System.Windows.Forms.Timer animationTimer;
        private int animationStep = 0;
        private List<string> historyList = new List<string>();
        public EmuDisplay()
        {
            lcd = new Lcd2004Core();
            InitializeComponent();
            lcd.CommandExecuted += OnLcdCommand;
            ApplyLayoutStyles();
            this.KeyPreview = true; 
            this.KeyDown += EmuDisplay_KeyDown; 
            this.Show();
            this.Update();
        }
            // Обработчик нажатия клавиш на форме
        private void EmuDisplay_KeyDown(object sender, KeyEventArgs e)
        {
            // Если пользователь нажал клавишу F1
            if (e.KeyCode == Keys.F1)
            {
                OpenHelpSystem();
            }
        }
        // Логика поиска и запуска файла справки CHM
        private void OpenHelpSystem()
        {
            string helpPath = System.IO.Path.Combine(Application.StartupPath, "EmuDisplayHelp.chm");

            if (System.IO.File.Exists(helpPath))
            {
                // Запуск встроенного просмотрщика справки Windows для файла CHM
                Help.ShowHelp(this, helpPath);
            }
            else
            {
                // Если файла нет
                MessageBox.Show(
                    $"Файл справки не найден.\n\nОжидаемый путь: {helpPath}\n\n" +
                    "Пожалуйста, убедитесь, что файл EmuDisplayHelp.chm скопирован в папку с запущенной программой.",
                    "Справка по эмулятору",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        private ToolTip uiToolTip;
        // Сборка элементов управления
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Эмулятор Дисплея(20x04)";
            this.Size = new Size(840, 700);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            viewPanel = new LcdVisualPanel(lcd);
            viewPanel.Location = new Point(20, 20);
            viewPanel.Size = new Size(560, 180);
            this.Controls.Add(viewPanel);
            grpTextControls = new GroupBox();
            grpTextControls.Text = "Операции с текстом";
            grpTextControls.Location = new Point(20, 215);
            grpTextControls.Size = new Size(360, 110);
            this.Controls.Add(grpTextControls);
            txtInput = new TextBox();
            txtInput.Location = new Point(15, 25);
            txtInput.Size = new Size(230, 23);
            txtInput.Text = "Hello World!";
            grpTextControls.Controls.Add(txtInput);
            btnWriteText = new Button();
            btnWriteText.Text = "Печать";
            btnWriteText.Location = new Point(255, 24);
            btnWriteText.Size = new Size(90, 25);
            btnWriteText.Click += (s, e) => lcd.WriteString(txtInput.Text);
            grpTextControls.Controls.Add(btnWriteText);
            btnClear = new Button();
            btnClear.Text = "Очистить";
            btnClear.Location = new Point(15, 65);
            btnClear.Size = new Size(160, 30);
            btnClear.Click += (s, e) => lcd.Clear();
            grpTextControls.Controls.Add(btnClear);
            btnHome = new Button();
            btnHome.Text = "Домой";
            btnHome.Location = new Point(185, 65);
            btnHome.Size = new Size(160, 30);
            btnHome.Click += (s, e) => lcd.ReturnHome();
            grpTextControls.Controls.Add(btnHome);
            grpHardwareControls = new GroupBox();
            grpHardwareControls.Text = "Контроллер дисплея (Режимы и Адресация)";
            grpHardwareControls.Location = new Point(20, 335);
            grpHardwareControls.Size = new Size(360, 160);
            this.Controls.Add(grpHardwareControls);
            chkDispOn = new CheckBox();
            chkDispOn.Text = "Экран включен";
            chkDispOn.Location = new Point(15, 25);
            chkDispOn.Checked = true;
            chkDispOn.CheckedChanged += HardwareFlagsChanged;
            grpHardwareControls.Controls.Add(chkDispOn);
            chkCursorOn = new CheckBox();
            chkCursorOn.Text = "Курсор";
            chkCursorOn.Location = new Point(140, 25);
            chkCursorOn.Checked = true;
            chkCursorOn.CheckedChanged += HardwareFlagsChanged;
            grpHardwareControls.Controls.Add(chkCursorOn);
            chkBlinkOn = new CheckBox();
            chkBlinkOn.Text = "Мигание";
            chkBlinkOn.Location = new Point(230, 25);
            chkBlinkOn.Checked = true;
            chkBlinkOn.CheckedChanged += HardwareFlagsChanged;
            grpHardwareControls.Controls.Add(chkBlinkOn);
            lblRow = new Label();
            lblRow.Text = "Стр:";
            lblRow.Location = new Point(15, 72);
            lblRow.Size = new Size(35, 20);
            grpHardwareControls.Controls.Add(lblRow);
            numRow = new NumericUpDown();
            numRow.Location = new Point(55, 70);
            numRow.Size = new Size(50, 23);
            numRow.Minimum = 0;
            numRow.Maximum = 3;
            grpHardwareControls.Controls.Add(numRow);
            lblCol = new Label();
            lblCol.Text = "Столб:";
            lblCol.Location = new Point(120, 72);
            lblCol.Size = new Size(45, 20);
            grpHardwareControls.Controls.Add(lblCol);
            numCol = new NumericUpDown();
            numCol.Location = new Point(170, 70);
            numCol.Size = new Size(50, 23);
            numCol.Minimum = 0;
            numCol.Maximum = 19;
            grpHardwareControls.Controls.Add(numCol);
            btnSetCursor = new Button();
            btnSetCursor.Text = "Уст. курсор";
            btnSetCursor.Location = new Point(235, 68);
            btnSetCursor.Size = new Size(110, 26);
            btnSetCursor.Click += (s, e) => lcd.SetCursor((int)numRow.Value, (int)numCol.Value);
            grpHardwareControls.Controls.Add(btnSetCursor);
            Button btnShiftL = new Button();
            btnShiftL.Text = "◀ Сдвиг экр.";
            btnShiftL.Location = new Point(15, 115);
            btnShiftL.Size = new Size(160, 30);
            btnShiftL.Click += (s, e) => lcd.CursorOrDisplayShift(true, false);
            grpHardwareControls.Controls.Add(btnShiftL);
            Button btnShiftR = new Button();
            btnShiftR.Text = "Сдвиг экр. ▶";
            btnShiftR.Location = new Point(185, 115);
            btnShiftR.Size = new Size(160, 30);
            btnShiftR.Click += (s, e) => lcd.CursorOrDisplayShift(true, true);
            grpHardwareControls.Controls.Add(btnShiftR);
            grpCgramControls = new GroupBox();
            grpCgramControls.Text = "Редактор CGRAM (Собственные символы)";
            grpCgramControls.Location = new Point(400, 215);
            grpCgramControls.Size = new Size(400, 280);
            this.Controls.Add(grpCgramControls);
            int startPixelX = 20;
            int startPixelY = 30;
            int pixelSize = 22;
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    CheckBox chk = new CheckBox();
                    chk.Appearance = Appearance.Button;
                    chk.Size = new Size(pixelSize, pixelSize);
                    chk.Location = new Point(startPixelX + (c * (pixelSize + 2)), startPixelY + (r * (pixelSize + 2)));
                    chk.FlatStyle = FlatStyle.Flat;
                    chk.FlatAppearance.CheckedBackColor = Color.Black;
                    chk.BackColor = Color.LightGray;
                    grpCgramControls.Controls.Add(chk);
                    cgramPixels[r, c] = chk;
                }
            }
            lblCgram = new Label();
            lblCgram.Text = "Ячейка памяти:";
            lblCgram.Location = new Point(160, 35);
            lblCgram.Size = new Size(100, 20);
            grpCgramControls.Controls.Add(lblCgram);
            cmbCgramIndex = new ComboBox();
            cmbCgramIndex.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCgramIndex.Location = new Point(270, 32);
            cmbCgramIndex.Size = new Size(110, 23);
            for (int i = 0; i < 8; i++) cmbCgramIndex.Items.Add($"Символ {i}");
            cmbCgramIndex.SelectedIndex = 0;
            grpCgramControls.Controls.Add(cmbCgramIndex);
            btnSaveChar = new Button();
            btnSaveChar.Text = "Сохранить в CGRAM";
            btnSaveChar.Location = new Point(160, 80);
            btnSaveChar.Size = new Size(220, 35);
            btnSaveChar.Click += SaveCustomCharacterData;
            grpCgramControls.Controls.Add(btnSaveChar);
            btnInsertCustom = new Button();
            btnInsertCustom.Text = "Напечатать символ";
            btnInsertCustom.Location = new Point(160, 130);
            btnInsertCustom.Size = new Size(220, 35);
            btnInsertCustom.Click += (s, e) => lcd.WriteChar((char)cmbCgramIndex.SelectedIndex);
            grpCgramControls.Controls.Add(btnInsertCustom);
            Button btnClearCgramGrid = new Button();
            btnClearCgramGrid.Text = "Очистить сетку";
            btnClearCgramGrid.Location = new Point(160, 230);
            btnClearCgramGrid.Size = new Size(220, 30);
            btnClearCgramGrid.Click += (s, e) => {
                for (int r = 0; r < 8; r++)
                    for (int c = 0; c < 5; c++) cgramPixels[r, c].Checked = false;
            };
            grpCgramControls.Controls.Add(btnClearCgramGrid);
            grpMacroControls = new GroupBox();
            grpMacroControls.Text = "Макросы";
            grpMacroControls.Location = new Point(600, 20);
            grpMacroControls.Size = new Size(200, 180);
            this.Controls.Add(grpMacroControls);
            btnMacro1 = new Button();
            btnMacro1.Text = "Тестовая таблица";
            btnMacro1.Location = new Point(15, 25);
            btnMacro1.Size = new Size(170, 32);
            btnMacro1.Click += LoadTestPattern;
            grpMacroControls.Controls.Add(btnMacro1);
            btnMacro2 = new Button();
            btnMacro2.Text = "Анимация бегущей строки";
            btnMacro2.Location = new Point(15, 70);
            btnMacro2.Size = new Size(170, 32);
            btnMacro2.Click += ToggleMarqueeAnimation;
            grpMacroControls.Controls.Add(btnMacro2);
            btnMacro3 = new Button();
            btnMacro3.Text = "Сброс к заводским";
            btnMacro3.Location = new Point(15, 115);
            btnMacro3.Size = new Size(170, 32);
            btnMacro3.Click += FactoryResetSimulation;
            grpMacroControls.Controls.Add(btnMacro3);
            grpLogPanel = new GroupBox();
            grpLogPanel.Text = "Анализатор командной шины (Лог интерфейса)";
            grpLogPanel.Location = new Point(20, 510);
            grpLogPanel.Size = new Size(780, 110);
            this.Controls.Add(grpLogPanel);
            cmbLogs = new ComboBox();
            cmbLogs.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLogs.Location = new Point(15, 35);
            cmbLogs.Size = new Size(750, 23);
            grpLogPanel.Controls.Add(cmbLogs);
            lblLogCount = new Label();
            lblLogCount.Text = "Всего транзакций на шине: 0";
            lblLogCount.Location = new Point(15, 75);
            lblLogCount.Size = new Size(300, 20);
            grpLogPanel.Controls.Add(lblLogCount);
            lblTheme = new Label();
            lblTheme.Text = "Цветовая схема:";
            lblTheme.Location = new Point(20, 638);
            lblTheme.Size = new Size(110, 20);
            this.Controls.Add(lblTheme);
            cmbTheme = new ComboBox();
            cmbTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTheme.Location = new Point(135, 635);
            cmbTheme.Size = new Size(160, 23);
            cmbTheme.Items.Add("Синяя (Blue/White)");
            cmbTheme.Items.Add("Зеленая (Green/Black)");
            cmbTheme.Items.Add("Желтая (Yellow/Black)");
            cmbTheme.SelectedIndex = 0;
            cmbTheme.SelectedIndexChanged += (s, e) => {
                viewPanel.Theme = (LcdColorTheme)cmbTheme.SelectedIndex;
            };
            this.Controls.Add(cmbTheme);
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 250;
            animationTimer.Tick += ExecuteAnimationStep;
            LoadDefaultHeartPattern();
            this.ResumeLayout(false);
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 250;
            animationTimer.Tick += ExecuteAnimationStep;
            LoadDefaultHeartPattern();
            ToolTip uiToolTip = new ToolTip();
            uiToolTip.AutoPopDelay = 5000; // Подсказка отображается 5 секунд
            uiToolTip.InitialDelay = 500;  // Появляется через 0.5 секунды после наведения
            uiToolTip.ReshowDelay = 500;   // Задержка при переходе между кнопками
            uiToolTip.SetToolTip(btnWriteText, "Вывести текст из поля ввода в DDRAM память дисплея");
            uiToolTip.SetToolTip(btnClear, "Очистить экран (заполнить DDRAM пробелами) и сбросить курсор в позицию (0,0)");
            uiToolTip.SetToolTip(btnHome, "Вернуть курсор в начальную позицию (0,0) без очистки экрана");
            uiToolTip.SetToolTip(btnSetCursor, "Переместить курсор на указанную строку и столбец");
            uiToolTip.SetToolTip(btnSaveChar, "Записать созданный битовый паттерн 5х8 в выбранную ячейку CGRAM");
            uiToolTip.SetToolTip(btnInsertCustom, "Напечатать кастомный символ из CGRAM в текущей позиции курсора");
            uiToolTip.SetToolTip(btnMacro2, "Запуск циклического сдвига экрана влево для демонстрации бегущей строки");
            this.ResumeLayout(false);
        }    
        // Форматирование внешнего вида шрифтов элементов интерфейса
        private void ApplyLayoutStyles()
        {
            Font mainFont = new Font("Segoe UI", 9f, FontStyle.Regular);
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Font = mainFont;
                if (ctrl is GroupBox grp)
                {
                    foreach (Control inner in grp.Controls) inner.Font = mainFont;
                }
            }
            txtInput.Font = new Font("Consolas", 10f);
            cmbLogs.Font = new Font("Consolas", 9f);
        }
        private void HardwareFlagsChanged(object sender, EventArgs e)
        {
            lcd.DisplayOnOffControl(chkDispOn.Checked, chkCursorOn.Checked, chkBlinkOn.Checked);
        }
        // Чтение чекбоксов и упаковка битовых строк в байты для отправки в CGRAM
        private void SaveCustomCharacterData(object sender, EventArgs e)
        {
            int cellIdx = cmbCgramIndex.SelectedIndex;
            byte[] rawBytes = new byte[8];
            for (int r = 0; r < 8; r++)
            {
                byte rowVal = 0;
                for (int c = 0; c < 5; c++)
                {
                    if (cgramPixels[r, c].Checked)
                    {
                        rowVal |= (byte)(1 << (4 - c)); // Формирование битовой маски строки 
                    }
                }
                rawBytes[r] = rowVal;
            }
            lcd.StoreCustomChar(cellIdx, rawBytes);
            MessageBox.Show($"Графический образ записан в ячейку CGRAM [{cellIdx}]. Вы можете напечатать его на экране.", "Запись завершена", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        // Генерация стартового пользовательского паттерна в ячейку 0
        private void LoadDefaultHeartPattern()
        {
            byte[] heart = { 0x00, 0x0A, 0x1F, 0x1F, 0x1F, 0x0E, 0x04, 0x00 };
            lcd.StoreCustomChar(0, heart);
            cgramPixels[1, 1].Checked = true; cgramPixels[1, 3].Checked = true;
            cgramPixels[2, 0].Checked = true; cgramPixels[2, 1].Checked = true; cgramPixels[2, 2].Checked = true; cgramPixels[2, 3].Checked = true; cgramPixels[2, 4].Checked = true;
            cgramPixels[3, 0].Checked = true; cgramPixels[3, 1].Checked = true; cgramPixels[3, 2].Checked = true; cgramPixels[3, 3].Checked = true; cgramPixels[3, 4].Checked = true;
            cgramPixels[4, 0].Checked = true; cgramPixels[4, 1].Checked = true; cgramPixels[4, 2].Checked = true; cgramPixels[4, 3].Checked = true; cgramPixels[4, 4].Checked = true;
            cgramPixels[5, 1].Checked = true; cgramPixels[5, 2].Checked = true; cgramPixels[5, 3].Checked = true;
            cgramPixels[6, 2].Checked = true;
        }
        // Обработка прерываний командной шины и обновление списка транзакций
        private void OnLcdCommand(object sender, LcdCommandEventArgs e)
        {
            string timeStr = e.Timestamp.ToString("HH:mm:ss.fff");
            string entry = $"[{timeStr}] {e.CommandName.PadRight(20)} -> {e.Details}";
            historyList.Add(entry);
            cmbLogs.DataSource = null;
            cmbLogs.DataSource = historyList;
            cmbLogs.SelectedIndex = cmbLogs.Items.Count - 1;
            lblLogCount.Text = $"Всего транзакций на шине: {historyList.Count}";
        }
        // Макрос 1: Заполнение экрана тестовыми строками
        private void LoadTestPattern(object sender, EventArgs e)
        {
            lcd.Clear();
            lcd.SetCursor(0, 0);
            lcd.WriteString("LCD Emulator 2004");
            lcd.SetCursor(1, 0);
            lcd.WriteString("Built on GDI+ Engine");
            lcd.SetCursor(2, 0);
            lcd.WriteString("Custom Char test: \x00\x00");
            lcd.SetCursor(3, 0);
            lcd.WriteString("System OK. Year 2026");
        }
        // Макрос 2: Управление циклическим сдвигом экрана 
        private void ToggleMarqueeAnimation(object sender, EventArgs e)
        {
            if (animationTimer.Enabled)
            {
                animationTimer.Stop();
                btnMacro2.Text = "Анимация бегущей строки";
                btnMacro2.BackColor = SystemColors.Control;
            }
            else
            {
                lcd.Clear();
                lcd.SetCursor(1, 0);
                lcd.WriteString("<< Бегущая строка >>");
                animationStep = 0;
                animationTimer.Start();
                btnMacro2.Text = "Остановить анимацию";
                btnMacro2.BackColor = Color.LightGreen;
            }
        }
        private void ExecuteAnimationStep(object sender, EventArgs e)
        {
            lcd.CursorOrDisplayShift(true, false);
            animationStep++;
            if (animationStep > 40)
            {
                animationTimer.Stop();
                btnMacro2.Text = "Анимация бегущей строки";
                btnMacro2.BackColor = SystemColors.Control;
                lcd.ReturnHome();
            }
        }
        // Макрос 3: Полный аппаратный сброс регистров и памяти
        private void FactoryResetSimulation(object sender, EventArgs e)
        {
            animationTimer.Stop();
            btnMacro2.Text = "Анимация бегущей строки";
            btnMacro2.BackColor = SystemColors.Control;
            historyList.Clear();
            cmbLogs.DataSource = null;
            lblLogCount.Text = "Всего транзакций на шине: 0";
            lcd.Clear();
            lcd.ReturnHome();
            chkDispOn.Checked = true;
            chkCursorOn.Checked = true;
            chkBlinkOn.Checked = true;
            lcd.DisplayOnOffControl(true, true, true);
            cmbTheme.SelectedIndex = 0;
            LoadDefaultHeartPattern();
        }
    }
}