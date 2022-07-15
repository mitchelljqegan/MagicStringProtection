using Confuser.Core;
using dnlib.DotNet;
using System;
using System.Collections.Generic;

namespace MagicString
{
    internal interface IMagicStringService
    {
        void ExcludeMethod(ConfuserContext context, MethodDef method);
    }

    internal class MagicStringProtection : Protection, IMagicStringService
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
           pipeline.InsertPreStage(PipelineStage.Inspection, new MagicStringPhase(this));
        }
    }
}
