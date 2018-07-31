using SqlCrawler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCrawler
{
    class MarkdownFormatter
    {
        private StringBuilder _sb;

        public MarkdownFormatter(string database, SqlCrawler.Model.Object obj)
        {
            _sb = new StringBuilder();

            _sb.AppendFormat("# [{0}]({1})\r\n", obj.Name, "../../../" + database + "/" + obj.Name.Substring(0, 4) + "/" + obj.Name + "/" + obj.Name + ".sql");          
            
            if (obj.Parameters != null && obj.Parameters.Any())
            {
                _sb.Append("## Parameters\r\n");
                _sb.AppendLine("|Name|Type (Size, Precision, Scale)|Notes|");
                _sb.AppendLine("|:---|:---|:---|");
                foreach (var parameter in obj.Parameters.OrderBy(p => p.Order))
                {
                    _sb.AppendFormat("|{0}|{1} ({2}, {3}, {4})||\r\n",                      
                        parameter.Name,
                        parameter.Type,
                        parameter.Size,
                        parameter.Precision,
                        parameter.Scale);
                }    
            }
            else if (obj.Columns != null && obj.Columns.Any())
            {
                _sb.Append("## Columns\r\n");
                _sb.AppendLine("|Name|Type (Size, Precision, Scale)|Notes|");
                _sb.AppendLine("|:---|:---|:---|");
                foreach (var column in obj.Columns.OrderBy(c => c.Order))
                {
                    _sb.AppendFormat("|{0}|{1} ({2}, {3}, {4})|...|\r\n",
                        column.Name,
                        column.Type,
                        column.Size,
                        column.Precision,
                        column.Scale);
                }
            }

            _sb.AppendLine();
            _sb.AppendLine("## Description");
            _sb.AppendLine("- ");
            _sb.AppendLine();
            _sb.AppendLine("## Examples");
            _sb.AppendLine("```sql");
            _sb.AppendLine("-- " + obj.Name + " examples");
            _sb.AppendLine("```");
            _sb.AppendLine();
            _sb.AppendLine("## References");

            foreach (var or in obj.ObjectReferences)
            {
                _sb.AppendFormat("- [{0}]({1})\r\n", 
                    or.Reference,
                    "../../../" + (string.IsNullOrWhiteSpace(or.ReferenceDatabase) ? database : or.ReferenceDatabase ) 
                    + "/" + (or.Reference.Length > 4 ? or.Reference.Substring(0, 4) : or.Reference) 
                    + "/" + or.Reference + "/" + or.Reference + ".md");
            }

            _sb.AppendLine("## Notes");
            _sb.AppendLine("- ");
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}
