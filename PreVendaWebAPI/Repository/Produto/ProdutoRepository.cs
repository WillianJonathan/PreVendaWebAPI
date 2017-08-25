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
    public class ProdutoRepository : IProdutoRepository
    {
        #region "Contrutores"

        public ProdutoRepository()
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

        public int Add(Produto item)
        {

            try
            {
                var _conn = new SqlConnection(_connectionString);

                var _cmdText = "";

                if (item.ProdutoId == 0)
                {
                    _cmdText = " INSERT INTO PRODUTOS ( " +
                               "  PROD_NOME, " +
                               "  PROD_IMAGEM, " +
                               "  PROD_DESC, " +
                               "  PROD_PROD_GRUPO_ID, " +
                               "  PROD_VALOR) " +
                               " VALUES ( " +
                               "  @PROD_NOME, " +
                               "  @PROD_IMAGEM, " +
                               "  @PROD_DESC, " +
                               "  @PROD_PROD_GRUPO_ID, " +
                               "  @PROD_VALOR); " +
                               " SELECT SCOPE_IDENTITY(); ";
                }
                else
                {
                    _cmdText = " UPDATE PRODUTOS SET " +
                               "  PROD_NOME = @PROD_NOME, " +
                               "  PROD_IMAGEM = @PROD_IMAGEM " +
                               "  PROD_DESC = @PROD_DESC " +
                               "  PROD_PROD_GRUPO_ID = @PROD_PROD_GRUPO_ID " +
                               "  PROD_VALOR = @PROD_VALOR " +
                               " WHERE PROD_ID = @PROD_ID ";
                }


                var _cmd = new SqlCommand(_cmdText, _conn);

                if (item.ProdutoId > 0)
                    _cmd.Parameters.AddWithValue("@PROD_ID", item.ProdutoId);

                _cmd.Parameters.AddWithValue("@PROD_NOME", item.Nome);
                _cmd.Parameters.AddWithValue("@PROD_IMAGEM", item.Imagem);
                _cmd.Parameters.AddWithValue("@PROD_DESC", item.Descricao);
                _cmd.Parameters.AddWithValue("@PROD_VALOR", item.Valor);

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

        public Produto GetItem(int Id)
        {
            if (Id <= 0)
                throw new Exception("O parâmetro Id deve ser um número inteiro positivo!");

            try
            {

                var _produtoGrupoRepository = new ProdutoGrupoRepository();

                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  PROD_ID, " +
                               "  PROD_NOME, " +
                               "  PROD_IMAGEM, " +
                               "  PROD_DESC, " +
                               "  PROD_PROD_GRUPO_ID, " +
                               "  PROD_VALOR " +
                               " FROM PRODUTOS " +
                               " WHERE PROD_ID = @PROD_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@PROD_ID", Id);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                var _produto = new Produto();

                if (_dt.Rows.Count > 0)
                {
                    _produto.ProdutoId = Convert.ToInt32(_dt.Rows[0]["PROD_ID"]);
                    _produto.Nome = _dt.Rows[0]["PROD_NOME"].ToString();

                    if (DBNull.Value != _dt.Rows[0]["PROD_IMAGEM"])
                        _produto.Imagem = (byte[])_dt.Rows[0]["PROD_IMAGEM"];

                    _produto.Descricao = _dt.Rows[0]["PROD_DESC"].ToString();
                    _produto.Grupo = _produtoGrupoRepository.GetItem(Convert.ToInt32(_dt.Rows[0]["PROD_PROD_GRUPO_ID"]));
                    _produto.Valor = Convert.ToDecimal(_dt.Rows[0]["PROD_VALOR"]);
                }

                return _produto;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Produto> GetAll()
        {
            try
            {
                var _produtos = new List<Produto>();
                var _produtoGrupoRepository = new ProdutoGrupoRepository();

                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  PROD_ID, " +
                               "  PROD_NOME, " +
                               "  PROD_IMAGEM, " +
                               "  PROD_DESC, " +
                               "  PROD_PROD_GRUPO_ID, " +
                               "  PROD_VALOR " +
                               " FROM PRODUTOS " +
                               " ORDER BY PROD_NOME ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                foreach (DataRow _row in _dt.Rows)
                {
                    var _produto = new Produto();

                    _produto.ProdutoId = Convert.ToInt32(_row["PROD_ID"]);
                    _produto.Nome = _row["PROD_NOME"].ToString();

                    if (DBNull.Value != _row["PROD_IMAGEM"])
                        _produto.Imagem = (byte[])_row["PROD_IMAGEM"];

                    _produto.Descricao = _row["PROD_DESC"].ToString();
                    _produto.Grupo = _produtoGrupoRepository.GetItem(Convert.ToInt32(_row["PROD_PROD_GRUPO_ID"]));
                    _produto.Valor = Convert.ToDecimal(_row["PROD_VALOR"]);

                    _produtos.Add(_produto);

                }

                return _produtos;

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

                var _cmdText = " DELETE FROM PRODUTOS WHERE PROD_ID = @PROD_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@PROD_ID", Id);

                _conn.Open();
                _cmd.ExecuteScalar();
                _conn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Produto> RetornarTodosPorProdutoGrupoId(int produtoGrupoId)
        {

            if (produtoGrupoId <= 0)
                throw new Exception("O parâmetro produtoGrupoId deve ser um número inteiro positivo!");

            try
            {
                var _produtos = new List<Produto>();
                var _produtoGrupoRepository = new ProdutoGrupoRepository();

                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  PROD_ID, " +
                               "  PROD_NOME, " +
                               "  PROD_IMAGEM, " +
                               "  PROD_DESC, " +
                               "  PROD_PROD_GRUPO_ID, " +
                               "  PROD_VALOR " +
                               " FROM PRODUTOS " +
                               " WHERE PROD_PROD_GRUPO_ID = @PROD_PROD_GRUPO_ID " +
                               " ORDER BY PROD_NOME ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@PROD_PROD_GRUPO_ID", produtoGrupoId);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                foreach (DataRow _row in _dt.Rows)
                {
                    var _produto = new Produto();

                    _produto.ProdutoId = Convert.ToInt32(_row["PROD_ID"]);
                    _produto.Nome = _row["PROD_NOME"].ToString();

                    if (DBNull.Value != _row["PROD_IMAGEM"])
                        _produto.Imagem = (byte[])_row["PROD_IMAGEM"];

                    _produto.Descricao = _row["PROD_DESC"].ToString();
                    _produto.Grupo = _produtoGrupoRepository.GetItem(Convert.ToInt32(_row["PROD_PROD_GRUPO_ID"]));
                    _produto.Valor = Convert.ToDecimal(_row["PROD_VALOR"]);

                    _produtos.Add(_produto);

                }

                return _produtos;

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
