using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreNetFramework.Model;
using System.Data.SqlClient;
using System.Data;
using CoreNetFramework.Model.Enums;
using System.Configuration;

namespace CoreNetFramework.Repository
{
    public class MesaRepository : IMesaRepository
    {

        #region "Contrutores"

        public MesaRepository()
        {
            var conn = ConfigurationManager.AppSettings["connectionString"].ToString();
            _connectionString = conn;
        }

        #endregion

        #region "Propriedades"

        private string _connectionString;

        #endregion

        #region "Métodos"

        #region "Padrão"

        public int Add(Mesa item)
        {
            try
            {
                var _conn = new SqlConnection(_connectionString);

                var _cmdText = "";

                if (item.MesaId == 0)
                {
                    _cmdText = " INSERT INTO MESAS ( " +
                               "  MESA_NUMERO, " +
                               "  MESA_STATUS ) " +
                               " VALUES ( " +
                               "  @MESA_NUMERO, " +
                               "  @MESA_STATUS ); " +
                               " SELECT SCOPE_IDENTITY(); ";
                }
                else
                {
                    _cmdText = " UPDATE MESAS SET " +
                               "  MESA_NUMERO = @MESA_NUMERO, " +
                               "  MESA_STATUS = @MESA_STATUS " +
                               " WHERE MESA_ID = @MESA_ID ";
                }


                var _cmd = new SqlCommand(_cmdText, _conn);

                if (item.MesaId > 0)
                    _cmd.Parameters.AddWithValue("@MESA_ID", item.MesaId);

                _cmd.Parameters.AddWithValue("@MESA_NUMERO", item.Numero);
                _cmd.Parameters.AddWithValue("@MESA_STATUS", item.Status);

                _conn.Open();
                var _retorno = Convert.ToInt32(_cmd.ExecuteScalar());
                _conn.Close();

                return _retorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Mesa GetItem(int Id)
        {
            if (Id <= 0)
                throw new Exception("O parâmetro Id deve ser um número inteiro positivo!");

            try
            {
                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  MESA_ID, " +
                               "  MESA_NUMERO, " +
                               "  MESA_STATUS " +
                               " FROM MESAS " +
                               " WHERE MESA_ID = @MESA_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@MESA_ID", Id);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                var _mesa = new Mesa();

                if (_dt.Rows.Count > 0)
                {
                    _mesa.MesaId = Convert.ToInt32(_dt.Rows[0]["MESA_ID"]);
                    _mesa.Numero = Convert.ToInt32(_dt.Rows[0]["MESA_NUMERO"].ToString());
                    _mesa.Status = (MesaStatus)Convert.ToInt32(_dt.Rows[0]["MESA_STATUS"]);
                }

                return _mesa;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Mesa> GetAll()
        {

            try
            {
                var _mesas = new List<Mesa>();

                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  MESA_ID, " +
                               "  MESA_NUMERO, " +
                               "  MESA_STATUS " +
                               " FROM MESAS " +
                               " ORDER BY MESA_NUMERO ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                foreach (DataRow _row in _dt.Rows)
                {
                    var _mesa = new Mesa();

                    _mesa.MesaId = Convert.ToInt32(_row["MESA_ID"]);
                    _mesa.Numero = Convert.ToInt32(_row["MESA_NUMERO"]);
                    _mesa.Status = (MesaStatus)Convert.ToInt32(_row["MESA_STATUS"]);
                   
                    _mesas.Add(_mesa);

                }

                return _mesas;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Remove(int Id)
        {

            if (Id <= 0)
                throw new Exception("O parâmetro Id deve ser um número inteiro positivo!");

            try
            {
                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " DELETE FROM MESAS WHERE MESA_ID = @MESA_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@MESA_ID", Id);

                _conn.Open();
                _cmd.ExecuteScalar();
                _conn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region "Específico"


        #endregion

        #endregion
    }
}
