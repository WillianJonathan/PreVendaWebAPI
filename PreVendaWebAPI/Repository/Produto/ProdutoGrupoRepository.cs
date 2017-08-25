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
    public class ProdutoGrupoRepository : IProdutoGrupoRepository
    {
        #region "Contrutores"

        public ProdutoGrupoRepository()
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

        public int Add(ProdutoGrupo item)
        {
            try
            {
                var _conn = new SqlConnection(_connectionString);

                var _cmdText = "";

                if (item.ProdutoGrupoId == 0)
                {
                    _cmdText = " INSERT INTO PRODUTO_GRUPOS ( " +
                               "  PROD_GRUPO_DESC, " +
                               "  PROD_GRUPO_ATIVO ) " +
                               " VALUES ( " +
                               "  @PROD_GRUPO_DESC, " +
                               "  @PROD_GRUPO_ATIVO ); " +
                               " SELECT SCOPE_IDENTITY(); ";
                }
                else
                {
                    _cmdText = " UPDATE PRODUTO_GRUPOS SET " +
                               "  PROD_GRUPO_DESC = @PROD_GRUPO_DESC, " +
                               "  PROD_GRUPO_ATIVO = @PROD_GRUPO_ATIVO " +
                               " WHERE PROD_GRUPO_ID = @PROD_GRUPO_ID ";
                }


                var _cmd = new SqlCommand(_cmdText, _conn);

                if (item.ProdutoGrupoId > 0)
                    _cmd.Parameters.AddWithValue("@PROD_GRUPO_ID", item.ProdutoGrupoId);

                _cmd.Parameters.AddWithValue("@PROD_GRUPO_DESC", item.Descricao);
                _cmd.Parameters.AddWithValue("@PROD_GRUPO_ATIVO", item.Ativo);

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

        public ProdutoGrupo GetItem(int Id)
        {
            if (Id <= 0)
                throw new Exception("O parâmetro Id deve ser um número inteiro positivo!");

            try
            {
                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  PROD_GRUPO_ID, " +
                               "  PROD_GRUPO_DESC, " +
                               "  PROD_GRUPO_ATIVO " +
                               " FROM PRODUTO_GRUPOS " +
                               " WHERE PROD_GRUPO_ID = @PROD_GRUPO_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@PROD_GRUPO_ID", Id);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                var _produtoGrupo = new ProdutoGrupo();

                if (_dt.Rows.Count > 0)
                {
                    _produtoGrupo.ProdutoGrupoId = Convert.ToInt32(_dt.Rows[0]["PROD_GRUPO_ID"]);
                    _produtoGrupo.Descricao = _dt.Rows[0]["PROD_GRUPO_DESC"].ToString();
                    _produtoGrupo.Ativo = Convert.ToBoolean(_dt.Rows[0]["PROD_GRUPO_ATIVO"]);
                }

                return _produtoGrupo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProdutoGrupo> GetAll()
        {
            try
            {
                var _produtoGrupos = new List<ProdutoGrupo>();

                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  PROD_GRUPO_ID, " +
                               "  PROD_GRUPO_DESC, " +
                               "  PROD_GRUPO_ATIVO " +
                               " FROM PRODUTO_GRUPOS " +
                               " ORDER BY PROD_GRUPO_DESC ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                foreach (DataRow _row in _dt.Rows)
                {
                    var _produtoGrupo = new ProdutoGrupo();

                    _produtoGrupo.ProdutoGrupoId = Convert.ToInt32(_row["PROD_GRUPO_ID"]);
                    _produtoGrupo.Descricao = _row["PROD_GRUPO_DESC"].ToString();
                    _produtoGrupo.Ativo = Convert.ToBoolean(_row["PROD_GRUPO_ATIVO"]);

                    _produtoGrupos.Add(_produtoGrupo);

                }

                return _produtoGrupos;

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

                var _cmdText = " DELETE FROM PRODUTO_GRUPOS WHERE PROD_GRUPO_ID = @PROD_GRUPO_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@PROD_GRUPO_ID", Id);

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
