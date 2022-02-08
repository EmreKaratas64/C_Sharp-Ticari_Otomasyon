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
using DevExpress.Charts;

namespace Ticari_Otomasyon
{
    public partial class FrmKasa : Form
    {
        public FrmKasa()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void FirmaHareketler()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Execute FirmaHareketler", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        void MusteriHareketler()
        {
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter("Execute MusteriHareketler", bgl.baglanti());
            da2.Fill(dt2);
            gridControl3.DataSource = dt2;
        }

        void Gider_Listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_GIDERLER", bgl.baglanti());
            da.Fill(dt);
            gridControl2.DataSource = dt;
        }

        public string ad;
        private void FrmKasa_Load(object sender, EventArgs e)
        {
            FirmaHareketler();

            MusteriHareketler();

            Gider_Listele();

            //  Kasadaki toplam tutarı hesaplama
            SqlCommand kasa_toplam = new SqlCommand("Select Sum(TUTAR) From TBL_FATURADETAY", bgl.baglanti());
            SqlDataReader dr1 = kasa_toplam.ExecuteReader();
            while (dr1.Read())
            {
                LblKasaToplam.Text = dr1[0].ToString() + "TL";
            }
            bgl.baglanti().Close();


            //Ödemeler
            SqlCommand Odemeler = new SqlCommand("Select (ELEKTRIK+SU+DOGALGAZ+INTERNET+EKSTRA) From TBL_GIDERLER order by ID asc", bgl.baglanti());
            SqlDataReader dr2 = Odemeler.ExecuteReader();
            while (dr2.Read())
            {
                LblOdemeler.Text = dr2[0].ToString() + "TL";
            }
            bgl.baglanti().Close();

            // Son ayın toplam maaş
            SqlCommand Personel_Maaslar = new SqlCommand("Select MAASLAR From TBL_GIDERLER order by ID asc", bgl.baglanti());
            SqlDataReader dr3 = Personel_Maaslar.ExecuteReader();
            while (dr3.Read())
            {
                LblPersonelMaaslar.Text = dr3[0].ToString() + "TL";
            }
            bgl.baglanti().Close();


            // Toplam Müşteri sayısı
            SqlCommand Musteri_Sayisi = new SqlCommand("Select Count(*) From TBL_MUSTERILER", bgl.baglanti());
            SqlDataReader dr4 = Musteri_Sayisi.ExecuteReader();
            while (dr4.Read())
            {
                LblMusteriSayisi.Text = dr4[0].ToString();
            }
            bgl.baglanti().Close();


            // Toplam Firma sayısı
            SqlCommand Firma_Sayisi = new SqlCommand("Select Count(*) From TBL_FIRMALAR", bgl.baglanti());
            SqlDataReader dr5 = Firma_Sayisi.ExecuteReader();
            while (dr5.Read())
            {
                LblFirmaSayisi.Text = dr5[0].ToString();
            }
            bgl.baglanti().Close();


            // Firma şehir sayısı
            SqlCommand Firma_Sehir = new SqlCommand("Select Count(Distinct(IL)) From TBL_FIRMALAR", bgl.baglanti());
            SqlDataReader dr6 = Firma_Sehir.ExecuteReader();
            while (dr6.Read())
            {
                LblSehirSayisi.Text = dr6[0].ToString();
            }
            bgl.baglanti().Close();


            // müşteri şehir sayısı
            SqlCommand Musteri_Sehir = new SqlCommand("Select Count(Distinct(IL)) From TBL_MUSTERILER", bgl.baglanti());
            SqlDataReader dr7 = Musteri_Sehir.ExecuteReader();
            while (dr7.Read())
            {
                LblSehirSayisi2.Text = dr7[0].ToString();
            }
            bgl.baglanti().Close();


            //Personel Sayısı
            SqlCommand Personel_Sayisi = new SqlCommand("Select Count(*) From TBL_PERSONELLER", bgl.baglanti());
            SqlDataReader dr8 = Personel_Sayisi.ExecuteReader();
            while (dr8.Read())
            {
                LblPersonelSayisi.Text = dr8[0].ToString();
            }
            bgl.baglanti().Close();




            //Stok sayısı
            SqlCommand Stok_Sayisi = new SqlCommand("Select Sum(ADET) From TBL_URUNLER", bgl.baglanti());
            SqlDataReader dr9 = Stok_Sayisi.ExecuteReader();
            while (dr9.Read())
            {
                LblStokSayisi.Text = dr9[0].ToString();
            }
            bgl.baglanti().Close();


            AktifKullanici.Text = ad;
        }


