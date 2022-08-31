using MiPrimeraApi.Model;
using System.Data;
using System.Data.SqlClient;

namespace MiPrimeraApi.Repository
{
    public static class ProductoHandler
    {
        const string ConnectionString = "Server=localhost;Database=SistemaGestion;Trusted_Connection=true";

        public static List<Producto> TraerProductosPorVenta(int idVenta)
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryTraerProducto = "SELECT * FROM Producto INNER JOIN ProductoVendido ON Producto.Id = ProductoVendido.IdProducto " +
                    "WHERE ProductoVendido.IdVenta = @vIdVenta";

                SqlParameter parametroIdVenta = new SqlParameter();
                parametroIdVenta.ParameterName = "vIdVenta";
                parametroIdVenta.SqlDbType = SqlDbType.BigInt;
                parametroIdVenta.Value = idVenta;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryTraerProducto, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parametroIdVenta);

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Producto producto = new Producto();

                                producto.Id = Convert.ToInt32(dataReader["Id"]);
                                producto.Descripcion = dataReader["Descripciones"].ToString();
                                producto.Costo = Convert.ToDouble(dataReader["Costo"]);
                                producto.PrecioVenta = Convert.ToDouble(dataReader["PrecioVenta"]);
                                producto.Stock = Convert.ToInt32(dataReader["Stock"]);
                                producto.IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]);

                                productos.Add(producto);
                            }
                        }
                        return null;
                    }
                    sqlConnection.Close();
                }
                return productos;
            }
        }
        public static List<Producto> TraerProductos(Usuario pUsuario)
        {
            List<Producto> productos = new List<Producto>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryTraerProductos = "SELECT * FROM Producto WHERE IdUsuario = @vIdUsuario";

                SqlParameter parametroIdUsuario = new SqlParameter();
                parametroIdUsuario.ParameterName = "vIdUsuario";
                parametroIdUsuario.SqlDbType = System.Data.SqlDbType.Int;
                parametroIdUsuario.Value = pUsuario.Id;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryTraerProductos, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parametroIdUsuario);

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Producto producto = new Producto();

                                producto.Id = Convert.ToInt32(dataReader["Id"]);
                                producto.Descripcion = dataReader["Descripciones"]?.ToString();
                                producto.Costo = Convert.ToDouble(dataReader["Costo"]);
                                producto.PrecioVenta = Convert.ToDouble(dataReader["PrecioVenta"]);
                                producto.Stock = Convert.ToInt32(dataReader["Stock"]);
                                producto.IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]);

                                productos.Add(producto);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return productos;
        }


        public static int EliminarProducto(int productoId)
        {
            int resultado = 0;
            int productoVendidoElimExitosa = 0;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {

                    //Verifico que haya ProductoVendido de el Producto a eliminar, ya que si no hay alguno, no hay necesidad de enviar un delete para ProductoVendido
                    string querySelect = "SELECT * FROM ProductoVendido INNER JOIN Producto ON ProductoVendido.IdProducto = Producto.Id " +
                        "WHERE ProductoVendido.IdProducto = @vIdProducto";

                    SqlParameter sqlParameterSelect = new SqlParameter("vIdProducto", SqlDbType.BigInt);
                    sqlParameterSelect.Value = productoId;

                    sqlConnection.Open();

                    using (SqlCommand sqlCommandSelect = new SqlCommand(querySelect, sqlConnection))
                    {
                        sqlCommandSelect.Parameters.Add(sqlParameterSelect);

                        using (SqlDataReader sqlDataReader = sqlCommandSelect.ExecuteReader())
                        {
                            if (sqlDataReader.HasRows)
                            {
                                //Elimino el Producto en ProductoVendido y verifico si la eliminacion fue exitosa
                                productoVendidoElimExitosa = ProductoVendidoHandler.EliminarProductoVendidoPorProducto(productoId);
                            }

                            string queryDelete = "DELETE FROM Producto WHERE Id = @vId";
                            SqlParameter sqlParameterDelete = new SqlParameter("vId", SqlDbType.BigInt);
                            sqlParameterDelete.Value = productoId;

                            using (SqlCommand sqlCommandDelete = new SqlCommand(queryDelete, sqlConnection))
                            {
                                sqlCommandDelete.Parameters.Add(sqlParameterDelete);
                                int numberOfRows = sqlCommandDelete.ExecuteNonQuery();
                                if (numberOfRows > 0)
                                {
                                    if (productoVendidoElimExitosa == 1)
                                    {
                                        resultado = 2;
                                    }
                                    else
                                    {
                                        resultado = 1;
                                    }
                                }
                            }
                        }
                    }
                    sqlConnection.Close();
                }
                return (resultado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static bool CrearProducto(Producto producto/*, Usuario usuario*/)
        {
            bool resultado = false;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryInsert = "INSERT INTO Producto " +
                        "(Descripciones, Costo, PrecioVenta, Stock, IdUsuario) VALUES " +
                        "(@vDescripcionParameter, @vCostoParameter, @vPrecioVentaParameter, @vStockParameter, @vIdUsuarioParameter);";

                    SqlParameter descripcionParameter = new SqlParameter("vDescripcionParameter", SqlDbType.VarChar) { Value = producto.Descripcion };
                    SqlParameter costoParameter = new SqlParameter("vCostoParameter", SqlDbType.Money) { Value = producto.Costo };
                    SqlParameter precioVentaParameter = new SqlParameter("vPrecioVentaParameter", SqlDbType.Money) { Value = producto.PrecioVenta };
                    SqlParameter stockParameter = new SqlParameter("vStockParameter", SqlDbType.Int) { Value = producto.Stock };
                    SqlParameter idUsuarioParameter = new SqlParameter("vIdUsuarioParameter", SqlDbType.BigInt) { Value = /*usuario.Id*/producto.IdUsuario };

                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(descripcionParameter);
                        sqlCommand.Parameters.Add(costoParameter);
                        sqlCommand.Parameters.Add(precioVentaParameter);
                        sqlCommand.Parameters.Add(stockParameter);
                        sqlCommand.Parameters.Add(idUsuarioParameter);

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


        public static bool ModificarProducto(Producto producto)
        {
            bool resultado = false;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryUpdate = "UPDATE Producto SET Descripciones = @vDescripcionParameter, Costo = @vCostoParameter" +
                        ", PrecioVenta = @vPrecioVentaParameter, Stock = @vStockParameter WHERE Id = @vIdParameter";

                    SqlParameter idParameter = new SqlParameter("vIdParameter", SqlDbType.Int) { Value = producto.Id };
                    SqlParameter descripcionParameter = new SqlParameter("vDescripcionParameter", SqlDbType.VarChar) { Value = producto.Descripcion };
                    SqlParameter costoParameter = new SqlParameter("vCostoParameter", SqlDbType.Money) { Value = producto.Costo };
                    SqlParameter precioVentaParameter = new SqlParameter("vPrecioVentaParameter", SqlDbType.Money) { Value = producto.PrecioVenta };
                    SqlParameter stockParameter = new SqlParameter("vStockParameter", SqlDbType.Int) { Value = producto.Stock };

                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(descripcionParameter);
                        sqlCommand.Parameters.Add(costoParameter);
                        sqlCommand.Parameters.Add(precioVentaParameter);
                        sqlCommand.Parameters.Add(stockParameter);
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public static bool RestarStockProducto(Producto producto, int idUsuario)
        {
            bool resultado = false;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryUpdate = "UPDATE Producto SET Stock = Stock - @vStockParameter WHERE Id = @vIdParameter " +
                        "AND IdUsuario = @vIdUsuario";

                    SqlParameter idParameter = new SqlParameter("vIdParameter", SqlDbType.Int) { Value = producto.Id };
                    SqlParameter stockParameter = new SqlParameter("vStockParameter", SqlDbType.Int) { Value = producto.Stock };
                    SqlParameter idUsuarioParameter = new SqlParameter("vIdUsuario", SqlDbType.BigInt) { Value = idUsuario };

                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(stockParameter);
                        sqlCommand.Parameters.Add(idParameter);
                        sqlCommand.Parameters.Add(idUsuarioParameter);

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
        public static bool RecuperarStockProducto(Producto producto, ProductoVendido productoVendido)
        {
            bool resultado = false;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryUpdate = "UPDATE Producto SET Stock = Stock + @vStockParameter WHERE Id = @vIdParameter " +
                        "AND IdUsuario = @vIdUsuario";

                    SqlParameter idParameter = new SqlParameter("vIdParameter", SqlDbType.Int) { Value = producto.Id };
                    SqlParameter stockParameter = new SqlParameter("vStockParameter", SqlDbType.Int) { Value = productoVendido.Stock };

                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(stockParameter);
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

    }
}
