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


// button1.BackColor= System.Drawing.Color.FromArgb(0,255,255);
namespace Ticari_Otomasyon
{
    public partial class FrmAyarlar : Form
    {
        public FrmAyarlar()
        {
            InitializeComponent();
        }


        sqlbaglantisi bgl = new sqlbaglantisi();


        void Listele()
        {

            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_ADMIN", bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        private void BtnIslem_MouseHover(object sender, EventArgs e)
        {
            BtnIslem.BackColor = System.Drawing.Color.FromArgb(0, 118, 215);
            BtnIslem.ForeColor = Color.White;
        }

        private void BtnIslem_MouseLeave(object sender, EventArgs e)
        {
            BtnIslem.BackColor = Color.White;
            BtnIslem.ForeColor = System.Drawing.Color.FromArgb(0, 118, 215);
        }

        private void PictureClose_MouseHover(object sender, EventArgs e)
        {
            PictureClose.BackColor = Color.White;
        }

        private void PictureClose_MouseLeave(object sender, EventArgs e)
        {
            PictureClose.BackColor = Color.Transparent;
        }

        private void FrmAyarlar_Load(object sender, EventArgs e)
        {

            MessageBox.Show("Var olan kullanıcı adlarında değişiklik yapılamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            Listele();


            TxtKullaniciAd.Text = "";
            TxtSifre.Text = "";
        }

        private void PictureClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                TxtKullaniciAd.Text = dr["KullaniciAd"].ToString();
                TxtSifre.Text = dr["Sifre"].ToString();
            }

        }

       

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = System.Drawing.Color.FromArgb(244, 51, 30);
            button1.ForeColor = Color.White;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.White;
            button1.ForeColor = System.Drawing.Color.FromArgb(244, 51, 30);
        }

        private void BtnIslem_Click(object sender, EventArgs e)
        {
           
                SqlCommand Kaydet_komut = new SqlCommand("insert into TBL_ADMIN VALUES (@P1,@P2)", bgl.baglanti());
                Kaydet_komut.Parameters.AddWithValue("@P1", TxtKullaniciAd.Text);
                Kaydet_komut.Parameters.AddWithValue("@P2", TxtSifre.Text);
                Kaydet_komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Kullanıcı sisteme kaydedildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele();
            
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Kullancının şifresi değiştirilsinmi ?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (onay == DialogResult.Yes)
            {
                SqlCommand Guncelle_komut = new SqlCommand("Update TBL_ADMIN SET Sifre=@P1 Where KullaniciAd=@P2", bgl.baglanti());
                Guncelle_komut.Parameters.AddWithValue("@P1", TxtSifre.Text);
                Guncelle_komut.Parameters.AddWithValue("@P2", TxtKullaniciAd.Text);
                Guncelle_komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Kullanıcı kaydı başarıyla güncellendi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele();
            }

            else
            {
                MessageBox.Show("Şifre değiştirme işlemi iptal edildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Kullanıcı kaydını silmek istediğinize eminmisiniz ?", "Kayıt silinsinmi?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (onay == DialogResult.Yes)
            {
                SqlCommand Sil_komut = new SqlCommand("Delete From TBL_ADMIN WHERE KullaniciAd=@P1", bgl.baglanti());
                Sil_komut.Parameters.AddWithValue("@P1", TxtKullaniciAd.Text);
                Sil_komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Kullanıcı kaydı sistemden silindi !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Listele();
            }
            else
            {
                MessageBox.Show("Kullanıcı kaydı silme işlemi iptal edildi", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void BtnGuncelle_MouseHover(object sender, EventArgs e)
        {
            BtnGuncelle.BackColor = System.Drawing.Color.FromArgb(0, 118, 215);
            BtnGuncelle.ForeColor = Color.White;
        }

        private void BtnGuncelle_MouseLeave(object sender, EventArgs e)
        {
            BtnGuncelle.BackColor = Color.White;
            BtnGuncelle.ForeColor = System.Drawing.Color.FromArgb(0, 118, 215);
        }
    }
}
