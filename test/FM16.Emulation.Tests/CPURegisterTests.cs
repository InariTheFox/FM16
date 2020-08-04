using FM16.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FM16.Emulation.Tests
{
    [TestClass]
    public class CPURegisterTests
    {
        private EmulatedCPU _cpu;

        [TestInitialize]
        public void Setup()
        {
            _cpu = new EmulatedCPU(null);
        }

        [TestMethod]
        public void ByteRegisterOverflowTest()
        {
            _cpu.SetRegister(ByteRegisters.CL, 0x100);
            var value = _cpu.GetRegister(ByteRegisters.CL);

            Assert.AreNotEqual(0x100, value);
            Assert.AreEqual(0x00, value);
        }

        [TestMethod]
        public void HighByteAssignmentTest()
        {
            _cpu.SetRegister(WordRegisters.DX, 0xEDED);
            _cpu.SetRegister(ByteRegisters.DH, 0xAB);

            var highByte = _cpu.GetRegister(ByteRegisters.DH);
            var lowByte = _cpu.GetRegister(ByteRegisters.DL);

            Assert.AreEqual(0xAB, highByte);
            Assert.AreEqual(0xED, lowByte);
        }

        [TestMethod]
        public void LowByteAssignmentTest()
        {
            _cpu.SetRegister(WordRegisters.DX, 0x4545);
            _cpu.SetRegister(ByteRegisters.DL, 0x12);

            var highByte = _cpu.GetRegister(ByteRegisters.DH);
            var lowByte = _cpu.GetRegister(ByteRegisters.DL);

            Assert.AreEqual(0x45, highByte);
            Assert.AreEqual(0x12, lowByte);
        }

        [TestMethod]
        public void NegativeIntegerHighByteTest()
        {
            _cpu.SetRegister(ByteRegisters.AH, -1);
            var value = _cpu.GetRegister(ByteRegisters.AH);

            Assert.AreEqual(0xFF, value);
        }

        [TestMethod]
        public void NegativeIntegerLowByteTest()
        {
            _cpu.SetRegister(ByteRegisters.AL, -1);
            var value = _cpu.GetRegister(ByteRegisters.AL);

            Assert.AreEqual(0xFF, value);
        }

        [TestMethod]
        public void NegativeIntegerWordTest()
        {
            _cpu.SetRegister(WordRegisters.AX, -1);
            var value = _cpu.GetRegister(WordRegisters.AX);

            Assert.AreEqual(0xFFFF, value);
        }

        [TestMethod]
        public void SplitByteRegistersTest()
        {
            _cpu.SetRegister(WordRegisters.BX, 0x903A);
            var lowByte = _cpu.GetRegister(ByteRegisters.BL);
            var highByte = _cpu.GetRegister(ByteRegisters.BH);

            Assert.AreEqual(0x3A, lowByte);
            Assert.AreEqual(0x90, highByte);
        }
    }
}
