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
using System.Windows.Shapes;

namespace IronApp
{
  public partial class BarCodeReader : Window
  {
    public MainWindow Main { get; set; }

    public BarCodeReader()
    {
      InitializeComponent();
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      foreach (var u in Main.DB.Clienti)
        CLCtrl.Items.Add(u);

      foreach (var i in Main.DB.Indumenti)
        INCtrl.Items.Add(i);
    }

    private void CLCtrl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      Main.ClienteSelected(CLCtrl.SelectedItem as TCliente);
    }

    private void INCtrl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      Main.IndumentoSelected(INCtrl.SelectedItem as TIndumento);
    }
  }
}
