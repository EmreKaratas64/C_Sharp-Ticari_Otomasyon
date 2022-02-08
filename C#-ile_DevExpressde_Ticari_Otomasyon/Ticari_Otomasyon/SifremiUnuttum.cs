using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Data.SqlClient;

namespace Ticari_Otomasyon
{
    public partial class SifremiUnuttum : Form
    {
        public SifremiUnuttum()
        {
            InitializeComponent();
        }


        sqlbaglantisi bgl = new sqlbaglantisi();
     
        void Sifre_Olustur()
        {
            Random rastgele = new Random();
            int[] sayiuret = new int[6];
            sayiuret[0] = rastgele.Next(1, 9);
            sayiuret[1] = rastgele.Next(1, 9);
            sayiuret[2] = rastgele.Next(33, 46);
            sayiuret[3] = rastgele.Next(65, 91);
            sayiuret[4] = rastgele.Next(65, 91);
            sayiuret[5] = rastgele.Next(65, 91);

            char[] harfuret = new char[4];
            harfuret[0] = Convert.ToChar(sayiuret[2]);
            harfuret[1] = Convert.ToChar(sayiuret[3]);
            harfuret[2] = Convert.ToChar(sayiuret[4]);
            harfuret[3] = Convert.ToChar(sayiuret[5]);

            string olusturulansifre;
            olusturulansifre = harfuret[1].ToString().ToUpper() + sayiuret[0].ToString() + harfuret[0].ToString() + sayiuret[1].ToString() + harfuret[2].ToString().ToLower() + harfuret[3].ToString();
            LblSifre.Text = olusturulansifre;
        }


        public string KullaniciAd;

        private void SifremiUnuttum_Load(object sender, EventArgs e)
        {
            LblKullaniciAd.Text = KullaniciAd;
        }

        private void PictureClose_MouseHover(object sender, EventArgs e)
        {
            PictureClose.BackColor = Color.White;
        }

        private void PictureClose_MouseLeave(object sender, EventArgs e)
        {
            PictureClose.BackColor = Color.Transparent;
        }

        private void PictureClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void BtnGonder_Click(object sender, EventArgs e)
        {
            Sifre_Olustur();
            if (TxtKullaniciAd.Text == LblKullaniciAd.Text)
            {
                SqlCommand komut = new SqlCommand("Update TBL_ADMIN set Sifre=@P1 Where KullaniciAd=@P2", bgl.baglanti());
                komut.Parameters.AddWithValue("@P1", LblSifre.Text);
                komut.Parameters.AddWithValue("@P2", TxtKullaniciAd.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Şifreniz değiştirilmiştir yeni şifreniz  " + LblSifre.Text + "\nLütfen kimseyle paylaşmayın. ", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                MessageBox.Show("Lütfen kullanıcı adını kontrol ediniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
