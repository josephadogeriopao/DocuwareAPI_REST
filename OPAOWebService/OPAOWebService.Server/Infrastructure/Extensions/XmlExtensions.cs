using System.Xml.Linq;

namespace OPAOWebService.Server.Infrastructure.Extensions
{
    public static class XmlExtensions
    {
        public static List<int> GetCommentNumbers(this IEnumerable<XElement> commentList)
        {
            return commentList
                .Where(x => x.Name == "COMNT")
                .Select(x => x.Element("COMNTNO")?.Value)
                .Where(val => !string.IsNullOrEmpty(val))
                .Select(int.Parse)
                .ToList();
        }
    }
}
