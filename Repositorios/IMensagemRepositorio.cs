using System.Collections.Generic;

namespace SimpleBot.Repositorios
{
    public interface IMensagemRepositorio
    {
        void RegistrarMensagem(string usuario, string mensagem);
        List<string> ListarHistorico(string usuario);
    }
}