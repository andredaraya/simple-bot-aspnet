using MongoDB.Driver;
using SimpleBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBot.Repositorios
{
    public class MensagemMongoRepositorio : IMensagemRepositorio
    {
        private static MongoClient _cliente;
        private string _connectionString = string.Empty;
        private readonly string _nomeBanco = "BOT";
        private readonly string _nomeColecao = "Historico";

        public MensagemMongoRepositorio()
        {
            if (_cliente == null)
                _cliente = new MongoClient();
        }

        public MensagemMongoRepositorio(string connectionString)
        {
            this._connectionString = connectionString;

            if (_cliente == null)
                _cliente = new MongoClient(_connectionString);
        }

        public void RegistrarMensagem(string usuario, string mensagem)
        {
            try
            {
                //var colecao = ObterColecao<BsonDocument>("BOT", "Historico");

                //var hist = new BsonDocument()
                //{
                //    {"Usuário", usuario },
                //    {"Texto", mensagem },
                //    {"Data", DateTime.Now }
                //};

                //colecao.InsertOne(hist);

                var colecao = ObterColecao<Mensagem>(_nomeBanco, _nomeColecao); ;

                var hist = new Mensagem()
                {
                    Usuario = usuario,
                    Texto = mensagem,
                    Data = DateTime.Now
                };

                colecao.InsertOne(hist);


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<string> ListarHistorico(string usuario)
        {
            List<string> resultado = new List<string>();

            try
            {
                var colecao = ObterColecao<Mensagem>("BOT", "Historico");
                var filtro = Builders<Mensagem>.Filter.Eq("Usuário", usuario);
                var result = colecao.Find(filtro).ToList();

                result.ForEach(msg => resultado.Add(msg.ToString()));

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IMongoCollection<T> ObterColecao<T>(string nomeBanco, string nomeColecao)
        {
            //Pega um determinado banco, se não encontra, cria automaticamente
            var database = _cliente.GetDatabase(nomeBanco);
            //Pega a coleção desejada, mesma coisa que especificar a tabela para uso
            var colecao = database.GetCollection<T>(nomeColecao);

            return colecao;
        }
    }
}