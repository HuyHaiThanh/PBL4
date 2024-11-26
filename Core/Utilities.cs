using Newtonsoft.Json;
using System.IO;

namespace Core
{
    public class Utilities
    {
        private static readonly string FileName = "settings.json";

        public static void SaveToJson(SettingInfo obj)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), FileName);

            string json = JsonConvert.SerializeObject(obj);

            File.WriteAllText(filePath, json);
        }

        public static SettingInfo LoadFromJson()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), FileName);

            if (!File.Exists(filePath))
            {
                return null; 
            }
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<SettingInfo>(json);
        }
        public static string FileDetailToJson(FileDetail fo)
        {
            return JsonConvert.SerializeObject(fo);
        }
        public static FileDetail JsonToFileDetail(string json)
        {
            return JsonConvert.DeserializeObject<FileDetail>(json);
        }
    }
}
