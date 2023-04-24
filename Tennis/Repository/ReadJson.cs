using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.Compression;
using System.Text.Json;
using Tennis.Models;


namespace Tennis.Repository
{
    public class ReadJson : IReadJson<Player>
    {
        private readonly string _jsonFilePath = "headtohead.json";
        private string path;

        public ReadJson()
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            path = Path.Combine(root, _jsonFilePath);
        }

        public async Task<IEnumerable<Player>?> GetAllPlayers()
        {
            if (!File.Exists(path))
            {
                return null;
            }
                
            using StreamReader reader = new(path);
            string json = await reader.ReadToEndAsync();
                        
            var root = JsonConvert.DeserializeObject<Root>(json);
            var players = root["players"];


            return players;
        }

        public async Task<Player?> Get(int id)
        {
            if (!File.Exists(path))
            {
                throw new InvalidDataException();
            }

            using StreamReader reader = new(path);
            var json = await reader.ReadToEndAsync();
            var root = JsonConvert.DeserializeObject<Root>(json);
            var players = root["players"];

            //List<Player>? entities = JsonConvert.DeserializeObject<List<Player>>(json);
            return players?.Where(e => e.Id == id).FirstOrDefault();
        }

        public async Task<(string Country, double IMC, double Mediane)> GetStatistic()
        {
            if (!File.Exists(path))
            {
                throw new InvalidDataException();
            }

            using StreamReader reader = new(path);
            var json = await reader.ReadToEndAsync();
            var root = JsonConvert.DeserializeObject<Root>(json);
            var entities = root["players"];

            //List<Player>? entities = JsonConvert.DeserializeObject<List<Player>>(json);
            var country = entities!.GroupBy(c => c.Country.Code)
                                .Select(x => new { country = x.Key, points = x.Sum(y => y.Data.Points) }).OrderByDescending(x => x.points).Select(x => x.country).First();                

            //IMC = poids (kg) / taille² (m)
            double imc = (double) entities!.Select(x => ((x.Data.Weight/1000)/(Math.Pow((x.Data.Height/100), 2)))).Average();
            double mediane = 0;

            return (Country: country, IMC: imc, Mediane: mediane);

        }

    }

    public class Root : Dictionary<string, List<Player>> { }
}
