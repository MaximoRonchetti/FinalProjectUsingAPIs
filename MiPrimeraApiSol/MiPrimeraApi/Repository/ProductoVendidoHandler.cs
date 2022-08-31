using MiPrimeraApi.Model;
using System.Data;
using System.Data.SqlClient;

namespace MiPrimeraApi.Repository
{
    public static class ProductoVendidoHandler 
    {
        const string ConnectionString = "Server=localhost;Database=SistemaGestion;Trusted_Connection=true";
        public static List<ProductoVendido> TraerProductosVendidos(Usuario pUsuario)
        {
            List<ProductoVendido> productosVendidos = new List<ProductoVendido>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryTraerProductos = "SELECT ProductoVendido.Id, ProductoVendido.Stock, ProductoVendido.IdProducto, ProductoVendido.IdVenta " +
                    "FROM ProductoVendido INNER JOIN Producto ON ProductoVendido.IdProducto = Producto.Id WHERE Producto.IdUsuario = @vIdUsuario";

                SqlParameter parametroIdUsuario = new SqlParameter();
                parametroIdUsuario.ParameterName = "vIdUsuario";
                parametroIdUsuario.SqlDbType = SqlDbType.Int;
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
                                ProductoVendido productoVendido = new ProductoVendido();

                                productoVendido.Id = Convert.ToInt32(dataReader["Id"]);
                                productoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.IdProducto = Convert.ToInt32(dataReader["IdProducto"]);
                                productoVendido.IdVenta = Convert.ToInt32(dataReader["IdVenta"]);

                                productosVendidos.Add(productoVendido);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return productosVendidos;
        }
        public static ProductoVendido TraerProductoVendidoPorVenta(int idVenta, Producto producto)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryTraerProductoVendido = "SELECT * FROM ProductoVendido WHERE ProductoVendido.IdVenta = @vIdVenta AND " +
                    "ProductoVendido.IdProducto = @vIdProducto ";

                SqlParameter parametroIdVenta = new SqlParameter();
                parametroIdVenta.ParameterName = "vIdVenta";
                parametroIdVenta.SqlDbType = SqlDbType.BigInt;
                parametroIdVenta.Value = idVenta;

                SqlParameter parametroIdProducto = new SqlParameter();
                parametroIdProducto.ParameterName = "vIdProducto";
                parametroIdProducto.SqlDbType = SqlDbType.BigInt;
                parametroIdProducto.Value = producto.Id;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryTraerProductoVendido, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parametroIdVenta);

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ProductoVendido productoVendido = new ProductoVendido();

                                productoVendido.Id = Convert.ToInt32(dataReader["Id"]);
                                productoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.IdProducto = Convert.ToInt32(dataReader["IdProducto"]);
                                productoVendido.IdVenta = Convert.ToInt32(dataReader["IdVenta"]);

                                return productoVendido;
                            }
                        }
                        return null;
                    }
                    sqlConnection.Close();
                }
            }
        }


        public static int EliminarProductoVendidoPorProducto(int productoId)
        {
            int resultado = 0;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryDelete = "DELETE FROM ProductoVendido WHERE IdProducto = @vId";

                    SqlParameter sqlParameter = new SqlParameter("vId", SqlDbType.BigInt);
                    sqlParameter.Value = productoId;

                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(sqlParameter);
                        int numberOfRows = sqlCommand.ExecuteNonQuery();
                        if (numberOfRows > 0)
                        {
                            resultado = 1;
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
        public static int EliminarProductosVendidoPorVenta(int ventaId)
        {
            int resultado = 0;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryDelete = "DELETE FROM ProductoVendido WHERE IdVenta = @vId";

                    SqlParameter sqlParameter = new SqlParameter("vId", SqlDbType.BigInt);
                    sqlParameter.Value = ventaId;

                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(sqlParameter);
                        int numberOfRows = sqlCommand.ExecuteNonQuery();
                        if (numberOfRows > 0)
                        {
                            resultado = numberOfRows;
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


        public static bool AgregarProductoVendido(Producto producto, Venta venta)
        {
            bool resultado = false;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryInsert = "INSERT INTO ProductoVendido " +
                        "(Stock, IdProducto, IdVenta,) VALUES " +
                        "(@vStockParameter, @vIdProductoParameter, @vIdVentaParameter,)";

                    SqlParameter stockParameter = new SqlParameter("vStockParameter", SqlDbType.Int) { Value = producto.Stock };
                    SqlParameter idProductoParameter = new SqlParameter("vIdProductoParameter", SqlDbType.BigInt) { Value = producto.Id };
                    SqlParameter idVentaParameter = new SqlParameter("vIdVentaParameter", SqlDbType.BigInt) { Value = venta.Id };

                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(stockParameter);
                        sqlCommand.Parameters.Add(idProductoParameter);
                        sqlCommand.Parameters.Add(idVentaParameter);

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
