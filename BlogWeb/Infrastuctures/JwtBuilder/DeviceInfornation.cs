using UAParser;

namespace BlogWeb.Infrastuctures.JwtBuilder
{
    public class DeviceInfornation
    {
        public static string Info(string http)
        {
            var uaParser = Parser.GetDefault();
            ClientInfo info = uaParser.Parse(http);
            var os = info.OS.Family;
            var device = info.UA.Family;
            return $"Device : {device} - OS : {os}";
        }
    }
}
