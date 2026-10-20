using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using EmuDisplayWpf.Models;
namespace EmuDisplayWpf.Controls
{
    public class LcdDisplayControl : FrameworkElement
    {
        public static readonly DependencyProperty LcdCoreProperty =
            DependencyProperty.Register("LcdCore", typeof(Lcd2004Core), typeof(LcdDisplayControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnCoreChanged));
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register("Theme", typeof(LcdColorTheme), typeof(LcdDisplayControl),
                new FrameworkPropertyMetadata(LcdColorTheme.BlueWhite, FrameworkPropertyMetadataOptions.AffectsRender));
        public Lcd2004Core LcdCore
        {
            get => (Lcd2004Core)GetValue(LcdCoreProperty);
            set => SetValue(LcdCoreProperty, value);
        }
        public LcdColorTheme Theme
        {
            get => (LcdColorTheme)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }
        private static void OnCoreChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LcdDisplayControl ctrl && e.NewValue is Lcd2004Core core)
            {
                core.DisplayUpdated += (s, args) => ctrl.InvalidateVisual();
            }
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (LcdCore == null) return;
            Color bgCol = Colors.DarkBlue, fgCol = Colors.White, offCol = Colors.Navy;
            if (Theme == LcdColorTheme.GreenBlack) { bgCol = Color.FromRgb(30, 140, 30); fgCol = Color.FromRgb(5, 20, 5); offCol = Color.FromRgb(35, 125, 35); }
            if (Theme == LcdColorTheme.YellowBlack) { bgCol = Color.FromRgb(180, 170, 10); fgCol = Color.FromRgb(10, 10, 0); offCol = Color.FromRgb(165, 155, 10); }
            var bgBrush = new SolidColorBrush(bgCol);
            var fgBrush = new SolidColorBrush(fgCol);
            var offBrush = new SolidColorBrush(offCol);
            drawingContext.DrawRectangle(new SolidColorBrush(Color.FromRgb(40, 40, 40)), null, new Rect(0, 0, ActualWidth, ActualHeight));
            int borderWidth = 12;
            double displayW = ActualWidth - (borderWidth * 2);
            double displayH = ActualHeight - (borderWidth * 2);
            drawingContext.DrawRectangle(bgBrush, null, new Rect(borderWidth, borderWidth, displayW, displayH));
            if (!LcdCore.DisplayOn) return;
            double cellW = displayW / Lcd2004Core.Columns;
            double cellH = displayH / Lcd2004Core.Rows;
            double paddingX = cellW * 0.08;
            double paddingY = cellH * 0.08;
            var typeface = new Typeface(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
            for (int r = 0; r < Lcd2004Core.Rows; r++)
            {
                for (int c = 0; c < Lcd2004Core.Columns; c++)
                {
                    double x = borderWidth + (c * cellW) + paddingX;
                    double y = borderWidth + (r * cellH) + paddingY;
                    double w = cellW - (paddingX * 2);
                    double h = cellH - (paddingY * 2);
                    char ch = LcdCore.GetCharAt(r, c);
                    if (ch == ' ') { }
                    else if (ch >= 0 && ch < 8)
                    {
                        byte[] pattern = LcdCore.GetCustomChar(ch);
                        double pW = w / 5.0;
                        double pH = h / 8.0;
                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < 5; j++)
                            {
                                bool bit = (pattern[i] & (1 << (4 - j))) != 0;
                                drawingContext.DrawRectangle(bit ? fgBrush : offBrush, null, new Rect(x + (j * pW), y + (i * pH), pW - 0.5, pH - 0.5));
                            }
                        }
                    }
                    else
                    {
                        var formattedText = new FormattedText(ch.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, h * 0.85, fgBrush, VisualTreeHelper.GetDpi(this).PixelsPerDip);
                        drawingContext.DrawText(formattedText, new Point(x + (w - formattedText.Width) / 2, y + (h - formattedText.Height) / 2));
                    }
                    if (r == LcdCore.CursorY && c == LcdCore.CursorX)
                    {
                        if (LcdCore.CursorOn)
                        {
                            var pen = new Pen(fgBrush, 2);
                            drawingContext.DrawLine(pen, new Point(x, y + h), new Point(x + w, y + h));
                        }
                        if (LcdCore.BlinkOn)
                        {
                            drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(140, fgCol.R, fgCol.G, fgCol.B)), null, new Rect(x, y, w, h));
                        }
                    }
                }
            }
        }
    }
}