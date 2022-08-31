using MiPrimeraApi.Model;
using System.Data;
using System.Data.SqlClient;

namespace MiPrimeraApi.Repository
{
    public static class UsuarioHandler
    {
        const string ConnectionString = "Server=localhost;Database=SistemaGestion;Trusted_Connection=true";
        public static Usuario TraerUsuario(string pNombreUsuario)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryTraerUsuario = "SELECT * FROM Usuario WHERE NombreUsuario = @vNombreUsuario";

                SqlParameter parametroNombreUsuario = new SqlParameter();
                parametroNombreUsuario.ParameterName = "vNombreUsuario";
                parametroNombreUsuario.SqlDbType = System.Data.SqlDbType.VarChar;
                parametroNombreUsuario.Value = pNombreUsuario;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryTraerUsuario, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parametroNombreUsuario);

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Usuario usuario = new Usuario();

                                usuario.Id = Convert.ToInt32(dataReader["Id"]);
                                usuario.NombreUsuario = dataReader["NombreUsuario"].ToString();
                                usuario.Nombre = dataReader["Nombre"].ToString();
                                usuario.Apellido = dataReader["Apellido"].ToString();
                                usuario.Contraseña = dataReader["Contraseña"].ToString();
                                usuario.Mail = dataReader["Mail"].ToString();

