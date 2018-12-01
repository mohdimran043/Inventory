using Microsoft.EntityFrameworkCore;
using MOI.Patrol.Helpers;
using MOI.Patrol.Interface;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Threading.Tasks;

namespace MOI.Patrol.DataAccessLayer
{
    public class PostgresWrapper : IDbLayer
    {
        private readonly PatrolsContext _dbContext = new PatrolsContext();


        /// <summary>
        /// The _NPGSQL connection
        /// </summary>
        private readonly NpgsqlConnection _npgsqlConnection = new NpgsqlConnection("server=localhost;Port=5432;User Id=postgres;password=12345;Database=Patrols");


        /// <summary>
        /// Creates the command with parameters.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Command Object</returns>
        private NpgsqlCommand CreateCommandWithParameters(string procName, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            string commandText = procName;
            if (!commandText.Contains("."))
                commandText = PatrolConstants.SCHEMA + "." + procName;

            var command = new NpgsqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                Connection = _npgsqlConnection,
                CommandText = commandText
            };
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(new NpgsqlParameter { ParameterName = parameter.Key, Value = parameter.Value });
            }
            return command;
        }

        /// <summary>
        /// Creates the command with parameters.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Command Object</returns>
        private NpgsqlCommand CreateCommandWithParameters(string procName, IEnumerable<DbParameter> parameters)
        {
            string commandText = procName;
            if (!commandText.Contains("."))
                commandText =  PatrolConstants.SCHEMA  + "." + procName;

            var command = new NpgsqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                Connection = _npgsqlConnection,
                CommandText = commandText
            };
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = parameter.ParameterName,
                    DbType = parameter.DbType,
                    Value = parameter.Value
                });
            }
            return command;
        }

        /// <summary>
        /// creates the command for executing plain sql statement.
        /// </summary>
        /// <param name="sqlStatement">sql statement</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private NpgsqlCommand CreateCommandForSqlStatement(string sqlStatement, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            var command = new NpgsqlCommand
            {
                CommandType = CommandType.Text,
                Connection = _npgsqlConnection,
                CommandText = sqlStatement
            };
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(new NpgsqlParameter { ParameterName = parameter.Key, Value = parameter.Value });
            }
            return command;
        }

        /// <summary>
        /// creates the command for executing plain sql statement.
        /// </summary>
        /// <param name="sqlStatement">sql statement</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private NpgsqlCommand CreateCommandForSqlStatement(string sqlStatement)
        {
            var command = new NpgsqlCommand
            {
                CommandType = CommandType.Text,
                Connection = _npgsqlConnection,
                CommandText = sqlStatement
            };
            return command;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgresWrapper"/> class.
        /// </summary>
        public PostgresWrapper()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgresWrapper"/> class.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        public PostgresWrapper(PatrolsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PostgresWrapper(string PostgresConnectionString) //SecurityContext
        {
            if (!String.IsNullOrEmpty(PostgresConnectionString))
            {
                _dbContext = new PatrolsContext();
                _npgsqlConnection = new NpgsqlConnection(PostgresConnectionString);
            }
        }

        public PostgresWrapper(string PostgresConnectionString, int commandTimeOut) //SecurityContext
        {
            PostgresConnectionString += String.Format("CommandTimeout={0}", commandTimeOut);
            if (!String.IsNullOrEmpty(PostgresConnectionString))
            {
                _dbContext = new PatrolsContext();
                _npgsqlConnection = new NpgsqlConnection(PostgresConnectionString);
            }
        }

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// id of the last inserted entity
        /// </returns>
        public long SaveEntity<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
            dynamic obj = entity;
            return obj.id;
        }

        /// <summary>
        /// Saves the entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">list of entities.</param>
        /// <returns>
        /// true/false
        /// </returns>
        public bool SaveEntities<T>(IEnumerable<T> entities) where T : class
        {
            foreach (T entity in entities)
            {
                _dbContext.Set<T>().Add(entity);
            }
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">entity.</param>
        /// <returns>
        /// entity
        /// </returns>
        public T UpdateEntity<T>(T entity) where T : class
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return entity;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attr"></param>
        /// <returns>
        /// list of entities
        /// </returns>
        public IEnumerable<T> GetEntities<T>(Expression<Func<T, bool>> attr = null) where T : class
        {
            if (attr == null)
                return (from entity in _dbContext.Set<T>().Cast<T>() select entity);
            return (from entity in _dbContext.Set<T>().Where(attr) select entity);
        }

        /// <summary>
        /// Gets specific entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attr">The attr.</param>
        /// <returns>
        /// entity
        /// </returns>
        public T GetEntity<T>(Expression<Func<T, bool>> attr) where T : class
        {
            return (from entity in _dbContext.Set<T>().Where(attr) select entity).FirstOrDefault();
        }

        /// <summary>
        /// Removes the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attr">The attr.</param>
        /// <param name="entity">The entity.</param>
        public void RemoveEntity<T>(dynamic attr, T entity) where T : class
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Executes the stored proc.
        /// </summary>
        /// <param name="procName">Stored procedure name</param>
        /// <param name="parameters">Proc Parameters</param>
        /// <returns>
        /// Dataset
        /// </returns>
        public DataSet ExecuteStoredProc(string procName, IDictionary<string, object> parameters)
        {
            using (var command = CreateCommandWithParameters(procName, parameters))
            {
                _npgsqlConnection.Open();
                var dataAdapter = new NpgsqlDataAdapter(command);
                _npgsqlConnection.Close();
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
        }

        /// <summary>
        /// Implementing IDisposable interface
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Adding all the resources here which needs to be disposed of by IDisposable
        /// </summary>
        /// <param name="isDisposable"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool isDisposable)
        {
            if (!isDisposable) return;

            _dbContext.Dispose();
            _npgsqlConnection.Close();
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>DataReader</returns>
        public IDataReader ExecuteReader(string procName, IDictionary<string, object> parameters)
        {
            var command = CreateCommandWithParameters(procName, parameters);
            if (_npgsqlConnection.State != ConnectionState.Open)
            {
                _npgsqlConnection.Open();
            }
            return command.ExecuteReader();
        }

        public TResult ExecuteSqlStatementScalar<TResult>(string sqlStatement)
        {
            var command = CreateCommandForSqlStatement(sqlStatement);
            if (_npgsqlConnection.State != ConnectionState.Open)
            {
                _npgsqlConnection.Open();
            }
            return (TResult)command.ExecuteScalar();
        }

        public IDataReader ExecuteReaderSqlStatement(string sqlStatement, IDictionary<string, object> parameters)
        {
            var command = CreateCommandForSqlStatement(sqlStatement, parameters);
            if (_npgsqlConnection.State != ConnectionState.Open)
            {
                _npgsqlConnection.Open();
            }
            return command.ExecuteReader();
        }

        public IDataReader ExecuteReaderSqlStatement(string sqlStatement, IDictionary<string, object> parameters, Action<string, string> onError)
        {
            try
            {
                var command = CreateCommandForSqlStatement(sqlStatement, parameters);
                if (_npgsqlConnection.State != ConnectionState.Open)
                {
                    _npgsqlConnection.Open();
                }
                return command.ExecuteReader();
            }
            catch (NpgsqlException npgsqlException)
            {
                if (onError != null)
                {
                    onError(npgsqlException.ErrorCode.ToString(), npgsqlException.Message);
                }
                return null;
            }

        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Scalar Object</returns>
        public object ExecuteScalar(string procName, IDictionary<string, object> parameters)
        {
            try
            {
                using (var command = CreateCommandWithParameters(procName, parameters))
                {
                    if (_npgsqlConnection.State != ConnectionState.Open)
                    {
                        _npgsqlConnection.Open();
                    }
                    return command.ExecuteScalar();
                }

            }
            catch (Exception ex)
            {
               // throw RtExceptionManager.GetRtException(ex, "", "", System.Net.HttpStatusCode.InternalServerError, additionalInfo: string.Format("Procedure Name :{0} Parameters :{1}", procName, GetDictonaryParameters(parameters)));
            }
            return "";
        }

        /// <summary>
        /// Get parameter from dictonay object
        /// </summary>        
        /// <param name="parameters">The parameters</param>
        /// <returns>string object</returns>
        private string GetDictonaryParameters(IDictionary<string, object> parameters)
        {
            string parameter = string.Empty;
            foreach (var item in parameters)
            {
                parameter += string.Format("Key :{0} , Value :{1} ", item.Key, item.Value);
            }
            return parameter;
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The number of rows affected if known; -1 otherwise.
        /// </returns>
        public int ExecuteNonQuery(string procName, IDictionary<string, object> parameters)
        {
            using (var command = CreateCommandWithParameters(procName, parameters))
            {
                if (_npgsqlConnection.State != ConnectionState.Open)
                {
                    _npgsqlConnection.Open();
                }
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void CloseConnection()
        {
            if (_npgsqlConnection.State != ConnectionState.Closed)
            {
                _npgsqlConnection.Close();
            }
        }

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>Returns <see cref="NpgsqlParameter"/> object</returns>
        public DbParameter CreateParameter(string name, DbType type, object value)
        {
            return new NpgsqlParameter
            {
                DbType = type,
                Value = value,
                ParameterName = name,
            };
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// Scalar Object
        /// </returns>
        public object ExecuteScalar(string procName, IEnumerable<DbParameter> parameters)
        {
            using (var command = CreateCommandWithParameters(procName, parameters))
            {
                if (_npgsqlConnection.State != ConnectionState.Open)
                {
                    _npgsqlConnection.Open();
                }
                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The number of rows affected if known; -1 otherwise.
        /// </returns>
        public int ExecuteNonQuery(string procName, IEnumerable<DbParameter> parameters)
        {
            using (var command = CreateCommandWithParameters(procName, parameters))
            {
                if (_npgsqlConnection.State != ConnectionState.Open)
                {
                    _npgsqlConnection.Open();
                }
                return command.ExecuteNonQuery();
            }
        }

        public void OpenConnection()
        {
            if (_npgsqlConnection.State != ConnectionState.Open)
            {
                _npgsqlConnection.Open();
            }
        }


        /// <summary>
        /// Begins the NPGSQL transaction.
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginNpgsqlTransaction()
        {
            return _npgsqlConnection.BeginTransaction(IsolationLevel.Unspecified);
        }


        public T2 SaveEntity<T1, T2>(T1 entity) where T1 : class
        {
            _dbContext.Set<T1>().Add(entity);
            _dbContext.SaveChanges();
            dynamic obj = entity;
            return obj.id;
        }
    }
}
