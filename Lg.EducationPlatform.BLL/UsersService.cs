using Lg.EducationPlatform.Common;
using Lg.EducationPlatform.Enum;
using Lg.EducationPlatform.IBLL;
using Lg.EducationPlatform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lg.EducationPlatform.BLL
{
    public partial class UsersService : BaseService<Users>, IUsersService
    {
        public UserDto GetUser(string userName)
        {
            var whereExp = PredicateBuilder.True<Users>();
            whereExp = whereExp.And(p => p.IsActive && !p.IsDeleted && p.UserName == userName);
            var users = CurrentDAL.GetDataListBy(whereExp);
            var user = (from p in users
                       select new UserDto {
                           UserId = p.Id,
                           UserName = p.UserName,
                           PassWord = p.HashPassword,
                           RoleId = p.RoleId
                       }).FirstOrDefault();
            return user;
        }

        public List<ItemModel> GetTeacherItems()
        {
            var whereExp = PredicateBuilder.True<Users>();
            whereExp = whereExp.And(p => p.IsActive && !p.IsDeleted && p.RoleId == (int)UserRole.教师);
            var users = CurrentDAL.GetDataListBy(whereExp);
            var items = (from p in users
                        select new ItemModel
                        {
                            Text = p.RealName,
                            Value = p.Id.ToString()
                        }).ToList();
            return items;
        }
    }
}
