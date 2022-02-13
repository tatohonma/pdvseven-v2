using System;

namespace a7D.Fmk.CRUD.DAL
{
  [AttributeUsage(AttributeTargets.Class)]
  public class CRUDClassDALAttribute : Attribute
  {
    public string Tabela { get; }

    public double Version { get; } = 1.0;

    public CRUDClassDALAttribute(string tabela = null)
    {
      Tabela = tabela;
    }
  }

  [AttributeUsage(AttributeTargets.Property)]
  public class CRUDParameterDALAttribute : Attribute
  {
    public bool Pk { get; }

    public string Coluna { get; }

    public string Fk { get; }

    public double Version { get; } = 1.0;

    public CRUDParameterDALAttribute(bool pk = false, string coluna = null, string fk = null)
    {
      Pk = pk;
      Coluna = coluna;
      Fk = fk;
    }
  }
}
