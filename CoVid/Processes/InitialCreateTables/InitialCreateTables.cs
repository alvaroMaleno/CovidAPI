using System;
using System.Threading.Tasks;
using CoVid.DAOs.Abstracts;
using CoVid.Models.PathModels;
using CoVid.Processes.Interfaces;
using CoVid.Utils;

namespace CoVid.Processes.InitialCreateTables
{
    public class InitialCreateTables : ITaskable
    {
        private static InitialCreateTables _instance;
        private CovidDAO _oCovidDAO;
        private string[] _oPathsArray;

        private InitialCreateTables(CovidDAO pCovidDAO)
        {
            this._oCovidDAO = pCovidDAO;
            this._oPathsArray = new string[]{"geozone", "dates", "countries"};
        }

        public static InitialCreateTables GetInstance(CovidDAO pCovidDAO)
        {
            if(_instance is null)
            {
                _instance = new InitialCreateTables(pCovidDAO);
            }
            return _instance;
        }

        public async Task CreateTables()
        {
            foreach (var path in _oPathsArray)
            {
                this._oCovidDAO.CreateTable(path);
            }
        }

        public Task Taskable()
        {
            return this.CreateTables();
        }
    }
}