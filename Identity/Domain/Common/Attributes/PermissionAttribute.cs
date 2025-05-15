using System.Diagnostics;

namespace Microsoft.AspNetCore.Http
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
    [DebuggerDisplay("{ToString(),nq}")]
    public sealed class PermissionAttribute : Attribute
    {
        public PermissionAttribute(string code)
        {
            this.Code = code;
        }

        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }
    }
}