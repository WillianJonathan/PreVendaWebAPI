using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreNetFramework.Model;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace CoreNetFramework.Repository
{
    public class ClienteRepository : IClienteRepository
    {

        #region "Contrutores"

        public ClienteRepository()
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

        public int Add(Cliente item)
        {
            try
            {
                var _conn = new SqlConnection(_connectionString);

                var _cmdText = "";

                if (item.ClienteId == 0)
                {
                    _cmdText = " INSERT INTO CLIENTES ( " +
                               "  CLI_NOME, " +
                               "  CLI_DTA_CADASTRO, " +
                               "  CLI_DTA_NASCIMENTO, " +
                               "  CLI_EMAIL ) " +
                               " VALUES ( " +
                               "  @CLI_NOME, " +
                               "  @CLI_DTA_CADASTRO, " +
                               "  @CLI_DTA_NASCIMENTO, " +
                               "  @CLI_EMAIL ); " +
                               "  SELECT SCOPE_IDENTITY(); ";
                }
                else
                {
                    _cmdText = " UPDATE CLIENTES SET " +
                               "  CLI_NOME = @CLI_NOME, " +
                               "  CLI_DTA_CADASTRO = @CLI_DTA_CADASTRO, " +
                               "  CLI_DTA_NASCIMENTO = @CLI_DTA_NASCIMENTO, " +
                               "  CLI_EMAIL = @CLI_EMAIL " +
                               " WHERE CLI_ID = @CLI_ID ";
                }


                var _cmd = new SqlCommand(_cmdText, _conn);

                if (item.ClienteId > 0)
                    _cmd.Parameters.AddWithValue("@CLI_ID", item.ClienteId);

                _cmd.Parameters.AddWithValue("@CLI_DTA_CADASTRO", item.DataCadastro);
                _cmd.Parameters.AddWithValue("@CLI_DTA_NASCIMENTO", item.DataNascimento);
                _cmd.Parameters.AddWithValue("@CLI_EMAIL", item.Email);

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

        public Cliente GetItem(int Id)
        {

            if (Id <= 0)
                throw new Exception("O parâmetro Id deve ser um número inteiro positivo!");

            try
            {
                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  CLI_ID, " +                              
                               "  CLI_DTA_CADASTRO, " +
                               "  CLI_DTA_NASCIMENTO, " +
                               "  CLI_EMAIL " +
                               " FROM CLIENTES " +
                               " WHERE CLI_ID = @CLI_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@CLI_ID", Id);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                var _cliente = new Cliente();

                if (_dt.Rows.Count > 0)
                {
                    _cliente.ClienteId = Convert.ToInt32(_dt.Rows[0]["CLI_ID"]);
                    //_cliente.Nome = _dt.Rows[0]["CLI_NOME"].ToString();
                    _cliente.DataCadastro = Convert.ToDateTime(_dt.Rows[0]["CLI_DTA_CADASTRO"]);
                    _cliente.DataNascimento = Convert.ToDateTime(_dt.Rows[0]["CLI_DTA_NASCIMENTO"]);
                    _cliente.Email = _dt.Rows[0]["CLI_EMAIL"].ToString();
                }

                return _cliente;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Cliente> GetAll()
        {
            try
            {
                var _clientes = new List<Cliente>();

                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  CLI_ID, " +
                               "  CLI_NOME, " +
                               "  CLI_DTA_CADASTRO, " +
                               "  CLI_DTA_NASCIMENTO, " +
                               "  CLI_EMAIL " +
                               " FROM CLIENTES " +
                               " ORDER BY CLI_NOME ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                foreach (DataRow _row in _dt.Rows)
                {
                    var _cliente = new Cliente();

                    _cliente.ClienteId = Convert.ToInt32(_row["CLI_ID"]);
                    _cliente.Nome = _row["CLI_NOME"].ToString();
                    _cliente.DataCadastro = Convert.ToDateTime(_row["CLI_DTA_CADASTRO"]);
                    _cliente.DataNascimento = Convert.ToDateTime(_row["CLI_DTA_NASCIMENTO"]);
                    _cliente.Email = _row["CLI_EMAIL"].ToString();

                    _clientes.Add(_cliente);

                }

                return _clientes;

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

                var _cmdText = " DELETE FROM CLIENTES WHERE CLI_ID = @CLI_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@CLI_ID", Id);

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
