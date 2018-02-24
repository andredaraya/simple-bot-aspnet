using MongoDB.Bson;
using MongoDB.Driver;
using SimpleBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleBot
{
    public class SimpleBotUser
    {
        private static MongoClient _cliente = new MongoClient();

        public static string Reply(Message message)
        {
            var perfil = GetProfile(message.Id);
            perfil.Visitas += 1;

            SetProfile(message.Id, perfil);

            RegistrarMensagem(message.User, message.Text);

            if (message.Text.ToUpper().Equals("HISTORICO"))
                return ExibirHistorico(message.User);

            return $"{message.User} conversou '{message.Text}' vez(es)";
        }

        public static UserProfile GetProfile(string id)
        {
            var colecao = ObterColecao<UserProfile>("BOT", "Perfil");

            var filtro = Builders<UserProfile>.Filter.Eq("Id", id);
            var resultado = colecao.Find(filtro).SingleOrDefault();

            return resultado != null ? resultado : new UserProfile();
        }

        public static void SetProfile(string id, UserProfile profile)
        {
            var colecao = ObterColecao<UserProfile>("BOT", "Perfil");

            //if(profile.Id == 0)
        }

        private static void RegistrarMensagem(string usuario, string mensagem)
        {
            try
            {
                var colecao = ObterColecao<BsonDocument>("BOT", "Historico");

                var hist = new BsonDocument()
                {
                    {"Usuário", usuario },
                    {"Texto", mensagem },
                    {"Data", DateTime.Now }
                };

                colecao.InsertOne(hist);

                //var colecao = ObterColecao<Mensagem>("BOT", "Historico");;

                //var hist = new Mensagem()
                //{
                //    Usuario = usuario,
                //    Texto = mensagem,
                //    Data = DateTime.Now
                //};

                //colecao.InsertOne(hist);


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static string ExibirHistorico(string usuario)
        {
            try
            {
                var colecao = ObterColecao<BsonDocument>("BOT", "Historico");
                var filtro = Builders<BsonDocument>.Filter.Eq("Usuário", usuario);
                var resultado = colecao.Find(filtro).ToList();

                return resultado.Count() == 0 ? "Sem historico" : resultado.ToString();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static IMongoCollection<T> ObterColecao<T>(string nomeBanco, string nomeColecao)
        {
            //Pega um determinado banco, se não encontra, cria automaticamente
            var database = _cliente.GetDatabase(nomeBanco);
            //Pega a coleção desejada, mesma coisa que especificar a tabela para uso
            var colecao = database.GetCollection<T>(nomeColecao);

            return colecao;
        }
    }


}