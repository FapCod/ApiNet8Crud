using MiWebApi.Models;
using System.Data;
using System.Data.SqlClient;
namespace MiWebApi.Data
{
    public class EmpleadoData
    {
        private readonly string connectionString;
        public EmpleadoData(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<List<Empleado>> GetEmpleados()
        {
            List<Empleado> empleados = new List<Empleado>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("sp_ObtenerEmpleados", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            empleados.Add(new Empleado
                            {
                                IdEmpleado = reader.GetInt32(0),
                                NombreCompleto = reader.GetString(1),
                                Correo = reader.GetString(2),
                                Sueldo = reader.GetDecimal(3),
                                FechaContrato = reader.GetDateTime(4).ToString("yyyy-MM-dd")
                            });
                        }
                    }

                }
            }
            return empleados;
        }
        public async Task<Empleado> GetEmpleado(int id)
        {
            Empleado empleado = new Empleado();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("sp_ObtenerEmpleadoPorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdEmpleado", id);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            empleado.IdEmpleado = reader.GetInt32(0);
                            empleado.NombreCompleto = reader.GetString(1);
                            empleado.Correo = reader.GetString(2);
                            empleado.Sueldo = reader.GetDecimal(3);
                            empleado.FechaContrato = reader.GetDateTime(4).ToString("yyyy-MM-dd");
                        }
                    }
                }
            }
            return empleado;
        }
        public async Task<bool> InsertEmpleado(Empleado empleado)
        {
            bool result = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("sp_InsertarEmpleado", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NombreCompleto", empleado.NombreCompleto);
                        command.Parameters.AddWithValue("@Correo", empleado.Correo);
                        command.Parameters.AddWithValue("@Sueldo", empleado.Sueldo);
                        command.Parameters.AddWithValue("@FechaContrato", empleado.FechaContrato);
                        result = await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                result = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                result = false;
            }
            return result;
        }
        public async Task<bool> UpdateEmpleado(Empleado empleado)
        {
            bool result = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("sp_ActualizarEmpleado", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdEmpleado", empleado.IdEmpleado);
                        command.Parameters.AddWithValue("@NombreCompleto", empleado.NombreCompleto);
                        command.Parameters.AddWithValue("@Correo", empleado.Correo);
                        command.Parameters.AddWithValue("@Sueldo", empleado.Sueldo);
                        command.Parameters.AddWithValue("@FechaContrato", empleado.FechaContrato);
                        result = await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                result = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                result = false;
            }
            return result;
        }
        public async Task<bool> DeleteEmpleado(int id)
        {
            bool result = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("sp_EliminarEmpleado", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdEmpleado", id);
                        result = await command.ExecuteNonQueryAsync() > 0;
                       
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                result = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                result = false;
            }
            return result;
        }

    }
}
