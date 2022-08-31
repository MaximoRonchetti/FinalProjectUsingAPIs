using MiPrimeraApi.Model;
using System.Data;
using System.Data.SqlClient;

namespace MiPrimeraApi.Repository
{
    public static class VentaHandler
    {
        public const string ConnectionString = "Server=localhost;Database=SistemaGestion;Trusted_Connection=true";
        public static List<Venta> TraerVentas(Usuario pUsuario)
        {
            List<Venta> ventas = new List<Venta>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryTraerVentas = "SELECT Venta.Id, Venta.Comentarios FROM (Venta INNER JOIN ProductoVendido ON Venta.Id = ProductoVendido.IdVenta)" +
                    " INNER JOIN Producto ON ProductoVendido.IdProducto = Producto.Id WHERE Producto.IdUsuario = @vIdUsuario";

                SqlParameter parametroIdUsuario = new SqlParameter();
                parametroIdUsuario.ParameterName = "vIdUsuario";
                parametroIdUsuario.SqlDbType = System.Data.SqlDbType.Int;
                parametroIdUsuario.Value = pUsuario.Id;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryTraerVentas, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parametroIdUsuario);

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Venta venta = new Venta();

                                venta.Id = Convert.ToInt32(dataReader["Id"]);
                                venta.Comentarios = dataReader["Comentarios"].ToString();

                                ventas.Add(venta);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return ventas;
        }
        public static Venta TraerUltimaVenta()
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryTraerUltimaVenta = "SELECT TOP 1 * FROM Venta ORDER BY Id DESC";

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryTraerUltimaVenta, sqlConnection))
                {

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Venta venta = new Venta();

                                venta.Id = Convert.ToInt32(dataReader["Id"]);
                                venta.Comentarios = dataReader["Comentarios"].ToString();

                                return venta;
                            }
                        }
                        return null;
                    }
                    sqlConnection.Close();
                }
            }

        }


        public static bool EliminarVenta(int idVenta)
        {
            bool resultado = false;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryDelete = "DELETE FROM Venta WHERE Id = @vId";
                    SqlParameter sqlParameterDelete = new SqlParameter("vId", SqlDbType.BigInt);
                    sqlParameterDelete.Value = idVenta;

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

                    //List<ProductoVendido> productosVendidos = ProductoHandler.TraerProductosVendidosPorVenta(idVenta);

                    //int cantProductosVendidos = ProductoVendidoHandler.EliminarProductosVendidoPorVenta(idVenta);

                    List<Producto> productos = ProductoHandler.TraerProductosPorVenta(idVenta);

                    foreach (var producto in productos)
                    {
                        ProductoVendido productoVendido = ProductoVendidoHandler.TraerProductoVendidoPorVenta(idVenta, producto);
                        ProductoHandler.RecuperarStockProducto(producto, productoVendido);
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


        public static bool AgergarVenta(List<Producto> productos, int idUsuario)
        {
            bool resultado = false;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryInsert = "INSERT INTO Venta " +
                        "(Comentarios) VALUES (@vComentariosParameter);";

                    SqlParameter comentarioParameter = new SqlParameter("vComentariosParameter", SqlDbType.VarChar) { Value = null };

                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(comentarioParameter);

                        int numberOfRows = sqlCommand.ExecuteNonQuery();

                        if (numberOfRows > 0)
                        {
                            resultado = true;
                        }
                    }
                    sqlConnection.Close();
                }

                Venta ultimaVenta = TraerUltimaVenta();

                foreach (Producto producto in productos)
                {
                    ProductoHandler.RestarStockProducto(producto, idUsuario);
                    ProductoVendidoHandler.AgregarProductoVendido(producto, ultimaVenta);
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
