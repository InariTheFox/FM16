namespace FM16.Emulation.Operations.Simple
{
    public class Halt : Operation
    {
        public override bool Execute(EmulatedCPU cpu)
        {
            cpu.Halt(false);

            return true;
        }
    }
}
