using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace coba_tugas
{
    public partial class Form1 : Form
    {
        private MySqlConnection conn;
        private DataTable dataTable;
        private string connectionString = "server=localhost;user id=root;password=;database=novel;";

        public Form1()
        {
            InitializeComponent();
            conn = new MySqlConnection(connectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDataNovel();

            buttonTambah.Click += buttonTambah_Click;
            buttonUbah.Click += buttonUbah_Click;
            buttonHapus.Click += buttonHapus_Click;
            buttonCari.Click += buttonCari_Click;
        }

        private void LoadDataNovel()
        {
            string query = "SELECT * FROM buku";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView2.DataSource = null;
                    dataGridView2.DataSource = dt;

                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }
  

        private void buttonTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox6.Text) || string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Kode buku dan judul harus diisi!");
                return;
            }
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO buku (kode_buku, judul_buku, nama_penulis, genre, deskripsi) " +
                                   "VALUES (@kode, @judul, @penulis, @genre, @deskripsi)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@kode", textBox6.Text);
                    cmd.Parameters.AddWithValue("@judul", textBox1.Text);
                    cmd.Parameters.AddWithValue("@penulis", textBox2.Text);
                    cmd.Parameters.AddWithValue("@genre", textBox3.Text);
                    cmd.Parameters.AddWithValue("@deskripsi", textBox4.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil ditambahkan!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menambahkan data: " + ex.Message);
                }
            }

            LoadDataNovel();  // Panggil setelah proses selesai
            ClearForm();
        }


        private void buttonUbah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Pilih data yang akan diubah terlebih dahulu!");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE buku SET judul_buku=@judul, nama_penulis=@penulis, genre=@genre, deskripsi=@deskripsi " +
                                   "WHERE kode_buku=@kode";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@kode", textBox6.Text);
                    cmd.Parameters.AddWithValue("@judul", textBox1.Text);
                    cmd.Parameters.AddWithValue("@penulis", textBox2.Text);
                    cmd.Parameters.AddWithValue("@genre", textBox3.Text);
                    cmd.Parameters.AddWithValue("@deskripsi", textBox4.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil diubah!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mengubah data: " + ex.Message);
                }
            }

            LoadDataNovel();
            ClearForm();
        }


        private void buttonHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Pilih data yang akan dihapus terlebih dahulu!");
                return;
            }

            if (MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi",
                MessageBoxButtons.YesNo) == DialogResult.Yes)

                using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM buku WHERE kode_buku = @kode";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@kode", textBox6.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil dihapus.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menghapus data: " + ex.Message);
                }
            }

            LoadDataNovel();
            ClearForm();
        }


        private void buttonCari_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM buku WHERE kode_buku = @kode";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@kode", textBox5.Text);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView2.DataSource = null;
                    dataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mencari data: " + ex.Message);
                }
            }
        }


        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                textBox6.Text = row.Cells["kode_buku"].Value.ToString();
                textBox1.Text = row.Cells["judul_buku"].Value.ToString();
                textBox2.Text = row.Cells["nama_penulis"].Value.ToString();
                textBox3.Text = row.Cells["genre"].Value.ToString();
                textBox4.Text = row.Cells["deskripsi"].Value.ToString();
            }
        }

        private void ClearForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
    }
}