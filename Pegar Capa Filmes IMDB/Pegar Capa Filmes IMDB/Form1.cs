using System.Text.RegularExpressions;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pegar_Capa_Filmes_IMDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox2.Visible = false;
            label1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Visible = false;
            label1.Visible = true;
            label1.Text = "Aguarde...";
            Thread.Sleep(1000);
            buscar();
        }

        async Task buscar()
        {
            string url = textBox1.Text;
            string html = await GetHtmlAsync(url);

            string pattern = @"""([^""]*380w[^""]*)""";
            Match match = Regex.Match(html, pattern);

            if (match.Success)
            {
                string[] f = match.Groups[1].Value.Split("https");
                int ct = f.Count();


                string passo1 = "https" + f[ct - 1].Replace(" 380w", "");
               // string passo2 = passo1.Replace("562_", "1200_");
               // string passo3 = passo2.Replace("0,380,", "0,0,");
                textBox2.Text = passo1;

                textBox2.Visible = true;
                label1.Visible = false;
            }
            else
            {
                textBox2.Text = "Nenhuma ocorrência encontrada.";
            }
        }

        static async Task<string> GetHtmlAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetStringAsync(url);
            }
        }

    }
}