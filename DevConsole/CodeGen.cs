using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutisticMatt.DevConsole
{
    public class CodeCreator
    {
        private List<PropertyInfo> props;
        private Type classType;
        public CodeCreator(Type classType)
        {
            this.classType = classType;
            GetProps(classType);
        }

        public string GetAll()
        {
            var sb = new StringBuilder();
            sb.AppendLine(ToCSVHeader());
            sb.AppendLine(ToCSV());
            sb.AppendLine(FromCSV());
            return sb.ToString();
        }
        public string ToCSVHeader()
        {
            var sb = new StringBuilder();
            sb.AppendLine("public static string ToCSVHeader()");
            sb.AppendLine("{");
            sb.Append("return \"");
            foreach (var prop in props)
            {
                sb.Append(prop.Name + ",");
            }
            sb.AppendLine("\";");
            sb.AppendLine("}");
            return sb.ToString();
        }
        public string ToCSV()
        {
            var sb = new StringBuilder();
            //Do individual.
            sb.AppendLine("public string ToCSV()");
            sb.AppendLine("{");
            sb.AppendLine("var sb = new StringBuilder();");
            foreach (var prop in props)
            {
                sb.AppendLine($"sb.Append({prop.Name + ".ToString() + \",\""});");
            }
            sb.AppendLine("return sb.ToString();");
            sb.AppendLine("}");

            //Do List.
            sb.AppendLine($"public string ToCSV(List<{classType.Name}> all)");
            sb.AppendLine("{");
            sb.AppendLine("var sb = new StringBuilder();");
            sb.AppendLine($"foreach( var a in all)");
            sb.AppendLine("{");
            sb.AppendLine($"sb.AppendLine(a.ToCSV());");
            sb.AppendLine("}");
            sb.AppendLine("return sb.ToString();");
            sb.AppendLine("}");

            return sb.ToString();
        }
        public string FromCSV()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"public static {classType.Name} FromCSV(string line)");
            sb.AppendLine("{");
            sb.AppendLine("var flds = line.Split(',');");
            sb.AppendLine($"var bh = new {classType.Name}();");
            var cnt = 0;
            foreach (var prop in props)
            {
                sb.Append($"bh.{prop.Name} = ");
                var propName = prop.PropertyType.Name.ToLower();
                if (propName == "datetime") propName = "DateTime";

                var parse = prop.PropertyType == typeof(string) ?
                    $"flds[{cnt}];" :
                    $"{propName}.Parse(flds[{cnt}]);";
                sb.AppendLine(parse);
                cnt++;
            }
            sb.AppendLine("return bh;");
            sb.AppendLine("}");

            //Now do all lines
            sb.AppendLine($"public static List<{classType.Name}> FromCSV(string[] lines)");
            sb.AppendLine("{");
            sb.AppendLine($"var all = new List<{classType.Name}>(lines.Length);");
            sb.AppendLine($"foreach(var line in lines)");
            sb.AppendLine("{");
            sb.AppendLine($"all.Add({classType.Name}.FromCSV(line));");
            sb.AppendLine("}");
            sb.AppendLine("return all;");
            sb.AppendLine("}");



            return sb.ToString();
        }


        private void GetProps(Type yourType)
        {
            props = yourType.GetProperties().ToList().OrderBy(rd => rd.Name).ToList();
            var id = props.Where(rd => rd.Name.ToUpper() == "ID").FirstOrDefault();
            if (id != null)
            {
                props.Remove(id);
                props.Insert(0, id);
            }
        }

    }

}
