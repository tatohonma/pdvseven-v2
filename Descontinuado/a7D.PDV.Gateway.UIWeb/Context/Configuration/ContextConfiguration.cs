using a7D.PDV.Gateway.UIWeb.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IdentityModel.Configuration;
using System.Linq;
using System.Web;

namespace a7D.PDV.Gateway.UIWeb.Context.Configuration
{
    public class ContextConfiguration
    {
        [ImportMany(typeof(IEntityConfiguration))]
        public IEnumerable<IEntityConfiguration> Configurations { get; set; }
    }
}