using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mandelbrot.App
{
    /// <summary>
    /// Interaction logic for MandelbrotControl.xaml
    /// </summary>
    public partial class MandelbrotControl : UserControl
    {
        private MandelbrotRenderer _renderer = new MandelbrotRenderer();
        private ComplexDouble _center = new ComplexDouble();

        public MandelbrotControl()
        {
            InitializeComponent();
            DataContext = this;

            Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        private void RenderMandelbrotImage()
        {
            backgroundBrush.ImageSource = _renderer.GenerateImage(
                (int)ActualWidth,
                (int)ActualHeight,
                new ComplexDouble(-0.5, 0),
                0.003,
                GradientPaletteGenerator.GeneratePalette(100));
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
