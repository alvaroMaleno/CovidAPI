using System.Threading;
using System;
using CoVid.Controllers.DAOs.Interfaces;
using Npgsql;

namespace CoVid.Controllers.DAOs.Connection
{
    public class ConnectionPostgreSql : IDataBaseConnector<NpgsqlConnection>
    {
        private string _connectionProperties;
        private NpgsqlConnection _oNpgsqlConnection;
        private ConnectionPostgreProperties _oProperties;

        public ConnectionPostgreSql()
        {
            this.SetProperties();
        }

        private void SetProperties()
        {
            _connectionProperties = String.Empty;
            string so = Utils.UtilsSO.GetInstance().GetSO();
            if(so.Contains("unix"))
            {
                _connectionProperties = @"./DAOs/Connection/connectionProperties.json";
            }
            else
            {
                _connectionProperties = @".\DAOs\Connection\connectionProperties.json";
            }

            var connectionProp = Utils.UtilsStreamReaders.GetInstance().ReadStreamFile(_connectionProperties);
            Utils.UtilsJSON.GetInstance().DeserializeFromString(out _oProperties, connectionProp);
            
        }

        public bool CloseConnection()
        {
            try
            {
                _oNpgsqlConnection.Close();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public void Connect()
        {
            string conectString = string.Format(
                "Server={0};Port={1};User Id={2};Password={3};Database={4};",
                this._oProperties.server, this._oProperties.port, 
                this._oProperties.userId, this._oProperties.pass, this._oProperties.dataBase);

            try
            {
                _oNpgsqlConnection = new NpgsqlConnection(conectString);
                _oNpgsqlConnection.Open();
            }
            catch (System.Exception)
            {
                if(_oNpgsqlConnection != null)
                    _oNpgsqlConnection.Close();
            }
        }

        public NpgsqlConnection GetConnection()
        {
            return this._oNpgsqlConnection;
        }

        public bool ExecuteCommand(string querySentence)
        {
            NpgsqlConnection oConnection = null;

            try
            {
                this.Connect();
                oConnection = this.GetConnection();
                using (var oCommand = new NpgsqlCommand(querySentence, oConnection))
                {
                    oCommand.ExecuteNonQueryAsync();
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            finally
            {
                if(oConnection != null)
                    oConnection.Close();
            }

            return true;
        }

        public bool ExecuteCommand(params string[] pQuerySentences)
        {
            NpgsqlConnection oConnection = null;

            try
            {
                //Experience has shown in a normal pc the database needs
                //about 300 ms between sentences.
                int milisecondsBetweenSentences = 350;
                this.Connect();
                oConnection = this.GetConnection();
                foreach (var sentence in pQuerySentences)
                {
                    using (var oCommand = new NpgsqlCommand(sentence, oConnection))
                    {
                        oCommand.ExecuteNonQueryAsync();
                        Thread.Sleep(milisecondsBetweenSentences);
                    }
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            finally
            {
                if(oConnection != null)
                    oConnection.Close();
            }

            return true;
        }

    }
}