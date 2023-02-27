using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    
    public class SqlScripts
    {
        public static string CreateTable = @"CREATE TABLE [dbo].[ImportedPunchRecord] ("
                                           + @" [MachineID]     UNIQUEIDENTIFIER  NOT NULL,"
                                           + @" [CardID]        UNIQUEIDENTIFIER  NOT NULL,"
                                           + @" [PunchIn]       DATETIME NOT NULL,"
                                           + @" [PunchOut]      DATETIME NOT NULL,"
                                           + @" [FileName]      NVARCHAR (100)  NOT NULL,"
                                           + @" [LineNumber]    INT NOT NULL,"
                                           + @" [DateOfImport]  DATE DEFAULT (getdate()) NOT NULL,"
                                           + @" );";
        
        public static string CheckTable = @"USE master " 
                                        + @"SELECT * FROM sys.tables WHERE [name]='ImportedPunchRecord' ";

        public static string DroplocalDB = @"DROP DATABASE [punchDB] ";

        public static string CreatelocalDB = @"CREATE DATABASE [punchDB]   "
                                           + @"ON PRIMARY (  "
                                           + @" NAME = punchDB_dat, "
                                           + @" FILENAME = '{0}\punchDB.mdf',  "
                                           + @" SIZE = 10, "
                                           + @" MAXSIZE = 50,  "
                                           + @" FILEGROWTH = 5 )  "
                                           + @"LOG ON ( "
                                           + @" NAME = punchDB_log,  "
                                           + @" FILENAME = '{1}\punchDB.ldf',  "
                                           + @" SIZE = 5MB, "
                                           + @" MAXSIZE = 25MB, "
                                           + @" FILEGROWTH = 5MB ); ";

        public static string localDB = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=punchDB.mdf;Integrated Security=True;Connect Timeout=30";
    }
}
