using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mandelbrot.App.Palettes;

namespace Mandelbrot.App
{
    public partial class MandelbrotControl : UserControl, INotifyPropertyChanged
    {
        private MandelbrotRenderer _renderer = new MandelbrotRenderer();

        bool _showUI;
        ComplexDouble _center;
        double _step;
        int _depth;
        int[]? _palette;
        TimeSpan _renderTime;

        #region Dependency properties

        public bool ShowUI
        {
            get => _showUI;
            set
            {
                if (_showUI != value)
                {
                    _showUI = value;
                    OnPropertyChanged(nameof(ShowUI));
                }
            }
        }

        public ComplexDouble Center
        {
            get => _center;
            set
            {
                if (_center != value)
                {
                    _center = value;
                    OnPropertyChanged(nameof(Center));
                }
            }
        }

        public double Step
        {
            get => _step;
            set
            {
                if (_step != value)
                {
                    _step = value;
                    OnPropertyChanged(nameof(Step));
                }
            }
        }

        public int Depth
        {
            get => _depth;
            set
            {
                if (_depth != value)
                {
                    _depth = value;
                    OnPropertyChanged(nameof(Depth));
                }
            }
        }

        public int[] Palette
        {
            get => _palette!;
            set
            {
                if (_palette != value)
                {
                    _palette = value;
                    OnPropertyChanged(nameof(Palette));
                }
            }
        }

        public bool UseGpu
        {
            get => _renderer.UseGpu;
            set
            {
                if (_renderer.UseGpu != value)
                {
                    _renderer.UseGpu = value;
                    OnPropertyChanged(nameof(UseGpu));
                }
            }
        }

        public TimeSpan RenderTime
        {
            get => _renderTime;
            private set
            {
                if (value != _renderTime)
                {
                    _renderTime = value;
                    OnPropertyChanged(nameof(RenderTime));
                    OnPropertyChanged(nameof(RenderTimeMs));
                }
            }
        }

        public int RenderTimeMs
        {
            get => Convert.ToInt32(RenderTime.TotalMilliseconds);
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        public MandelbrotControl()
        {
            InitializeComponent();
            DataContext = this;

            PropertyChanged += MandelbrotControl_PropertyChanged;
            PaletteGeneratorCombo.SelectionChanged += PaletteGeneratorCombo_SelectionChanged;

            SetDefaults();

            Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        private void PaletteGeneratorCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Palette = ((IPaletteGenerator)PaletteGeneratorCombo.SelectedItem).GeneratePalette(_depth);
            RenderMandelbrotImage();
        }

        private void MandelbrotControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Depth":
                    Palette = ((IPaletteGenerator)PaletteGeneratorCombo.SelectedItem).GeneratePalette(_depth);
                    break;
            }
        }

        public void Reset()
        {
            SetDefaults();
            RenderMandelbrotImage();
        }

        private void RenderMandelbrotImage()
        {
            var start = DateTime.Now;

            backgroundBrush.ImageSource = _renderer.GenerateImage(
                (int)ActualWidth,
                (int)ActualHeight,
                Center,
                Step,
                Palette);

            RenderTime = DateTime.Now - start;
        }

        private void SetDefaults()
        {
            ShowUI = true;
            Depth = 100;
            Step = 0.003;
            Center = new ComplexDouble(-0.5, 0);
            PaletteGeneratorCombo.SelectedIndex = 2;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);

            Center += new ComplexDouble((position.X - ActualWidth / 2) * Step, -(position.Y - ActualHeight / 2) * Step);

            RenderMandelbrotImage();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                if (Depth > 50 || e.Delta > 0)
                {
                    Depth += e.Delta > 0 ? 25 : -25;
                }
            }
            else
            {
                if (e.Delta < 0)
                    Step *= 1.2;
                else
                    Step /= 1.2;
            }

            RenderMandelbrotImage();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            RenderMandelbrotImage();
        }

        private void Dispatcher_ShutdownStarted(object? sender, EventArgs e)
        {
            _renderer.Dispose();
        }
    }
}
