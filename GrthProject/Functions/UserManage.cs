using CMP.Operation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMP.Operation.Functions
{
    public class UserManage
    {
        public enum Roles { Admin , User };
        #region Private/protected fields
        protected readonly RepositoryContainer _repository;
        #endregion

        #region Constructor
        public UserManage()
        {
            _repository = new RepositoryContainer();
        }
        #endregion

        public string GetUserPassword(string email)
        {
            var user = _repository.UserRepository.Where(o => o.Email.ToLower().Equals(email));
            if (user.Any())
                return user.FirstOrDefault().Password;
            else
                return string.Empty;
        }

        public bool IsUserInRole(string email, Roles roleName)
        {
            var user = _repository.UserRepository.WhereAndInclude(u => u.Email.ToLower().Equals(email), u=>u.Role).FirstOrDefault();
            if (user != null)
            {
                var role = user.Role.RoleName;
                if (role == roleName.ToString())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
