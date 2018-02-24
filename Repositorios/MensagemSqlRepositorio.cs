using SimpleBot.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimpleBot.Repositorios
{
    public class MensagemSqlRepositorio : IMensagemRepositorio
    {
        private string _connectionString = string.Empty;

        public MensagemSqlRepositorio(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public void RegistrarMensagem(string usuario, string mensagem)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var query = @"INSERT INTO TB_HISTORICO VALUES (NULL, @USUARIO, @TEXTO, @DATA)";
                    SqlCommand comando = new SqlCommand(query, connection);

                    comando.Parameters.Add(new SqlParameter("@USUARIO", usuario));
                    comando.Parameters.Add(new SqlParameter("@TEXTO", mensagem));
                    comando.Parameters.Add(new SqlParameter("@DATA", DateTime.Now.ToString()));

                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> ListarHistorico(string usuario)
        {
            List<string> resultado = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var query = @"SELECT * FROM TB_HISTORICO WHERE USUARIO = @USUARIO";
                    SqlCommand comando = new SqlCommand(query, connection);
                    comando.Parameters.Add(new SqlParameter("@USUARIO", usuario));

                    var result = comando.ExecuteReader();

                    while (result.Read())
                    {
                        var mensagem = new Mensagem();
                        mensagem.Usuario = result["USUARIO"].ToString();
                        mensagem.Texto = result["TEXTO"].ToString();
                        mensagem.Data = Convert.ToDateTime(result["DATA"]);

                        resultado.Add(mensagem.ToString());
                    }
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}