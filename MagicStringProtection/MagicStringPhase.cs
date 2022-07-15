using Confuser.Core;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Linq;

namespace MagicString
{
    class MagicStringPhase : ProtectionPhase
    {
        public MagicStringPhase(MagicStringProtection parent) : base(parent) { }

        public override ProtectionTargets Targets => ProtectionTargets.Modules;

        public override string Name => "Magic string injection";

        protected override void Execute(ConfuserContext context, ProtectionParameters parameters)
        {
            foreach (ModuleDef module in parameters.Targets.OfType<ModuleDef>())
            {
                foreach (TypeDef type in module.GetTypes())
                {
                    foreach (MethodDef method in type.Methods)
                    {
                        method.Body.Instructions.Insert(0, OpCodes.Nop.ToInstruction());

                        context.Logger.InfoFormat("Instruction NOP injected into {0}.", method.ToString());
                    }
                }
            }
        }
    }
}
