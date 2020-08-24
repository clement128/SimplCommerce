using System;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SecurityTest
{
    public class Class1Options
    {
        public string ConnectionString { get; set; }
    }

    public class Class1
    {
        private readonly ILogger<Class1> logger;
        private readonly IOptions<Class1Options> options;

        public Class1(ILogger<Class1> logger, IOptions<Class1Options> options)
        {
            this.logger = logger;
            this.options = options;
        }

        public void Method1(string title, string contents, string user)
        {
            var connection = new SqlConnection();
            var sb = new StringBuilder();
            try
            {
                connection.ConnectionString = options.Value.ConnectionString;
                connection.Open();
                var insertSql =
                    string.Format("insert into BlogEntries (Title, Contents, Author, PostedDate) values ('{0}','{1}','{2}',{3:yyyy-MM-dd}); select top 1 * from blogentries order by Id desc;",
                        title,
                        contents,
                        user,
                        DateTime.Now);

                var insertCommand = new SqlCommand(insertSql, connection);

                var dataReader = insertCommand.ExecuteReader();

                while (dataReader.Read()) { 
                    var x = string.Format("Id:<br />{0}<br />Title:<br />{1}<br />Contents:<br />{2}<br />Author:<br />{3}<br />Posted date:<br />{4}<br />",
                        dataReader[0],
                        dataReader[1],
                        dataReader[2],
                        dataReader[3],
                        dataReader[4]);
                    sb.AppendLine(x);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("A problem has occured.  Please try again. Error={0}", ex.Message));
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}
