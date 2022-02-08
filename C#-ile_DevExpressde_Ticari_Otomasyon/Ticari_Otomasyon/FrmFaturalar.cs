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

namespace Ticari_Otomasyon
{
    public partial class FrmFaturalar : Form
    {
        public FrmFaturalar()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void Listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_FATURABILGI", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        void temizle()
        {
            TxtID.Text = "";
            TxtSeri.Text = "";
            TxtSiraNo.Text = "";
            MskTarih.Text = "";
            MskSaat.Text = "";
            TxtVergi.Text = "";
            TxtAlici.Text = "";
            TxtTeslimEden.Text = "";
            TxtTeslimAlan.Text = "";
            CmbCariTur.Text = "";
        }


        void Personel_Listele()
        {
            SqlCommand komut = new SqlCommand("Select * From TBL_PERSONELLER", bgl.baglanti());
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbPersonel.ValueMember = "ID";
            CmbPersonel.DisplayMember = "AD";
            CmbPersonel.DataSource = dt;
        }

        void Firma_Listesi()
        {
            SqlCommand komut = new SqlCommand("Select ID,AD From TBL_FIRMALAR", bgl.baglanti());
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbFirma.ValueMember = "ID";
            CmbFirma.DisplayMember = "AD";
            CmbFirma.DataSource = dt;
        }

        void Musteri_Listesi()
        {
            SqlCommand komut = new SqlCommand("Select ID,AD From TBL_MUSTERILER", bgl.baglanti());
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbMusteri.ValueMember = "ID";
            CmbMusteri.DisplayMember = "AD";
            CmbMusteri.DataSource = dt;
        }

