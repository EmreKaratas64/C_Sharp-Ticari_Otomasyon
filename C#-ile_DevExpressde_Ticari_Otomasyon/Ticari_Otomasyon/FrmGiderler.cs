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
    public partial class FrmGiderler : Form
    {
        public FrmGiderler()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void gider_listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_GIDERLER", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        void temizle()
        {
            Txtid.Text = "";
            CmbAy.Text = "";
            CmbYıl.Text = "";
            TxtElektrik.Text = "";
            TxtSu.Text = "";
            TxtDogalgaz.Text = "";
            TxtInternet.Text = "";
            TxtMaaslar.Text = "";
            TxtExtra.Text = "";
            RchNotlar.Text = "";
        }

        private void FrmGiderler_Load(object sender, EventArgs e)
        {
            gider_listele();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand ekle_komut = new SqlCommand("insert into TBL_GIDERLER (AY,YIL,ELEKTRIK,SU,DOGALGAZ,INTERNET,MAASLAR,EKSTRA,NOTLAR) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", bgl.baglanti());
            ekle_komut.Parameters.AddWithValue("@p1", CmbAy.Text);
            ekle_komut.Parameters.AddWithValue("@p2", CmbYıl.Text);
            ekle_komut.Parameters.AddWithValue("@p3", decimal.Parse(TxtElektrik.Text));
            ekle_komut.Parameters.AddWithValue("@p4", decimal.Parse(TxtSu.Text));
            ekle_komut.Parameters.AddWithValue("@p5", decimal.Parse(TxtDogalgaz.Text));
            ekle_komut.Parameters.AddWithValue("@p6", decimal.Parse(TxtInternet.Text));
            ekle_komut.Parameters.AddWithValue("@p7", decimal.Parse(TxtMaaslar.Text));
            ekle_komut.Parameters.AddWithValue("@p8", decimal.Parse(TxtExtra.Text));
            ekle_komut.Parameters.AddWithValue("@p9", RchNotlar.Text);
            ekle_komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Gider kaydı eklendi", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gider_listele();
            temizle();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                Txtid.Text = dr["ID"].ToString();
                CmbAy.Text = dr["AY"].ToString();
                CmbYıl.Text = dr["YIL"].ToString();
                TxtElektrik.Text = dr["ELEKTRIK"].ToString();
                TxtSu.Text = dr["SU"].ToString();
                TxtDogalgaz.Text = dr["DOGALGAZ"].ToString();
                TxtInternet.Text = dr["INTERNET"].ToString();
                TxtMaaslar.Text = dr["MAASLAR"].ToString();
                TxtExtra.Text = dr["EKSTRA"].ToString();
                RchNotlar.Text = dr["NOTLAR"].ToString();
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Gider kaydını silmek istediğinize eminmisiniz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (onay == DialogResult.Yes)
            {
                SqlCommand sil_komut = new SqlCommand("Delete From TBL_GIDERLER Where ID=@p1", bgl.baglanti());
                sil_komut.Parameters.AddWithValue("@p1", Txtid.Text);
                sil_komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Gider kaydı silinmiştir !", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                gider_listele();
                temizle();
            }
            else
            {
                MessageBox.Show("Gider kaydı silme işlemi iptal edildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand guncelle_komut = new SqlCommand("Update TBL_GIDERLER set AY=@P1,YIL=@P2,ELEKTRIK=@P3,SU=@P4,DOGALGAZ=@P5,INTERNET=@P6,MAASLAR=@P7,EKSTRA=@P8,NOTLAR=@P9 Where ID=@P10", bgl.baglanti());
            guncelle_komut.Parameters.AddWithValue("@P1", CmbAy.Text);
            guncelle_komut.Parameters.AddWithValue("@P2", CmbYıl.Text);
            guncelle_komut.Parameters.AddWithValue("@P3", decimal.Parse(TxtElektrik.Text));
            guncelle_komut.Parameters.AddWithValue("@P4", decimal.Parse(TxtSu.Text));
            guncelle_komut.Parameters.AddWithValue("@P5", decimal.Parse(TxtDogalgaz.Text));
            guncelle_komut.Parameters.AddWithValue("@P6", decimal.Parse(TxtInternet.Text));
            guncelle_komut.Parameters.AddWithValue("@P7", decimal.Parse(TxtMaaslar.Text));
            guncelle_komut.Parameters.AddWithValue("@P8", decimal.Parse(TxtExtra.Text));
            guncelle_komut.Parameters.AddWithValue("@P9", RchNotlar.Text);
            guncelle_komut.Parameters.AddWithValue("@P10", Txtid.Text);
            guncelle_komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Gider kaydı başarıyla güncellenmiştir.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gider_listele();
            temizle();
        }
    }
}
