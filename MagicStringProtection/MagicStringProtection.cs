using Confuser.Core;
using dnlib.DotNet;

namespace MagicString
{
    public interface IMagicStringService
    {
        void ExcludeMethod(ConfuserContext context, MethodDef method);
    }

    class MagicStringProtection : Protection, IMagicStringService
    {

        public override ProtectionPreset Preset => ProtectionPreset.None;

        public override string Name => "Magic String";

        public override string Description => "Injects a string into the start of every method which throws an exception.";

        public override string Id => "magic string";

        public override string FullId => "mitchelljqegan.magicstring";

        public void ExcludeMethod(ConfuserContext context, MethodDef method)
        {
            ProtectionParameters.GetParameters(context, method).Remove(this);
        }

        protected override void Initialize(ConfuserContext context)
        {
            context.Registry.RegisterService(FullId, typeof(IMagicStringService), this);
        }

        protected override void PopulatePipeline(ProtectionPipeline pipeline)
        {
            throw new System.NotImplementedException();
        }
    }

    class MagicStringPhase : ProtectionPhase
    {
        public MagicStringPhase(MagicStringProtection parent) : base(parent) { }

        public override ProtectionTargets Targets => throw new System.NotImplementedException();

        public override string Name => throw new System.NotImplementedException();

        protected override void Execute(ConfuserContext context, ProtectionParameters parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}