        private void FrmFaturalar_Load(object sender, EventArgs e)
        {
            Listele();

            Firma_Listesi();

            Personel_Listele();

            Musteri_Listesi();

            temizle();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (TxtFaturaID.Text == "")
            {
                SqlCommand FaturaBilgi = new SqlCommand("insert into TBL_FATURABILGI (SERI,SIRANO,TARIH,SAAT,VERGIDAIRE,ALICI,TESLIMEDEN,TESLIMALAN) VALUES (@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8)", bgl.baglanti());
                FaturaBilgi.Parameters.AddWithValue("@P1", TxtSeri.Text);
                FaturaBilgi.Parameters.AddWithValue("@P2", TxtSiraNo.Text);
                FaturaBilgi.Parameters.AddWithValue("@P3", MskTarih.Text);
                FaturaBilgi.Parameters.AddWithValue("@P4", MskSaat.Text);
                FaturaBilgi.Parameters.AddWithValue("@P5", TxtVergi.Text);
                FaturaBilgi.Parameters.AddWithValue("@P6", TxtAlici.Text);
                FaturaBilgi.Parameters.AddWithValue("@P7", TxtTeslimEden.Text);
                FaturaBilgi.Parameters.AddWithValue("@P8", TxtTeslimAlan.Text);
                FaturaBilgi.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Fatura bilgisi sisteme kaydedildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele();
            }

            if (TxtFaturaID.Text != "")
            {

                if(CmbCariTur.Text=="Firma")
                {
                    double fiyat, miktar, tutar;
                    fiyat = Convert.ToDouble(TxtFiyat.Text);
                    miktar = Convert.ToDouble(TxtMiktar.Text);
                    tutar = fiyat * miktar;
                    TxtTutar.Text = tutar.ToString();


                    SqlCommand Fatura_urun_ekle = new SqlCommand("insert into TBL_FATURADETAY (URUNAD,MIKTAR,FIYAT,TUTAR,FATURAID) VALUES (@P1,@P2,@P3,@P4,@P5)", bgl.baglanti());
                    Fatura_urun_ekle.Parameters.AddWithValue("@P1", TxtUrunAd.Text);
                    Fatura_urun_ekle.Parameters.AddWithValue("@P2", TxtMiktar.Text);
                    Fatura_urun_ekle.Parameters.AddWithValue("@P3", decimal.Parse(TxtFiyat.Text));
                    Fatura_urun_ekle.Parameters.AddWithValue("@P4", decimal.Parse(TxtTutar.Text));
                    Fatura_urun_ekle.Parameters.AddWithValue("@P5", TxtFaturaID.Text);
                    Fatura_urun_ekle.ExecuteNonQuery();
                    bgl.baglanti().Close();


                    //Hareket tablosuna veri girişi
                    SqlCommand hareket_tablosuna_ekle = new SqlCommand("insert into TBL_FIRMAHAREKETLER (URUNID,ADET,PERSONEL,FIRMA,FIYAT,TOPLAM,FATURAID,TARIH) VALUES (@A1,@A2,@A3,@A4,@A5,@A6,@A7,@A8)", bgl.baglanti());
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A1", TxtUrunID.Text);
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A2", TxtMiktar.Text);
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A3", CmbPersonel.SelectedValue);
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A4", CmbFirma.SelectedValue);
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A5", decimal.Parse(TxtFiyat.Text));
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A6", decimal.Parse(TxtTutar.Text));
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A7", TxtFaturaID.Text);
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A8", MskTarih.Text);
                    hareket_tablosuna_ekle.ExecuteNonQuery();
                    bgl.baglanti().Close();



                    //Stok sayısını azaltma
                    SqlCommand komut1 = new SqlCommand("Update TBL_URUNLER SET ADET=ADET-@B1 WHERE ID=@B2", bgl.baglanti());
                    komut1.Parameters.AddWithValue("@B1", TxtMiktar.Text);
                    komut1.Parameters.AddWithValue("@B2", TxtUrunID.Text);
                    komut1.ExecuteNonQuery();
                    bgl.baglanti().Close();

                    
                }

                if (CmbCariTur.Text == "Müşteri")
                {
                    double fiyat, miktar, tutar;
                    fiyat = Convert.ToDouble(TxtFiyat.Text);
                    miktar = Convert.ToDouble(TxtMiktar.Text);
                    tutar = fiyat * miktar;
                    TxtTutar.Text = tutar.ToString();


                    SqlCommand Fatura_urun_ekle = new SqlCommand("insert into TBL_FATURADETAY (URUNAD,MIKTAR,FIYAT,TUTAR,FATURAID) VALUES (@P1,@P2,@P3,@P4,@P5)", bgl.baglanti());
                    Fatura_urun_ekle.Parameters.AddWithValue("@P1", TxtUrunAd.Text);
                    Fatura_urun_ekle.Parameters.AddWithValue("@P2", TxtMiktar.Text);
                    Fatura_urun_ekle.Parameters.AddWithValue("@P3", decimal.Parse(TxtFiyat.Text));
                    Fatura_urun_ekle.Parameters.AddWithValue("@P4", decimal.Parse(TxtTutar.Text));
                    Fatura_urun_ekle.Parameters.AddWithValue("@P5", TxtFaturaID.Text);
                    Fatura_urun_ekle.ExecuteNonQuery();
                    bgl.baglanti().Close();


                    //Hareket tablosuna veri girişi
                    SqlCommand hareket_tablosuna_ekle = new SqlCommand("insert into TBL_MUSTERIHAREKETLER (URUNID,ADET,PERSONEL,MUSTERI,FIYAT,TOPLAM,FATURAID,TARIH) VALUES (@A1,@A2,@A3,@A4,@A5,@A6,@A7,@A8)", bgl.baglanti());
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A1", TxtUrunID.Text);
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A2", TxtMiktar.Text);
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A3", CmbPersonel.SelectedValue);
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A4", CmbMusteri.SelectedValue);
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A5", decimal.Parse(TxtFiyat.Text));
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A6", decimal.Parse(TxtTutar.Text));
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A7", TxtFaturaID.Text);
                    hareket_tablosuna_ekle.Parameters.AddWithValue("@A8", MskTarih.Text);
                    hareket_tablosuna_ekle.ExecuteNonQuery();
                    bgl.baglanti().Close();



                    //Stok sayısını azaltma
                    SqlCommand komut1 = new SqlCommand("Update TBL_URUNLER SET ADET=ADET-@B1 WHERE ID=@B2", bgl.baglanti());
                    komut1.Parameters.AddWithValue("@B1", TxtMiktar.Text);
                    komut1.Parameters.AddWithValue("@B2", TxtUrunID.Text);
                    komut1.ExecuteNonQuery();
                    bgl.baglanti().Close();

                }

                MessageBox.Show("Faturaya ait ürün sisteme kaydedildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                TxtID.Text = dr["FATURABILGIID"].ToString();
                TxtSeri.Text = dr["SERI"].ToString();
                TxtSiraNo.Text = dr["SIRANO"].ToString();
                MskTarih.Text = dr["TARIH"].ToString();
                MskSaat.Text = dr["SAAT"].ToString();
                TxtVergi.Text = dr["VERGIDAIRE"].ToString();
                TxtAlici.Text = dr["ALICI"].ToString();
                TxtTeslimEden.Text = dr["TESLIMEDEN"].ToString();
                TxtTeslimAlan.Text = dr["TESLIMALAN"].ToString();     
            }

        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Bu faturaya ait ürün olmadığından eminmisiniz ?", "Silmek istiyormusunuz ?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (onay == DialogResult.Yes)
            {
                SqlCommand Fatura_Sil = new SqlCommand("Delete From TBL_FATURABILGI Where FATURABILGIID=@P1", bgl.baglanti());
                Fatura_Sil.Parameters.AddWithValue("@P1", TxtID.Text);
                Fatura_Sil.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Fatura sistemden silindi !", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Listele();
            }
            else
            {
                MessageBox.Show("Fatura silme işlemi iptal edildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand Fatura_Guncelle = new SqlCommand("Update TBL_FATURABILGI set SERI=@P1,SIRANO=@P2,TARIH=@P3,SAAT=@P4,VERGIDAIRE=@P5,ALICI=@P6,TESLIMEDEN=@P7,TESLIMALAN=@P8 WHERE FATURABILGIID=@P9", bgl.baglanti());
            Fatura_Guncelle.Parameters.AddWithValue("@P1", TxtSeri.Text);
            Fatura_Guncelle.Parameters.AddWithValue("@P2", TxtSiraNo.Text);
            Fatura_Guncelle.Parameters.AddWithValue("@P3", MskTarih.Text);
            Fatura_Guncelle.Parameters.AddWithValue("@P4", MskSaat.Text);
            Fatura_Guncelle.Parameters.AddWithValue("@P5", TxtVergi.Text);
            Fatura_Guncelle.Parameters.AddWithValue("@P6", TxtAlici.Text);
            Fatura_Guncelle.Parameters.AddWithValue("@P7", TxtTeslimEden.Text);
            Fatura_Guncelle.Parameters.AddWithValue("@P8", TxtTeslimAlan.Text);
            Fatura_Guncelle.Parameters.AddWithValue("@P9", TxtID.Text);
            Fatura_Guncelle.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Fatura bilgisi güncellendi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Listele();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FrmFaturaUrunler fr = new FrmFaturaUrunler();
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                fr.id = dr["FATURABILGIID"].ToString();
            }
            fr.Show();
        }

        private void BtnBul_Click(object sender, EventArgs e)
        {
            SqlCommand bul = new SqlCommand("Select URUNAD,SATISFIYAT FROM TBL_URUNLER WHERE ID=@P1", bgl.baglanti());
            bul.Parameters.AddWithValue("@P1", TxtUrunID.Text);
            SqlDataReader dr = bul.ExecuteReader();
            while (dr.Read())
            {
                TxtUrunAd.Text = dr[0].ToString();
                TxtFiyat.Text = dr[1].ToString();
            }
            bgl.baglanti().Close();
        }
    }
}
