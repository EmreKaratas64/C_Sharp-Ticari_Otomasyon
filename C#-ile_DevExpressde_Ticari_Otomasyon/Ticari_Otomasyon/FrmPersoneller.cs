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
    public partial class FrmPersoneller : Form
    {
        public FrmPersoneller()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void personellistele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_PERSONELLER", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        void sehirleri_getir()
        {
            SqlCommand sehirgetir = new SqlCommand("Select SEHIR From TBL_ILLER", bgl.baglanti());
            SqlDataReader dr = sehirgetir.ExecuteReader();
            while (dr.Read())
            {
                Cmbil.Properties.Items.Add(dr[0]);
            }
            bgl.baglanti().Close();
        }

        void temizle()
        {
            Txtid.Text = "";
            TxtAd.Text = "";
            TxtSoyad.Text = "";
            MskTelefon.Text = "";
            MskTC.Text = "";
            TxtMail.Text = "";
            TxtGorev.Text = "";
            RchAdres.Text = "";
            Cmbil.Text = "";
            Cmbilce.Text = "";
        }

        private void FrmPersoneller_Load(object sender, EventArgs e)
        {
            personellistele();
            sehirleri_getir();
        }

        private void Cmbil_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cmbilce.Properties.Items.Clear();
            SqlCommand komut = new SqlCommand("Select ILCE From TBL_ILCELER where SEHIR=@p1", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", Cmbil.SelectedIndex + 1);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                Cmbilce.Properties.Items.Add(dr[0]);
            }
            bgl.baglanti().Close();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                Txtid.Text = dr["ID"].ToString();
                TxtAd.Text = dr["AD"].ToString();
                TxtSoyad.Text = dr["SOYAD"].ToString();
                MskTelefon.Text = dr["TELEFON"].ToString();
                MskTC.Text = dr["TC"].ToString();
                TxtMail.Text = dr["MAIL"].ToString();
                Cmbil.Text = dr["IL"].ToString();
                Cmbilce.Text = dr["ILCE"].ToString();
                TxtGorev.Text = dr["GOREV"].ToString();
                RchAdres.Text = dr["ADRES"].ToString();
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand ekle_komut = new SqlCommand("insert into TBL_PERSONELLER (AD,SOYAD,TELEFON,TC,MAIL,IL,ILCE,ADRES,GOREV) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", bgl.baglanti());
            ekle_komut.Parameters.AddWithValue("@p1", TxtAd.Text);
            ekle_komut.Parameters.AddWithValue("@p2", TxtSoyad.Text);
            ekle_komut.Parameters.AddWithValue("@p3", MskTelefon.Text);
            ekle_komut.Parameters.AddWithValue("@p4", MskTC.Text);
            ekle_komut.Parameters.AddWithValue("@p5", TxtMail.Text);
            ekle_komut.Parameters.AddWithValue("@p6", Cmbil.Text);
            ekle_komut.Parameters.AddWithValue("@p7", Cmbilce.Text);
            ekle_komut.Parameters.AddWithValue("@p8", RchAdres.Text);
            ekle_komut.Parameters.AddWithValue("@p9", TxtGorev.Text);
            ekle_komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            personellistele();
            MessageBox.Show("Personel sisteme kaydedilmiştir", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            temizle();
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Personeli silmek istediğinize eminmisiniz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
           
            if (onay == DialogResult.Yes)
            {
                SqlCommand sil_komut = new SqlCommand("Delete From TBL_PERSONELLER Where ID=@P1", bgl.baglanti());
                sil_komut.Parameters.AddWithValue("@P1", Txtid.Text);
                sil_komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Personel sistemden silinmiştir !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                personellistele();
            }

            else
            {
                MessageBox.Show("Personel kaydı silme işlemi iptal edildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand guncelle_komut = new SqlCommand("Update TBL_PERSONELLER set AD=@P1,SOYAD=@P2,TELEFON=@P3,TC=@P4,MAIL=@P5,IL=@P6,ILCE=@P7,ADRES=@P8,GOREV=@P9 Where ID=@P10", bgl.baglanti());
            guncelle_komut.Parameters.AddWithValue("@P1", TxtAd.Text);
            guncelle_komut.Parameters.AddWithValue("@P2", TxtSoyad.Text);
            guncelle_komut.Parameters.AddWithValue("@P3", MskTelefon.Text);
            guncelle_komut.Parameters.AddWithValue("@P4", MskTC.Text);
            guncelle_komut.Parameters.AddWithValue("@P5", TxtMail.Text);
            guncelle_komut.Parameters.AddWithValue("@P6", Cmbil.Text);
            guncelle_komut.Parameters.AddWithValue("@P7", Cmbilce.Text);
            guncelle_komut.Parameters.AddWithValue("@P8", RchAdres.Text);
            guncelle_komut.Parameters.AddWithValue("@P9", TxtGorev.Text);
            guncelle_komut.Parameters.AddWithValue("@P10", Txtid.Text);
            guncelle_komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Personel bilgileri güncellendi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            personellistele();
        }
    }
}
