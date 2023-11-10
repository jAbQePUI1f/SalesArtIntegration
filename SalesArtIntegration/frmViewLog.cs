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
    }
}
