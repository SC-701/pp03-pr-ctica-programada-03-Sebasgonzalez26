using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class VehiculoDA : IVehiculoDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        #region Constructor 

        public VehiculoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        #endregion


        #region Operaciones
        public async Task<Guid> Agregar(VehiculoRequest vehiculo)
        {

            string query = @"AgregarUnVehiculo";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {

                Id = Guid.NewGuid(),
                IdModelo = vehiculo.IdModelo ,
                Placa = vehiculo.Placa,
                Color = vehiculo.Color,
                Anio = vehiculo.Anio,
                Precio = vehiculo.Precio,
                CorreoPropietario = vehiculo.CorreoPropietario,
                TelefonoPropietario = vehiculo.TelefonoPropietario
         
            });

            return resultadoConsulta;
        }

        public  async Task<Guid> Editar(Guid Id, VehiculoRequest vehiculo)
        {
            await verificarVehiculoExistencia(Id);
            string query = @"EditarUnVehiculo";

            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {

                Id = Id,
                IdModelo = vehiculo.IdModelo,
                Placa = vehiculo.Placa,
                Color = vehiculo.Color,
                Anio = vehiculo.Anio,
                Precio = vehiculo.Precio,
                CorreoPropietario = vehiculo.CorreoPropietario,
                TelefonoPropietario = vehiculo.TelefonoPropietario

            });

            return resultadoConsulta;
        }

       

        public async Task<Guid> Eliminar(Guid Id)
        {
            await verificarVehiculoExistencia(Id);
            string query = @"EliminarUnVehiculo";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {

                Id = Id
               

            });

            return resultadoConsulta;
        }

        public async Task<IEnumerable<VehiculoResponse>> Obtener()
        {
            string query = @"ObtenerVehiculos";
           var resultadoConsulta = await _sqlConnection.QueryAsync<VehiculoResponse>(query);//se pone queryasync ya que no solo espera el id, espera uno o mas registro(una dupla)
            return resultadoConsulta;
        }

        public async Task<VehiculoDetalle> Obtener(Guid Id)
        {
            string query = @"ObtenerUnVehiculo";
            var resultadoConsulta = await _sqlConnection.QueryAsync<VehiculoDetalle>(query, new {Id = Id});//se pone queryasync ya que no solo espera el id, espera uno o mas registro(una dupla)
            return resultadoConsulta.FirstOrDefault();//el first porque solo devuele un registro
        }
        #endregion

        #region Helpers
        private async Task verificarVehiculoExistencia(Guid Id)
        {
            VehiculoResponse? resultadoConsultaVehiculo = await Obtener(Id);//esta linea es para validar si el vehiculo existe, se utiliza el obtner por id del metodo de abajo de obtener
            if (resultadoConsultaVehiculo == null)//si no existe el vehiculo
                throw new Exception("No se encontro el vehiculo");//NO se puede editar, se lanza una excepcion
        }
        #endregion
    }
}
