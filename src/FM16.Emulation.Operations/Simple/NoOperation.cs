namespace FM16.Emulation.Operations.Simple
{
    public class NoOperation : Operation
    {
        public override bool Execute(EmulatedCPU cpu) 
            => true;
    }
}
