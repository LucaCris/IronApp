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
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace IronApp
{
  public class TUtente
  {
    public int ID { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string PWD { get; set; }
    public TRuolo Ruolo { get; set; }
  }

  public class TRuolo
  {
    public int ID { get; set; }
    public string Descrizione { get; set; }
    public int Tipo { get; set; }
  }

  public class TCliente
  {
    public int ID { get; set; }
    public string Nome { get; set; }
    public int BarCode { get; set; }

    public override string ToString()
    {
      return Nome;
    }
  }

  public class TIndumento
  {
    public int ID { get; set; }
    public string Nome { get; set; }
    public int BarCode { get; set; }

    public override string ToString()
    {
      return Nome;
    }
  }

  public class IronDB : DbContext
  {
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
    }
    public DbSet<TUtente> Utenti { get; set; }
    public DbSet<TRuolo> Ruoli { get; set; }
    public DbSet<TCliente> Clienti { get; set; }
    public DbSet<TIndumento> Indumenti { get; set; }
  }

  public partial class MainWindow : Window
  {
    public IronDB DB;

    public MainWindow()
    {
      InitializeComponent();

      Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

      DB = new IronDB();
      if (DB.Utenti.Count() == 0) {
        for (int i = 0; i < 3; i++) {
          var u = new TUtente() { Nome = $"User{i}" };
          DB.Utenti.Add(u);
        }
      }
      if (DB.Ruoli.Count() == 0) {
        for (int i = 0; i < 5; i++) {
          var r = new TRuolo() { Descrizione = $"Role{i}" };
          DB.Ruoli.Add(r);
        }
      }
      foreach (var u in DB.Utenti) {
        if (u.Ruolo == null)
          u.Ruolo = DB.Ruoli.FirstOrDefault<TRuolo>();
      }
      if (DB.Clienti.Count() == 0) {
        for (int i = 0; i < 10; i++) {
          var c = new TCliente() { Nome = $"Cliente{i}" };
          DB.Clienti.Add(c);
        }
      }
      if (DB.Indumenti.Count() == 0) {
        for (int i = 0; i < 5; i++) {
          var n = new TIndumento() { Nome = $"Indumento{i}" };
          DB.Indumenti.Add(n);
        }
      }
      DB.SaveChanges();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      base.OnClosing(e);
      DB.Dispose();
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      var bc = new BarCodeReader();
      bc.Main = this;
      bc.Top = Top;
      bc.Left = Left + Width;
      bc.Show();
    }

    public void ClienteSelected(TCliente cli)
    {
      foreach (TabItem tab in WOCtrl.Items) {
        TCliente tc = tab.Tag as TCliente;
        if (tc.ID == cli.ID) {
          WOCtrl.SelectedItem = tab;
          return;
        }
      }

      var ntab = new TabItem() { Header = cli.Nome, Tag = cli };
      ntab.Content = new ListView();
      WOCtrl.Items.Add(ntab);
      WOCtrl.SelectedItem = ntab;
    }

    public void IndumentoSelected(TIndumento ind)
    {
      var sel = WOCtrl.SelectedItem as TabItem;
      if (sel == null)
        return;

      var lv = sel.Content as ListView;
      lv.Items.Add(ind);
    }
  }
}
