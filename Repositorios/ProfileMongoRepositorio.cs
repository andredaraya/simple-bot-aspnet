using MongoDB.Driver;
using System.Linq;

namespace SimpleBot.Repositorios
{
    public class ProfileMongoRepositorio : IProfileRepositorio
    {
        private static MongoClient _cliente;
        private string _connectionString = string.Empty;
        private readonly string _nomeBanco = "BOT";
        private readonly string _nomeColecao = "Perfil";

        public ProfileMongoRepositorio()
        {
            if (_cliente == null)
                _cliente = new MongoClient();
        }

        public ProfileMongoRepositorio(string connectionString)
        {
            this._connectionString = connectionString;

            if (_cliente == null)
                _cliente = new MongoClient(_connectionString);
        }

        public UserProfile GetProfile(string id)
        {
            var colecao = ObterColecao<UserProfile>(_nomeBanco, _nomeColecao);

            var filtro = Builders<UserProfile>.Filter.Eq("Id", id);
            var resultado = colecao.Find(filtro).SingleOrDefault();

            return resultado != null ? resultado : new UserProfile();
        }

        public void SetProfile(string id, UserProfile profile)
        {
            var colecao = ObterColecao<UserProfile>(_nomeBanco, _nomeColecao);

            if (string.IsNullOrEmpty(profile.Id))
                profile.Id = id;

            var filtro = Builders<UserProfile>.Filter.Eq("Id", id);
            var opcao = new UpdateOptions();
            //Se o registro não é encontrado, insere um novo
            opcao.IsUpsert = true;

            colecao.ReplaceOne(filtro, profile, opcao);
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