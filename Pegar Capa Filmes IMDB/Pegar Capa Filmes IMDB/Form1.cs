using System.Text.RegularExpressions;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;

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

            //--------------------------------------------
            string tituklo = await GetStringAsync(url);
            Match dddddddddddd = Regex.Match(html, @"<title>\s*(.+?)\s*</title>", RegexOptions.IgnoreCase);

            //--------------------------------------------

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

                BaixarIMG(passo1, dddddddddddd.Groups[1].Value);

                textBox2.Visible = true;
                label1.Visible = false;
            }
            else
            {
                textBox2.Text = "Nenhuma ocorrência encontrada.";
            }
        }

        static async Task<string> GetStringAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetStringAsync(url);
            }
        }
        static async Task<string> GetHtmlAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetStringAsync(url);
            }
        }
        async Task BaixarIMG(string imageUrl, string nome)
        {
           // string localPath = GerarSenha() + ".jpg";
            string localPath = nome.Replace(" - IMDb", "") + ".jpg";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    byte[] imageBytes = await client.GetByteArrayAsync(imageUrl);
                    await File.WriteAllBytesAsync(localPath, imageBytes);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = $"/select,\"{localPath}\"",
                        UseShellExecute = true
                    });

                }
                catch (Exception ex)
                {
                    textBox2.Text = $"Erro ao baixar imagem: {ex.Message}";
                }
            }
        }

        public static string GerarSenha(int tamanho = 10)
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder senha = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < tamanho; i++)
            {
                int index = rnd.Next(caracteres.Length);
                senha.Append(caracteres[index]);
            }

            return senha.ToString();
        }



    }
}