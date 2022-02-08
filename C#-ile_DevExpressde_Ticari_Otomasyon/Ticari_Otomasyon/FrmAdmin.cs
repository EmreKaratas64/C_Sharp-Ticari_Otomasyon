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
    public partial class FrmAdmin : Form
    {
        public FrmAdmin()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        private void LblSifreUnuttum_MouseHover(object sender, EventArgs e)
        {
            LblSifreUnuttum.Font = new Font(LblSifreUnuttum.Font, FontStyle.Bold);
        }

        private void LblSifreUnuttum_MouseLeave(object sender, EventArgs e)
        {
            LblSifreUnuttum.Font = new Font(LblSifreUnuttum.Font, FontStyle.Regular);
        }

        private void BtnGirisYap_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Select * From TBL_ADMIN Where KullaniciAd=@p1 and Sifre=@p2", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", TxtKullaniciAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtSifre.Text);
            SqlDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                FrmAnaModul fr = new FrmAnaModul();
                fr.kullanici = TxtKullaniciAd.Text;
                fr.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı Adı veya Şifre hatalı lütfen tekrar deneyin !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            bgl.baglanti().Close();    
            
        }

        private void PictureClose_MouseHover(object sender, EventArgs e)
        {
            PictureClose.BackColor = System.Drawing.Color.FromArgb(14, 45, 90);
        }

        private void PictureClose_MouseLeave(object sender, EventArgs e)
        {
            PictureClose.BackColor = Color.Transparent;
        }

        private void PictureClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LblSifreUnuttum_Click(object sender, EventArgs e)
        {
            if(TxtKullaniciAd.Text!="")
            {
                SifremiUnuttum Frm = new SifremiUnuttum();
                Frm.KullaniciAd = TxtKullaniciAd.Text;
                Frm.Show();
            }
            else
            {
                MessageBox.Show("Lütfen kullanıcı adını girin ve tekrar deneyin !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }
    }
}
