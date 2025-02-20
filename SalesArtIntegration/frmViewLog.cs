using MetroFramework.Forms;
using System;
using System.Collections.Generic; 
using System.IO; 
using System.Windows.Forms;

namespace invoiceIntegration
{
    public partial class frmViewLog : MetroForm
    {
        public frmViewLog()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void LogViewForm_Load(object sender, EventArgs e)
        {   
            string line = "";
            try
            {
                List<string> logs = new List<string>();
                using (StreamReader inputFile = new StreamReader("logfile.txt"))
                {
                    while ((line = inputFile.ReadLine()) != null)
                    {
                        string log = line;
                        logs.Add(log);
                    }  
                }

                List<string> logsReverse = new List<string>();
                for (int i = 1; i <= logs.Count; i++)  // son kayıt en başta görünmesi için yazıldı
                {
                    var asdf = logs[logs.Count - i].ToString();
                    logsReverse.Add(logs[logs.Count-i].ToString());
                } 
                listBox1.DataSource = logsReverse; 
            }
            catch 
            {
                MessageBox.Show("Log Dosyası Bulunamadı", "Log Bilgisi", MessageBoxButtons.OK);
            }
        }
        private void btnLogDelete_Click(object sender, EventArgs e)
        {
            string logFolderPath = Path.Combine(Application.StartupPath, "logfile.txt"); // Log dosyanızın tam yolunu belirtin

            try
            {
                if (File.Exists(logFolderPath))
                {
                    File.WriteAllText(logFolderPath, string.Empty); // Log dosyasını temizle

                    // DataSource’u sıfırla ve yeniden ata
                    listBox1.DataSource = null;
                    listBox1.DataSource = new List<string>(); // Boş bir liste ata

                    MessageBox.Show("Log başarıyla temizlendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Log dosyası bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Log temizlenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
