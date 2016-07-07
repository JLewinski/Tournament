using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tournament.Core.Data;

namespace Tournament.Portable
{
    public class Class1
    {
        readonly ITournamentContextBase _dbContext;

        public Class1(TournamentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(object t)
        {
            _dbContext.Update(t);
        }
    }
}
