using System.Threading;
using System;
using CoVid.Controllers.DAOs.Interfaces;
using Npgsql;
using System.Collections.Generic;

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
                _oNpgsqlConnection.Dispose();
                _oNpgsqlConnection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Connect()
        {
            string conectString = string.Format(
                "Server={0};Port={1};User Id={2};Password={3};Database={4};Timeout=20;",
                this._oProperties.server, this._oProperties.port, 
                this._oProperties.userId, this._oProperties.pass, this._oProperties.dataBase);

            try
            {
                _oNpgsqlConnection = new NpgsqlConnection(conectString);
                _oNpgsqlConnection.Open();
            }
            catch (Exception)
            {
                if(_oNpgsqlConnection != null)
                    this.CloseConnection();
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
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if(oConnection != null)
                    this.CloseConnection();
            }

            return true;
        }

        public bool ExecuteCommand(params string[] pQuerySentences)
        {
            NpgsqlConnection oConnection = null;

            try
            {
                //Experience has shown in a normal pc the database needs
                //about 350 ms between sentences.
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
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if(oConnection != null)
                    this.CloseConnection();
            }

            return true;
        }

        public bool ExecuteSelectCommand(string querySentence, List<object[]> pResultList)
        {
            NpgsqlConnection oConnection = null;
            NpgsqlDataReader oReader;
            try
            {
                this.Connect();
                oConnection = this.GetConnection();
                using (var oCommand = new NpgsqlCommand(querySentence, oConnection))
                {
                    oReader = oCommand.ExecuteReader();
                    while (oReader.Read())
                    {
                        object[] oRow = new object[oReader.FieldCount];
                        oReader.GetValues(oRow);
                        pResultList.Add(oRow);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if(oConnection != null)
                    this.CloseConnection();
            }
            return true;
        }

    }
}