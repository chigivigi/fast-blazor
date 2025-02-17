﻿using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CustomElementsParser;

public class Program
{
    public static void Main()
    {
        const string appPath = "C:\\Source\\Blazor\\fast-blazor\\src\\CustomElementsParser\\";

        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,  // set camelCase       
            WriteIndented = true                                // write pretty json
        };


        string fileName = "custom-elements.json";
        string jsonString = File.ReadAllText(Path.Combine(appPath, fileName));

        CustomElements customElements = JsonSerializer.Deserialize<CustomElements>(jsonString, options)!;

        StringBuilder classBuilder = new();
        StringBuilder elementBuilder = new();

        foreach (Module m in customElements.modules)
        {
            foreach (Declaration d in m.declarations)
            {
                if (d.kind != "variable" && d.superclass != null && (d.superclass.name == "FASTElement" || d.superclass.name == "FASTAnchor" || d.superclass.name.StartsWith("FormAssociated")))
                {
                    bool writeClassFile = true;
                    string name;
                    if (d.name.StartsWith("FAST"))
                        name = d.name[4..];
                    else
                        name = d.name;

                    classBuilder.AppendLine("using Microsoft.AspNetCore.Components;\n");
                    classBuilder.AppendLine("namespace Microsoft.Fast.Components.FluentUI;\n");

                    string classname = $"Fluent{name}";
                    classBuilder.AppendLine($"public partial class {classname} : FluentComponentBase");

                    classBuilder.AppendLine("{");

                    name = Regex.Replace(name, @"([A-Z])", "-$1");

                    elementBuilder.AppendLine("@inherits FluentComponentBase\n");

                    elementBuilder.AppendLine($"<fluent{name.ToLower()} ");

                    if (d.attributes is not null)
                    {
                        foreach (Attribute a in d.attributes)
                        {
                            string attName;

                            if (!string.IsNullOrEmpty(a.name))
                                attName = a.name;
                            else
                                attName = a.fieldName;


                            bool addToDesc = false;

                            string composedName = attName[0].ToString().ToUpper() + attName[1..];

                            composedName = Regex.Replace(composedName, @"(-[a-z])", m => m.ToString()[1].ToString().ToUpper());
                            string t = a.type.text;
                            if (t == "boolean") t = "bool";
                            if (t == "number") t = "int";
                            if (t.Contains('"') || t.Contains('|'))
                            {
                                t = "string";
                                addToDesc = true;
                            }

                            classBuilder.AppendLine("\t/// <summary>");
                            string desc = a.description.Replace("\n", "\n\t/// ");
                            if (addToDesc) desc += $"\n\t/// Possible values: {a.type.text}";
                            classBuilder.AppendLine($"\t/// {desc}");
                            classBuilder.AppendLine("\t/// </summary>");
                            classBuilder.AppendLine("\t[Parameter]");


                            classBuilder.AppendLine($"\tpublic {t}? {composedName} {{ get; set; }}\n");

                            elementBuilder.AppendLine($"\t{attName}=@{composedName}");

                        }
                        if (d.superclass.name.StartsWith("FormAssociated"))
                        {
                            classBuilder.AppendLine("\t/// <summary>");
                            classBuilder.AppendLine($"\t/// The id attribute of the element.Used for label association.");
                            classBuilder.AppendLine("\t/// </summary>");
                            classBuilder.AppendLine("\t[Parameter]");
                            classBuilder.AppendLine($"\tpublic string? Id {{ get; set; }}\n");
                            elementBuilder.AppendLine($"\tid=@Id");

                            classBuilder.AppendLine("\t/// <summary>");
                            classBuilder.AppendLine($"\t/// The value of the element");
                            classBuilder.AppendLine("\t/// </summary>");
                            classBuilder.AppendLine("\t[Parameter]");
                            classBuilder.AppendLine($"\tpublic string? Value {{ get; set; }}\n");
                            elementBuilder.AppendLine($"\tvalue=@Value");

                            classBuilder.AppendLine("\t/// <summary>");
                            classBuilder.AppendLine($"\t/// The element's current value ");
                            classBuilder.AppendLine("\t/// </summary>");
                            classBuilder.AppendLine("\t[Parameter]");
                            classBuilder.AppendLine($"\tpublic string? CurrentValue {{ get; set; }}\n");
                            elementBuilder.AppendLine($"\tcurrent-value=@CurrentValue");

                            classBuilder.AppendLine("\t/// <summary>");
                            classBuilder.AppendLine($"\t/// Disables the form control, ensuring it doesn't participate in form submission");
                            classBuilder.AppendLine("\t/// </summary>");
                            classBuilder.AppendLine("\t[Parameter]");
                            classBuilder.AppendLine($"\tpublic bool Disabled {{ get; set; }}\n");
                            elementBuilder.AppendLine($"\tdisabled=@Disabled");

                            classBuilder.AppendLine("\t/// <summary>");
                            classBuilder.AppendLine($"\t/// The name of the element.Allows access by name from the associated form.");
                            classBuilder.AppendLine("\t/// </summary>");
                            classBuilder.AppendLine("\t[Parameter]");
                            classBuilder.AppendLine($"\tpublic string? Name {{ get; set; }}\n");
                            elementBuilder.AppendLine($"\tname=@Name");

                            classBuilder.AppendLine("\t/// <summary>");
                            classBuilder.AppendLine($"\t/// The element needs to have a value");
                            classBuilder.AppendLine("\t/// </summary>");
                            classBuilder.AppendLine("\t[Parameter]");
                            classBuilder.AppendLine($"\tpublic bool Required {{ get; set; }}\n");
                            elementBuilder.AppendLine($"\trequired=@Required");



                        }
                    }
                    else
                    {
                        writeClassFile = false;
                    }
                    classBuilder.AppendLine("}\n");
                    elementBuilder.AppendLine($">\n\t@ChildContent\n</fluent{name.ToLower()}>");

                    if (writeClassFile)
                        File.WriteAllText(Path.Combine(appPath, "GeneratedClasses", classname) + ".razor.cs", classBuilder.ToString());

                    classBuilder.Clear();

                    File.WriteAllText(Path.Combine(appPath, "GeneratedClasses", classname) + ".razor", elementBuilder.ToString());
                    elementBuilder.Clear();

                }

            }

        }
    }
}
