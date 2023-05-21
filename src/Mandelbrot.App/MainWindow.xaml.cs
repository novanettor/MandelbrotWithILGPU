using System.Windows;
using System.Windows.Input;

namespace Mandelbrot.App
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                mandelbrot.Reset();
                return;
            }

            if (e.Key == Key.F1)
            {
                mandelbrot.ShowUI = !mandelbrot.ShowUI;
                return;
            }

            if (e.Key == Key.G)
            {
                mandelbrot.UseGpu = !mandelbrot.UseGpu;
                return;
            }

            base.OnKeyDown(e);
        }
    }
}
