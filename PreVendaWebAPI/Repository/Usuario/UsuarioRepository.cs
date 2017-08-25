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
    public class UsuarioRepository : IUsuarioRepository
    {
#region "Contrutores"

        public UsuarioRepository()
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

        public int Add(Usuario item)
        {
            try
            {
                var _conn = new SqlConnection(_connectionString);

                var _cmdText = "";

                if (item.UsuarioId == 0)
                {
                    _cmdText = " INSERT INTO USUARIOS ( " +
                               "  USR_LOGIN, " +
                               "  USR_SENHA ) " +
                               " VALUES ( " +
                               "  @USR_LOGIN, " +
                               "  @USR_SENHA ); " +
                               " SELECT SCOPE_IDENTITY(); ";
                }
                else
                {
                    _cmdText = " UPDATE USUARIOS SET " +
                               "  USR_LOGIN = @USR_LOGIN, " +
                               "  USR_SENHA = @USR_SENHA " +
                               " WHERE USR_ID = @USR_ID ";
                }


                var _cmd = new SqlCommand(_cmdText, _conn);

                if (item.UsuarioId > 0)
                    _cmd.Parameters.AddWithValue("@USR_ID", item.UsuarioId);

                _cmd.Parameters.AddWithValue("@USR_LOGIN", item.Login);
                _cmd.Parameters.AddWithValue("@USR_SENHA", item.Senha);

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

        public Usuario GetItem(int Id)
        {
            if (Id <= 0)
                throw new Exception("O parâmetro Id deve ser um número inteiro positivo!");

            try
            {
                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  USR_ID, " +
                               "  USR_LOGIN, " +
                               "  USR_SENHA " +
                               " FROM USUARIOS " +
                               " WHERE USR_ID = @USR_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@USR_ID", Id);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                var _usuario = new Usuario();

                if (_dt.Rows.Count > 0)
                {
                    _usuario.UsuarioId = Convert.ToInt32(_dt.Rows[0]["USR_ID"]);
                    _usuario.Login = _dt.Rows[0]["USR_LOGIN"].ToString();
                    _usuario.Senha =_dt.Rows[0]["USR_SENHA"].ToString();
                }

                return _usuario;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Usuario> GetAll()
        {
            try
            {
                var _usuarios = new List<Usuario>();

                var _conn = new SqlConnection(_connectionString);

                var _cmdText = " SELECT " +
                               "  USR_ID, " +
                               "  USR_LOGIN, " +
                               "  USR_SENHA " +
                               " FROM USUARIOS " +
                               " ORDER BY USR_LOGIN ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                var _dt = new DataTable();

                _conn.Open();
                _dt.Load(_cmd.ExecuteReader());
                _conn.Close();

                foreach (DataRow _row in _dt.Rows)
                {
                    var _usuario = new Usuario();

                    _usuario.UsuarioId = Convert.ToInt32(_row["USR_ID"]);
                    _usuario.Login = _row["USR_LOGIN"].ToString();
                    _usuario.Senha = _row["USR_SENHA"].ToString();

                    _usuarios.Add(_usuario);

                }

                return _usuarios;

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

                var _cmdText = " DELETE FROM USUARIOS WHERE USR_ID = @USR_ID ";

                var _cmd = new SqlCommand(_cmdText, _conn);

                _cmd.Parameters.AddWithValue("@USR_ID", Id);

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