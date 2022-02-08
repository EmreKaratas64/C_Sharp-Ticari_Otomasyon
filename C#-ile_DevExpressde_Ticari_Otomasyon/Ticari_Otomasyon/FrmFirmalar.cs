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
    public partial class FrmFirmalar : Form
    {
        public FrmFirmalar()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_FIRMALAR", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }


        void sehirlistesi()
        {
            SqlCommand komut = new SqlCommand("Select SEHIR From TBL_ILLER", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                Cmbil.Properties.Items.Add(dr[0]);
            }
            bgl.baglanti().Close();
        }

        void temizle()
        {
            TxtID.Text = "";
            TxtFirmaAd.Text = "";
            TxtSektor.Text = "";
            TxtYetkili.Text = "";
            TxtYetkiliGorev.Text = "";
            MskYetkiliTC.Text = "";
            MskTelefon1.Text = "";
            MskTelefon2.Text = "";
            MskTelefon3.Text = "";
            MskFax.Text = "";
            TxtMail.Text = "";
            TxtVergi.Text = "";
            RchAdres.Text = "";
            TxtKod1.Text = "";
            TxtKod2.Text = "";
            TxtKod3.Text = "";
        }

        void carikodaciklamalar()
        {
            SqlCommand komut = new SqlCommand("Select FIRMAKOD1 From TBL_KODLAR", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                RchKod1.Text = dr[0].ToString();
            }
            bgl.baglanti().Close();
        }

        private void FrmFirmalar_Load(object sender, EventArgs e)
        {
            listele();

            sehirlistesi();

            carikodaciklamalar();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                TxtID.Text = dr["ID"].ToString();
                TxtFirmaAd.Text = dr["AD"].ToString();
                TxtSektor.Text = dr["SEKTOR"].ToString();
                TxtYetkili.Text = dr["YETKILIADSOYAD"].ToString();
                TxtYetkiliGorev.Text = dr["YETKILISTATU"].ToString();
                MskYetkiliTC.Text = dr["YETKILITC"].ToString();
                MskTelefon1.Text = dr["TELEFON1"].ToString();
                MskTelefon2.Text = dr["TELEFON2"].ToString();
                MskTelefon3.Text = dr["TELEFON3"].ToString();
                MskFax.Text = dr["FAX"].ToString();
                TxtMail.Text = dr["MAIL"].ToString();
                Cmbil.Text = dr["IL"].ToString();
                Cmbilce.Text = dr["ILCE"].ToString();
                TxtVergi.Text = dr["VERGIDAIRE"].ToString();
                RchAdres.Text = dr["ADRES"].ToString();
                TxtKod1.Text = dr["OZELKOD1"].ToString();
                TxtKod2.Text = dr["OZELKOD2"].ToString();
                TxtKod3.Text = dr["OZELKOD3"].ToString();

            }
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

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("insert into TBL_FIRMALAR (AD,YETKILISTATU,YETKILIADSOYAD,YETKILITC,SEKTOR,TELEFON1,TELEFON2,TELEFON3,MAIL,FAX,IL,ILCE,VERGIDAIRE,ADRES,OZELKOD1,OZELKOD2,OZELKOD3) values(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17)", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", TxtFirmaAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtYetkiliGorev.Text);
            komut.Parameters.AddWithValue("@p3", TxtYetkili.Text);
            komut.Parameters.AddWithValue("@p4", MskYetkiliTC.Text);
            komut.Parameters.AddWithValue("@p5", TxtSektor.Text);
            komut.Parameters.AddWithValue("@p6", MskTelefon1.Text);
            komut.Parameters.AddWithValue("@p7", MskTelefon2.Text);
            komut.Parameters.AddWithValue("@p8", MskTelefon3.Text);
            komut.Parameters.AddWithValue("@p9", TxtMail.Text);
            komut.Parameters.AddWithValue("@p10", MskFax.Text);
            komut.Parameters.AddWithValue("@p11", Cmbil.Text);
            komut.Parameters.AddWithValue("@p12", Cmbilce.Text);
            komut.Parameters.AddWithValue("@p13", TxtVergi.Text);
            komut.Parameters.AddWithValue("@p14", RchAdres.Text);
            komut.Parameters.AddWithValue("@p15", TxtKod1.Text);
            komut.Parameters.AddWithValue("@p16", TxtKod2.Text);
            komut.Parameters.AddWithValue("@p17", TxtKod3.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Firma sisteme kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listele();
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Bu firmanın herhangi bir bankada kaydı olmadığına ve bu firmayı silmek istediğinize eminmisiniz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (onay == DialogResult.Yes)
            {
                SqlCommand silmekomutu = new SqlCommand("Delete From TBL_FIRMALAR Where ID=@P1", bgl.baglanti());
                silmekomutu.Parameters.AddWithValue("@P1", TxtID.Text);
                silmekomutu.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Firma kaydı sistemden silinmiştir !", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                temizle();
            }
            else
            {
                MessageBox.Show("Firma kardı silinmeden işlem iptal edildi", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand güncellekomut = new SqlCommand("Update TBL_FIRMALAR set AD=@p1,YETKILISTATU=@p2,YETKILIADSOYAD=@p3,YETKILITC=@p4,SEKTOR=@p5,TELEFON1=@p6,TELEFON2=@p7,TELEFON3=@p8,MAIL=@p9,FAX=@p10,IL=@p11,ILCE=@p12,VERGIDAIRE=@p13,ADRES=@p14,OZELKOD1=@p15,OZELKOD2=@p16,OZELKOD3=@p17 Where ID=@p18", bgl.baglanti());
            güncellekomut.Parameters.AddWithValue("@p1", TxtFirmaAd.Text);
            güncellekomut.Parameters.AddWithValue("@p2", TxtYetkiliGorev.Text);
            güncellekomut.Parameters.AddWithValue("@p3", TxtYetkili.Text);
            güncellekomut.Parameters.AddWithValue("@p4", MskYetkiliTC.Text);
            güncellekomut.Parameters.AddWithValue("@p5", TxtSektor.Text);
            güncellekomut.Parameters.AddWithValue("@p6", MskTelefon1.Text);
            güncellekomut.Parameters.AddWithValue("@p7", MskTelefon2.Text);
            güncellekomut.Parameters.AddWithValue("@p8", MskTelefon3.Text);
            güncellekomut.Parameters.AddWithValue("@p9", TxtMail.Text);
            güncellekomut.Parameters.AddWithValue("@p10", MskFax.Text);
            güncellekomut.Parameters.AddWithValue("@p11", Cmbil.Text);
            güncellekomut.Parameters.AddWithValue("@p12", Cmbilce.Text);
            güncellekomut.Parameters.AddWithValue("@p13", TxtVergi.Text);
            güncellekomut.Parameters.AddWithValue("@p14", RchAdres.Text);
            güncellekomut.Parameters.AddWithValue("@p15", TxtKod1.Text);
            güncellekomut.Parameters.AddWithValue("@p16", TxtKod2.Text);
            güncellekomut.Parameters.AddWithValue("@p17", TxtKod3.Text);
            güncellekomut.Parameters.AddWithValue("@p18", TxtID.Text);
            güncellekomut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Firma bilgileri güncellendi", "Bilgirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listele();
            temizle();
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }
    }
}
