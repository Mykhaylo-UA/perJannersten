using System.Data.OleDb;

OleDbConnection connection = new ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\BOS\\Events\\BWS_test\\DATA\\bws_test.bws;Persist Security Info=True");
connection.Open();
string query = "ALTER TABLE SETTINGS DROP BM2GameSummary"; 
OleDbCommand command = new (query, connection);
command.ExecuteNonQuery();