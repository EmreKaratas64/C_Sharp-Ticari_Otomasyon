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
    public partial class FrmNotlar : Form
    {
        public FrmNotlar()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();


        void Listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_NOTLAR", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }


        void temizle()
        {
            Txtid.Text = "";
            MskTarih.Text = "";
            MskSaat.Text = "";
            TxtBaslik.Text = "";
            TxtOlusturan.Text = "";
            TxtHitap.Text = "";
            RchDetay.Text = "";
        }

        private void FrmNotlar_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand Ekle_komut = new SqlCommand("insert into TBL_NOTLAR (TARIH,SAAT,BASLIK,DETAY,OLUSTURAN,HITAP) VALUES (@P1,@P2,@P3,@P4,@P5,@P6)", bgl.baglanti());
            Ekle_komut.Parameters.AddWithValue("@P1", MskTarih.Text);
            Ekle_komut.Parameters.AddWithValue("@P2", MskSaat.Text);
            Ekle_komut.Parameters.AddWithValue("@P3", TxtBaslik.Text);
            Ekle_komut.Parameters.AddWithValue("@P4", RchDetay.Text);
            Ekle_komut.Parameters.AddWithValue("@P5", TxtOlusturan.Text);
            Ekle_komut.Parameters.AddWithValue("@P6", TxtHitap.Text);
            Ekle_komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Not sisteme kaydedildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Listele();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                Txtid.Text = dr["ID"].ToString();
                MskTarih.Text = dr["TARIH"].ToString();
                MskSaat.Text = dr["SAAT"].ToString();
                TxtBaslik.Text = dr["BASLIK"].ToString();
                TxtOlusturan.Text = dr["OLUSTURAN"].ToString();
                TxtHitap.Text = dr["HITAP"].ToString();
                RchDetay.Text = dr["DETAY"].ToString();
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Notu silmek istediğinize eminmisiniz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (onay == DialogResult.Yes)
            {
                SqlCommand Sil_komut = new SqlCommand("Delete From TBL_NOTLAR WHERE ID=@P1", bgl.baglanti());
                Sil_komut.Parameters.AddWithValue("@P1", Txtid.Text);
                Sil_komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Not sistemden silindi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Listele();
            }
            else
            {
                MessageBox.Show("Not silme işlemi iptal edildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand Guncelle_komut = new SqlCommand("Update TBL_NOTLAR SET TARIH=@P1,SAAT=@P2,BASLIK=@P3,DETAY=@P4,OLUSTURAN=@P5,HITAP=@P6 WHERE ID=@P7", bgl.baglanti());
            Guncelle_komut.Parameters.AddWithValue("@P1", MskTarih.Text);
            Guncelle_komut.Parameters.AddWithValue("@P2", MskSaat.Text);
            Guncelle_komut.Parameters.AddWithValue("@P3", TxtBaslik.Text);
            Guncelle_komut.Parameters.AddWithValue("@P4", RchDetay.Text);
            Guncelle_komut.Parameters.AddWithValue("@P5", TxtOlusturan.Text);
            Guncelle_komut.Parameters.AddWithValue("@P6", TxtHitap.Text);
            Guncelle_komut.Parameters.AddWithValue("@P7", Txtid.Text);
            Guncelle_komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Not başarıyla güncellendi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Listele();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FrmNotDetay fr = new FrmNotDetay();
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                fr.metin = dr["DETAY"].ToString();
            }
            fr.Show();
        }
    }
}
