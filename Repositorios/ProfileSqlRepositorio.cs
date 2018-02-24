using MongoDB.Driver;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace SimpleBot.Repositorios
{
    public class ProfileSqlRepositorio : IProfileRepositorio
    {
        private string _connectionString = string.Empty;

        public ProfileSqlRepositorio(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public UserProfile GetProfile(string id)
        {
            UserProfile resultado = new UserProfile();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var query = @"SELECT * FROM TB_PROFILE WHERE ID = @ID";
                    SqlCommand comando = new SqlCommand(query, connection);
                    comando.Parameters.Add(new SqlParameter("@ID", id));

                    var result = comando.ExecuteReader();

                    while (result.Read())
                    {
                        resultado.Id = result["ID"].ToString();
                        resultado.Visitas= Convert.ToInt32(result["VISITAS"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultado;
        }

        public void SetProfile(string id, UserProfile profile)
        {
            try
            {
                string query = string.Empty;

                if (string.IsNullOrEmpty(profile.Id))
                    query = @"INSERT INTO TB_PROFILE VALUES (@ID, @VISITAS)";
                else
                    query = @"UPDATE TB_PROFILE SET VISITAS = @VISITAS WHERE ID = @ID";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand comando = new SqlCommand(query, connection);

                    comando.Parameters.Add(new SqlParameter("@ID", id));
                    comando.Parameters.Add(new SqlParameter("@VISITAS", profile.Visitas));
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}