using a7D.PDV.Ativacao.API.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace a7D.PDV.Ativacao.API.Services
{
    public class ItemDB
    {
        public string sql;
        public object[] parameters;

        public ItemDB(string sql, object[] obj)
        {
            this.sql = sql;
            this.parameters = obj;
        }
    }

    public static class UpdateDB
    {
        private static Queue<ItemDB> commandList = new Queue<ItemDB>();
        private static Task task;

        public static void RequestChanges(string sql, params object[] obj)
        {
            var cmd = new ItemDB(sql, obj);
            lock (commandList)
                commandList.Enqueue(cmd);

            if (task == null)
                task = Task.Factory.StartNew(Execute);
        }

        private static void Execute()
        {
            try
            {
                using (var db = new AtivacaoContext())
                {
                    while (commandList.Count > 0)
                    {
                        ItemDB cmd;
                        lock (commandList)
                            cmd = commandList.Dequeue();

                        db.Database.ExecuteSqlCommand(cmd.sql, cmd.parameters);
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                task = null;
            }
        }
    }
}