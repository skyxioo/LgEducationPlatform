using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lg.EducationPlatform.IDAL;
using System.Runtime.Remoting.Messaging;

namespace Lg.EducationPlatform.DAL
{
    public class DbSessionFactory : IDbSessionFactory
    {
        //依赖接口编程，尽量返回给外界抽象的东西
        public IDbSession GetDbSession()
        {
            IDbSession dbSession = CallContext.GetData("DbSession") as DbSession;            

            if (dbSession == null)
            {
                dbSession = new DbSession();
                CallContext.SetData("DbSession", dbSession);
            }

            return dbSession;
        }
    }
}
