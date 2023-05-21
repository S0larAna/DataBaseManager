using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using System.Runtime.CompilerServices;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace DataBaseManager
{
    public class PGManage
    {
        public static NpgsqlConnection con = null;
        public static string table;

        public static void Connect(string host, string port, string user, string pass, string db)
        {
            if (con!=null)
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
            }
            else
            {
                con = new NpgsqlConnection(@"Server=" + host + ";Port=" + port + ";User Id=" + user + ";Password=" + "1Oshibka" + ";Database=" + db);
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    DialogResult result = MessageBox.Show("Connected");
                    if (result==DialogResult.OK)
                    {
                        DBViewer form = new DBViewer();
                        form.Show();

                    }
                }
                else MessageBox.Show("Not Connected");
            }
        }

        public static List<string> listTheCatalogs()
        {
            NpgsqlCommand com = con.CreateCommand();
            com.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema='public' AND table_type='BASE TABLE'";
            NpgsqlDataReader dt = com.ExecuteReader(CommandBehavior.Default);
            List<string> table_names = new List<string>();

            while (dt.Read())
            {
                try
                {
                    for (int i=0; i<dt.FieldCount; i++)
                    {
                        table_names.Add(dt.GetString(i));
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            dt.Close();
            return table_names;
        }

        public static void Populate_Grid(string table_name, DataGridView g)
        {
            try
            {
                List<List<string>> listOfLists = Select_all(table_name);
                g.Rows.Clear();
                g.Columns.Clear();

                foreach (string ss in listOfLists[0])
                {
                    g.Columns.Add(ss, ss);
                }
                bool skip1 = false;
                foreach (List<string> list in listOfLists)
                {
                    if (skip1)
                    {
                        string[] v = new string[list.Count];
                        int kk = 0;
                        foreach (string ss in list)
                        {
                            v[kk] = ss;
                            kk++;
                        }
                        g.Rows.Add(v);
                    }
                    skip1 = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public static List<List<string>> Select_all(string table_view)
        {
            List<List<string>> list = new List<List<string>>();
            if (con != null)
            {
                if (con.State == ConnectionState.Open)
                {
                    NpgsqlCommand com = con.CreateCommand();
                    com.CommandText = "Select * from public.\"" + table_view + "\"";
                    NpgsqlDataReader dt = com.ExecuteReader(CommandBehavior.Default);
                    List<string> list_names = new List<string>();
                    for (int i = 0; i < dt.FieldCount; i++)
                    {
                        list_names.Add(dt.GetName(i));
                    }
                    list.Add(list_names);
                    while (dt.Read())
                    {
                        try
                        {
                            List<string> inside_list = new List<string>();
                            for (int i = 0; i < dt.FieldCount; i++)
                            {
                                inside_list.Add(dt.GetValue(i).ToString());
                            }
                            list.Add(inside_list);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                    dt.Close();
                }
                else throw new Exception("Not Opened Connection!");
            }
            else throw new Exception("Not Connected!");
            return list;
        }
    }


    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}