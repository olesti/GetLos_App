﻿using MySqlConnector;
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

namespace GetLos_App
{
    /// <summary>
    /// Window1.xaml etkileşim mantığı
    /// </summary>
    public partial class Kira : Window
    {
        public Kira()
        {
            InitializeComponent();
            kp.Listele();
            musteripopodata.ItemsSource = kp.Listele();
            kp.Listele1();
            aracpopodata.ItemsSource = kp.Listele1();
            time1.DateTime = DateTime.Now;
            time2.DateTime = DateTime.Now;
            kp.Listelemiete();
            mietedata.ItemsSource = kp.Listelemiete();


        }

        Class1 kp=new Class1(); 
        private void musteribtn_Click(object sender, RoutedEventArgs e)
        {
            musteripopupxaml musteri = new musteripopupxaml();
            musteri.Show();

            

        }

        private void aracbtn_Click(object sender, RoutedEventArgs e)
        {
            //datagrid.ItemsSource = kp.Listele1();

            Window2 sa = new Window2();
            sa.Show();

        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

        }
        
        private void aracpopodata_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var asa = (aracclass)aracpopodata.SelectedItem as aracclass;
            markatxt.Text = asa.Marke;
            modeltxt.Text = asa.Model;
            yakıt_txt.Text = asa.Kraftstoff;
            plakatxt.Text = asa.Nummernschild;
            vitestxt.Text = asa.Getriebetype;
            kasatxt.Text = asa.Karosserientyp;
            kostentxt.Text = asa.Kosten.Substring(0, asa.Kosten.Length - 1); ;

            
        }

        private void musteripopodata_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var sas = (mustericlass)musteripopodata.SelectedItem as mustericlass;

            adtxt.Text = sas.Ad;
            soyadtxt.Text = sas.Soyad;
            tctxt.Text = sas.Tcnummer;
            teltxt.Text = sas.Telefonu;
            mailtxt.Text = sas.Mail;
            ehlinotxt.Text = sas.Ehliyetno.ToString();
        }

        private void musteripopodata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        int? gun=null;
        
        private void time2_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime bugunTarihi = time1.DateTime.Value;
            DateTime sinavTarihi = time2.DateTime.Value;

            TimeSpan ts = sinavTarihi - bugunTarihi;
            gun = ts.Days;
            teltxt_Copy.Text = "Kalan Gün : " + ts.Days.ToString();
            if (kostentxt.Text != "")
            {
                if (gun != null && gun != 0)
                {
                    preistxt.Text = (gun * Convert.ToInt32(kostentxt.Text)).ToString();
                    time1txt.Text = time1.DateTime.Value.ToString();
                    time2txt.Text = timenormal.ToString();
                }
            }
            

        }

        bool saaatt;
        bool saat(DateTime ass, DateTime ass1)
        {
            DateTime yeni1 = time1.DateTime.Value;
            DateTime yeni2 = time2.DateTime.Value;
            /*
            for (int i = 0; i < mietedata.Items.Count; i++)
            {

            }*/
            if ((yeni1 < ass && ass < yeni2) || (yeni1 < ass1 && ass1 < yeni2))
            {
                saaatt = false;


            }
            else
            {
                saaatt = true;



            }
            return saaatt;
        }
        MySqlConnection con;

        private void Open_File_Copy_Click(object sender, RoutedEventArgs e)
        {
            var arac = (aracclass)aracpopodata.SelectedItem as aracclass;
            var musteri = (mustericlass)musteripopodata.SelectedItem as mustericlass;
            Mieteclass miete = new Mieteclass();
            miete.Vorname =musteri.Ad;
            miete.Nachname =musteri.Soyad;
            miete.Tcnummer =musteri.Tcnummer;
            miete.Telefonnumer =musteri.Telefonu;
            miete.Email =musteri.Mail;
            miete.Führerscheinno =musteri.Ehliyetno.ToString();
            miete.Model=arac.Model;
            miete.Marke =arac.Marke;
            miete.Nummerschild = arac.Nummernschild;
            miete.Kraftstoff =arac.Kraftstoff;
            miete.Kosten =kostentxt.Text;
            miete.Rechnungsno=rechnungtxt.Text;
            miete.Sondate = Convert.ToDateTime(time2.DateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            miete.Ilkdate = Convert.ToDateTime(time1.DateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            saat(miete.Ilkdate, miete.Sondate);
            List<Mieteclass> studentList = new List<Mieteclass>();

            Console.WriteLine();
            try
            {
                
                MySqlConnection con = new MySqlConnection("Server=localhost;Database=testdb;Uid=root;Pwd='atalay528';AllowUserVariables=True;UseCompression=True;");

                MySqlDataAdapter baglayici = new MySqlDataAdapter();
                MySqlCommand komut = new MySqlCommand("Select * from testdb.miete where Nummerschild = '" + miete.Nummerschild + "'", con);

                con.Open();
                MySqlDataReader reader = komut.ExecuteReader();

                if (reader.Read())
                {
                    if (saaatt)
                    {
                        kp.Eklemiete(musteri, arac, miete);
                        mietedata.ItemsSource = kp.Listelemiete();

                        kp.Listelemiete();
                    }
                    else
                    {
                        MessageBox.Show("zamanda sorun var");
                    }
                }
                else
                {
                    MessageBox.Show("Username And Password Not Match!", "VINSMOKE MJ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
           
            


        }

        private void time1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Open_File_Click(object sender, RoutedEventArgs e)
        {
            Mieteclass aracclass2 = new Mieteclass();
            aracclass2 = (Mieteclass)mietedata.SelectedItem as Mieteclass;
            kp.Silmiete(aracclass2); ;
            mietedata.ItemsSource = kp.Listelemiete();
            
        }
    }
}
