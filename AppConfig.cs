using System.IO;
using System.Reflection;
using System.Text.Json;

namespace AppConfigFileRw;

/// <summary>
/// 此类用于解析对应的JSON配置文件
/// 只有公共属性(具有公开set方法)会被 JSON 解析和序列化
/// </summary>
public class AppConfig
{
    // 单例
    private static AppConfig? instance;

    /// <summary>
    /// 获取应用配置
    /// </summary>
    /// <param name="filePath">JSON文件路径</param>
    /// <returns></returns>
    public static AppConfig GetAppConfig(string filePath = "./app_config.json")
    {
        bool checkErr = true;

        do
        {
            if (instance != null)
            {
                checkErr = false;
                break;
            }
            if (!File.Exists(filePath))
            {
                break;
            }

            string json = File.ReadAllText(filePath);
            instance = JsonSerializer.Deserialize<AppConfig>(json);
            if (instance == null)
            {
                break;
            }

            checkErr = false;
        } while (false);

        if (checkErr)
        {
            instance = new AppConfig()
            {
                // 默认界面语言为中文
                InterfaceLanguage = "zh-CN",
                VersionNumber = 100,
                // 其他配置项
            };
            string json = JsonSerializer.Serialize(instance, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        return instance;
    }


    /// <summary>
    /// 保存全部配置
    /// </summary>
    /// <param name="filePath">保存的文件路径</param>
    public static void SaveAppConfig(string filePath = "./app_config.json")
    {
        if (instance == null)
        {
            GetAppConfig(filePath);
        }

        string json = JsonSerializer.Serialize(instance, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// 保存单一配置
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="filePath">文件路径</param>
    public static void SaveSingleConfig(string key, string value, string filePath = "./app_config.json")
    {
        if (instance == null)
        {
            GetAppConfig(filePath);
        }

        PropertyInfo? property = typeof(AppConfig).GetProperty(key, BindingFlags.Public | BindingFlags.Instance);

        if (property != null && property.CanWrite)
        {
            try
            {
                object? convertedValue = Convert.ChangeType(value, property.PropertyType);
                property.SetValue(instance, convertedValue);
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Failed to set property '{key}' with value '{value}': {ex.Message}");
                string message = $"Failed to set property '{key}' with value '{value}': {ex.Message}";
                // 记录异常到日志文件
                LogExceptionToFile(ex, filePath, message);
            }
        }

        SaveAppConfig(filePath);
    }

    /// <summary>
    /// 记录异常到日志文件
    /// </summary>
    /// <param name="ex">异常对象</param>
    /// <param name="logFilePath">日志文件路径</param>
    private static void LogExceptionToFile(Exception ex, string logFilePath, string errStr)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now} - {errStr}");
                writer.WriteLine("Stack Trace:");
                writer.WriteLine(ex.StackTrace);
                writer.WriteLine("----------------------------------------");
            }
        }
        catch (Exception)
        {
            // 处理日志写入异常
            // 可以选择记录到控制台或其他方式
        }
    }

    // 会被解析-----------------------------------------

    // 界面语言,这个会被解析
    public string? InterfaceLanguage { get; set; }

    // 应用版本号(测试使用)(实际在应用程序中写死)
    public uint VersionNumber { get; set; }

}
