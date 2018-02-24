using SimpleBot.Repositorios;

namespace SimpleBot
{
    public class SimpleBotUser
    {
        private static IMensagemRepositorio _msgRepo = new MensagemMongoRepositorio();
        private static IProfileRepositorio _profileRepo = new ProfileMongoRepositorio();

        public static string Reply(Message message)
        {
            var perfil = _profileRepo.GetProfile(message.Id);
            perfil.Visitas += 1;

            _profileRepo.SetProfile(message.Id, perfil);

            _msgRepo.RegistrarMensagem(message.User, message.Text);

            if (message.Text.ToUpper().Equals("HISTORICO"))
                return _msgRepo.ListarHistorico(message.User).ToString();

            return $"{message.User} conversou '{perfil.Visitas}' vez(es)";
        }
    }


}