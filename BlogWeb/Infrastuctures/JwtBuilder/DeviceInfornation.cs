using UAParser;

namespace BlogWeb.Infrastuctures.JwtBuilder
{
    public class DeviceInfornation
    {
        public static string Info()
        {
            var uaParser = Parser.GetDefault();
            ClientInfo info = uaParser.Parse("Device");
            return $"Device : {info.Device.Family} - OS : {info.OS.Family}";
        }
    }
}
