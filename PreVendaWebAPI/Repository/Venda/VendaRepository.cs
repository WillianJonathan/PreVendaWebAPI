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
    public class VendaRepository : IVendaRepository
    {
        #region "Contrutores"

        public VendaRepository()
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

        public int Add(Venda item)
        {
            try
            {
                var _conn = new SqlConnection(_connectionString);

                var _cmdText = "";

                if (item.VendaId == 0)
                {
                    _cmdText = " INSERT INTO VENDAS ( " +
                               "  VEN_CLI_ID, " +
                               "  VEN_DTA_CADASTRO, " +
                               "  VEN_DTA_FATURAMENTO, " +
                               "  VEN_STATUS, " +
                               "  VEN_VALOR, " +
                               "  VEN_MESA_ID, " +
                               "  VEN_USR_ID ) " +
                               " VALUES ( " +
                               "  @VEN_CLI_ID, " +
                               "  @VEN_DTA_CADASTRO, " +
                               "  @VEN_DTA_FATURAMENTO, " +
                               "  @VEN_STATUS, " +
                               "  @VEN_VALOR, " +
                               "  @VEN_MESA_ID, " +
                               "  @VEN_USR_ID ); " +
                               " SELECT SCOPE_IDENTITY(); ";
                }
                else
                {
                    _cmdText = " UPDATE VENDAS SET " +
                               "  VEN_CLI_ID = @VEN_CLI_ID, " +
                               "  VEN_DTA_CADASTRO = @VEN_DTA_CADASTRO, " +
                               "  VEN_DTA_FATURAMENTO = @VEN_DTA_FATURAMENTO, " +
                               "  VEN_STATUS = @VEN_STATUS, " +
                               "  VEN_VALOR = @VEN_VALOR, " +
                               "  VEN_MESA_ID = @VEN_MESA_ID, " +
                               "  VEN_USR_ID = @VEN_USR_ID " +
                               " WHERE VEN_ID = @VEN_ID ";
                }


                var _cmd = new SqlCommand(_cmdText, _conn);

                if (item.VendaId > 0)
                    _cmd.Parameters.AddWithValue("@VEN_ID", item.VendaId);

                _cmd.Parameters.AddWithValue("@VEN_CLI_ID", item.Cliente.ClienteId);
                _cmd.Parameters.AddWithValue("@VEN_DTA_CADASTRO", item.DataCadastro);
                _cmd.Parameters.AddWithValue("@VEN_DTA_FATURAMENTO", item.DataFaturamento);
                _cmd.Parameters.AddWithValue("@VEN_STATUS", item.Status);
                _cmd.Parameters.AddWithValue("@VEN_VALOR", item.Valor);
                _cmd.Parameters.AddWithValue("@VEN_MESA_ID", item.Mesa.MesaId);
                _cmd.Parameters.AddWithValue("@VEN_USR_ID", item.Usuario.UsuarioId);

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

        public List<Venda> GetAll()
        {
            try
            {
                var _vendas = new List<Venda>();
                var _clienteRepository = new ClienteRepository();
                var _usuarioRepository = new UsuarioRepository();
                var _mesaRepository = new MesaRepository();
                var _vendaItemRepository = new VendaItemRepository();

                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  VEN_ID, " +
                               "  VEN_CLI_ID, " +
                               "  VEN_DTA_CADASTRO, " +
                               "  VEN_DTA_FATURAMENTO, " +
                               "  VEN_STATUS, " +
                               "  VEN_VALOR, " +
                               "  VEN_MESA_ID, " +
                               "  VEN_USR_ID " +
                               " FROM VENDAS " +
                               " ORDER BY VEN_DTA_CADASTRO ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                foreach (DataRow _row in _dt.Rows)
                {
                    var _venda = new Venda();

                    _venda.VendaId = Convert.ToInt32(_row["VEN_ID"]);
                    _venda.Cliente = _clienteRepository.GetItem(Convert.ToInt32(_row["VEN_CLI_ID"]));
                    _venda.DataCadastro = Convert.ToDateTime(_row["VEN_DTA_CADASTRO"]);
                    _venda.DataFaturamento = Convert.ToDateTime(_row["VEN_DTA_FATURAMENTO"]);
                    _venda.Status = (VendaStatus)Convert.ToInt32(_row["VEN_STATUS"]);
                    _venda.Valor = Convert.ToDecimal(_row["VEN_VALOR"]);
                    _venda.Mesa = _mesaRepository.GetItem(Convert.ToInt32(_row["VEN_MESA_ID"]));
                    _venda.Usuario = _usuarioRepository.GetItem(Convert.ToInt32(_row["VEN_USR_ID"]));

                    _venda.Itens = _vendaItemRepository.RetornarTodosPorVendaId(_venda.VendaId);

                    _vendas.Add(_venda);

                }

                return _vendas;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Venda GetItem(int Id)
        {

            if (Id <= 0)
                throw new Exception("O parâmetro Id deve ser um número inteiro positivo!");

            try
            {

                var _clienteRepository = new ClienteRepository();
                var _usuarioRepository = new UsuarioRepository();
                var _mesaRepository = new MesaRepository();
                var _vendaItemRepository = new VendaItemRepository();

                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  VEN_ID, " +
                               "  VEN_CLI_ID, " +
                               "  VEN_DTA_CADASTRO, " +
                               "  VEN_DTA_FATURAMENTO, " +
                               "  VEN_STATUS, " +
                               "  VEN_VALOR, " +
                               "  VEN_MESA_ID, " +
                               "  VEN_USR_ID " +
                               " FROM VENDAS " +
                               " WHERE VEN_ID = @VEN_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@VEN_ID", Id);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                var _venda = new Venda();

                if (_dt.Rows.Count > 0)
                {

                    _venda.VendaId = Convert.ToInt32(_dt.Rows[0]["VEN_ID"]);
                    _venda.Cliente = _clienteRepository.GetItem(Convert.ToInt32(_dt.Rows[0]["VEN_CLI_ID"]));
                    _venda.DataCadastro = Convert.ToDateTime(_dt.Rows[0]["VEN_DTA_CADASTRO"]);
                    _venda.DataFaturamento = Convert.ToDateTime(_dt.Rows[0]["VEN_DTA_FATURAMENTO"]);
                    _venda.Status = (VendaStatus)Convert.ToInt32(_dt.Rows[0]["VEN_STATUS"]);
                    _venda.Valor = Convert.ToDecimal(_dt.Rows[0]["VEN_VALOR"]);
                    _venda.Mesa = _mesaRepository.GetItem(Convert.ToInt32(_dt.Rows[0]["VEN_MESA_ID"]));
                    _venda.Usuario = _usuarioRepository.GetItem(Convert.ToInt32(_dt.Rows[0]["VEN_USR_ID"]));

                    _venda.Itens = _vendaItemRepository.RetornarTodosPorVendaId(_venda.VendaId);
                }

                return _venda;

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

                var _cmdText = " DELETE FROM VENDAS WHERE VEN_ID = @VEN_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@VEN_ID", Id);

                _conn.Open();
                _cmd.ExecuteScalar();
                _conn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Venda RetornarVendaPorMesaId(int mesaId, VendaStatus vendaStatus)
        {
            if (mesaId <= 0)
                throw new Exception("O parâmetro mesaId deve ser um número inteiro positivo!");

            try
            {

                var _clienteRepository = new ClienteRepository();
                var _usuarioRepository = new UsuarioRepository();
                var _mesaRepository = new MesaRepository();
                var _vendaItemRepository = new VendaItemRepository();

                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  VEN_ID, " +
                               "  VEN_CLI_ID, " +
                               "  VEN_DTA_CADASTRO, " +
                               "  VEN_DTA_FATURAMENTO, " +
                               "  VEN_STATUS, " +
                               "  VEN_VALOR, " +
                               "  VEN_MESA_ID, " +
                               "  VEN_USR_ID " +
                               " FROM VENDAS " +
                               " WHERE VEN_MESA_ID = @VEN_MESA_ID " +
                               " AND VEN_STATUS = @VEN_STATUS ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@VEN_MESA_ID", mesaId);
                _cmd.Parameters.AddWithValue("@VEN_STATUS", vendaStatus);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                var _venda = new Venda();

                if (_dt.Rows.Count > 0)
                {

                    _venda.VendaId = Convert.ToInt32(_dt.Rows[0]["VEN_ID"]);
                    _venda.Cliente = _clienteRepository.GetItem(Convert.ToInt32(_dt.Rows[0]["VEN_CLI_ID"]));
                    _venda.DataCadastro = Convert.ToDateTime(_dt.Rows[0]["VEN_DTA_CADASTRO"]);
                    _venda.DataFaturamento = Convert.ToDateTime(_dt.Rows[0]["VEN_DTA_FATURAMENTO"]);
                    _venda.Status = (VendaStatus)Convert.ToInt32(_dt.Rows[0]["VEN_STATUS"]);
                    _venda.Valor = Convert.ToDecimal(_dt.Rows[0]["VEN_VALOR"]);
                    _venda.Mesa = _mesaRepository.GetItem(Convert.ToInt32(_dt.Rows[0]["VEN_MESA_ID"]));
                    _venda.Usuario = _usuarioRepository.GetItem(Convert.ToInt32(_dt.Rows[0]["VEN_USR_ID"]));

                    _venda.Itens = _vendaItemRepository.RetornarTodosPorVendaId(_venda.VendaId);
                }

                return _venda;

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
