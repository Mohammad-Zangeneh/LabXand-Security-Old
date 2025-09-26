using Kms.Security.Api.Controllers;
using Kms.Security.Common;
using Kms.Security.Identity;
using Kms.Security.Identity.Service;
using Kms.Security.Jwt;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using StructureMap;
using StructureMap.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kms.Security.Api.App_Start.Ioc
{
    public class ObjectFactory
    {
        private static IContainer _current;
        public static IContainer Current
        {
            get { return _current; }
        }
        static ObjectFactory()
        {
            _current = ConfigureContainer();
        }
        public static IContainer ConfigureContainer()
        {
            IContainer container = new Container();
            container.Configure(c => c.AddRegistry(new IdentityIocRegistery()));
            container.Configure(configuration => configuration.AddRegistry(new GlobalIocRegistry()));

            container.Configure(c => c.AddRegistry(new JwtIocRegistery()));
            return container;
        }


    }
}