using dnlib.DotNet;
using ICSharpCode.Decompiler.CSharp;
using System.Reflection.Metadata.Ecma335;

namespace TarkovDumper
{
    public sealed class Decompiler
    {
        private readonly CSharpDecompiler _decompiler;

        public Decompiler(CSharpDecompiler decompiler)
        {
            _decompiler = decompiler;
        }

        public readonly struct DecompiledMethod(MethodDef definition, string body)
        {
            public readonly MethodDef Definition = definition;
            public readonly string Body = body;
        }

        /// <summary>
        /// Decompiles all methods inside of a given class.
        /// </summary>
        public IReadOnlyList<DecompiledMethod> DecompileClassMethods(TypeDef type)
        {
            if (type == null)
                return [];

            List<DecompiledMethod> outptut = new();

            foreach (var method in type.Methods)
            {
                var handle = MetadataTokens.EntityHandle((int)method.MDToken.Raw);

                var decompiledMethod = _decompiler.DecompileAsString(handle);

                outptut.Add(new(method, decompiledMethod));
            }

            return outptut;
        }

        /// <summary>
        /// Decompiles a specific method inside of a given class.
        /// </summary>
        public DecompiledMethod DecompileClassMethod(TypeDef type, string methodName)
        {
            if (type == null)
                goto fail;

            var decompiledMethods = DecompileClassMethods(type);

            foreach (var method in decompiledMethods)
            {
                if (method.Definition.Humanize() == methodName)
                    return method;
            }

        fail:
            return new(null, null);
        }
    }
}