                                return usuario;
                            }
                        }
                        return null;
                    }
                    sqlConnection.Close();
                }
            }
        }


        internal static Usuario IniciarSesion(string pNombreUsuario, string pConstraseña)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string querySelectLogin = "SELECT NombreUsuario, Contraseña FROM Usuario WHERE NombreUsuario = @vNombreUsuario" +
                  " AND Contraseña = @vContraseña";

                SqlParameter parametroNombreUsuario = new SqlParameter();
                parametroNombreUsuario.ParameterName = "vNombreUsuario";
                parametroNombreUsuario.SqlDbType = System.Data.SqlDbType.VarChar;
                parametroNombreUsuario.Value = pNombreUsuario;

                SqlParameter parametroConstraseña = new SqlParameter();
                parametroConstraseña.ParameterName = "vContraseña";
                parametroConstraseña.SqlDbType = System.Data.SqlDbType.VarChar;
                parametroConstraseña.Value = pConstraseña;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(querySelectLogin, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parametroNombreUsuario);
                    sqlCommand.Parameters.Add(parametroConstraseña);

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            if (dataReader.Read())
                            {
                                Usuario usuario = new Usuario();

                                usuario.Id = Convert.ToInt32(dataReader["Id"]);
                                usuario.NombreUsuario = dataReader["NombreUsuario"].ToString();
                                usuario.Nombre = dataReader["Nombre"].ToString();
                                usuario.Apellido = dataReader["Apellido"].ToString();
                                usuario.Contraseña = dataReader["Contraseña"].ToString();
                                usuario.Mail = dataReader["Mail"].ToString();


                                return usuario;
                            }
                            else
                            {

                                Usuario usuario = new Usuario();

                                usuario.Id = 0;
                                usuario.NombreUsuario = String.Empty;
                                usuario.Nombre = String.Empty;
                                usuario.Apellido = String.Empty;
                                usuario.Contraseña = String.Empty;
                                usuario.Mail = String.Empty;


                                return usuario;
                            }
                        }
                        else
                        {

                            Usuario usuario = new Usuario();

                            usuario.Id = 0;
                            usuario.NombreUsuario = String.Empty;
                            usuario.Nombre = String.Empty;
                            usuario.Apellido = String.Empty;
                            usuario.Contraseña = String.Empty;
                            usuario.Mail = String.Empty;


                            return usuario;
                        }
                        sqlConnection.Close();
                    }
                }
            }
        }


        public static bool ModificarUsuario(Usuario usuario)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryUpdate = "UPDATE Usuario SET Nombre = @vNombre, Apellido = @vApellido, NombreUsuario = @vNombreUsuario," +
                    " Contraseña = @vConstraseña, Mail = @vMail WHERE Id = @vId ";

                SqlParameter nombreParameter = new SqlParameter("vNombre", SqlDbType.VarChar) { Value = usuario.Nombre };
                SqlParameter apellidoParameter = new SqlParameter("vApellido", SqlDbType.VarChar) { Value = usuario.Apellido };
                SqlParameter nombreUsuarioParameter = new SqlParameter("vNombreUsuario", SqlDbType.VarChar) { Value = usuario.NombreUsuario };
                SqlParameter constraseñaParameter = new SqlParameter("vConstraseña", SqlDbType.VarChar) { Value = usuario.Contraseña };
                SqlParameter mailParameter = new SqlParameter("vMail", SqlDbType.VarChar) { Value = usuario.Mail };
                SqlParameter idParameter = new SqlParameter("vId", SqlDbType.BigInt) { Value = usuario.Id };

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.Add(nombreParameter);
                    sqlCommand.Parameters.Add(apellidoParameter);
                    sqlCommand.Parameters.Add(nombreUsuarioParameter);
                    sqlCommand.Parameters.Add(constraseñaParameter);
                    sqlCommand.Parameters.Add(mailParameter);
                    sqlCommand.Parameters.Add(idParameter);

                    int numberOfRows = sqlCommand.ExecuteNonQuery();

                    if (numberOfRows > 0)
                    {
                        resultado = true;
                    }
                }

                sqlConnection.Close();
            }

            return resultado;
        }


        public static bool CrearUsuario(Usuario usuario)
        {
            bool resultado = false;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryInsert = "INSERT INTO Usuario " +
                        "(Nombre, Apellido, NombreUsuario, Constraseña, Mail) VALUES " +
                        "(@vNombre, @vApellido, @vNombreUsuario, @vContraseña, @vMail);";

                    SqlParameter nombreParameter = new SqlParameter("vNombre", SqlDbType.VarChar) { Value = usuario.Nombre };
                    SqlParameter apellidoParameter = new SqlParameter("vApellido", SqlDbType.VarChar) { Value = usuario.Apellido };
                    SqlParameter nombreUsuarioParameter = new SqlParameter("vNombreUsuario", SqlDbType.VarChar) { Value = usuario.NombreUsuario };
                    SqlParameter contraseñaParameter = new SqlParameter("vConstraseña", SqlDbType.VarChar) { Value = usuario.Contraseña };
                    SqlParameter mailParameter = new SqlParameter("vMail", SqlDbType.VarChar) { Value = usuario.Mail };

                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                    {
                        if (TraerUsuario(usuario.NombreUsuario) == null)
                        {
                            if (!String.IsNullOrEmpty(usuario.Nombre))
                            {
                                sqlCommand.Parameters.Add(nombreParameter);
                            }
                            if (!String.IsNullOrEmpty(usuario.Apellido))
                            {
                                sqlCommand.Parameters.Add(apellidoParameter);
                            }
                            if (!String.IsNullOrEmpty(usuario.NombreUsuario))
                            {
                                sqlCommand.Parameters.Add(nombreUsuarioParameter);
                            }
                            if (!String.IsNullOrEmpty(usuario.Contraseña))
                            {
                                sqlCommand.Parameters.Add(contraseñaParameter);
                            }
                            if (!String.IsNullOrEmpty(usuario.Mail))
                            {
                                sqlCommand.Parameters.Add(mailParameter);
                            }
                        }
                        else
                        {
                            return resultado;
                        }

                        int numberOfRows = sqlCommand.ExecuteNonQuery();

                        if (numberOfRows > 0)
                        {
                            resultado = true;
                        }
                    }

                    sqlConnection.Close();
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static bool EliminarUsuario(int usuarioId)
        {
            bool resultado = false;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryDelete = "DELETE FROM Usuario WHERE Id = @vId";
                    SqlParameter sqlParameterDelete = new SqlParameter("vId", SqlDbType.BigInt);
                    sqlParameterDelete.Value = usuarioId;

                    sqlConnection.Open();

                    using (SqlCommand sqlCommandDelete = new SqlCommand(queryDelete, sqlConnection))
                    {
                        sqlCommandDelete.Parameters.Add(sqlParameterDelete);
                        int numberOfRows = sqlCommandDelete.ExecuteNonQuery();
                        if (numberOfRows > 0)
                        {
                            resultado = true;
                        }
                    }

                    sqlConnection.Close();
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}

