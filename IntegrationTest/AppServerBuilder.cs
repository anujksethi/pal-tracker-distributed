using System;
using System.Collections.Generic;

namespace IntegrationTest
{
    public sealed partial class AppServerBuilder
    {
        private int? _port;
        private string _appName;
        private readonly IDictionary<string, string> _environmentVariables = new Dictionary<string, string>();
        private string _database;

        public static AppServerBuilder TestAppServerBuilder() => new AppServerBuilder();

        public AppServerBuilder Port(int port)
        {
            _port = port;
            return this;
        }

        public AppServerBuilder AppName(string appName)
        {
            _appName = appName;
            return this;
        }

        public AppServerBuilder SetEnvironmentVariable(string key, string value)
        {
            _environmentVariables.Add(key, value);
            return this;
        }

        public AppServerBuilder Database(string database)
        {
            _database = database;
            return this;
        }

        public AppServer Build()
        {
            if (_port.HasValue && _appName != null && _database != null)
            {
                return new AppServer(_port.Value, _appName, _environmentVariables, _database);
            }

            throw new InvalidOperationException("AppServer object is fully configured.");
        }
    }
}