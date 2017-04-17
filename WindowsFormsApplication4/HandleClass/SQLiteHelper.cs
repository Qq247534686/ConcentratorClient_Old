using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Configuration;
using System.IO;

namespace 集中器控制客户端.HandleClass
{
    public class SQLiteHelper
    {
        private static string connectionString = string.Empty;
        private static string dbPath =ConfigurationManager.ConnectionStrings["CacheData"].ConnectionString;//存放的目录
        /// <summary>  
        /// 根据数据源、密码、版本号设置连接字符串。  
        /// </summary>  
        /// <param name="datasource">数据源。</param>  
        /// <param name="password">密码。</param>  
        /// <param name="version">版本号（缺省为3）。</param>  
        public SQLiteHelper()
        {
            connectionString = string.Format("Data Source={0}", dbPath);
            if (!File.Exists(dbPath))
            {
                CreateDB(dbPath);
                CreateTable();
            }
        }

        /// <summary>  
        /// 创建一个数据库文件。如果存在同名数据库文件，则会覆盖。  
        /// </summary>  
        /// <param name="dbName">数据库文件名。为null或空串时不创建。</param>  
        /// <param name="password">（可选）数据库密码，默认为空。</param>  
        /// <exception cref="Exception"></exception>  
        public static void CreateDB(string dbName)
        {
            if (!string.IsNullOrEmpty(dbName))
            {
                try { SQLiteConnection.CreateFile(dbName); }
                catch (Exception) { throw; }
            }
        }

        /// <summary>   
        /// 对SQLite数据库执行增删改操作，返回受影响的行数。   
        /// </summary>   
        /// <param name="sql">要执行的增删改的SQL语句。</param>   
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param>   
        /// <returns></returns>   
        /// <exception cref="Exception"></exception>  
        public int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            int affectedRows = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = sql;
                        if (parameters!=null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        affectedRows = command.ExecuteNonQuery();
                    }
                    catch (Exception msg) {
                        Log.LogWrite(msg);
                    }
                }
            }
            return affectedRows;
        }

        /// <summary>  
        /// 批量处理数据操作语句。  
        /// </summary>  
        /// <param name="list">SQL语句集合。</param>  
        /// <exception cref="Exception"></exception>  
        public void ExecuteNonQueryBatch(List<KeyValuePair<string, SQLiteParameter[]>> list)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try { conn.Open(); }
                catch { throw; }
                using (SQLiteTransaction tran = conn.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        try
                        {
                            foreach (var item in list)
                            {
                                cmd.CommandText = item.Key;
                                if (item.Value != null)
                                {
                                    cmd.Parameters.AddRange(item.Value);
                                }
                                cmd.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch (Exception) { tran.Rollback(); throw; }
                    }
                }
            }
        }

        /// <summary>  
        /// 执行查询语句，并返回第一个结果。  
        /// </summary>  
        /// <param name="sql">查询语句。</param>  
        /// <returns>查询结果。</returns>  
        /// <exception cref="Exception"></exception>  
        public object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandText = sql;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteScalar();
                    }
                    catch (Exception)
                    { 
                        throw;
                    }
                }
            }
        }

        /// <summary>   
        /// 执行一个查询语句，返回一个包含查询结果的DataTable。   
        /// </summary>   
        /// <param name="sql">要执行的查询语句。</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param>   
        /// <returns></returns>   
        /// <exception cref="Exception"></exception>  
        public DataTable ExecuteQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    try { adapter.Fill(data); }
                    catch (Exception) { throw; }
                    return data;
                }
            }
        }

        /// <summary>   
        /// 执行一个查询语句，返回一个关联的SQLiteDataReader实例。   
        /// </summary>   
        /// <param name="sql">要执行的查询语句。</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param>   
        /// <returns></returns>   
        /// <exception cref="Exception"></exception>  
        public SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] parameters)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            try
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                connection.Open();
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception) { throw; }
        }

        /// <summary>   
        /// 查询数据库中的所有数据类型信息。  
        /// </summary>   
        /// <returns></returns>   
        /// <exception cref="Exception"></exception>  
        public DataTable GetSchema()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return connection.GetSchema("TABLES");
                }
                catch (Exception) { throw; }
            }
        }
        /// <summary> 
        /// 创建SQLite数据库文件 
        /// </summary> 
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param> 
        public void CreateTable()
        {
            /*
         *遥测表：id,设备地址（deviceAddress）,遥信地址（TelemeteringAddress）,第几路（belongLine）,数值（numberValue）,品质描述（theDescribe）//deviceAddress,TelemeteringAddress,belongLine,numberValue,theDescribe
         * 遥信表：id,设备地址（deviceAddress）,遥信地址（RemoteAddress）,第几路（belongLine）,数值（numberValue）,时标（the Characteristic）
            */
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "CREATE TABLE [Telemetering] ([id] INTEGER PRIMARY KEY, [deviceAddress] VARCHAR (100), [TelemeteringAddress] VARCHAR (100), [belongLine] VARCHAR (100), [numberValue] VARCHAR (100), [theDescribe] VARCHAR (100))";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE [Remote] ([id] INTEGER  NOT NULL PRIMARY KEY,[deviceAddress] VARCHAR(100)  NULL,[RemoteAddress] VARCHAR(100)  NULL,[belongLine] VARCHAR(100)  NULL,[numberValue] VARCHAR(100)  NULL,[theCharacteristic] VARCHAR(100)  NULL)";
                    command.ExecuteNonQuery();
                }
            }
        }
        public void TruncateTable(string table)
        {
            ExecuteNonQuery("delete from " + table, null);
            ExecuteNonQuery("update sqlite_sequence set seq = 0 where name = '" + table + "'", null);
        }
        /// <summary> 
        /// 执行一个查询语句，返回一个包含查询结果的DataTable 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }
    }
}