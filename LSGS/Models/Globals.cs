using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;

namespace LSGS.Models
{
    public static class Globals
    {
        public static Profile profile;
        public static MySqlConnection connection;
        public static MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
        {
            Server = "db4free.net",
            UserID = "is502grp3",
            Password = "metu2022is502",
            Database = "lsgsg3",
        };
    }
}
