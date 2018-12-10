using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace myApp
{
  class Program
  {
    static HttpClient client = new HttpClient();

    static async Task<clArquivo> GetMusicaAsync(string path)
    {
      HttpResponseMessage response = await client.GetAsync(path);
      if (response.IsSuccessStatusCode)
      {
        clArquivo arq = new clArquivo();
        arq.conteudo = await response.Content.ReadAsByteArrayAsync();
        arq.nome = response.Content.Headers.ContentDisposition.FileName.Trim('"');

        arq.nome = Regex.Replace(arq.nome, @"<[^>]+>|&nbsp;", "").Trim();
        arq.nome = Regex.Replace(arq.nome, @"[^\u0020-\u007F]", String.Empty);

        arq.nome = arq.nome.Replace("/", "_");
        arq.nome = arq.nome.Replace("?", "_");
        arq.nome = arq.nome.Replace(" ", "_");
        // Console.WriteLine(arq.nome);
        return arq;
      }
      // else Console.WriteLine("Não existe");
      return null;
    }

    static async Task RunAsync()
    {
      for (int i = 4400; i <= 9100; i++)
      {
        try
        {
          clArquivo arquivo = new clArquivo();
          arquivo = await GetMusicaAsync($"http://www.cante.com.br/baixar.php?id={i}");
          if (arquivo != null)
          {
            // salvar o arquivo
            Console.WriteLine($"{i}: {arquivo.nome} ");

            File.WriteAllBytes($"musicas/{i}_{arquivo.nome}", arquivo.conteudo);
          }
          else Console.WriteLine($"{i}: não existe");
        }
        catch (Exception e)
        {
          Console.WriteLine($"{i}: ERRO - {e.Message}");
        }
      }
    }
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      RunAsync().GetAwaiter().GetResult();
    }
  }
}

public class clArquivo
{
  public string nome;
  public byte[] conteudo;
}
