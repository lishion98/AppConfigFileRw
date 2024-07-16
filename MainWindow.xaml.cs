using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Resources;
using System.IO;

namespace AppConfigFileRw;

public partial class MainWindow : Window
{
    private AppConfig appConfig = AppConfig.GetAppConfig();

    public MainWindow()
    {
        InitializeComponent();

        DisplayAppConfigProperties();
    }

    private void SaveConfig_Click(object sender, RoutedEventArgs e)
    {
        string json = File.ReadAllText("./app_config.json");
        x_fileShow.Text = json;
    }

    private void DisplayAppConfigProperties()
    {
        PropertyInfo[] properties = typeof(AppConfig).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var showC = new AttributeDisplay();

            showC.x_attributeName.Text = property.Name;
            showC.x_attributeValue.Text = property.GetValue(appConfig)?.ToString(); // 获取当前属性值并设置 TextBox 文本

            x_group.Children.Add(showC);
        }
    }

    

}