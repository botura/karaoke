using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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
        arq.nome = arq.nome.Replace("/","_");
        return arq;
      }
      // else Console.WriteLine("Não existe");
      return null;
    }

    static async Task RunAsync()
    {
      try
      {
        for (int i = 312; i <= 9100; i++)
        {
          clArquivo arquivo = new clArquivo();
          arquivo = await GetMusicaAsync($"http://www.cante.com.br/baixar.php?id={i}");
          if (arquivo != null)
          {
            // salvar o arquivo
            Console.WriteLine($"{i}: Salvando {arquivo.nome} ");

            File.WriteAllBytes($"musicas/{i}_{arquivo.nome}", arquivo.conteudo);
          }
          else Console.WriteLine($"{i}: não existe");
        }
      }
      catch (Exception e)
      { 
        Console.WriteLine(e.Message);
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
