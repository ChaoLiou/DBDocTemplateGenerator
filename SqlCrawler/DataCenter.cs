using SqlCrawler.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCrawler
{
    class DataCenter
    {
        public List<SqlCrawler.Model.Object> Objects { get; set; }
        public List<ObjectReference> ObjectReferences { get; set; }
        public List<Parameter> Parameters { get; set; }
        public List<Column> Columns { get; set; }

        private static Dictionary<string, string> _sqlPaths;
        private static string _dataSource = ConfigurationManager.AppSettings["dataSource"];
        private static string _password = ConfigurationManager.AppSettings["password"];
        private static string _userId = ConfigurationManager.AppSettings["userId"];
        private SqlConnectionStringBuilder _sqlBuilder;

        public DataCenter(string database)
        {
            _sqlPaths = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Sql"), "*.sql")
                .ToDictionary(path => Path.GetFileNameWithoutExtension(path), path => path);

            _sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = _dataSource,
                InitialCatalog = database,
                Password = _password,
                UserID = _userId
            };
        }

        public DataCenter Produce()
        {
            using (var conn = new SqlConnection(_sqlBuilder.ToString()))
            {
                conn.Open();
                var tasks = new List<Task>();
                foreach (var sqlPath in _sqlPaths)
                {
                    var sql_str = File.ReadAllText(sqlPath.Value);
                    switch ((DataType)Enum.Parse(typeof(DataType), sqlPath.Key))
                    {
                        case DataType.Column:
                            this.Columns = query<Column>(conn, sql_str);
                            break;
                        case DataType.Object:
                            this.Objects = query<SqlCrawler.Model.Object>(conn, sql_str);
                            break;
                        case DataType.ObjectReference:
                            this.ObjectReferences = query<ObjectReference>(conn, sql_str);
                            break;
                        case DataType.Parameter:
                            this.Parameters = query<Parameter>(conn, sql_str);
                            break;
                    }
                }
            }

            return this;
        }

        public void MergeToObjects()
        {
            foreach (var obj in this.Objects)
            {
                switch (obj.Type.Trim())
                {
                    case "U":
                        obj.Columns = this.Columns.Where(c => c.ParentName == obj.Name);
                        break;
                    default:
                        obj.Parameters = this.Parameters.Where(p => p.ParentName == obj.Name);
                        break;
                }

                obj.ObjectReferences = this.ObjectReferences.Where(or => or.Name == obj.Name);
            }
        } 

        private static List<T> query<T>(SqlConnection conn, string sql)
        {
            var list = new List<T>();
            var properties = typeof(T).GetProperties();

            using (var cmd = new SqlCommand(sql, conn))
            {
                var reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        var temp = (T)Activator.CreateInstance(typeof(T));
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var name = reader.GetName(i);
                            var property = properties.FirstOrDefault(p => p.Name == name);
                            switch (property.PropertyType.FullName)
                            {
                                case "System.Int32":
                                    property.SetValue(temp, int.Parse(reader[i].ToString()), null);
                                    break;
                                case "System.Boolean":
                                    property.SetValue(temp, bool.Parse(reader[i].ToString()), null);
                                    break;
                                default:
                                    property.SetValue(temp, reader[i].ToString(), null);
                                    break;
                            }
                        }

                        list.Add(temp);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            return list;
        }
    }

    public enum DataType
    {
        Column,
        Object,
        ObjectReference,
        Parameter
    }
}
