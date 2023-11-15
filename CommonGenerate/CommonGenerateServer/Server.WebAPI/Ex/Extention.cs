using System.Text;

namespace Server.WebAPI.Ex
{
    public static class Extention
    {

        public static byte[] ToBytes(this string str)
        {
            return Encoding.Default.GetBytes(str);
        }

        public static byte[] ToBytes(this string str, Encoding theEncoding)
        {
            return theEncoding.GetBytes(str);
        }
    }
}
