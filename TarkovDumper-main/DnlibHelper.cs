using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Spectre.Console;

namespace TarkovDumper
{
    public sealed class DnlibHelper
    {
        private readonly ModuleDefMD _module;

        public DnlibHelper(ModuleDefMD module)
        {
            _module = module;
        }

        public enum SearchType
        {
            Field,
            Property,
            Method
        }

        /// <summary>
        /// Finds a class that contains the given entity of a specific type.
        /// </summary>
        public TypeDef FindClassWithEntityName(string entityName, SearchType searchType, bool mustReferenceField = false)
        {
            foreach (TypeDef type in _module.GetTypes())
            {
                switch (searchType)
                {
                    case SearchType.Field:
                        foreach (FieldDef field in type.Fields)
                            if (field.Name == entityName)
                                return type;
                        break;
                    case SearchType.Property:
                        foreach (PropertyDef property in type.Properties)
                            if (property.Name == entityName)
                                return type;
                        break;
                    case SearchType.Method:
                        foreach (MethodDef method in type.Methods)
                            if (method.Name == entityName)
                            {
                                if (mustReferenceField && GetNthFieldReferencedByMethod(method) == null)
                                    continue;

                                return type;
                            }
                        break;
                    default:
                        break;
                }
            }

            return null;
        }

        public readonly struct EntitySearchListEntry(string name, SearchType searchType)
        {
            public readonly string Name = name;
            public readonly SearchType SearchType = searchType;
        }

        /// <summary>
        /// Finds a class that contains all of the supplied entities.
        /// </summary>
        public TypeDef FindClassWithEntities(List<EntitySearchListEntry> entities)
        {
            foreach (TypeDef type in _module.GetTypes())
            {
                int foundCount = 0;

                foreach (EntitySearchListEntry entity in entities)
                {
                    switch (entity.SearchType)
                    {
                        case SearchType.Field:
                            foreach (FieldDef field in type.Fields)
                            {
                                if (field.Name == entity.Name)
                                {
                                    foundCount++;
                                    break;
                                }
                            }
                            break;
                        case SearchType.Property:
                            foreach (PropertyDef property in type.Properties)
                            {
                                if (property.Name == entity.Name)
                                {
                                    foundCount++;
                                    break;
                                }
                            }
                            break;
                        case SearchType.Method:
                            foreach (MethodDef method in type.Methods)
                            {
                                if (method.Name == entity.Name)
                                {
                                    foundCount++;
                                    break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

                if (foundCount == entities.Count)
                    return type;
            }

            return null;
        }

        /// <summary>
        /// Finds an enum by it's type name.
        /// </summary>
        public TypeDef FindEnumByTypeName(string typeName)
        {
            TypeDef tdFind = _module.Find(typeName, true);
            if (tdFind != null)
                return tdFind;

            foreach (TypeDef type in _module.GetTypes())
            {
                string fullName = type.FullName.Replace('/', '.');

                if (fullName == typeName || type.Name == typeName)
                    return type;
            }

            return null;
        }

        /// <summary>
        /// Returns the fields of an enum.
        /// </summary>
        public List<FieldDef> GetEnumValues(TypeDef enumType)
        {
            if (enumType == null || !enumType.IsEnum)
                return null;

            var enumFields = enumType.Fields.Where(f => f.IsLiteral && f.HasConstant).Select(f => f);

            return enumFields.ToList();
        }

        /// <summary>
        /// Finds a class by it's fully qualified name (Namespace+Name in "." format)
        /// </summary>
        public TypeDef FindClassByTypeName(string typeName)
        {
            TypeDef tdFind = _module.Find(typeName, true);
            if (tdFind != null)
                return tdFind;

            foreach (TypeDef type in _module.GetTypes())
            {
                if (type.Humanize() == typeName)
                    return type;
            }

            return null;
        }

        /// <summary>
        /// Finds a method by name inside of a given class.
        /// </summary>
        public MethodDef FindMethodByName(TypeDef type, string methodName)
        {
            if (type == null)
                return null;

            static MethodDef SearchForMethod(TypeDef type, string methodName)
            {
                foreach (var method in type.Methods)
                {
                    const StringComparison sc = StringComparison.OrdinalIgnoreCase;
                    if (method.Humanize().Equals(methodName, sc) ||
                        method.HumanizeAlt().Equals(methodName, sc))
                    {
                        return method;
                    }
                }

                return null;
            }

            var fMethod = SearchForMethod(type, methodName);
            if (fMethod != null)
                return fMethod;

            // Search nested types
            foreach (var nestedType in type.NestedTypes)
            {
                var fMethodNested = SearchForMethod(nestedType, methodName);
                if (fMethodNested != null)
                    return fMethodNested;
            }

            return null;
        }

        /// <summary>
        /// Finds a method that returns a given string (variable).
        /// </summary>
        public MethodDef FindMethodThatReturns(Decompiler decompiler, TypeDef type, string searchValue)
        {
            var decompiledMethods = decompiler.DecompileClassMethods(type);

            foreach (var method in decompiledMethods)
            {
                string returnValueRaw = method.Body.Split('\n').FirstOrDefault(line => line.Trim().StartsWith("return "));
                
                if (returnValueRaw != null)
                {
                    string returnValue = returnValueRaw.Replace(";", "").Split("return ")[1].Trim();

                    if (returnValue.Equals(searchValue, StringComparison.OrdinalIgnoreCase))
                        return method.Definition;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a method that contains a given string (code) in it's method body.
        /// </summary>
        public MethodDef FindMethodThatContains(Decompiler decompiler, TypeDef type, string searchValue, bool logMethodBodies = false)
        {
            var decompiledMethods = decompiler.DecompileClassMethods(type);

            foreach (var method in decompiledMethods)
            {
                if (logMethodBodies)
                    AnsiConsole.WriteLine(method.Body);

                if (method.Body.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                    return method.Definition;
            }

            return null;
        }

        /// <summary>
        /// Extracts the nth field referenced by a given method.
        /// </summary>
        public FieldDef GetNthFieldReferencedByMethod(MethodDef method, int index = 1)
        {
            int fIndex = 1;

            if (method != null && method.HasBody)
            {
                foreach (Instruction instruction in method.Body.Instructions)
                {
                    if (instruction.OpCode == OpCodes.Ldfld ||
                        instruction.OpCode == OpCodes.Stfld ||
                        instruction.OpCode == OpCodes.Ldsfld ||
                        instruction.OpCode == OpCodes.Stsfld)
                    {
                        if (instruction.Operand is FieldDef fieldDef && fIndex == index)
                            return fieldDef;

                        fIndex++;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Extracts the method called by a linq query. (only compatible with obfuscated method names)
        /// </summary>
        public string ExtractLinqCalledMethod(string methodBody)
        {
            if (methodBody == null)
                return null;

            var lines = methodBody.Split('\n');
            foreach (var line in lines)
            {
                if (line.Contains("\\uE", StringComparison.OrdinalIgnoreCase))
                {
                    int startIndex = line.IndexOf("\\uE", StringComparison.OrdinalIgnoreCase);
                    return @"\uE" + line.Substring(startIndex + 3, 3).ToUpper();
                }
            }

            return null;
        }
    }
}
