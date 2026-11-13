using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TRS_1._1
{
    public partial class Form1 : Form
    {
        public int adminNo;
        public static int adminNo2;

        public Form1()
        {
            InitializeComponent();            
        }        

        public void button1_Click(object sender, EventArgs e)
        {
            baglanti bgl = new baglanti();

            const string adminQuery = "SELECT admin_no, admin_password_hash FROM dbo.admin WHERE admin_id=@admin_id";
            int authenticatedAdminNo = -1;
            using (SqlConnection connection = bgl.baglantim())
            using (SqlCommand command = new SqlCommand(adminQuery, connection))
            {
                command.Parameters.AddWithValue("@admin_id", textBox1.Text);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read() && PasswordHasher.VerifyPassword(
                        textBox2.Text, Convert.ToString(reader["admin_password_hash"])))
                    {
                        authenticatedAdminNo = reader.GetInt32(0);
                    }
                }
            }

            if (authenticatedAdminNo >= 0)
            {
                int adminNo = authenticatedAdminNo;
                Log log = new Log(adminNo);
                log.LogWrite(adminNo + " No'lu Admin Toplantı Randevu Sistemine Giriş Yaptı");

                Anasayfa anasayfaForm = new Anasayfa(adminNo);
                anasayfaForm.adminNo = adminNo;
                anasayfaForm.Show();
                MessageBox.Show("Admin olarak giriş yaptınız. Admin sayfasını görüntülemektesiniz.");
                this.Hide();
            }
            else
            {
                const string userQuery = "SELECT users_no, users_ad, users_soyad, users_password_hash FROM dbo.users WHERE users_id=@users_id";
                int authenticatedUsersNo = -1;
                string usersAd = string.Empty;

                using (SqlConnection connection = bgl.baglantim())
                using (SqlCommand command = new SqlCommand(userQuery, connection))
                {
                    command.Parameters.AddWithValue("@users_id", textBox1.Text);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read() && PasswordHasher.VerifyPassword(
                            textBox2.Text, Convert.ToString(reader["users_password_hash"])))
                        {
                            authenticatedUsersNo = reader.GetInt32(0);
                            usersAd = reader.GetString(1).Trim();
                        }
                    }
                }

                if (authenticatedUsersNo >= 0)
                {
                    KAnasayfa userHome = new KAnasayfa(authenticatedUsersNo, usersAd);
                    userHome.usersNo = authenticatedUsersNo;
                    userHome.usersAd = usersAd;
                    KLog log = new KLog(authenticatedUsersNo);
                    log.usersNo = authenticatedUsersNo;

                    userHome.Show();
                    MessageBox.Show("Kullanıcı olarak giriş yaptınız. Kullanıcı sayfasını görüntülemektesiniz.");
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Kullanıcı Adınız Veya Şifreniz Hatalı");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            textBox2.PasswordChar = '*';
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Checkbox'ın "Checked" özelliğini kontrol ediyorum
            if (checkBox1.Checked)
            {
                // Butona basıldığında şifre alanını "görünür" hale getiriyorum
                textBox2.PasswordChar = '\0';
            }
            else
            {
                // Checkbox tekrar inaktif hale geldiğin (ilk konumuna) şifre alanını "gizli" hale getiriyorum
                textBox2.PasswordChar = '*';
            }
        }
        //Log kaydı için kullandığım metod.
        public void logtut(string value)
        {
            string koşul = string.Format(value, adminNo);
            Log a = new Log(koşul);
        }
    }
}
