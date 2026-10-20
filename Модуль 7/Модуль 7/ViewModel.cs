using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EmuDisplayWpf.Models;
namespace EmuDisplayWpf.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute; _canExecute = canExecute;
        }
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
    public class CgramPixel : INotifyPropertyChanged
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set { _isChecked = value; OnPropertyChanged(); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Lcd2004Core _lcd;
        private System.Timers.Timer _animationTimer;
        private int _animationStep = 0;
        public MainViewModel()
        {
            _lcd = new Lcd2004Core();
            _lcd.CommandExecuted += OnLcdCommand;
            _lcd.DisplayUpdated += (s, e) => OnPropertyChanged(nameof(LcdCore));
            LogHistory = new ObservableCollection<string>();
            CgramGrid = new ObservableCollection<CgramPixel>();
            for (int i = 0; i < 40; i++) CgramGrid.Add(new CgramPixel());
            WriteTextCommand = new RelayCommand(_ => _lcd.WriteString(InputText));
            ClearCommand = new RelayCommand(_ => _lcd.Clear());
            HomeCommand = new RelayCommand(_ => _lcd.ReturnHome());
            SetCursorCommand = new RelayCommand(_ => _lcd.SetCursor(CursorRow, CursorCol));
            ShiftLeftCommand = new RelayCommand(_ => _lcd.CursorOrDisplayShift(true, false));
            ShiftRightCommand = new RelayCommand(_ => _lcd.CursorOrDisplayShift(true, true));
            ClearGridCommand = new RelayCommand(_ => ClearCgramGrid());
            SaveCharCommand = new RelayCommand(_ => SaveCustomCharacter());
            InsertCharCommand = new RelayCommand(_ => _lcd.WriteChar((char)CgramIndex));
            Macro1Command = new RelayCommand(_ => LoadTestPattern());
            Macro2Command = new RelayCommand(_ => ToggleMarqueeAnimation());
            Macro3Command = new RelayCommand(_ => FactoryReset());
            TogglePixelCommand = new RelayCommand(param =>
            {
                if (param is CgramPixel pixel)
                {
                    pixel.IsChecked = !pixel.IsChecked;
                }
            });
            LoadDefaultHeartPattern();
        }
        public Lcd2004Core LcdCore => _lcd;
        public ObservableCollection<CgramPixel> CgramGrid { get; }
        public ObservableCollection<string> LogHistory { get; }
        private string _inputText = "Hello World!";
        public string InputText { get => _inputText; set { _inputText = value; OnPropertyChanged(); } }
        private int _cursorRow;
        public int CursorRow { get => _cursorRow; set { _cursorRow = value; OnPropertyChanged(); } }
        private int _cursorCol;
        public int CursorCol { get => _cursorCol; set { _cursorCol = value; OnPropertyChanged(); } }
        private int _cgramIndex;
        public int CgramIndex { get => _cgramIndex; set { _cgramIndex = value; OnPropertyChanged(); } }
        private LcdColorTheme _currentTheme;
        public LcdColorTheme CurrentTheme
        {
            get => _currentTheme;
            set { _currentTheme = value; OnPropertyChanged(); }
        }
        private bool _chkDispOn = true;
        public bool ChkDispOn { get => _chkDispOn; set { _chkDispOn = value; OnPropertyChanged(); UpdateHardwareFlags(); } }
        private bool _chkCursorOn = true;
        public bool ChkCursorOn { get => _chkCursorOn; set { _chkCursorOn = value; OnPropertyChanged(); UpdateHardwareFlags(); } }
        private bool _chkBlinkOn = true;
        public bool ChkBlinkOn { get => _chkBlinkOn; set { _chkBlinkOn = value; OnPropertyChanged(); UpdateHardwareFlags(); } }
        private string _macro2Text = "Анимация бегущей строки";
        public string Macro2Text { get => _macro2Text; set { _macro2Text = value; OnPropertyChanged(); } }
        private string _logCountText = "Всего транзакций на шине: 0";
        public string LogCountText { get => _logCountText; set { _logCountText = value; OnPropertyChanged(); } }
        public ICommand WriteTextCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand HomeCommand { get; }
        public ICommand SetCursorCommand { get; }
        public ICommand ShiftLeftCommand { get; }
        public ICommand ShiftRightCommand { get; }
        public ICommand ClearGridCommand { get; }
        public ICommand SaveCharCommand { get; }
        public ICommand InsertCharCommand { get; }
        public ICommand Macro1Command { get; }
        public ICommand Macro2Command { get; }
        public ICommand Macro3Command { get; }
        public ICommand TogglePixelCommand { get; }
        private void UpdateHardwareFlags() => _lcd.DisplayOnOffControl(ChkDispOn, ChkCursorOn, ChkBlinkOn);
        private void OnLcdCommand(object sender, LcdCommandEventArgs e)
        {
            string entry = $"[{e.Timestamp:HH:mm:ss.fff}] {e.CommandName.PadRight(20)} -> {e.Details}";
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                LogHistory.Add(entry);
                LogCountText = $"Всего транзакций на шине: {LogHistory.Count}";
                if (LogHistory.Count > 100) LogHistory.RemoveAt(0);
            });
        }
        private void ClearCgramGrid()
        {
            foreach (var pixel in CgramGrid) pixel.IsChecked = false;
        }
        private void SaveCustomCharacter()
        {
            byte[] pattern = new byte[8];
            for (int row = 0; row < 8; row++)
            {
                byte rowByte = 0;
                for (int col = 0; col < 5; col++)
                {
                    int index = (row * 5) + col;
                    if (CgramGrid[index].IsChecked)
                    {
                        rowByte |= (byte)(1 << (4 - col));
                    }
                }
                pattern[row] = rowByte;
            }
            _lcd.StoreCustomChar(CgramIndex, pattern);
            System.Windows.MessageBox.Show($"Символ сохранен в ячейку {CgramIndex}", "Успех");
        }
        private void LoadTestPattern()
        {
            _lcd.Clear();
            _lcd.SetCursor(0, 0); _lcd.WriteString("LCD Emulator 2004");
            _lcd.SetCursor(1, 0); _lcd.WriteString("Built on WPF Engine");
            _lcd.SetCursor(2, 0); _lcd.WriteString("Custom Char test: \x00\x00");
            _lcd.SetCursor(3, 0); _lcd.WriteString("System OK. Year 2026");
        }
        private void ToggleMarqueeAnimation()
        {
            if (_animationTimer != null && _animationTimer.Enabled)
            {
                _animationTimer.Stop();
                Macro2Text = "Анимация бегущей строки";
            }
            else
            {
                _lcd.Clear();
                _lcd.SetCursor(1, 0); _lcd.WriteString("<< Бегущая строка >>");
                _animationStep = 0;
                _animationTimer = new System.Timers.Timer(250);
                _animationTimer.Elapsed += (s, e) =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        _lcd.CursorOrDisplayShift(true, false);
                        _animationStep++;
                        if (_animationStep > 40)
                        {
                            _animationTimer.Stop();
                            Macro2Text = "Анимация бегущей строки";
                            _lcd.ReturnHome();
                        }
                    });
                };
                _animationTimer.Start();
                Macro2Text = "Остановить анимацию";
            }
        }
        private void FactoryReset()
        {
            if (_animationTimer != null) _animationTimer.Stop();
            Macro2Text = "Анимация бегущей строки";
            LogHistory.Clear();
            _lcd.Clear();
            _lcd.ReturnHome();
            ChkDispOn = true; ChkCursorOn = true; ChkBlinkOn = true;
            CurrentTheme = LcdColorTheme.BlueWhite;
            LoadDefaultHeartPattern();
        }
        private void LoadDefaultHeartPattern()
        {
            byte[] heart = { 0x00, 0x0A, 0x1F, 0x1F, 0x1F, 0x0E, 0x04, 0x00 };
            _lcd.StoreCustomChar(0, heart);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}