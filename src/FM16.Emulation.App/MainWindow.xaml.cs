using Fluent;
using System.IO;
using System.Windows;

namespace FM16.Emulation.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        private readonly EmulatedCPU _cpu;
        private byte[] _data;

        public MainWindow()
        {
            InitializeComponent();

            _data = new byte[24] {
                0x0, 0x0, 0x0, 0x0,
                0x0, 0x0, 0x0, 0x0,
                0x0, 0x0, 0x0, 0x0,
                0x0, 0x0, 0x0, 0x0,
                0x0, 0x0, 0x0, 0x0,
                0x0, 0x0, 0x0, 0x0
            };

            _cpu = new EmulatedCPU(null);

            Registers = new CPURegistersFacade(_cpu);
            Stream = new MemoryStream(_data);
        }

        public CPURegistersFacade Registers { get; }

        public MemoryStream Stream { get; }

        private void OnResetCPU(object sender, RoutedEventArgs e)
            => _cpu.Reset();
    }
}
