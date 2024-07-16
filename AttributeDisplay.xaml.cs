using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppConfigFileRw;

/// <summary>
/// AttributeDisplay.xaml 的交互逻辑
/// </summary>
public partial class AttributeDisplay : UserControl
{
    public AttributeDisplay()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        AppConfig.SaveSingleConfig(x_attributeName.Text, x_attributeValue.Text);
    }
}
