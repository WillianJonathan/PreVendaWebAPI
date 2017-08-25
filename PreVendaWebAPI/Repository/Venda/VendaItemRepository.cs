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
    public class VendaItemRepository : IVendaItemRepository
    {
        #region "Contrutores"

        public VendaItemRepository()
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

        public int Add(VendaItem item)
        {

            try
            {
                var _conn = new SqlConnection(_connectionString);

                var _cmdText = "";

                if (item.VendaItemId == 0)
                {
                    _cmdText = " INSERT INTO VENDA_ITENS ( " +
                               "  VEN_ITEM_VEN_ID, " +
                               "  VEN_ITEM_PROD_ID, " +
                               "  VEN_ITEM_QTDE, " +
                               "  VEN_ITEM_VALOR_UNITARIO, " +
                               "  VEN_ITEM_DESCONTO ) " +
                               " VALUES ( " +
                               "  @VEN_ITEM_VEN_ID, " +
                               "  @VEN_ITEM_PROD_ID, " +
                               "  @VEN_ITEM_QTDE, " +
                               "  @VEN_ITEM_VALOR_UNITARIO, " +
                               "  @VEN_ITEM_DESCONTO ); " +
                               " SELECT SCOPE_IDENTITY(); ";
                }
                else
                {
                    _cmdText = " UPDATE VENDA_ITENS SET " +
                               "  VEN_ITEM_VEN_ID = @VEN_ITEM_VEN_ID, " +
                               "  VEN_ITEM_PROD_ID = @VEN_ITEM_PROD_ID, " +
                               "  VEN_ITEM_QTDE = @VEN_ITEM_QTDE, " +
                               "  VEN_ITEM_VALOR_UNITARIO = @VEN_ITEM_VALOR_UNITARIO, " +
                               "  VEN_ITEM_DESCONTO = @VEN_ITEM_DESCONTO " +
                               " WHERE VEN_ITEM_ID = @VEN_ITEM_ID ";
                }


                var _cmd = new SqlCommand(_cmdText, _conn);

                if (item.VendaItemId > 0)
                    _cmd.Parameters.AddWithValue("@VEN_ITEM_ID", item.VendaItemId);

                _cmd.Parameters.AddWithValue("@VEN_ITEM_VEN_ID", item.VendaId);
                _cmd.Parameters.AddWithValue("@VEN_ITEM_PROD_ID", item.Produto.ProdutoId);
                _cmd.Parameters.AddWithValue("@VEN_ITEM_QTDE", item.Quantidade);
                _cmd.Parameters.AddWithValue("@VEN_ITEM_VALOR_UNITARIO", item.ValorUnitario);
                _cmd.Parameters.AddWithValue("@VEN_ITEM_DESCONTO", item.Desconto);

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

        public VendaItem GetItem(int Id)
        {

            if (Id <= 0)
                throw new Exception("O parâmetro Id deve ser um número inteiro positivo!");

            try
            {

                var _produtoRepository = new ProdutoRepository();

                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  VEN_ITEM_ID, " +
                               "  VEN_ITEM_VEN_ID, " +
                               "  VEN_ITEM_PROD_ID, " +
                               "  VEN_ITEM_QTDE, " +
                               "  VEN_ITEM_VALOR_UNITARIO, " +
                               "  VEN_ITEM_DESCONTO " +
                               " FROM VENDA_ITENS " +
                               " WHERE VEN_ITEM_ID = @VEN_ITEM_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@VEN_ITEM_ID", Id);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                var _vendaItem = new VendaItem();

                if (_dt.Rows.Count > 0)
                {
                    _vendaItem.VendaItemId = Convert.ToInt32(_dt.Rows[0]["VEN_ITEM_ID"]);
                    _vendaItem.VendaId = Convert.ToInt32(_dt.Rows[0]["VEN_ITEM_VEN_ID"].ToString());
                    _vendaItem.Produto = _produtoRepository.GetItem(Convert.ToInt32( (_dt.Rows[0]["VEN_ITEM_PROD_ID"])));
                    _vendaItem.Quantidade = Convert.ToInt32(_dt.Rows[0]["VEN_ITEM_QTDE"].ToString());
                    _vendaItem.ValorUnitario = Convert.ToDecimal(_dt.Rows[0]["VEN_ITEM_VALOR_UNITARIO"]);
                    _vendaItem.Desconto = Convert.ToDecimal(_dt.Rows[0]["VEN_ITEM_DESCONTO"].ToString());
                }

                return _vendaItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<VendaItem> GetAll()
        {
            try
            {
                var _vendaItens = new List<VendaItem>();
                var _produtoRepository = new ProdutoRepository();

                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  VEN_ITEM_ID, " +
                               "  VEN_ITEM_VEN_ID, " +
                               "  VEN_ITEM_PROD_ID, " +
                               "  VEN_ITEM_QTDE, " +
                               "  VEN_ITEM_VALOR_UNITARIO, " +
                               "  VEN_ITEM_DESCONTO " +
                               " FROM VENDA_ITENS ";                             

                var _cmd = new SqlCommand(_cmdText, _conn);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                foreach (DataRow _row in _dt.Rows)
                {
                    var _vendaItem = new VendaItem();

                    _vendaItem.VendaItemId = Convert.ToInt32(_row["VEN_ITEM_ID"]);
                    _vendaItem.VendaId = Convert.ToInt32(_row["VEN_ITEM_VEN_ID"].ToString());
                    _vendaItem.Produto = _produtoRepository.GetItem(Convert.ToInt32((_row["VEN_ITEM_PROD_ID"])));
                    _vendaItem.Quantidade = Convert.ToInt32(_row["VEN_ITEM_QTDE"].ToString());
                    _vendaItem.ValorUnitario = Convert.ToDecimal(_row["VEN_ITEM_VALOR_UNITARIO"]);
                    _vendaItem.Desconto = Convert.ToDecimal(_row["VEN_ITEM_DESCONTO"].ToString());

                    _vendaItens.Add(_vendaItem);

                }

                return _vendaItens;

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

                var _cmdText = " DELETE FROM VENDA_ITENS WHERE VEN_ITEM_ID = @VEN_ITEM_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@VEN_ITEM_ID", Id);

                _conn.Open();
                _cmd.ExecuteScalar();
                _conn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<VendaItem> RetornarTodosPorVendaId(int vendaId)
        {
            try
            {
                var _vendaItens = new List<VendaItem>();
                var _produtoRepository = new ProdutoRepository();

                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  VEN_ITEM_ID, " +
                               "  VEN_ITEM_VEN_ID, " +
                               "  VEN_ITEM_PROD_ID, " +
                               "  VEN_ITEM_QTDE, " +
                               "  VEN_ITEM_VALOR_UNITARIO, " +
                               "  VEN_ITEM_DESCONTO " +
                               " FROM VENDA_ITENS " +
                               " WHERE VEN_ITEM_VEN_ID = @VEN_ITEM_VEN_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);
                _cmd.Parameters.AddWithValue("@VEN_ITEM_VEN_ID",vendaId);


                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                foreach (DataRow _row in _dt.Rows)
                {
                    var _vendaItem = new VendaItem();

                    _vendaItem.VendaItemId = Convert.ToInt32(_row["VEN_ITEM_ID"]);
                    _vendaItem.VendaId = Convert.ToInt32(_row["VEN_ITEM_VEN_ID"].ToString());
                    _vendaItem.Produto = _produtoRepository.GetItem(Convert.ToInt32((_row["VEN_ITEM_PROD_ID"])));
                    _vendaItem.Quantidade = Convert.ToInt32(_row["VEN_ITEM_QTDE"].ToString());
                    _vendaItem.ValorUnitario = Convert.ToDecimal(_row["VEN_ITEM_VALOR_UNITARIO"]);
                    _vendaItem.Desconto = Convert.ToDecimal(_row["VEN_ITEM_DESCONTO"].ToString());

                    _vendaItens.Add(_vendaItem);

                }

                return _vendaItens;

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
