using DataAccess;
using DataAccess.IRepo;
using DataAccess.Repo;
using DataProvider.IProvider;
using Infrastructure.helpers.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.Provider
{
    public class AuthProvider : IAuthProvider
    {
        private readonly ApplicationDBContext _dbContext;
        public IAuthenticationRepo AuthenticationRepo { get; private set; }

        public AuthProvider(ApplicationDBContext context)
        {
            _dbContext = context;
            AuthenticationRepo = new AuthenticationRepo(_dbContext);
        }
    }
}
