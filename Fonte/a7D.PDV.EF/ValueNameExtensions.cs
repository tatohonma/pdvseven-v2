using a7D.PDV.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace a7D.PDV.EF
{
    public static class ValueNameExtensions
    {
        public static void Fill<TEntity>(this ComboBox cmb, int? value, bool vazio = false, Expression<Func<TEntity, bool>> where = null) where TEntity : class, IValueName, new()
        {
            using (var pdv = new pdv7Context())
            {
                var dbSet = pdv.DbSet<TEntity>();

                List<TEntity> dbList;
                if (where == null)
                    dbList = dbSet.ToList();
                else
                    dbList = dbSet.Where(where).ToList();

                var list = (from q in dbList
                            select ValueName.From(q)).ToList();

                if (vazio)
                    list.Insert(0, new ValueName(0, "(nenhum)"));

                cmb.ValueMember = "Value";
                cmb.DisplayMember = "Name";
                cmb.DataSource = list;

                if (value != null)
                    cmb.SelectedValue = value;
            }
        }
    }
}
