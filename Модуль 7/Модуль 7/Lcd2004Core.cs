using System;
namespace EmuDisplayWpf.Models
{
    public class Lcd2004Core
    {
        public const int Columns = 20;
        public const int Rows = 4;
        private char[,] ddram = new char[Rows, Columns];
        private byte[][] cgram = new byte[8][];
        private int cursorX = 0;
        private int cursorY = 0;
        private bool displayOn = true;
        private bool cursorOn = true;
        private bool blinkOn = true;
        private bool shiftIncrement = true;
        private bool displayShift = false;
        private int displayOffset = 0;
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
        private void OnCommand(string name, string details)
        {
            CommandExecuted?.Invoke(this, new LcdCommandEventArgs(name, details));
            DisplayUpdated?.Invoke(this, EventArgs.Empty);
        }
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
        public void Clear()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    ddram[r, c] = ' ';
            cursorX = 0; cursorY = 0; displayOffset = 0;
            OnCommand("Clear Display", "DDRAM заполнена пробелами, курсор в (0,0)");
        }
        public void ReturnHome()
        {
            cursorX = 0; cursorY = 0; displayOffset = 0;
            OnCommand("Return Home", "Курсор возвращен в (0,0), сдвиг сброшен");
        }
        public void DisplayOnOffControl(bool display, bool cursor, bool blink)
        {
            displayOn = display; cursorOn = cursor; blinkOn = blink;
            OnCommand("Display On/Off", $"Экран: {display}, Курсор: {cursor}, Мигание: {blink}");
        }
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
                if (shiftRight) MoveCursorRight(); else MoveCursorLeft();
                OnCommand("Shift Cursor", "Курсор сдвинут " + (shiftRight ? "вправо" : "влево"));
            }
        }
        public void SetCursor(int row, int col)
        {
            if (row >= 0 && row < Rows) cursorY = row;
            if (col >= 0 && col < Columns) cursorX = col;
            OnCommand("Set Cursor Pos", $"Строка: {cursorY}, Столбец: {cursorX}");
        }
        public void StoreCustomChar(int index, byte[] pattern)
        {
            if (index >= 0 && index < 8 && pattern != null && pattern.Length == 8)
            {
                for (int i = 0; i < 8; i++) cgram[index][i] = (byte)(pattern[i] & 0x1F);
                OnCommand("CGRAM Write", $"Создан кастомный символ в ячейке {index}");
            }
        }
        public void WriteChar(char c)
        {
            if (!displayOn) return;
            ddram[cursorY, cursorX] = c;
            if (displayShift)
            {
                if (shiftIncrement) displayOffset = (displayOffset + 1) % Columns;
                else displayOffset = (displayOffset - 1 + Columns) % Columns;
            }
            if (shiftIncrement) MoveCursorRight(); else MoveCursorLeft();
            DisplayUpdated?.Invoke(this, EventArgs.Empty);
        }
        public void WriteString(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            foreach (char c in text)
            {
                if (c == '\n') { cursorX = 0; cursorY = (cursorY + 1) % Rows; }
                else if (c == '\r') { cursorX = 0; }
                else { WriteChar(c); }
            }
            OnCommand("Write String", $"Выведена строка длиной {text.Length} симв.");
        }
        private void MoveCursorRight()
        {
            cursorX++;
            if (cursorX >= Columns) { cursorX = 0; cursorY++; if (cursorY >= Rows) cursorY = 0; }
        }
        private void MoveCursorLeft()
        {
            cursorX--;
            if (cursorX < 0) { cursorX = Columns - 1; cursorY--; if (cursorY < 0) cursorY = Rows - 1; }
        }
    }
}