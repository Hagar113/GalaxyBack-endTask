using DataAccess;
using DataAccess.IRepo;
using DataProvider.IProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.Provider
{
    public class AuthProvider:IAuthProvider
    {
        private readonly ApplicationDBContext _dbContext;
        public IAuthenticationRepo AuthenticationRepo { get; private set; }

        public AuthProvider(ApplicationDBContext context, IAuthenticationRepo authenticationRepo)
        {
            _dbContext = context;
            AuthenticationRepo = authenticationRepo;
        }
    }
}