        int sayac = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            sayac++;

            if(sayac>0 && sayac <= 5)
            {
                //Elektrik
                chartControl1.Series["Aylar"].Points.Clear();
                groupControl2.Text = "Elektrik";
                SqlCommand komut = new SqlCommand("Select top 4 AY,ELEKTRIK From TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr[0], dr[1]));
                }
                bgl.baglanti().Close();
            }

            //Su
            if(sayac>5 && sayac <= 10)
            {
                chartControl1.Series["Aylar"].Points.Clear();
                groupControl2.Text = "Su";
                SqlCommand komut = new SqlCommand("Select top 4 AY,SU From TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr[0], dr[1]));
                }
                bgl.baglanti().Close();
            }

            //Doğalgaz
            if(sayac>10 && sayac <= 15)
            {
                chartControl1.Series["Aylar"].Points.Clear();
                groupControl2.Text = "Doğalgaz";
                SqlCommand komut = new SqlCommand("Select top 4 AY,DOGALGAZ From TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr[0], dr[1]));
                }
                bgl.baglanti().Close();
            }

            //İnternet
            if (sayac > 15 && sayac <= 20)
            {
                chartControl1.Series["Aylar"].Points.Clear();
                groupControl2.Text = "İnternet";
                SqlCommand komut = new SqlCommand("Select top 4 AY,INTERNET From TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr[0], dr[1]));
                }
                bgl.baglanti().Close();
            }

            //Ekstra
            if (sayac > 20 && sayac <= 25)
            {
                chartControl1.Series["Aylar"].Points.Clear();
                groupControl2.Text = "Ekstra";
                SqlCommand komut = new SqlCommand("Select top 4 AY,EKSTRA From TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr[0], dr[1]));
                }
                bgl.baglanti().Close();
            }

            if (sayac == 26)
            {
                sayac = 0;
            }
        }

        int sayac2 = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            sayac2++;

            if (sayac2 > 0 && sayac2 <= 5)
            {
                //Elektrik
                chartControl2.Series["Aylar"].Points.Clear();
                groupControl3.Text = "Elektrik";
                SqlCommand komut = new SqlCommand("Select top 4 AY,ELEKTRIK From TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr[0], dr[1]));
                }
                bgl.baglanti().Close();
            }

            //Su
            if (sayac2 > 5 && sayac2 <= 10)
            {
                chartControl2.Series["Aylar"].Points.Clear();
                groupControl3.Text = "Su";
                SqlCommand komut = new SqlCommand("Select top 4 AY,SU From TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr[0], dr[1]));
                }
                bgl.baglanti().Close();
            }

            //Doğalgaz
            if (sayac2 > 10 && sayac2 <= 15)
            {
                chartControl2.Series["Aylar"].Points.Clear();
                groupControl3.Text = "Doğalgaz";
                SqlCommand komut = new SqlCommand("Select top 4 AY,DOGALGAZ From TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr[0], dr[1]));
                }
                bgl.baglanti().Close();
            }

            //İnternet
            if (sayac2 > 15 && sayac2 <= 20)
            {
                chartControl2.Series["Aylar"].Points.Clear();
                groupControl3.Text = "İnternet";
                SqlCommand komut = new SqlCommand("Select top 4 AY,INTERNET From TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr[0], dr[1]));
                }
                bgl.baglanti().Close();
            }

            //Ekstra
            if (sayac2 > 20 && sayac2 <= 25)
            {
                chartControl2.Series["Aylar"].Points.Clear();
                groupControl3.Text = "Ekstra";
                SqlCommand komut = new SqlCommand("Select top 4 AY,EKSTRA From TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr[0], dr[1]));
                }
                bgl.baglanti().Close();
            }

            if (sayac2 == 26)
            {
                sayac2 = 0;
            }
        }
    }
}
