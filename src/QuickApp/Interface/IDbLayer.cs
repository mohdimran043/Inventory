using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MOI.Patrol.Interface
{
    public interface IDbLayer : IDisposable
    {

        /// <summary>
        /// Opens the connection.
        /// </summary>
        void OpenConnection();

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// id of inserted entity
        /// </returns>
        long SaveEntity<T>(T entity) where T : class;


        T2 SaveEntity<T1, T2>(T1 entity) where T1 : class;
        /// <summary>
        /// Saves the entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">entities.</param>
        /// <returns>
        /// true/false
        /// </returns>
        bool SaveEntities<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// entity
        /// </returns>
        T UpdateEntity<T>(T entity) where T : class;

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attr">The attr.</param>
        /// <returns>
        /// list of entities
        /// </returns>
        IEnumerable<T> GetEntities<T>(Expression<Func<T, bool>> attr = null) where T : class;

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attr">The attr.</param>
        /// <returns>
        /// entity
        /// </returns>
        T GetEntity<T>(Expression<Func<T, bool>> attr) where T : class;

        /// <summary>
        /// Removes the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attr">The attr.</param>
        /// <param name="entity">The entity.</param>
        void RemoveEntity<T>(dynamic attr, T entity) where T : class;

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        DbParameter CreateParameter(string name, DbType type, object value);

        /// <summary>
        /// Executes the stored proc.
        /// </summary>
        /// <param name="procName">Stored procedure name</param>
        /// <param name="parameters">Proc Parameters</param>
        /// <returns>
        /// Dataset
        /// </returns>
        DataSet ExecuteStoredProc(string procName, IDictionary<string, object> parameters);

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>DataReader</returns>
        IDataReader ExecuteReader(string procName, IDictionary<string, object> parameters);

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Scalar Object</returns>
        object ExecuteScalar(string procName, IDictionary<string, object> parameters);

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Scalar Object</returns>
        object ExecuteScalar(string procName, IEnumerable<DbParameter> parameters);

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The number of rows affected if known; -1 otherwise.
        /// </returns>
        int ExecuteNonQuery(string procName, IDictionary<string, object> parameters);

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The number of rows affected if known; -1 otherwise.
        /// </returns>
        int ExecuteNonQuery(string procName, IEnumerable<DbParameter> parameters);

        /// <summary>
        /// Closes the connection.
        /// </summary>
        void CloseConnection();

        /// <summary>
        /// Begins the NPGSQL transaction.
        /// </summary>
        /// <returns></returns>
        DbTransaction BeginNpgsqlTransaction();
    }
}
