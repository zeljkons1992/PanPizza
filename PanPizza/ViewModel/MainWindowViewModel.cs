using PanPizza.Command;
using PanPizza.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PanPizza.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        MainWindow mainWindow;

        public static double smsUkupnaCena = 0;

        #region Constructor
        public MainWindowViewModel(MainWindow mainOpen)
        {
            mainWindow = mainOpen;
            //allIngredients = new List<Ingredients>();
            InputSize();
            UnosZacina();
        }

        #endregion
        private List<string> size = new List<string>();
        public List<string> Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        private void InputSize()
        {
            Size.Add("Mala");
            Size.Add("Srednja");
            Size.Add("Velika");
        }

        private List<Ingredients> zacini = new List<Ingredients>();
        public List<Ingredients> Zacini
        {
            get
            {
                return zacini;
            }
            set
            {
                zacini = value;
            }
        }

        private void UnosZacina()
        {
            Ingredients i1 = new Ingredients("Salama", 40);
            Ingredients i2 = new Ingredients("Sunka", 40);
            Ingredients i3 = new Ingredients("Kulen", 40);
            Ingredients i4 = new Ingredients("Kecap", 0);
            Ingredients i5 = new Ingredients("Majonez", 0);
            Ingredients i6 = new Ingredients("Ljuta paprika", 25);
            Ingredients i7 = new Ingredients("Masline", 40);
            Ingredients i8 = new Ingredients("Origano", 0);
            Ingredients i9 = new Ingredients("Susam", 0);
            Ingredients i10 = new Ingredients("Sir", 20);

            zacini.Add(i1);
            zacini.Add(i2);
            zacini.Add(i3);
            zacini.Add(i4);
            zacini.Add(i5);
            zacini.Add(i6);
            zacini.Add(i7);
            zacini.Add(i8);
            zacini.Add(i9);
            zacini.Add(i10);
        }

        private ICommand manageNewPizza;
        public ICommand ManageNewPizza
        {
            get
            {
                if (manageNewPizza == null)
                {
                    manageNewPizza = new RelayCommand(param => ManageNewExecute(), param => CanManagePizzaExecute());
                }
                return manageNewPizza;
            }
        }
        private void ManageNewExecute()
        {

            if (mainWindow.cmbxSize.SelectedItem==null)
            {
                MessageBox.Show("Niste selektovali velicinu pice.");
            }
            else
            {

            try
            {
                bool flag = false;
                double ukupnaCena=0;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("********************************");
                sb.AppendLine("Racun");
                sb.AppendLine("---------------------------------");
                sb.AppendLine("Velicina pice: " + mainWindow.cmbxSize.SelectedItem.ToString());
                if (mainWindow.cmbxSize.SelectedItem.ToString().Equals("Mala"))
                {
                    ukupnaCena += 150;
                }
                if (mainWindow.cmbxSize.SelectedItem.ToString()=="Srednja")
                {
                    ukupnaCena += 229;
                }
                else 
                {
                    ukupnaCena += 299;
                }

                sb.AppendLine();
                sb.AppendLine("Dodaci:");
                sb.AppendLine();
                foreach (var item in Zacini)
                {
                    if (item.IsChecked == true)
                    {
                        sb.AppendLine(item.Name);
                        ukupnaCena+=item.Price;
                        flag = true;
                    }    
                }
                if (flag == false)
                {
                    sb.AppendLine("Bez priloga.");
                }
                sb.AppendLine("---------------------------------");
                sb.AppendLine("Vas racun iznosi " + ukupnaCena + " rsd.");

                smsUkupnaCena = ukupnaCena;

                mainWindow.lblsms.Visibility = Visibility.Visible;
                mainWindow.lblRacun.Visibility = Visibility.Visible;

                mainWindow.lblRacun.Content = sb.ToString();

                mainWindow.ListBox.IsEnabled = false;
                mainWindow.cmbxSize.IsEnabled = false;
                mainWindow.btnRacun.IsEnabled = false;
                mainWindow.btnNova.IsEnabled = true;

                Thread t1 = new Thread(() => slanjeSMS());
                t1.Start();     
            }
     
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        }
        private bool CanManagePizzaExecute()
        {
            return true;
        }

          private ICommand novaPorudzbina;
          public ICommand NovaPorudzbina
        {
            get
            {
                if (novaPorudzbina == null)
                {
                    novaPorudzbina = new RelayCommand(param => ManageNewPorudzbina(), param => CanManagePorudzbinaExecute());
                }
                return novaPorudzbina;
            }
        }
        private void ManageNewPorudzbina()
        {
            if (mainWindow.cmbxSize.SelectedItem==null)
            {
                MessageBox.Show("Niste selektovali velicinu pice.");
            }
            else
            {

            try
            {
                mainWindow.lblRacun.Visibility = Visibility.Hidden;
                mainWindow.lblsms.Visibility = Visibility.Hidden;
                mainWindow.btnRacun.IsEnabled = true;
                mainWindow.ListBox.IsEnabled = true;
                mainWindow.cmbxSize.IsEnabled = true;
                mainWindow.btnNova.IsEnabled = false;
                mainWindow.cmbxSize.SelectedIndex = -1;
                mainWindow.lblRacun.Content = "";

                foreach (var item in Zacini)
                {
                    item.IsChecked = false;
                }

                mainWindow.ListBox.Items.Refresh();
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            }
        }
        private bool CanManagePorudzbinaExecute()
        {
            return true;
        }
       
        private static void slanjeSMS()
        {
            TwilioClient.Init("AC191d51f60bdfc6186ae168b8ff857d4f", "d683d30561422ad700a4c520b559ea53");
              MessageResource.Create(
              to: new PhoneNumber("+381641810049"),
              from: new PhoneNumber("+18302242798"),
              body: "Vaš račun iznosi " + smsUkupnaCena + " rsd");
        }
    }
}
