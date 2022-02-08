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
    public partial class FrmBankalar : Form
    {
        public FrmBankalar()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void Listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Execute BankaBilgileri", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        void Sehir_Listesi()
        {
            SqlCommand komut = new SqlCommand("Select SEHIR From TBL_ILLER", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                Cmbil.Properties.Items.Add(dr[0]);
            }
            bgl.baglanti().Close();
        }

        void Firma_Listele()
        {
            SqlCommand komut = new SqlCommand("Select * From TBL_FIRMALAR", bgl.baglanti());
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbFirma.ValueMember = "ID";
            CmbFirma.DisplayMember = "AD";
            CmbFirma.DataSource = dt;
        }



        void temizle()
        {
            Txtid.Text = "";
            TxtAd.Text = "";
            Cmbil.Text = "";
            Cmbilce.Text = "";
            TxtSube.Text = "";
            TxtIban.Text = "";
            TxtHesapNo.Text = "";
            TxtYetkili.Text = "";
            MskTelefon.Text = "";
            MskTarih.Text = "";
            TxtHesapTuru.Text = "";

        }

       

        private void FrmBankalar_Load(object sender, EventArgs e)
        {
            Listele();
            Sehir_Listesi();
            Firma_Listele();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand ekle_komut = new SqlCommand("insert into TBL_BANKALAR (BANKAADI,IL,ILCE,SUBE,IBAN,HESAPNO,YETKILI,TELEFON,TARIH,HESAPTURU,FIRMAID) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)", bgl.baglanti());
            ekle_komut.Parameters.AddWithValue("@p1", TxtAd.Text);
            ekle_komut.Parameters.AddWithValue("@p2", Cmbil.Text);
            ekle_komut.Parameters.AddWithValue("@p3", Cmbilce.Text);
            ekle_komut.Parameters.AddWithValue("@p4", TxtSube.Text);
            ekle_komut.Parameters.AddWithValue("@p5", TxtIban.Text);
            ekle_komut.Parameters.AddWithValue("@p6", TxtHesapNo.Text);
            ekle_komut.Parameters.AddWithValue("@p7", TxtYetkili.Text);
            ekle_komut.Parameters.AddWithValue("@p8", MskTelefon.Text);
            ekle_komut.Parameters.AddWithValue("@p9", MskTarih.Text);
            ekle_komut.Parameters.AddWithValue("@p10", TxtHesapTuru.Text);
            ekle_komut.Parameters.AddWithValue("@p11", CmbFirma.SelectedValue);
            ekle_komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Banka sisteme eklenmiştir.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Listele();

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
                TxtAd.Text = dr["BANKAADI"].ToString();
                Cmbil.Text = dr["IL"].ToString();
                Cmbilce.Text = dr["ILCE"].ToString();
                TxtSube.Text = dr["SUBE"].ToString();
                TxtIban.Text = dr["IBAN"].ToString();
                TxtHesapNo.Text = dr["HESAPNO"].ToString();
                TxtYetkili.Text = dr["YETKILI"].ToString();
                MskTelefon.Text = dr["TELEFON"].ToString();
                MskTarih.Text = dr["TARIH"].ToString();
                TxtHesapTuru.Text = dr["HESAPTURU"].ToString();
                CmbFirma.Text = dr["AD"].ToString();
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Bankayı sistemden silmek istediğinize eminmisiniz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (onay == DialogResult.Yes)
            {
                SqlCommand sil_komut = new SqlCommand("Delete From TBL_BANKALAR Where ID=@p1", bgl.baglanti());
                sil_komut.Parameters.AddWithValue("@p1", Txtid.Text);
                sil_komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Banka sistemden silindi !", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Listele();
                temizle();
            }
            else
            {
                MessageBox.Show("Banka silme işlemi iptal edildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand guncelle_komut = new SqlCommand("Update TBL_BANKALAR set BANKAADI=@P1,IL=@P2,ILCE=@P3,SUBE=@P4,IBAN=@P5,HESAPNO=@P6,YETKILI=@P7,TELEFON=@P8,TARIH=@P9,HESAPTURU=@P10,FIRMAID=@P11 Where ID=@P12", bgl.baglanti());
            guncelle_komut.Parameters.AddWithValue("@P1", TxtAd.Text);
            guncelle_komut.Parameters.AddWithValue("@P2", Cmbil.Text);
            guncelle_komut.Parameters.AddWithValue("@P3", Cmbilce.Text);
            guncelle_komut.Parameters.AddWithValue("@P4", TxtSube.Text);
            guncelle_komut.Parameters.AddWithValue("@P5", TxtIban.Text);
            guncelle_komut.Parameters.AddWithValue("@P6", TxtHesapNo.Text);
            guncelle_komut.Parameters.AddWithValue("@P7", TxtYetkili.Text);
            guncelle_komut.Parameters.AddWithValue("@P8", MskTelefon.Text);
            guncelle_komut.Parameters.AddWithValue("@P9", MskTarih.Text);
            guncelle_komut.Parameters.AddWithValue("@P10", TxtHesapTuru.Text);
            guncelle_komut.Parameters.AddWithValue("@P11", CmbFirma.SelectedValue);
            guncelle_komut.Parameters.AddWithValue("@P12",Txtid.Text);
            guncelle_komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Banka kaydı başarıyla güncellendi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Listele();
        }

    }
}
