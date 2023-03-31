using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Kitaplik_project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OleDbConnection connect = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\huseyin.ozturk\Desktop\Kitaplik.mdb");

        void listele()
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from Kitaplar",connect);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            listele();
        }

        string durum = "";

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            connect.Open();
            OleDbCommand kaydet = new OleDbCommand("insert into kitaplar (KitapAd,Yazar,Tur,Sayfa,Durum) values (@p1,@p2,@p3,@p4,@p5", connect);
            kaydet.Parameters.AddWithValue("@p1", TxtKitapAd.Text);
            kaydet.Parameters.AddWithValue("@p2", TxtYazar.Text);
            kaydet.Parameters.AddWithValue("@p3", CmbTur.Text);
            kaydet.Parameters.AddWithValue("@p4", TxtSayfa.Text);
            kaydet.Parameters.AddWithValue("@p5", durum);
            kaydet.ExecuteNonQuery();
            connect.Close();
            MessageBox.Show("Kitap sisteme kaydedildi.","Bilgi");
            listele();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            durum = "0";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            durum = "1";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            TxtKitapId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtKitapAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            TxtYazar.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            CmbTur.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            TxtSayfa.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            if (dataGridView1.Rows[secilen].Cells[5].Value.ToString() == "True")
            {
                radioButton2.Checked = true;
            }
            else
            {
                radioButton1.Checked = true;
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            connect.Open();
            OleDbCommand sil = new OleDbCommand("delete from kitaplar where kitapid=@p1", connect);
            sil.Parameters.AddWithValue("@p1", TxtKitapId.Text);
            sil.ExecuteNonQuery();
            connect.Close();
            MessageBox.Show("Kitap listeden silindi.", "Bilgi");
            listele();
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            connect.Open();
            OleDbCommand komut = new OleDbCommand("update kitaplar set Kitapad=@p1, Yazar=@p2, Tur=@p3, Sayfa=@p4, Durum=@p5 where Kitapid=@p6",connect);
            komut.Parameters.AddWithValue("@p1", TxtKitapAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtYazar.Text);
            komut.Parameters.AddWithValue("@p3", CmbTur.Text);
            komut.Parameters.AddWithValue("@p4", TxtSayfa.Text);
            if (radioButton1.Checked == true)
            {
                komut.Parameters.AddWithValue("@p5", durum);
            }
            if(radioButton2.Checked == true)
            {
                komut.Parameters.AddWithValue("@p5", durum);
            }
            komut.Parameters.AddWithValue("@p6", TxtKitapId.Text);
            komut.ExecuteNonQuery();
            connect.Close();
            MessageBox.Show("Kayıt Güncellendi.", "Bilgi");
        }

        private void BtnBul_Click(object sender, EventArgs e)
        {
            //Aramak istenilen kelimeyi komple yazmak gerekmektedir. 

            //OleDbCommand komut = new OleDbCommand("select * from Kitaplar where kitapad=@p1",connect);
            //komut.Parameters.AddWithValue("@p1", TxtKitapBul.Text);
            //DataTable dt = new DataTable();
            //OleDbDataAdapter da = new OleDbDataAdapter(komut);
            //da.Fill(dt);
            //dataGridView1.DataSource = dt;


            //Girilen harfler ile eşleşen aramalar gelmektedir.
            OleDbCommand komut = new OleDbCommand("select * from kitaplar where kitapad like '%"+TxtKitapBul.Text+ "%'", connect);
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(komut);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
