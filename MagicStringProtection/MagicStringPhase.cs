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
                TypeRefUser systemStringClass = new TypeRefUser(module, "System", "String", module.CorLibTypes.AssemblyRef);
                TypeRefUser systemExceptionClass = new TypeRefUser(module, "System", "Exception", module.CorLibTypes.AssemblyRef);

                MemberRefUser stringStartsWithMethod = new MemberRefUser(module, "StartsWith", MethodSig.CreateInstance(module.CorLibTypes.Boolean, module.CorLibTypes.String), systemStringClass);
                MemberRefUser exceptionConstructor = new MemberRefUser(module, ".ctor", MethodSig.CreateInstance(module.CorLibTypes.Void), systemExceptionClass);

                foreach (TypeDef type in module.GetTypes())
                {
                    foreach (MethodDef method in type.Methods)
                    {
                        context.CheckCancellation();

                        if (method.HasBody)
                        {
                            method.Body.Instructions.Insert(0, OpCodes.Ldstr.ToInstruction("magic string"));
                            method.Body.Instructions.Insert(1, OpCodes.Ldstr.ToInstruction("magic"));
                            method.Body.Instructions.Insert(2, OpCodes.Call.ToInstruction(stringStartsWithMethod));
                            method.Body.Instructions.Insert(3, OpCodes.Brfalse_S.ToInstruction(method.Body.Instructions[3]));
                            method.Body.Instructions.Insert(4, OpCodes.Newobj.ToInstruction(exceptionConstructor));
                            method.Body.Instructions.Insert(5, OpCodes.Throw.ToInstruction());

                            method.Body.UpdateInstructionOffsets();
                        }
                    }
                }
            }
        }
    }
}
