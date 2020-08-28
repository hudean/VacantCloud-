using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using Org.BouncyCastle.Math.EC;
using Dapper;

namespace MyCoreMvc.Dapper
{
    public class DapperContest
    {
        private static readonly string ConnStr;
        public DbConnection dbConnection { get; set; }
       
        
        public DapperContest(DbConnection _dbConnection, string connStr)
        {
            dbConnection = _dbConnection;
            dbConnection.ConnectionString = ConnStr;
        }

        public  int ExecuteNonQuery(string sql, params DbParameter[] paras)
        {
            var transaction = dbConnection.BeginTransaction();
            try
            {
                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.Transaction = transaction;
                    dbConnection.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(paras);
                    int nonQuery=  cmd.ExecuteNonQuery();
                    if (nonQuery <= 0)
                    {
                        throw new Exception($"Sql ExecuteNonQuery erro:nonQuery={nonQuery}");
                    }
                    transaction.Commit();
                    return nonQuery;
                }
                   
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                string exstr = "数据库操作失败！" + ex.ToString();
                //记录日志
                dbConnection.Close();
                dbConnection.Dispose();
                return -1;
            }
            finally
            {
                dbConnection.Close();
            }
            
        }

        public static int Insert<T>(T model, string sql)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                return conn.Execute(sql, model);
            }
        }


        public static int Insert<T>(List<T> list, string sql)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                var transaction = conn.BeginTransaction();
                try
                {
                    int r= conn.Execute(sql, list,transaction);
                    transaction.Commit();
                    return r;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return -1;
                }
                
            }
        }



        public static List<T> Query<T>(string sql)
        {
            using (IDbConnection connection = new SqlConnection(ConnStr))
            {
                return connection.Query<T>(sql).ToList();
            }
        }

        /// <summary>
        /// 查询指定数据
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static T Query<T>(T model)
        {
            using (IDbConnection connection = new SqlConnection(ConnStr))
            {
                return connection.Query<T>("select * from Person where id=@ID", model).SingleOrDefault();
            }
        }

    }



    public class SqlServerHelper
    {
        private static readonly string ConnStr;

        /// <summary>
        /// 执行SQL语句返回受影响的行数,有事务版本
        /// 返回-1说明执行sql语句失败。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="paras">可变参数</param>
        /// <returns></returns>
        public static int ExecuteNonQueryByTransaction(string sql, params DbParameter[] paras)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                var transaction = conn.BeginTransaction();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        cmd.Transaction = transaction;
                        conn.Open();
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(paras);
                        int nonQuery = cmd.ExecuteNonQuery();
                        if (nonQuery <= 0)
                        {
                            throw new Exception($"Sql ExecuteNonQuery erro:nonQuery={nonQuery}");
                        }
                        transaction.Commit();
                        return nonQuery;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        string exstr = "数据库操作失败！" + ex.ToString();
                        //记录日志
                        conn.Close();
                        conn.Dispose();
                        return -1;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句返回受影响的行数
        /// 返回-1说明执行sql语句失败。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="paras">可变参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, params DbParameter[] paras)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    try
                    {

                        conn.Open();
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(paras);
                        return cmd.ExecuteNonQuery();


                    }
                    catch (Exception ex)
                    {
                        string exstr = "数据库操作失败！" + ex.ToString();
                        //记录日志
                        conn.Close();
                        conn.Dispose();
                        return -1;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行一个查询语句，返回第一行第一列的结果
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="paras">可变参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, params DbParameter[] paras)
        {
            using (var conn = new SqlConnection())
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(paras);
                        return cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        string exstr = "数据库操作失败！" + ex.ToString();
                        //记录日志
                        conn.Close();
                        conn.Dispose();
                        return null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句返回DataTable
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="paras">可变参数</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, params DbParameter[] paras)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqlConnection())
            {
                using (var da = new SqlDataAdapter(sql, conn))
                {
                    try
                    {
                        conn.Open();
                        da.SelectCommand.Parameters.Clear();
                        da.SelectCommand.Parameters.AddRange(paras);
                        da.Fill(dt);
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        string exstr = "数据库操作失败！" + ex.ToString();
                        //记录日志
                        conn.Close();
                        conn.Dispose();
                    }
                    finally
                    {

                        conn.Close();
                    }

                }

            }

            return dt;
        }

        /// <summary>
        /// Reader 多数据查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static DbDataReader ExecuteReader(string sql, params DbParameter[] paras)
        {
            using (var conn = new SqlConnection())
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(paras);
                        return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    }
                    catch (Exception ex)
                    {
                        string exstr = "数据库操作失败！" + ex.ToString();
                        //记录日志
                        conn.Close();
                        conn.Dispose();
                        //return null;
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }


                }

            }
        }


    }

    public class IDbHelper
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DatabaseType DbType { get; set; }

    }

    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DatabaseType
    {
        SqlServer,
        MySql,
        Oracle
    }
}
