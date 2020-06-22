using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQLInterface
{
    class _SQLInterface
    {
        private SqlConnection sConn1 = new SqlConnection();
        private SqlCommand sComm1 = new SqlCommand();

        public int login(string SQLConnectionString)
        {

            try
            {
                sConn1.ConnectionString = SQLConnectionString;
                ConnectionSwitch(SQLConnectionString, true);
                ConnectionSwitch(SQLConnectionString, false);

                Console.WriteLine(@"SQL Server login successful at: " + DateTime.Now);
                return 1;
            }
            catch (Exception ex)
            {

                ConnectionSwitch(SQLConnectionString, false);
                Console.WriteLine(@"Error during login: " + ex.Message);
                return 0;
            }

        }

        public void ConnectionSwitch(string SQLConnectionString, bool Open)
        {



            try
            {
                if (Open == true)
                {
                    if (sConn1.State.ToString() == @"Open")
                    {

                    }
                    else
                    {
                        sConn1.ConnectionString = SQLConnectionString;
                        sConn1.Open();
                    }
                }
                else
                {
                    if (sConn1.State.ToString() == @"Open")
                    {
                        sConn1.Close();
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(@"Error during Connection Switch: " + ex.Message);
            }
        }

        public DataTable ReturnResultSet(string SQLConnectionString, string SQL)
        {
            sConn1.ConnectionString = SQLConnectionString;
            DataTable dtResults = new DataTable();
            SqlDataAdapter sDA1 = new SqlDataAdapter(SQL, SQLConnectionString);

            try
            {

                ConnectionSwitch(SQLConnectionString, true);

                sDA1.SelectCommand.CommandText = SQL;
                sDA1.SelectCommand.CommandTimeout = 120;
                sDA1.Fill(dtResults);

                ConnectionSwitch(SQLConnectionString, false);

                Console.WriteLine(@"Query successful at: " + DateTime.Now);
                Console.WriteLine(@"Query returned " + dtResults.Rows.Count.ToString() + @" rows");
                return dtResults;
            }
            catch (Exception ex)
            {

                ConnectionSwitch(SQLConnectionString, false);

                Console.WriteLine(@"Error during query: " + ex.Message);
                return dtResults;
            }

        }


        public int ExecuteSQL(string SQLConnectionString, string SQL)
        {

            sConn1.ConnectionString = SQLConnectionString;

            try
            {

                ConnectionSwitch(SQLConnectionString, true);

                sComm1.Connection = sConn1;
                sComm1.CommandText = SQL;
                sComm1.CommandTimeout = 600;
                sComm1.ExecuteNonQuery();

                ConnectionSwitch(SQLConnectionString, false);

                Console.WriteLine(@"Insert successful at: " + DateTime.Now);
                return 1;
            }
            catch (Exception ex)
            {

                ConnectionSwitch(SQLConnectionString, false);

                Console.WriteLine(@"Error during query: " + ex.Message);
                return 0;
            }

        }

        public int BulkCopy(string SQLConnectionString, string DestinationTable, DataTable DataSet)
        {
            Console.WriteLine(Environment.NewLine + @"Initiating Bulk Copy to: " + DestinationTable);
            sConn1.ConnectionString = SQLConnectionString;

            try
            {

                using (sConn1)
                {
                    ConnectionSwitch(SQLConnectionString, true);
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sConn1))
                    {
                        bulkCopy.DestinationTableName = DestinationTable;
                        bulkCopy.BulkCopyTimeout = 900;
                        bulkCopy.BatchSize = 10000;

                        for (int i = 0; i < DataSet.Columns.Count; i++)
                        {
                            bulkCopy.ColumnMappings.Add(i, i + 1);
                        }

                        bulkCopy.WriteToServer(DataSet);
                    }
                }

                ConnectionSwitch(SQLConnectionString, false);
                Console.WriteLine(Environment.NewLine + @"Bulk Copy Complete. Transferred: " + DataSet.Rows.Count.ToString() + " rows");

                return 1;
            }
            catch (Exception ex)
            {

                ConnectionSwitch(SQLConnectionString, false);
                Console.WriteLine(@"Bulk Copy Failure at: " + DateTime.UtcNow + " " + ex.Message);
                return 0;
            }

        }

    }
}
