using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using MongoDB.Driver.Core.Operations;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MongoDB.Bson.Serialization.Attributes;
using System;
using MongoDB.Bson.Serialization;

namespace KURSACH
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        private  IMongoDatabase _database;
        private MongoClient client;

        public class User
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }

            public string nickname { get; set; }
            public string password { get; set; }
            

        }

        public Login()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ConnectToDatabase().GetAwaiter().GetResult();
        }

        private async Task ConnectToDatabase()
        {
            try
            {
                client = new MongoClient("mongodb+srv://admin:3p6oRw9VJ92Mn6vE@cluster0.tdescqt.mongodb.net/");
                _database = client.GetDatabase("users");
                Console.WriteLine("Connected to database successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to database: {ex.Message}");
            }
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => ConnectToDatabase());

            var collection = _database.GetCollection<User>("user");
            var user = collection.Find(u => u.nickname == txtUsername.Text && u.password == txtPassword.Password).FirstOrDefault();



            if (user != null)
            {

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                this.Close();

            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль");
            }
        }

        private async void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => ConnectToDatabase());

            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }
            var client = new MongoClient("mongodb+srv://admin:3p6oRw9VJ92Mn6vE@cluster0.tdescqt.mongodb.net/");
            var database = client.GetDatabase("users");
            var collection = database.GetCollection<User>("user");
            var user = collection.Find(u => u.nickname == txtUsername.Text).FirstOrDefault();
            if (user == null)
            {
                user = new User
                {
                    nickname = txtUsername.Text,
                    password = txtPassword.Password
                };
                collection.InsertOne(user);
                MessageBox.Show("Регистрация успешна!");
            }
            else
            {
                MessageBox.Show("Этот никнейм уже используется.");
            }

        }
    }
}
