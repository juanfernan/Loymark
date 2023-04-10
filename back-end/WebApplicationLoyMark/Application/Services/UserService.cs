using Application.Contracts.Repository;
using Application.Contracts.Service;
using Data.Entities;
using System.Transactions;

namespace WebApplicationLoyMark.Services
{
    public class UserService : IUserService
    {

        public readonly IUserRepository _userStore;
        public readonly IActivityRepository _activityStore;


        public UserService(IActivityRepository activityStore, IUserRepository userStore)
        {
            _activityStore = activityStore;
            _userStore = userStore;
        }

        public async Task<IList<User>> GetAll()
        {
            return await _userStore.GetAll();
        }

        public async Task<User> GetUserById(int id)
        {
            User? userFound = await _userStore.FindyById(id);

            if (userFound == null)
                throw new ApplicationException("User not found");

            return userFound;
        }

        public async Task Create(User user)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var id = await _userStore.Create(user);
                    await LogEvents("User creation", id);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task Update(int id, User user)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                User? userFound = await _userStore.FindyById(id);

                if (userFound == null)
                    throw new ApplicationException("User not found");

                userFound.Name = user.Name;
                userFound.LastName = user.LastName;
                userFound.Email = user.Email;
                userFound.BirthDate = user.BirthDate;
                userFound.Phone = user.Phone;
                userFound.Country = user.Country;

                await _userStore.Update(userFound);

                await LogEvents("User update", userFound.Id);

                scope.Complete();
            }
        }

        public async Task Delete(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                User? user = await _userStore.FindyById(id);

                if (user == null)
                    throw new Exception("User not found");

                await _userStore.Delete(user);
                scope.Complete();
            }
        }

        public async Task Disabled(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                User? user = await _userStore.FindyById(id);

                if (user == null)
                    throw new Exception("User not found");

                user.Enabled = false;
                await _userStore.Update(user);
                await LogEvents("User has been disabled", id);
                scope.Complete();
            }
        }

        public async Task LogEvents(string task, int user)
        {
            Activity activity = new Activity()
            {
                CreateDate = DateTime.Now,
                Task = task,
                Id_User = user,
            };
            await _activityStore.LogEvents(activity);
        }

        public async Task<List<Activity>> GetActivity()
        {
            return await _activityStore.GetActivity();
        }
    }
}
