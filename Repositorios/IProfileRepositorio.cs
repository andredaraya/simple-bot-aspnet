namespace SimpleBot.Repositorios
{
    public interface IProfileRepositorio
    {
        UserProfile GetProfile(string id);
        void SetProfile(string id, UserProfile profile);
    }
}
