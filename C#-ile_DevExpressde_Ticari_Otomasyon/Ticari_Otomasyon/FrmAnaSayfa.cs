using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml;

namespace Ticari_Otomasyon
{
    public partial class FrmAnaSayfa : Form
    {
        public FrmAnaSayfa()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void Azalan_Stoklar()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select URUNAD,SUM(ADET) AS 'ADET' FROM TBL_URUNLER GROUP BY URUNAD HAVING SUM(ADET) <= 20 ORDER BY SUM(ADET)", bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            GridAzalanStoklar.DataSource = dt;
        }


        void Ajanda()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select Top 5 TARIH,SAAT,BASLIK,DETAY From TBL_NOTLAR ORDER BY ID DESC", bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            GridAjanda.DataSource = dt;
        }

        void Son10Hareket()
        {
            SqlDataAdapter da = new SqlDataAdapter("Execute FirmaHareketler2", bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            GridSon10Hareket.DataSource = dt;
        }

        void fihrist()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select AD,TELEFON1 FROM TBL_FIRMALAR", bgl.baglanti());
            da.Fill(dt);
            GridFihrist.DataSource = dt;
        }




        private void FrmAnaSayfa_Load(object sender, EventArgs e)
        {
            Azalan_Stoklar();

            Ajanda();

            Son10Hareket();

            fihrist();

            webBrowser1.Navigate("https://www.tcmb.gov.tr/wps/wcm/connect/tr/tcmb+tr/main+page+site+area/bugun");

            //webBrowser2.Navigate("https://www.doviz.com/kripto-paralar");
        }
    }
}
