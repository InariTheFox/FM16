using FM16.Common;
using System.Collections.Generic;

namespace FM16.Emulation
{
    public class EmulatedMemory : IMemory
    {
        private readonly IList<byte> _map;

        private readonly EmulatedBus _bus;
        private readonly EmulatedCPU _cpu;
        private readonly uint _endAddress;
        private readonly bool _isReadOnly;
        private readonly uint _startAddress;

        private bool _isLatched;
        private uint _latchedAddress;

        public EmulatedMemory(
            EmulatedBus bus,
            EmulatedCPU cpu,
            uint startAddress,
            uint endAddress,
            bool isReadOnly = false)
        {
            _bus = bus;
            _cpu = cpu;
            _endAddress = endAddress;
            _isReadOnly = isReadOnly;
            _startAddress = startAddress;

            _map = new List<byte>();

            for (var i = 0; i < endAddress - startAddress; i++)
            {
                _map[i] = 0x00;
            }
        }

        public void Clock()
        {
            if (!_isLatched && _cpu.AddressLatchEnable)
            {
                _isLatched = true;
                _latchedAddress = _bus.ReadDouble();
            }
            else if (_isLatched && _cpu.Read && _cpu.DataEnable)
            {
                _cpu.Ready = false;
            }
            else if (_isLatched && !_cpu.Ready)
            {
                if (_cpu.BusHighEnable)
                {
                    _bus.WriteWord((ushort)(_map[(int)_latchedAddress] << 8));
                }
                else
                {
                    _bus.WriteByte(_map[(int)_latchedAddress]);
                }

                _cpu.Ready = true;
                _isLatched = false;
            }
        }
    }
}
