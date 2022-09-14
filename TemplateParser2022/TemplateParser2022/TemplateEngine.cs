using System.Reflection;
using System.Xml.Linq;

namespace TemplateParser2022
{
    public class TemplateEngine : ITemplateEngine
    {
        /// <summary>
        /// Applies the specified datasource to a string template, and returns a result string
        /// with substituted values.
        /// </summary>
        /// 

        private static object GetPropertyValue(object srcobj, string propertyName)
        {
            if (srcobj == null)
                return null;

            object obj = srcobj;

            string[] propertyNameParts = propertyName.Split('.');

            foreach (string propertyNamePart in propertyNameParts)
            {
                if (obj == null) return null;

                if (!propertyNamePart.Contains("["))
                {
                    PropertyInfo pi = obj.GetType().GetProperty(propertyNamePart);
                    if (pi == null) return null;
                    obj = pi.GetValue(obj, null);
                }
            }

            return obj;
        }

        public string Apply(string template, object dataSource)
        {
            //TODO: Write your implementation here that passes all tests in TemplateParser.Test project            
            
            //var start = template.IndexOf("[");
            //var end = template.IndexOf("]");
            //if (start == -1 || end == -1)
            //{
            //    return template;
            //}
            //else
            //{
            //    var prop = template.Substring(start + 1, end - start - 1);
            //    var staticString = template.Substring(0, start);  
            //    return String.Concat(staticString, (string)dataSource.GetType().GetProperty(prop).GetValue(dataSource));
            //}

            string[] strings = template.Split(" ");
            var returnString = "";
            if (strings.Length > 0)
            {
                foreach (string s in strings)
                {
                    if (s.IndexOf("[") == -1)
                    {
                        returnString = String.Concat(returnString, " ", s);
                    }
                    else
                    {
                        var prop = s.Substring(1, s.Length - 2);
                 
                        var obj = GetPropertyValue(dataSource, prop);
                        var substituteString = (string)obj;
                        returnString = String.Concat(returnString, " ", substituteString);
                    }
                }
            }
            if (returnString.IndexOf(" ") == 0)
            {
                return returnString.Substring(1, returnString.Length-1);
            }
            return returnString;

            // 4 out of 7 unit tests passed. 
            // Haven't thought about how to deal with ""With" scoped property substitute
            // Still need a bit research on how to work with Date/DateTime, this is always tricky.
        }
    }
}
