using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;
using LoginDataAccess;

namespace AvaTradeServer.Models
{
    public class UserLogin
    {
        private static readonly object _lock = new object();
        private static UserLogin instance = null;
        public static UserLogin Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new UserLogin();
                    }
                    return instance;
                }
            }
        }

        private const string CacheKeyLogin = "loginBlocked_";
        private ObjectCache cache = MemoryCache.Default;
        //private ObjectCache caceMasterPlans = MemoryCache.Default;

        private bool getIsBlocked(string userName)
        {
            if (cache.Contains(CacheKeyLogin + userName))
            {
                bool isBlocked = (bool)cache.Get(CacheKeyLogin + userName);
                return isBlocked;
            }
            else
            {
                return false;
            }
        }
        private bool CheckUserAndPass(string userName, string pass)
        {
            using (LoginDetailsEntities db = new LoginDetailsEntities())
            {
                var userLogin = db.Set<Login>();
                userLogin.Add(new Login { login_time = DateTime.Now, user_name = userName });

                db.SaveChanges();
            }
            using (LoginDetailsEntities entities = new LoginDetailsEntities())
            {

                var data = entities.UsersDetails.Where(x => x.user_name == userName &&
                       x.passward == pass);
                if ( data !=null &&data.Count() > 0)
                    return true;
            }
            return false;
        }
        public LoginResponse CheckLoginUser(string userName, string pass)
        {

            LoginResponse res = new LoginResponse();
            res.isBlocked = getIsBlocked(userName);
            if (res.isBlocked)
                return res;

            if (CheckUserAndPass(userName, pass))
                res.isLoginSuccess = true;


            using (LoginDetailsEntities entities = new LoginDetailsEntities())
            {
                DateTime time = DateTime.Now.AddMinutes(-5);
                int countLogin = entities.Login.Where(x => x.login_time >=time ).Count();
                if (countLogin >= 10)
                {
                    CacheItemPolicy _cacheItemPolicy = new CacheItemPolicy();
                    _cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(7);
                    cache.Add(CacheKeyLogin + userName, true, _cacheItemPolicy);
                    if (!res.isLoginSuccess)
                    {
                        res.isBlocked = true;
                        return res;
                    }
                }
            
            }

            return res;



        }

    }
}