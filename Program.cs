using System;
using System.Drawing;
using System.Threading;

namespace didaticos.redimencionador
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Iniciando redimensionador");

            Thread thread = new System.Threading.Thread(Redimensionado);
            thread.Start();




            Console.Read();
        }

        static void Redimensionado()
        {
            #region "Diretorios"
            string entrada = "Arquivo_Entrada";
            string redimencionado = "Redimencionado";
            string finalizados = "Arquivos Finalizados";

            if (!Directory.Exists(entrada))
            {
                Directory.CreateDirectory(entrada);
            }

            if (!Directory.Exists(redimencionado))
            {
                Directory.CreateDirectory(redimencionado);
            }

            if (!Directory.Exists(finalizados))
            {
                Directory.CreateDirectory(finalizados);
            }
            #endregion

            FileStream fileStream;
            FileInfo fileInfo;

            while (true)
            {
                //Meu programa vai olhar para a pasta de entrada
                //SE tiver arquivo, ele erá redimencionar
                var arquivosEntrada = Directory.EnumerateFiles(entrada);

                //ler o tamanho que irá redimensionar
                int novaAltura = 200;

                //Duas ou mais alturas

                foreach (var arquivo in arquivosEntrada)
                {
                    fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite);
                    fileInfo = new FileInfo(arquivo);

                    string caminho = Environment.CurrentDirectory + @"\" + redimencionado +
                        @"\" + DateTime.Now.Millisecond.ToString() + @"\" + fileInfo.Name;


                    //redimenciona + //copia os arquivos redimencionados para a pasta de redimensionados
                    Redimencionador(Image.FromStream(fileStream), novaAltura, caminho);

                    //fecha o arquivo
                    fileStream.Close();

                    //move arquivos de entrada para a pasta de finalizados
                    string caminhoFinalizado = Environment.CurrentDirectory + @"\" + finalizados + @"\" + fileInfo.Name;
                    fileInfo.MoveTo(finalizados);

                }
                Thread.Sleep(new TimeSpan(0, 0, 3));

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagem">Imagem a ser redimencionada</param>
        /// <param name="altura">Altura que desejamos redimencionar</param>
        /// <param name="caminho">Caminho aonde iremos gravar o arquivo redimencionado</param>
        /// <returns></returns>

        static void Redimencionador(Image imagem, int altura, string caminho)
        {
            double ratio = (double)altura / imagem.Height;
            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImagem = new Bitmap(novaLargura, novaAltura);

            using (Graphics g = Graphics.FromImage(novaImagem))
            {
                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
            }

            novaImagem.Save(caminho);
            imagem.Dispose();



        }
    }
}