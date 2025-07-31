using System.Text;

namespace TarkovDumper
{
    public static class Utilities
    {
        public static string JoinAsString(this List<string> list)
        {
            StringBuilder sb = new();

            for (int i = 0; i < list.Count; i++)
            {
                string type = list[i];

                if (i == list.Count - 1)
                    sb.Append(type);
                else
                    sb.Append(type + ", ");
            }

            return sb.ToString();
        }
    }
}
