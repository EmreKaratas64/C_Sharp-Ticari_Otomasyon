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
    public partial class FrmFaturaUrunler : Form
    {
        public FrmFaturaUrunler()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        public string id;

        void Listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_FATURADETAY WHERE FATURAID='" + id + "'", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        void temizle()
        {
            TxtUrunID.Text = "";
            TxtUrunAd.Text = "";
            TxtFiyat.Text = "";
            TxtMiktar.Text = "";
            TxtTutar.Text = "";
            TxtFaturaID.Text = "";
        }

        private void FrmFaturaUrunler_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                TxtUrunID.Text = dr["FATURAURUNID"].ToString();
                TxtUrunAd.Text = dr["URUNAD"].ToString();
                TxtMiktar.Text = dr["MIKTAR"].ToString();
                TxtFiyat.Text = dr["FIYAT"].ToString();
                TxtTutar.Text = dr["TUTAR"].ToString();
                TxtFaturaID.Text = dr["FATURAID"].ToString();
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Bu ürünü silme istediğinize eminmisiniz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (onay == DialogResult.Yes)
            {
                SqlCommand Sil_komut = new SqlCommand("Delete From TBL_FATURADETAY WHERE FATURAURUNID=@P1", bgl.baglanti());
                Sil_komut.Parameters.AddWithValue("@P1", TxtUrunID.Text);
                Sil_komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Ürün sistemden silinmiştir !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Listele();
            }
            else
            {
                MessageBox.Show("Ürün silme işlemi iptal edildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            double fiyat, miktar, tutar;
            fiyat = Convert.ToDouble(TxtFiyat.Text);
            miktar = Convert.ToDouble(TxtMiktar.Text);
            tutar = fiyat * miktar;
            TxtTutar.Text = tutar.ToString();

            SqlCommand Guncelle_komut = new SqlCommand("Update TBL_FATURADETAY SET URUNAD=@P1,MIKTAR=@P2,FIYAT=@P3,TUTAR=@P4,FATURAID=@P5 WHERE FATURAURUNID=@P6", bgl.baglanti());
            Guncelle_komut.Parameters.AddWithValue("@P1", TxtUrunAd.Text);
            Guncelle_komut.Parameters.AddWithValue("@P2", TxtMiktar.Text);
            Guncelle_komut.Parameters.AddWithValue("@P3", decimal.Parse(TxtFiyat.Text));
            Guncelle_komut.Parameters.AddWithValue("@P4", decimal.Parse(TxtTutar.Text));
            Guncelle_komut.Parameters.AddWithValue("@P5", int.Parse(TxtFaturaID.Text));
            Guncelle_komut.Parameters.AddWithValue("@P6", TxtUrunID.Text);
            Guncelle_komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Ürün kaydı başarıyla güncellendi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Listele();
        }
    }
}
