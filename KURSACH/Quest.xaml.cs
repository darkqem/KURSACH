using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MongoDB.Bson.Serialization;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace KURSACH
{
    /// <summary>
    /// Логика взаимодействия для Quest.xaml
    /// </summary>
    public partial class Quest : Window
    {

        private IMongoDatabase _database;
        private MongoClient client;

        public class Situa
        {
            public string Id { get; set; }
            public string situation { get; set; }
            public List<string> solution { get; set; } = new List<string>();
        }

        public Quest()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ConnectToDatabase().GetAwaiter().GetResult();
            var g = GetRandomSituationAndSolutionAsync();
                        
        }

        public async Task ConnectToDatabase()
        {
            try
            {
                client = new MongoClient("mongodb+srv://admin:3p6oRw9VJ92Mn6vE@cluster0.tdescqt.mongodb.net/");
                _database = client.GetDatabase("kr");    
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<string> GetRandomSituationAndSolutionAsync()
        {
            if (_database == null) return "Failed to connect to database.";

            var collection = _database.GetCollection<BsonDocument>("Level_1");

            // Получаем все документы из коллекции
            var documents = await collection.FindAsync(_ => true).Result.ToListAsync();

            if (!documents.Any()) return "No situations found in the database.";

            // Выбираем случайный документ
            var randomIndex = new Random().Next(0, documents.Count);
            var selectedDocument = documents[randomIndex];

            // Преобразуем документ в строку JSON
            string situation = selectedDocument.ToJson();
            Debug.WriteLine(situation);

            string input = situation;
            
            Regex regex = new Regex(@"""situation"" : ""([^""]+)""", RegexOptions.Singleline);
            Match match = regex.Match(input);

            if (match.Success)
            {
                // Извлечение группы 1, которая содержит нужную часть строки
                string result = match.Groups[1].Value.Trim('"');
                string formattedText = Regex.Replace(result, "Ситуация:", "\nСитуация:");
                Situation.Text = formattedText;
                
            }
            else
            {
                Debug.WriteLine("No match found.");
            }
 
            return situation;
        }
    }
}
