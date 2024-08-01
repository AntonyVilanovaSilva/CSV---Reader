using CSV___Reader.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CSV___Reader.Controllers
{
    public class CsvManipulationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<List<Produto>> CsvLeitor(IFormFile arquivocsv)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            string path;
            path = $"wwwroot/Content/{arquivocsv.Name}.csv";
            try
            {
                if (arquivocsv is not null)
                {
                    using var stream = new MemoryStream();
                    await arquivocsv.CopyToAsync(stream);
                    stream.Position = 0;
                    using var fileStream = new FileStream($"{path}", FileMode.OpenOrCreate);
                    await stream.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                    fileStream.Close();

                    using (var reader = new StreamReader(path))
                    using (var csv = new CsvReader(reader, config))
                    {
                        csv.Context.RegisterClassMap<ProdutoMap>();

                        return csv.GetRecords<Produto>().ToList();
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public void CsvEscritor(string filePath, List<Produto> produto)
        {
            filePath = "C:\\Users\\fusion\\Downloads";
            var finalPath = Path.Combine(filePath, nameof(produto) + ".csv");

            using (var writer = new StreamWriter(finalPath))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(produto);
                }; 
            }
        }
    }

    
}
